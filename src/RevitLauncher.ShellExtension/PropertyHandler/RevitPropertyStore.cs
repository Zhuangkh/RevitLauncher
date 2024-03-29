﻿using OpenMcdf;
using RevitLauncher.Core;
using RevitLauncher.ShellExtension.Interop;
using RevitLauncher.ShellExtension.Interop.PropSys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace RevitLauncher.ShellExtension.PropertyHandler
{
    [ComVisible(true)]
    [Guid("6542121F-1D79-4A27-9471-F80277DA8535")]
    public class RevitPropertyStore : IPropertyStore, IInitializeWithFile, IPropertyStoreCapabilities
    {
        private static Guid VersionPropdescId = new("{AD36148C-0DA3-4145-ADAD-0DDCCF79D4AE}");
        private static PROPERTYKEY rKey = new PROPERTYKEY(VersionPropdescId, 88);
        private string version = "unknown";

        public void Initialize([In, MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, STGM grfMode)
        {
            var info = new RevitFileInfo(pszFilePath);
            version = info.SavedInVersion;
        }

        public HRESULT IsPropertyWritable(in PROPERTYKEY key)
        {
            if (key == rKey)
            {
                return HRESULT.S_FALSE;
            }
            return HRESULT.S_OK;
        }

        public uint GetCount()
        {
            return 1;
        }

        public PROPERTYKEY GetAt(uint iProp)
        {
            return rKey;
        }

        public void GetValue(in PROPERTYKEY pkey, PROPVARIANT pv)
        {
            if (pkey == rKey)
            {
                PROPVARIANT.InitPropVariantFromString(version, pv);
            }
        }

        public void SetValue(in PROPERTYKEY pkey, PROPVARIANT pv) { }

        public void Commit() { }

    }
}
