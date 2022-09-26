using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ienumexplorercommand
    /// </summary>
    [ComImport]
    [Guid("a88826f8-186f-4987-aade-ea0cef8fbfe8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumExplorerCommand
    {
        [PreserveSig]
        HRESULT Next(uint celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 0)] IExplorerCommand[] pUICommand, out uint pceltFetched);

        [PreserveSig]
        HRESULT Skip(uint celt);

        void Reset();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumExplorerCommand Clone();
    }
}
