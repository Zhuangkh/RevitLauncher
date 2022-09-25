using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using OpenMcdf;
using System.Reflection.Metadata;

namespace RevitLauncher.ShellExtension.PropSys
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    public interface IPropertyStore
    {
       
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HRESULT GetCount([Out] out uint propertyCount);

        /// <summary>Get a property key located at a specific index.</summary>
        /// <param name="propertyIndex">The property index.</param>
        /// <param name="key">The key.</param>
        /// <returns>The result.</returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HRESULT GetAt([In] uint propertyIndex, out PROPERTYKEY key);

        /// <summary>Gets the value of a property from the store.</summary>
        /// <param name="key">The key.</param>
        /// <param name="pv">The pv.</param>
        /// <returns>The result.</returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HRESULT GetValue([In] ref PROPERTYKEY key, [Out] PropVariant pv);

        /// <summary>Sets the value of a property in the store.</summary>
        /// <param name="key">The key.</param>
        /// <param name="pv">The pv.</param>
        /// <returns>The result.</returns>
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HRESULT SetValue([In] ref PROPERTYKEY key, [In] PropVariant pv);

        /// <summary>Commits the changes.</summary>
        /// <returns>The result.</returns>
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HRESULT Commit();
    }
}