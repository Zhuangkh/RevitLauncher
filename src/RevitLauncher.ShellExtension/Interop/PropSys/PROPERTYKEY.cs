﻿using System;
using System.Runtime.InteropServices;

namespace RevitLauncher.ShellExtension.Interop.PropSys
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public readonly struct PROPERTYKEY : IEquatable<PROPERTYKEY>
    {
        public Guid FmtId { get; }
        public int PId { get; }

        public PROPERTYKEY(Guid formatId, int propertyId)
        {
            FmtId = formatId;
            PId = propertyId;
        }

        public static bool operator ==(PROPERTYKEY propKey1, PROPERTYKEY propKey2) => propKey1.Equals(propKey2);

        public static bool operator !=(PROPERTYKEY propKey1, PROPERTYKEY propKey2) => !propKey1.Equals(propKey2);

        public override readonly int GetHashCode() => FmtId.GetHashCode() ^ PId;

        public readonly bool Equals(PROPERTYKEY other) => other.Equals((object)this);

        public override readonly bool Equals(object obj) => obj is PROPERTYKEY other && other.FmtId.Equals(FmtId) && other.PId == PId;

        public override readonly string ToString() => GetCanonicalName() ?? $"{FmtId:B} {PId}";

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HRESULT PSGetNameFromPropertyKey(in PROPERTYKEY propkey, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszCanonicalName);

        public string GetCanonicalName()
        {
            var pk = this;
            return PSGetNameFromPropertyKey(pk, out var str) == HRESULT.S_OK ? str : null;
        }
    }
}
