using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace RevitLauncher.ShellExtension.Interop.PropSys
{
    [ComImport, Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithStream
    {
        void Initialize(IStream pstream, STGM grfMode);
    }
}
