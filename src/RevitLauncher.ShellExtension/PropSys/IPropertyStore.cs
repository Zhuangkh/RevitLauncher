using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using OpenMcdf;
using System.Reflection.Metadata;
using System.Security;

namespace RevitLauncher.ShellExtension.PropSys
{
    [SuppressUnmanagedCodeSecurity]
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    public interface IPropertyStore
    {
        uint GetCount();

        PROPERTYKEY GetAt(uint iProp);

        void GetValue(in PROPERTYKEY pkey, [In, Out] PROPVARIANT pv);

        void SetValue(in PROPERTYKEY pkey, [In] PROPVARIANT pv);
        void Commit();
    }
}