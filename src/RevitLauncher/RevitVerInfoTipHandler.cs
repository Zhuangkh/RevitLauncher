using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Attributes;
using SharpShell.SharpInfoTipHandler;

namespace RevitLauncher
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".rvt", ".rte", ".rft", ".rfa")]
    public class RevitVerInfoTipHandler : SharpInfoTipHandler
    {
        protected override string GetInfo(RequestedInfoType infoType, bool singleLine)
        {
            switch (infoType)
            {
                case RequestedInfoType.Name:
                    return Path.GetFileName(SelectedItemPath);
                case RequestedInfoType.InfoTip:
                    return $"Revit Version: {new RevitFileInfo(SelectedItemPath).SavedInVersion}";
                default:
                    return string.Empty;
            }
        }
    }
}
