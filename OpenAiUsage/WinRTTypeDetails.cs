using System.Runtime.InteropServices;
using ABI.Microsoft.CommandPalette.Extensions;
using ABI.System;
using WinRT;

namespace WinRT.OpenAiUsageVtableClasses;

internal sealed class UsageExtensionWinRTTypeDetails : IWinRTExposedTypeDetails
{
    public ComWrappers.ComInterfaceEntry[] GetExposedInterfaces() =>
    [
        new() { IID = IExtensionMethods.IID, Vtable = IExtensionMethods.AbiToProjectionVftablePtr },
        new() { IID = IDisposableMethods.IID, Vtable = IDisposableMethods.AbiToProjectionVftablePtr }
    ];
}

internal sealed class UsageCommandProviderWinRTTypeDetails : IWinRTExposedTypeDetails
{
    public ComWrappers.ComInterfaceEntry[] GetExposedInterfaces() =>
    [
        new() { IID = ICommandProviderMethods.IID, Vtable = ICommandProviderMethods.AbiToProjectionVftablePtr },
        new() { IID = IDisposableMethods.IID, Vtable = IDisposableMethods.AbiToProjectionVftablePtr },
        new() { IID = INotifyItemsChangedMethods.IID, Vtable = INotifyItemsChangedMethods.AbiToProjectionVftablePtr },
        new() { IID = ICommandProvider2Methods.IID, Vtable = ICommandProvider2Methods.AbiToProjectionVftablePtr },
        new() { IID = ICommandProvider3Methods.IID, Vtable = ICommandProvider3Methods.AbiToProjectionVftablePtr },
        new() { IID = ICommandProvider4Methods.IID, Vtable = ICommandProvider4Methods.AbiToProjectionVftablePtr }
    ];
}
