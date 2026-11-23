using Microsoft.CommandPalette.Extensions;

namespace OpenAiUsage;

public static class Program
{
    [MTAThread]
    public static void Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "--probe")
        {
            try
            {
                var result = UsageReader.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
                Console.WriteLine($"{result.Title} | {result.Subtitle}");
                Environment.ExitCode = 0;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Usage unavailable: {exception.GetType().Name}");
                Environment.ExitCode = 1;
            }
            return;
        }
        if (args.Length == 0 || args[0] != "-RegisterProcessAsComServer") return;
        using var server = new ExtensionServer();
        using var disposed = new ManualResetEvent(false);
        var extension = new UsageExtension(disposed);
        server.RegisterExtension(() => extension);
        disposed.WaitOne();
    }
}
