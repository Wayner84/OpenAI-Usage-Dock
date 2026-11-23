using Microsoft.CommandPalette.Extensions.Toolkit;

namespace OpenAiUsage;

public sealed partial class UsageDockItem : CommandItem, IDisposable
{
    public const string CommandId = "uk.townsendprecision.openaiusage.status";
    private readonly CancellationTokenSource _stop = new();
    private bool _hasValue;
    public static IconInfo UsageIcon => new(Path.Combine(AppContext.BaseDirectory, "Assets", "usage.png"));

    public UsageDockItem() : base(new NoOpCommand { Id = CommandId, Name = "OpenAI usage", Icon = UsageIcon })
    {
        Title = "AI usage …";
        Subtitle = "Checking Codex / Work allowance";
        Icon = UsageIcon;
        _ = PollAsync(_stop.Token);
    }

    private async Task PollAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var usage = await UsageReader.ReadAsync(token);
                Title = usage.Title;
                Subtitle = usage.Subtitle;
                _hasValue = true;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) { break; }
            catch
            {
                if (_hasValue) Subtitle = "Last known · refresh failed";
                else
                {
                    Title = "AI usage —";
                    Subtitle = "OpenAI allowance unavailable";
                }
            }
            try { await Task.Delay(TimeSpan.FromMinutes(3), token); }
            catch (OperationCanceledException) { break; }
        }
    }

    public void Dispose() { _stop.Cancel(); _stop.Dispose(); }
}
