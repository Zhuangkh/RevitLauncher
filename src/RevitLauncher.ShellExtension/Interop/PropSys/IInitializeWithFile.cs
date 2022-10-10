using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.PropSys
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
    public interface IInitializeWithFile
    {
        void Initialize([In, MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, STGM grfMode);
    }
}
