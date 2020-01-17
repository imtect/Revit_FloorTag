using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.IO;

namespace TagFloors {
    class Ribbon : IExternalApplication {
        public Result OnShutdown(UIControlledApplication application) {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application) {
            RibbonPanel panel = application.CreateRibbonPanel("楼板参数");
            string assemblyPath = Path.Combine(AssemblyDirectory, "TagFloors.dll");
            string iconPath = Path.Combine(AssemblyDirectory, "FloorParamater.jpg");
            PushButtonData buttonData = new PushButtonData("楼板参数", "楼板参数", assemblyPath, "TagFloors.Command");
            buttonData.LargeImage = new BitmapImage(new Uri(iconPath));
            panel.AddItem(buttonData);
            return Result.Succeeded;
        }

        public static string AssemblyDirectory {
            get {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
