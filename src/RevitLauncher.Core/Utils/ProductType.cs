using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitLauncher.Core.Utils
{
    /// <summary>An enumerated type containing the possible Revit product types. </summary>
    /// <since>
    ///    2012
    /// </since>
    public enum ProductType
    {
        /// <summary> Architecture. </summary>
        Architecture,
        /// <summary> Structure. </summary>
        Structure,
        /// <summary> MEP. </summary>
        MEP,
        /// <summary> Revit. </summary>
        Revit,
        /// <summary> Unknown. </summary>
        Unknown
    }
}
