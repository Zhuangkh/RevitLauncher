using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishellitem
    /// </summary>
    [ComImport]
    [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] ref Guid bhid, [In] ref Guid riid);

        void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

        void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

        void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
    }
}
