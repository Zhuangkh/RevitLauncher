using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitLauncher.ShellExtension.PropSys
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("c8e2d566-186e-4d49-bf41-6909ead56acc")]
    public interface IPropertyStoreCapabilities
    {
        [PreserveSig]
        HRESULT IsPropertyWritable(in PROPERTYKEY key);
    }
}
