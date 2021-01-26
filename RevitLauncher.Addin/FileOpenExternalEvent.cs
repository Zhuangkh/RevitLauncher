using System.IO;
using Autodesk.Revit;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;

namespace RevitLauncher.Addin
{
    public class FileOpenExternalEvent : IExternalEventHandler
    {
        public FileOpenExternalEvent()
        {
            ExternalEvent.Create(this);
        }
        public void Execute(UIApplication app)
        {
            if (!string.IsNullOrEmpty(App.Path) && File.Exists(App.Path))
            {
                app.OpenAndActivateDocument(App.Path);
            }

            App.Path = string.Empty;
        }

        public string GetName()
        {
            return "FileOpen Server";
        }

        
    }
}

