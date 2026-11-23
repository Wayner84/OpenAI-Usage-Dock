using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using WinRT;
using WinRT.OpenAiUsageVtableClasses;

namespace OpenAiUsage;

[WinRTRuntimeClassName("Microsoft.CommandPalette.Extensions.ICommandProvider")]
[WinRTExposedType(typeof(UsageCommandProviderWinRTTypeDetails))]
public sealed class UsageCommandProvider : CommandProvider
{
    private readonly UsageDockItem _item = new();
    public UsageCommandProvider()
    {
        Id = "uk.townsendprecision.openaiusage";
        DisplayName = "OpenAI usage";
        Icon = UsageDockItem.UsageIcon;
    }
    public override ICommandItem[] TopLevelCommands() => [_item];
    public override ICommandItem[] GetDockBands() => [_item];
    public override ICommandItem? GetCommandItem(string id) => id == UsageDockItem.CommandId ? _item : null;
    public override void Dispose() => _item.Dispose();
}
