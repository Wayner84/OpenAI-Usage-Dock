using System.Diagnostics;
using System.Text.Json;

namespace OpenAiUsage;

public sealed record UsageDisplay(string Title, string Subtitle);

public static class UsageReader
{
    public static async Task<UsageDisplay> ReadAsync(CancellationToken cancellationToken)
    {
        string home = Environment.GetEnvironmentVariable("HERMES_HOME")
            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "hermes");
        string hermes = Path.Combine(home, "hermes-agent", ".hermes", "bin", "hermes.exe");
        if (!File.Exists(hermes)) throw new FileNotFoundException("Hermes launcher not found");

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = hermes,
                WorkingDirectory = Path.Combine(home, "hermes-agent"),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }
        };
        process.StartInfo.ArgumentList.Add("usage");
        process.StartInfo.ArgumentList.Add("--provider");
        process.StartInfo.ArgumentList.Add("openai-codex");
        process.StartInfo.ArgumentList.Add("--json");
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(45));
        try
        {
            if (!process.Start()) throw new InvalidOperationException("Hermes could not start");
            var output = process.StandardOutput.ReadToEndAsync(timeout.Token);
            await process.WaitForExitAsync(timeout.Token);
            var text = await output;
            if (process.ExitCode != 0) throw new InvalidOperationException("Hermes usage read failed");
            return Parse(text);
        }
        catch
        {
            if (!process.HasExited) process.Kill(entireProcessTree: true);
            throw;
        }
    }

    public static UsageDisplay Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (!root.TryGetProperty("windows", out var windows) || windows.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("Missing usage windows");
        var available = new List<(string Label, double Remaining, DateTimeOffset? Reset)>();
        foreach (var window in windows.EnumerateArray())
        {
            if (!window.TryGetProperty("label", out var label) || label.ValueKind != JsonValueKind.String ||
                !window.TryGetProperty("used_percent", out var used) || used.ValueKind != JsonValueKind.Number ||
                !used.TryGetDouble(out var percent) || !double.IsFinite(percent) || percent < 0 || percent > 100)
                continue;
            DateTimeOffset? reset = null;
            if (window.TryGetProperty("resets_at", out var at) && at.ValueKind == JsonValueKind.String &&
                DateTimeOffset.TryParse(at.GetString(), out var parsed)) reset = parsed;
            available.Add((label.GetString()!, 100 - percent, reset));
        }
        if (available.Count == 0) throw new InvalidDataException("No current usage window");
        // Both limits must have capacity, so the lowest remaining percentage is the actionable one.
        var limiting = available.MinBy(w => w.Remaining);
        var title = $"AI {limiting.Remaining:0.#}% left";
        string subtitle = available.Count > 1
            ? string.Join(" · ", available.Select(w => $"{(w.Label == "Session" ? "5h" : w.Label)} {w.Remaining:0.#}%"))
            : $"{limiting.Label} allowance";
        if (available.Count == 1 && limiting.Reset is { } resetTime)
            subtitle += $" · resets {resetTime.ToLocalTime():ddd HH:mm}";
        return new UsageDisplay(title, subtitle);
    }
}
