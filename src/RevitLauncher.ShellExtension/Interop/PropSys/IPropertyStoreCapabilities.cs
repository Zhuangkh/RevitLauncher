using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.PropSys
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("c8e2d566-186e-4d49-bf41-6909ead56acc")]
    public interface IPropertyStoreCapabilities
    {
        [PreserveSig]
        HRESULT IsPropertyWritable(in PROPERTYKEY key);
    }
}
