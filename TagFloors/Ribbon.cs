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

namespace TagFloors
{
    class Ribbon : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = application.CreateRibbonPanel("楼板参数");
            PushButtonData addParameter = new PushButtonData("楼板参数", "楼板参数", @"E:\TagFloors(1)\TagFloors\TagFloors\bin\Debug\TagFloors.dll", "TagFloors.Command");
            addParameter.LargeImage = new BitmapImage(new Uri(@"E:\TagFloors(1)\TagFloors\TagFloors\addParameter1.jpg"));

           
            panel.AddItem(addParameter);

            return Result.Succeeded;
        }


    }
}
