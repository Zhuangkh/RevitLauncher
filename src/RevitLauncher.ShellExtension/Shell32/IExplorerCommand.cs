using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RevitLauncher.ShellExtension.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iexplorercommand
    /// </summary>
    [ComImport]
    [Guid("a08ce4d0-fa25-44ab-b57c-c7b1c323e0b9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IExplorerCommand
    {
        void GetTitle(IShellItemArray psiItemArray, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

        void GetIcon(IShellItemArray psiItemArray, [MarshalAs(UnmanagedType.LPWStr)] out string ppszIcon);

        void GetToolTip(IShellItemArray psiItemArray, [MarshalAs(UnmanagedType.LPWStr)] out string ppszInfotip);

        void GetCanonicalName(out Guid pguidCommandName);

        void GetState(IShellItemArray psiItemArray, [MarshalAs(UnmanagedType.Bool)] bool fOkToBeSlow, out EXPCMDSTATE pCmdState);

        void Invoke(IShellItemArray psiItemArray, [MarshalAs(UnmanagedType.Interface)] object pbc);

        void GetFlags(out EXPCMDFLAGS pFlags);

        [PreserveSig]
        HRESULT EnumSubCommands(out IEnumExplorerCommand ppEnum);
    }
}
