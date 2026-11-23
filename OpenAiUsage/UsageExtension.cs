using System.Runtime.InteropServices;
using Microsoft.CommandPalette.Extensions;
using WinRT;
using WinRT.OpenAiUsageVtableClasses;

namespace OpenAiUsage;

[ComVisible(true)]
[Guid("293948F1-09CE-4B3E-BDB5-2EFACBF04C86")]
[ComDefaultInterface(typeof(IExtension))]
[WinRTRuntimeClassName("Microsoft.CommandPalette.Extensions.IExtension")]
[WinRTExposedType(typeof(UsageExtensionWinRTTypeDetails))]
public sealed class UsageExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _disposed;
    private readonly UsageCommandProvider _provider = new();
    public UsageExtension(ManualResetEvent disposed) => _disposed = disposed;
    public object? GetProvider(ProviderType providerType) => providerType == ProviderType.Commands ? _provider : null;
    public void Dispose() { _provider.Dispose(); _disposed.Set(); }
}
