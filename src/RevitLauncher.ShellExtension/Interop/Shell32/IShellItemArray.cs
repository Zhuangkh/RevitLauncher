using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishellitemarray
    /// </summary>
    [ComImport]
    [Guid("b63ea76d-1f85-456f-a19c-48159efa858b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemArray
    {
        [return: MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)]
        object BindToHandler(object pbc, in Guid rbhid, in Guid riid);

        [return: MarshalAs(UnmanagedType.Interface, IidParameterIndex = 1)]
        object GetPropertyStore(uint flags, in Guid riid);

        [return: MarshalAs(UnmanagedType.Interface, IidParameterIndex = 1)]
        object GetPropertyDescriptionList(in uint keyType, in Guid riid);

        uint GetAttributes(uint dwAttribFlags, uint sfgaoMask);

        uint GetCount();

        IShellItem GetItemAt(uint dwIndex);

        object EnumItems();
    }
}
