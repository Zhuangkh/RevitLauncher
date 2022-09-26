using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RevitLauncher.ShellExtension.Interop.PropSys
{
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 8)]
    public sealed class PROPVARIANT : IDisposable
    {
        [FieldOffset(0)] public VarEnum vt;

        [FieldOffset(8)] internal IntPtr _ptr;

        public void Dispose()
        {
            PropVariantClear(this);
            GC.SuppressFinalize(this);
        }
        ~PROPVARIANT()
        {
            Dispose();
        }

        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        [DllImport("ole32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern HRESULT PropVariantClear([In, Out] PROPVARIANT pvar);

        public static HRESULT InitPropVariantFromString(string psz, [In, Out] PROPVARIANT ppropvar)
        {
            PropVariantClear(ppropvar);
            if (psz is null) return HRESULT.E_INVALIDARG;
            ppropvar._ptr = Marshal.StringToCoTaskMemUni(psz);
            ppropvar.vt = VarEnum.VT_LPWSTR;
            return HRESULT.S_OK;
        }
    }
}