using RevitLauncher.Core;
using RevitLauncher.ShellExtension.PropSys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace RevitLauncher.ShellExtension
{
    [ComVisible(true)]
    [Guid("6542121F-1D79-4A27-9471-F80277DA8535")]

    public class RevitPropertyStore : IPropertyStore, IInitializeWithStream, IPropertyStoreCapabilities
    {
        private static Guid VersionPropdescId = new("{AD36148C-0DA3-4145-ADAD-0DDCCF79D4AE}");
        private static PROPERTYKEY rKey = new PROPERTYKEY(VersionPropdescId, 88);
        private IStream _pstream;
        private STGM _grfMode;
        private string version;

        public void Initialize(IStream pstream, STGM grfMode)
        {
            _pstream = pstream;
            _grfMode = grfMode;
            using (MemoryStream ms = pstream.ReadToMemoryStream())
            {
                var info = new RevitFileInfo(ms);
                version = info.SavedInVersion;
            }
        }

        public HRESULT IsPropertyWritable(in PROPERTYKEY key)
        {
            if (key.FmtId == VersionPropdescId)
            {
                return HRESULT.S_OK;
            }
            return HRESULT.S_FALSE;
        }

        public HRESULT GetCount([Out] out uint propertyCount)
        {
            propertyCount = 1;
            return HRESULT.S_OK;
        }

        public HRESULT GetAt([In] uint propertyIndex, out PROPERTYKEY key)
        {
            if (propertyIndex == 0)
            {
                key = rKey;
                return HRESULT.S_OK;
            }
            key = default(PROPERTYKEY);
            return HRESULT.S_FALSE;
        }

        HRESULT IPropertyStore.GetValue(ref PROPERTYKEY key, PropVariant pv)
        {
            if (key == rKey)
            {
                pv = new PropVariant(version) ;
                return HRESULT.INPLACE_S_TRUNCATED;
            }
            pv = new PropVariant() { VarType = VarEnum.VT_EMPTY };
            return HRESULT.S_OK;
        }

        HRESULT IPropertyStore.SetValue(ref PROPERTYKEY key, PropVariant pv)
        {
            return HRESULT.S_OK;
        }

        HRESULT IPropertyStore.Commit()
        {
            _pstream.Commit((int)HRESULT.S_OK);
            return HRESULT.S_OK;
        }
    }
}
