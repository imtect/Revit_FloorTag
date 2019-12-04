using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Reflection;
using System.IO;

namespace TagFloors {

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand {

        #region 变量
        public UIDocument m_document;
        public UIApplication m_application;
        public MtSQLite m_sqlite;
        public Selection m_selection;

        //db
        public string m_sqliteFilePath;
        public string m_tableName;
        public string m_writeParams;
        public string m_dbConnectParams;
        public string m_revitConnectParams;
        public bool m_isWriteFloorArea;
        public bool m_isSettingColor;
        public string[] m_paramCodes;
        private bool m_isReadDataFinished;

        //excel
        public string excelPath;
        public string levelnum;
        public string levelName;
        public string para1txt;
        public string para2txt;

        //paramTransform
        public bool isExistParam;

        public string m_codeName;
        private bool m_isNeedBreak;

        //Color
        public Autodesk.Revit.DB.Color m_floorColor;
        public Autodesk.Revit.DB.Color m_corridorColor;
        public bool m_isClassifyColorByDep;

        Dictionary<string, string> m_floorInfoDic = new Dictionary<string, string>();
        public Dictionary<string, Color> m_paramColor = new Dictionary<string, Color>();

        bool m_isShowDialog = false;

        public string m_ElementIDText;
        public string m_combineParamters;
        public string m_destParamter;
        #endregion

        #region 初始化
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) {
            TagFloorForm m_tagFloorForm = new TagFloorForm(this);
            m_tagFloorForm.StartPosition = FormStartPosition.CenterParent;

            m_application = commandData.Application;
            m_document = m_application.ActiveUIDocument;

            m_selection = m_document.Selection;

            if (DialogResult.OK == m_tagFloorForm.ShowDialog()) {
                return Result.Succeeded;
            } else
                return Result.Failed;
        }

        #endregion

        #region CreateFloorTags
        public void AddFloorTags(Document doc) {
            ICollection<ElementId> selectedIds = m_selection.GetElementIds();

            if (selectedIds == null || selectedIds.Count == 0)
                TaskDialog.Show("Error", "必须选择一个楼板实例！");

            foreach (var item in selectedIds) {
                Element ele = doc.GetElement(item);
                Element level = doc.GetElement(ele.LevelId); //获得选择楼板的LEVEL;
                int tagIndex = GetCurrentLevelMaxFloorTags(level) + 1;
                string Label = RenameLabel(ele, tagIndex);
                CreateOneFloorIndependentTag(doc, ele as Floor, Label);
            }
        }

        private Element GetCurrentLevel(Document doc, string viewName) {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> collections = collector.OfClass(typeof(Level)).ToElements();

            foreach (var level in collections) {
                if (level.Name == viewName)
                    return level;
            }
            return null;
        }

        private List<Element> GetCurrentLevelWithMoreLevel(Document doc, string viewName) {
            List<Element> levels = new List<Element>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> collections = collector.OfClass(typeof(Level)).ToElements();

            foreach (var level in collections) {
                if (level.Name == viewName || level.Name.Contains(viewName))
                    levels.Add(level);
            }
            return levels;
        }


        public void CreateLevelFloorTags(Document doc) {
            Autodesk.Revit.DB.View view = doc.ActiveView;
            Element Level = GetCurrentLevel(doc, view.Name);

            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ElementLevelFilter elementLevelFilter = new ElementLevelFilter(Level.Id);
            FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().WherePasses(elementLevelFilter).ToElements();

            int index = 1;
            foreach (var floor in elementLists) {
                string labelContent = RenameLabel(floor, index);
                CreateOneFloorIndependentTag(doc, floor as Floor, labelContent);
                index++;
            }

            //ICollection<ElementId> elementids = m_selection.GetElementIds();

            //int index = 1;
            //foreach (var floorId in elementids)
            //{
            //    Element floor = doc.GetElement(floorId);
            //    string labelContent = RenameLabel(floor, index);
            //    CreateOneFloorIndependentTag(doc, floor as Floor, labelContent);
            //    index++;
            //}
        }

        //参考 https://stackoverflow.com/questions/25457886/c-sharp-revit-api-createindependenttag-method-failing-to-add-tag-to-ceilings-e
        public IndependentTag CreateOneFloorIndependentTag(Document document, Floor floor, string labelName, bool isMarkFloorInfo = false) {
            Autodesk.Revit.DB.View view = document.ActiveView;

            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagOri = TagOrientation.Horizontal;

            //Revit elements can be located by a point(most family instance),a line(walls, line based components)
            //or sketch(celling, floors etc);
            //Simply answer is to find the boundling of box of the floor and calculate the center of the 
            //if the floor is a large L shape or something the boundling box center may not be over the floor at all
            //need some other algorithm to find the center point;

            //calculate the center of mark
            XYZ centerPoint = new XYZ();
            IndependentTag newTag = null;
            if (floor != null) {
                centerPoint = CalculateCenterOfMark(floor, view);

                newTag = document.Create.NewTag(view, floor, false, tagMode, tagOri, centerPoint);

                if (null == newTag) {
                    throw new Exception("Create IndependentTag Failed!");
                }
                SetTagText(floor, labelName, isMarkFloorInfo);
            }
            return newTag;
        }

        public void FindReappearingFloor(Document doc) {
            FilteredElementCollector floorCol = new FilteredElementCollector(doc);
            ElementCategoryFilter floorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            floorCol.WherePasses(floorFilter);

            Options opt = new Options();
            opt.ComputeReferences = true;
            opt.DetailLevel = ViewDetailLevel.Fine;

            List<string> Origins = new List<string>();


            string reppear = "";
            string str = "此文件中重复的板id分别为：";
            List<double> xlist = new List<double>();
            Dictionary<string, string> idToOrigin = new Dictionary<string, string>();
            int i = 0;
            foreach (var floor in floorCol) {
                GeometryElement geometryFloor = floor.get_Geometry(opt);
                if (geometryFloor != null) {
                    foreach (GeometryObject floorGeoObj in geometryFloor) {
                        Solid floorSolid = floorGeoObj as Solid;
                        if (null != floorSolid) {
                            PlanarFace pf = null;
                            foreach (Face face in floorSolid.Faces) {
                                pf = face as PlanarFace;
                                if (null != pf) {
                                    if (Math.Abs(pf.FaceNormal.X) < 0.01 && Math.Abs(pf.FaceNormal.Y) < 0.01 && pf.FaceNormal.Z < 0) {

                                        double x = Math.Round(pf.Origin.X, 2);
                                        double y = Math.Round(pf.Origin.Y, 2);
                                        double z = Math.Round(pf.Origin.Z, 2);
                                        double area = Math.Round(pf.Area, 2);
                                        string floorOrigin = x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + area.ToString();
                                        xlist.Add(x);

                                        if (Origins.Contains(floorOrigin)) {
                                            foreach (KeyValuePair<string, string> kv in idToOrigin) {
                                                if (kv.Value == floorOrigin) {
                                                    reppear = kv.Key;
                                                }

                                            }
                                            //reppear = floor.Id.ToString();


                                            str += reppear + "&" + floor.Id.ToString() + ",";
                                            i++;
                                        } else {
                                            Origins.Add(floorOrigin);
                                            idToOrigin.Add(floor.Id.ToString(), floorOrigin);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                //TaskDialog.Show("1", floor.ToString());


            }
            if (i == 0) {
                TaskDialog.Show("查找重复楼板", "本项目不存在重复楼板（精确精z度为0.01）");
            } else {
                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Reappearing.txt")) {
                    FileStream fs1 = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Reappearing.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(str);
                    sw.Close();
                    fs1.Close();
                } else {

                    FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Reappearing.txt", FileMode.Open, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(str);
                    sr.Close();
                    fs.Close();
                }
                TaskDialog.Show("查找重复楼板", "已将有重复嫌疑的板id导出至桌面Reappearing文件（精确精度为0.01）");

            }
        }
        private XYZ CalculateCenterOfMark(Floor floor, Autodesk.Revit.DB.View view) {
            BoundingBoxXYZ boundingBoxXYZ = floor.get_BoundingBox(view);
            XYZ min = boundingBoxXYZ.Min;
            XYZ max = boundingBoxXYZ.Max;

            XYZ centerPoint = min + (max - min) / 2;

            Options opt = m_application.Application.Create.NewGeometryOptions();

            List<XYZ> points = GetFloorBoundaryPolygons(floor, opt);

            if (!IsInPolygon(centerPoint, points)) {
                centerPoint = IsInerPoint(points, centerPoint);
            }
            return centerPoint;
        }

        private void SetTagText(Floor floor, string labelName, bool isMarkFloorInfo = false) {
            Parameter foundParam = floor.LookupParameter("标记");
            bool result = foundParam.Set(labelName);
        }

        private string RenameLabel(Element floor, int index) {
            if (floor == null) return string.Empty;

            string level = floor.LookupParameter("楼层").AsString();
            string zone = floor.LookupParameter("分区").AsString();
            if (string.IsNullOrEmpty(zone))
                zone = "A";
            return level + zone + "-" + AddZeroToFloorIndex(index);
        }

        private string AddZeroToFloorIndex(int index) {
            string floorNum = string.Empty;
            switch (index.ToString().Length) {
                case 1:
                    floorNum = "00" + index.ToString();
                    break;
                case 2:
                    floorNum = "0" + index.ToString();
                    break;
                case 3:
                    floorNum = index.ToString();
                    break;
            }
            return floorNum;
        }

        private int GetCurrentLevelMaxFloorTags(Element level) {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ElementLevelFilter elementLevelFilter = new ElementLevelFilter(level.Id);
            FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().WherePasses(elementLevelFilter).ToElements();

            List<int> tagLabels = new List<int>();

            int max = 0;

            foreach (var floor in elementLists) {
                string tagName = floor.LookupParameter("标记").AsString();
                if (!string.IsNullOrEmpty(tagName)) {
                    int current = int.Parse(tagName.Split('-')[1]);
                    if (current > max)
                        max = current;
                }
            }

            TaskDialog.Show("Message", "该层最大标记值为： " + max.ToString());
            return max;
        }

        #endregion

        #region SetDepParameter

        public void WriteDataIntoRevit(Document doc) {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collectors = new FilteredElementCollector(doc);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().ToElements();

            ParseParam(m_writeParams);

            SetRandamColor();

            ReadDataFromDB(m_sqliteFilePath);

            if (m_isReadDataFinished) {
                foreach (var item in elementLists) {
                    SetParameters(item);
                    if (m_isNeedBreak)
                        break;
                }
            }
        }

        void ParseParam(string param) {
            if (string.IsNullOrEmpty(param)) {
                ShowErrorDialog("没有指定写入参数");
                return;
            }

            if (param.Contains(";")) {
                m_paramCodes = param.Split(';');
            } else {
                m_paramCodes = new string[] { param };
            }
        }

        public void ReadDataFromDB(string sqliteFilePath) {

            if (string.IsNullOrEmpty(sqliteFilePath)) {
                ShowErrorDialog("没有给定数据库文件路径");
                return;
            }

            m_sqlite = new MtSQLite(sqliteFilePath);
            JArray jarr = new JArray();

            string quarySql = "SELECT " + m_dbConnectParams + ",";

            for (int i = 0; i < m_paramCodes.Length; i++) {
                if (i != m_paramCodes.Length - 1)
                    quarySql += m_paramCodes[i] + ",";
                else
                    quarySql += m_paramCodes[i] + " From " + "'" + m_tableName + "'";
            }

            jarr = m_sqlite.ExecuteQueryToJarry(quarySql);

            if (jarr == null) {
                ShowErrorDialog("(1)数据库“表名”不存在" + "\n" +
                    "(2)数据库表中不存在“外部参数”对应的字段名" + "\n" +
                    "(3)数据库表中不存在“DB参数”对应的字段名");
                return;
            }

            foreach (var item in jarr) {

                string floorId = item.Value<string>(m_dbConnectParams).Trim();

                string info = string.Empty;

                for (int i = 0; i < m_paramCodes.Length; i++) {
                    if (i != m_paramCodes.Length - 1)
                        info += item.Value<string>(m_paramCodes[i]).Trim() + "*";
                    else
                        info += item.Value<string>(m_paramCodes[i]).Trim();
                }

                if (!m_floorInfoDic.ContainsKey(floorId)) {
                    m_floorInfoDic.Add(floorId, info);
                }
            }

            m_isReadDataFinished = true;
        }


        public void SetParameters(Element ele) {
            if (ele == null) return;

            if (m_isWriteFloorArea) {
                Parameter area = ele.LookupParameter("房间面积");
                string _area = (ele.LookupParameter("面积").AsDouble() / 10.7639104f).ToString("F2") + "m²";
                area.Set(_area);
            }

            string floorNum = string.Empty;
            string[] revitParams = GetRevitConnectParam();

            for (int i = 0; i < revitParams.Length; i++) {
                string paramValue = ReadOneParameter(ele, revitParams[i]);
                if (i != revitParams.Length - 1) {
                    floorNum += paramValue + "-";
                } else {
                    floorNum += paramValue;
                }
            }
            if (!string.IsNullOrEmpty(floorNum)) {
                if (m_floorInfoDic.ContainsKey(floorNum)) {

                    string[] paramValue = m_floorInfoDic[floorNum].Split('*');

                    for (int i = 0; i < m_paramCodes.Length; i++) {
                        string param = m_paramCodes[i].ToString();
                        Parameter parameter = ele.LookupParameter(param);
                        if (parameter == null) {
                            m_isShowDialog = false;
                            ShowErrorDialog("楼板的共享参数中不存在 \"" + param + "\" 参数");
                            m_isNeedBreak = true;
                            break;
                        }
                        if (string.IsNullOrEmpty(paramValue[i])) {
                            if (m_paramColor.ContainsKey(m_paramCodes[i]) && m_isSettingColor)
                                SetOneFloorColor(ele.Id, m_paramColor[m_paramCodes[i]]);
                        }
                        parameter.Set(paramValue[i]);
                    }
                } else {
                    SetOneFloorColor(ele.Id, new Color(255, 0, 0));
                    //m_isShowDialog = false;
                    //ShowErrorDialog("DB文件中参数不存在：" + "\n" +
                    //    " Revit的连接参数中的连接值在DB文件中不存在，标记颜色为红色！");
                }
            } else {
                SetOneFloorColor(ele.Id, new Color(0, 255, 0));
                ShowErrorDialog("Revit使用的连接参数不正确：" + "\n" +
                   "Revit中与数据库的关联的共享参数 \"" + m_revitConnectParams + "\"的参数的值为空，标记颜色为绿色");
            }
        }

        string ReadOneParameter(Element ele, string paramName) {
            if (ele == null || string.IsNullOrEmpty(paramName))
                return string.Empty;
            string paramValue = string.Empty;
            Parameter parameter = ele.LookupParameter(paramName);
            if (parameter == null) {
                ShowErrorDialog("楼板的共享参数中不存在 \"" + paramName + "\" 参数");
            } else {
                paramValue = parameter.AsString();
            }
            return paramValue;
        }


        void ShowErrorDialog(string message) {
            if (!m_isShowDialog) {
                TaskDialog.Show("Error", message);
                m_isShowDialog = true;
            }
        }

        string[] GetRevitConnectParam() {
            if (string.IsNullOrEmpty(m_revitConnectParams)) {
                ShowErrorDialog("没有给定Revit的连接参数");
                return null;
            }
            string[] _params = null;
            if (m_revitConnectParams.Contains(";")) {
                _params = m_revitConnectParams.Split(';');
            } else {
                _params = new string[] { m_revitConnectParams };
            }
            return _params;
        }


        public void SetOneFloorColor(ElementId floorId, Autodesk.Revit.DB.Color color) {
            FilteredElementCollector fillPatternElementFilter = new FilteredElementCollector(m_document.Document);
            fillPatternElementFilter.OfClass(typeof(FillPatternElement));
            FillPatternElement fillPatternElement = fillPatternElementFilter
                .First(f => (f as FillPatternElement).GetFillPattern().IsSolidFill) as FillPatternElement;

            OverrideGraphicSettings OverrideGraphicSettings = new OverrideGraphicSettings();
            OverrideGraphicSettings = m_document.Document.ActiveView.GetElementOverrides(floorId);
            OverrideGraphicSettings.SetProjectionFillPatternId(fillPatternElement.Id);

            OverrideGraphicSettings.SetProjectionFillColor(color);
            m_document.Document.ActiveView.SetElementOverrides(floorId, OverrideGraphicSettings);
        }

        //将修改后的颜色改回去
        public void ResetOneFloorColor(ElementId floorId) {

        }


        public void MarkFloorInfo(Document doc, bool isMoreLevel) {
            Autodesk.Revit.DB.View view = doc.ActiveView;

            List<Element> levels = new List<Element>();

            if (!isMoreLevel) {
                levels.Add(GetCurrentLevel(doc, view.Name));
            } else {
                levels = GetCurrentLevelWithMoreLevel(doc, view.Name);
            }

            IList<Element> totalFloors = new List<Element>();

            foreach (var level in levels) {
                ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                ElementLevelFilter elementLevelFilter = new ElementLevelFilter(level.Id);
                FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
                IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().WherePasses(elementLevelFilter).ToElements();
                totalFloors = totalFloors.Union(elementLists).ToList();
            }

            ParseParam(m_writeParams);

            foreach (var floor in totalFloors) {
                string labelContent = GetParamInfo(floor);
                CreateOneFloorIndependentTag(doc, floor as Floor, labelContent, true);
            }
        }

        string GetParamInfo(Element ele) {
            if (ele == null) return string.Empty;

            string info = string.Empty;

            for (int i = 0; i < m_paramCodes.Length; i++) {

                if (i != m_paramCodes.Length - 1) {
                    info += GetOneParameter(ele, m_paramCodes[i]) + "\n";
                } else {
                    info += GetOneParameter(ele, m_paramCodes[i]);
                }

                //Parameter param = ele.LookupParameter(m_paramCodes[i]);
                //if (param == null) {
                //    ShowErrorDialog("楼板的共享参数中不存在 \"" + m_paramCodes[i] + "\" 参数");
                //} else {
                //    if (i != m_paramCodes.Length - 1) {
                //        info += param.AsString() + "\n";
                //    } else {
                //        info += param.AsString();
                //    }
                //}
            }
            return info;
        }

        void SetRandamColor() {
            foreach (var item in m_paramCodes) {
                if (!m_paramColor.ContainsKey(item)) {
                    Random randam = new Random();
                    m_paramColor.Add(item, new Color((byte)randam.Next(0, 255), (byte)randam.Next(0, 255), (byte)randam.Next(0, 255)));
                }
            }
        }

        public void MarkFloorInfoOneParam(Document doc, bool isMoreLevel) {
            Autodesk.Revit.DB.View view = doc.ActiveView;

            List<Element> levels = new List<Element>();

            if (!isMoreLevel) {
                levels.Add(GetCurrentLevel(doc, view.Name));
            } else {
                levels = GetCurrentLevelWithMoreLevel(doc, view.Name);
            }

            IList<Element> totalFloors = new List<Element>();

            foreach (var level in levels) {
                ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                ElementLevelFilter elementLevelFilter = new ElementLevelFilter(level.Id);
                FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
                IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().WherePasses(elementLevelFilter).ToElements();
                totalFloors = totalFloors.Union(elementLists).ToList();
            }

            foreach (var floor in totalFloors) {
                string number = floor.LookupParameter(m_codeName).AsString();
                string labelContent = number;
                CreateOneFloorIndependentTag(doc, floor as Floor, labelContent, true);
            }

        }

        #endregion

        #region ExcelDataToRevit
        public void WriteDataIntoRevitbyExcel(Document doc) {
            string path = excelPath;
            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            excelApplication.UserControl = false;
            Excel.Workbook excelWorkBook = excelApplication.Workbooks.Open(path, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Autodesk.Revit.DB.View view = doc.ActiveView;
            string viewName = levelName;
            //TaskDialog.Show("1", view.Name);
            Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets[levelName];
            string sheetName = levelName;
            int totalRow = excelWorkSheet.UsedRange.Rows.Count;

            int i;
            Dictionary<string, string> tagToKe = new Dictionary<string, string>();
            Dictionary<string, string> tagToUse = new Dictionary<string, string>();

            Excel.Range para1Range;
            Excel.Range para2Range;
            Excel.Range para3Range;




            string str1 = para1txt;
            string str2 = para2txt;
            TaskDialog.Show("2", str1 + str2);
            para1Range = null;
            para1Range = (excelWorkSheet.UsedRange).Find(str1, Missing.Value, Missing.Value,
                Missing.Value, Excel.XlSearchDirection.xlNext,
                0, Missing.Value, Missing.Value);
            int x = para1Range.Column;
            TaskDialog.Show("1", x.ToString());
            para2Range = (excelWorkSheet.UsedRange).Find(str2, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Excel.XlSearchDirection.xlNext,
                 Missing.Value, Missing.Value);
            int y = para2Range.Column;
            TaskDialog.Show("1", y.ToString());
            para3Range = (excelWorkSheet.UsedRange).Find("房间号", Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Excel.XlSearchDirection.xlNext,
                 Missing.Value, Missing.Value);
            int z = para3Range.Column;


            for (i = 2; i < totalRow; i++) {
                if (excelWorkSheet.Cells[i, z].value2 != "" && excelWorkSheet.Cells[i, z].value != null) {
                    tagToKe.Add(excelWorkSheet.Cells[i, z].value, excelWorkSheet.Cells[i, x].value);
                    tagToUse.Add(excelWorkSheet.Cells[i, z].value, excelWorkSheet.Cells[i, y].value);
                }
            }

            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
            int j = 0;
            //Transaction t = new Transaction(doc, "addParameter");
            //t.Start();
            foreach (var floor in floors) {
                string lvl = floor.LookupParameter("楼层").AsString();
                string tag = floor.LookupParameter("标记").AsString();
                Parameter ke = floor.LookupParameter("科室");
                Parameter use = floor.LookupParameter("用途");

                if (lvl == sheetName) {
                    if (tag != null & tag != "") {
                        j++;
                        foreach (KeyValuePair<string, string> kv in tagToKe) {
                            if (tag == kv.Key) {
                                if (ke != null && kv.Value != null && kv.Value != "") {
                                    ke.Set(kv.Value.ToString());
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> kv in tagToUse) {
                            if (tag == kv.Key && kv.Value != null && kv.Value != "") {
                                if (use != null) {
                                    use.Set(kv.Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region DetectFloor
        public void detectFloors(Document doc) {

            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            string str = "";
            foreach (var e in floors) {
                string tag = e.LookupParameter("标记").AsString();
                if (tag != null) {
                    int taglenValue = tag.Length;
                    string taglenString = taglenValue.ToString();
                    string floorID = e.Id.ToString();

                    //TaskDialog.Show("1",  tag + "-" + floorID);   
                    if (tag.Length != 9) {
                        str += tag + "," + floorID + "    ";
                    }

                }
            }

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/temp.txt")) {
                FileStream fs1 = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/temp.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(str);//开始写入值
                sw.Close();
                fs1.Close();
            } else {
                FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/temp.txt", FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(str);//开始写入值
                sr.Close();
                fs.Close();
            }
            TaskDialog.Show("运行结果", "已将有问题的楼板元素id导出至桌面temp文件中，格式为tag名称，id");
        }
        #endregion

        #region Transfer Param


        public void CopyParam(Document doc, string param1, string param2) {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collectors = new FilteredElementCollector(doc);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().ToElements();

            foreach (var item in elementLists) {
                CopyOneParamToAnother(item, param1, param2);
                if (m_isNeedBreak)
                    break;
            }
        }

        private void CopyOneParamToAnother(Element ele, string param1, string param2) {
            string param1Content = GetOneParameter(ele, param1);

            if (!string.IsNullOrEmpty(param1Content)) {
                SetOneParameter(ele, param2, param1Content);
            }
        }


        public void TransferParam(Document doc, string param1, string param2) {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collectors = new FilteredElementCollector(doc);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().ToElements();

            foreach (var item in elementLists) {
                if (isExistParam) {
                    SetOneParamToAnother_IfNotExist(item, param1, param2);
                } else {
                    SetOneParamToAnother_Total(item, param1, param2);
                }
                if (m_isNeedBreak)
                    break;
            }
        }

        public void SetOneParamToAnother_Total(Element ele, string param1, string param2) {
            string param1Content = GetOneParameter(ele, param1);

            if (!string.IsNullOrEmpty(param1Content)) {
                SetOneParameter(ele, param2, param1Content);
                SetOneParameter(ele, param1, "");
            }
        }

        public void SetOneParamToAnother_IfNotExist(Element ele, string param1, string param2) {
            string param1Content = GetOneParameter(ele, param1);
            string param2Content = GetOneParameter(ele, param2);

            if (!string.IsNullOrEmpty(param1Content) && string.IsNullOrEmpty(param2Content)) {
                SetOneParameter(ele, param2, param1Content);
                SetOneParameter(ele, param1, "");
            }
        }

        string GetOneParameter(Element ele, string param) {
            if (ele == null) return string.Empty;

            string paramValue = string.Empty;

            Parameter parameter = ele.LookupParameter(param);
            if (parameter != null) {
                paramValue = parameter.AsString();

                if (string.IsNullOrEmpty(paramValue)) {
                    if (parameter.Definition.ParameterType == ParameterType.Area) {
                        paramValue = (parameter.AsDouble() / 10.7639104f).ToString("F2") + "m²";
                    } else {
                        paramValue = parameter.AsValueString();
                    }
                }
            } else {
                TaskDialog.Show("Error", "该元素没有共享参数：" + param);
                m_isNeedBreak = true;
            }

            return paramValue;
        }

        bool SetOneParameter(Element ele, string paramName, string paramValue) {
            if (ele == null) return false;

            bool succssed = false;

            Parameter param = ele.LookupParameter(paramName);
            if (param != null && !param.IsReadOnly) {
                succssed = param.Set(paramValue);
                if (!succssed)
                    succssed = param.SetValueString(paramValue);
            } else {
                TaskDialog.Show("Error", "该元素没有共享参数：" + paramName + ",或者该参数不可读写");
                m_isNeedBreak = true;
            }
            return succssed;
        }

        #endregion

        #region GetElementId
        public void SetElementIdToParamter(string paramterName) {
            ElementClassFilter instanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementClassFilter hostFilter = new ElementClassFilter(typeof(HostObject));
            LogicalOrFilter andFilter = new LogicalOrFilter(instanceFilter, hostFilter);

            FilteredElementCollector collector = new FilteredElementCollector(m_document.Document);
            collector.WherePasses(andFilter);

            foreach (var item in collector) {
                Parameter param = item.LookupParameter(paramterName);
                if (param != null && !param.IsReadOnly) {
                    bool b = param.Set(item.Id.ToString());
                    if (!b) param.SetValueString(item.Id.ToString());
                }
            }
        }
        #endregion

        #region ParamterCombine

        public void CombineParameters() {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().ToElements();

            if (string.IsNullOrEmpty(m_combineParamters) || string.IsNullOrEmpty(m_destParamter))
                ShowErrorDialog($"要合并的参数或写入的的目标参数为空！");

            string[] paramNames = m_combineParamters.Split(';');

            foreach (var ele in elementLists) {
                string combinName = string.Empty;

                for (int i = 0; i < paramNames.Length; i++) {
                    var parameter = ele.LookupParameter(paramNames[i]);
                    if (parameter == null) ShowErrorDialog($"楼板{ele.Id}没有该\"{paramNames[i]}\"共享参数!");
                    var paramValue = parameter.AsString();
                    if (paramValue == null) paramValue = parameter.AsValueString();

                    if (i != paramNames.Length - 1) {
                        if (paramNames[i] != "楼层")
                            combinName += paramValue + "-";
                        else {
                            if (m_combineParamters.Contains("分区"))
                                combinName += paramValue;
                            else
                                combinName += paramValue + "-";
                        }
                    } else {
                        combinName += paramValue;
                    }
                }

                var desParam = ele.LookupParameter(m_destParamter);
                if (desParam == null) ShowErrorDialog($"楼板{ele.Id}没有该\"{m_destParamter}\"共享参数!");
                bool b = desParam.Set(combinName);
                if (!b) desParam.SetValueString(combinName);
            }
        }

        #endregion

        #region GetFloorBoundary
        //参考 判断点是否在多边形内部 ： https://blog.csdn.net/u011722133/article/details/52813374
        bool IsInPolygon(XYZ centerPoint, List<XYZ> points) {
            //比较xy,判断centerpoint的xy是否在floor的边界内
            int crossing = 0;

            for (int i = 0; i < points.Count - 1; i++) {
                double slope = (points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X);
                bool cond1 = (points[i].X <= centerPoint.X) && (centerPoint.X < points[i + 1].X);
                bool cond2 = (points[i + 1].X <= centerPoint.X) && (centerPoint.X < points[i].X);
                bool above = (centerPoint.Y < slope * (centerPoint.X - points[i].X) + points[i].Y);
                if ((cond1 || cond2) && above)
                    crossing++;
            }
            return (crossing % 2 != 0);
        }

        List<XYZ> GetFloorBoundaryPolygons(Floor floor, Options opt) {
            List<XYZ> polygons = new List<XYZ>();
            GeometryElement geometryElement = floor.get_Geometry(opt);
            foreach (GeometryObject obj in geometryElement) {
                Solid solid = obj as Solid;
                if (null != solid) {
                    GetBoundary(floor, polygons, solid);
                }
            }
            return polygons;
        }

        bool GetBoundary(Floor floor, List<XYZ> ploygons, Solid solid) {
            PlanarFace heightest = null;
            FaceArray faceArrays = solid.Faces;
            if (faceArrays != null && faceArrays.Size != 0) {
                heightest = faceArrays.get_Item(0) as PlanarFace;
            } else
                TaskDialog.Show("Error", "This floor has no face!" + floor.Id.ToString());
            foreach (Face face in faceArrays) {
                //比较表面原点的Z轴确定最高点
                PlanarFace pf = face as PlanarFace;
                if (null != pf && IsHorizontal(pf)) {
                    if (null == heightest && pf.Origin.Z > heightest.Origin.Z) {
                        heightest = pf;
                    }
                }
            }

            if (null != heightest) {
                EdgeArrayArray loops = heightest.EdgeLoops;
                foreach (EdgeArray loop in loops) {
                    foreach (Edge edge in loop) {
                        IList<XYZ> points = edge.Tessellate();
                        foreach (var point in points) {
                            bool isEqual = false;
                            foreach (var item in ploygons) //去除相同的顶点
                            {
                                isEqual = IsEqualXYZ(item, point);
                            }
                            if (!isEqual)
                                ploygons.Add(ResetPoint(point));
                        }
                    }
                }
            }
            return null != heightest;
        }

        bool IsHorizontal(PlanarFace pf) {
            XYZ up = new XYZ(0, 1, 0);
            if (pf.FaceNormal.DotProduct(up) == 0)
                return false;
            else
                return true;
        }

        XYZ ResetPoint(XYZ point) {
            double x = double.Parse(point.X.ToString("F2"));
            double y = double.Parse(point.Y.ToString("F2"));
            double z = double.Parse(point.Z.ToString("F2"));
            return new XYZ(x, y, z);
        }

        bool IsEqualXYZ(XYZ point1, XYZ point2) {
            if ((int)point1.X == (int)point2.X && (int)point1.Y == (int)point2.Y && (int)point1.Z == (int)point1.Z)
                return true;
            else
                return false;
        }

        #region 内心算法
        //简单的内心算法，从中心点出发绘制两条垂直的线，垂直线与两条边相交，记录相交点

        XYZ IsInerPoint(List<XYZ> polygons, XYZ centerPoint) {
            //过centerPoint做垂直水平方向上的直线，计算两条直线与其他相邻顶点的相交点；
            //并分别计算同一侧相邻点的距离，距离最大的，取其中心点作为CenterPoint
            XYZ horizontal = new XYZ(0, 1, -centerPoint.Y);
            XYZ vertical = new XYZ(1, 0, -centerPoint.X);

            //horizontal
            List<XYZ> hori_interset_points = intersectPoints(polygons, horizontal);
            XYZ hor_point1 = new XYZ();
            XYZ hor_point2 = new XYZ();
            GetMaxLengthPoints(hori_interset_points, centerPoint, out hor_point1, out hor_point2);


            //vertical 
            List<XYZ> vertical_interset_points = intersectPoints(polygons, vertical);
            XYZ ver_point1 = new XYZ();
            XYZ ver_point2 = new XYZ();
            GetMaxLengthPoints(vertical_interset_points, centerPoint, out ver_point1, out ver_point2);

            if (GetLength(hor_point1, hor_point2) > GetLength(ver_point1, ver_point2)) {
                return (hor_point1 + hor_point2) / 2;
            } else {
                return (ver_point1 + ver_point2) / 2;
            }
        }

        List<XYZ> intersectPoints(List<XYZ> polygons, XYZ lineParas) {
            if (polygons == null && polygons.Count == 0) return null;
            XYZ axis = new XYZ(lineParas.Y, lineParas.X, 0);
            List<XYZ> intersectpoints = new List<XYZ>();
            for (int i = 0; i < polygons.Count - 1; i++) {
                //计算交点时，只计算与水平方向和垂直方向接近垂直的两个点组成的直线；即两个向量的夹角大于60小于120
                XYZ vector1 = (polygons[i] - polygons[i + 1]).Normalize();
                double cosValue = vector1.DotProduct(axis);
                if (cosValue > -0.5f && cosValue < 0.5f) {
                    XYZ temppoint = Intersect(polygons[i], polygons[i + 1], lineParas);
                    if (temppoint != null)
                        intersectpoints.Add(temppoint);
                }
            }
            return intersectpoints;
        }

        XYZ Intersect(XYZ point1, XYZ point2, XYZ lineParas) {
            double A1 = point2.Y - point1.Y;
            double B1 = point1.X - point2.X;
            double C1 = point2.X * point1.Y - point1.X * point2.Y;

            double A2 = lineParas.X;
            double B2 = lineParas.Y;
            double C2 = lineParas.Z;

            double x = 0, y = 0;
            double temp = A1 * B2 - A2 * B1;
            if (temp != 0) {
                x = (B1 * C2 - B2 * C1) / temp;
                y = (A1 * C2 - A2 * C1) / -temp;
            }

            XYZ intersectPoint = new XYZ(x, y, point1.Z);

            if (IsOnSegment(point1, point2, intersectPoint)) //是否在线段上
                return intersectPoint;
            else
                return null;
        }

        bool IsOnSegment(XYZ p1, XYZ p2, XYZ p) {
            int maxX = p1.X >= p2.X ? (int)p1.X : (int)p2.X;
            int minX = p1.X <= p2.X ? (int)p1.X : (int)p2.X;
            int maxY = p1.Y >= p2.Y ? (int)p1.Y : (int)p2.Y;
            int minY = p1.Y <= p2.Y ? (int)p1.Y : (int)p2.Y;
            if ((int)p.X >= minX && (int)p.X <= maxX &&
                (int)p.Y >= minY && (int)p.Y <= maxY)
                return true;
            else
                return false;
        }

        void GetMaxLengthPoints(List<XYZ> intersectPoints, XYZ lineParas, out XYZ point1, out XYZ point2) {
            double maxLength = 0;
            XYZ temp1 = new XYZ(0, 0, 0);
            XYZ temp2 = new XYZ(0, 0, 0);
            for (int i = 0; i < intersectPoints.Count - 1; i++) {
                if (IsInSameSide(intersectPoints[i], intersectPoints[i + 1], lineParas)) {
                    double length = GetLength(intersectPoints[i], intersectPoints[i + 1]);
                    if (maxLength > length)
                        continue;
                    else {
                        maxLength = length;
                        temp1 = intersectPoints[i];
                        temp2 = intersectPoints[i + 1];
                    }
                }
            }
            point1 = temp1;
            point2 = temp2;
        }

        double GetLength(XYZ point1, XYZ point2) {
            return Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
        }

        bool IsInSameSide(XYZ point1, XYZ point2, XYZ lineparas) {
            if (point1.Y == point2.Y && (point1.X <= lineparas.X && point2.X <= lineparas.X) ||
                point1.Y == point2.Y && (point1.X >= lineparas.X && point2.X >= lineparas.X) ||
                point1.X == point2.X && (point1.Y <= lineparas.Y && point2.Y <= lineparas.Y) ||
                point1.X == point2.X && (point1.Y >= lineparas.Y && point2.Y >= lineparas.Y)) {
                return true;
            } else
                return false;
        }

        #endregion

        #region Obsolete
        //参考 求任意多边形内点的算法 https://blog.csdn.net/yujinqiong/article/details/4465910
        //定理1：每个多边形至少有一个凸顶点
        //定理2：顶点数>=4的简单多边形至少有一条对角线
        //结论： x坐标最大，最小的点肯定是凸顶点
        //y坐标最大，最小的点肯定是凸顶点

        XYZ ResetTagCenterPoint(List<XYZ> points) {
            XYZ lowestPoint, leftPoint, rightPoint;  //三个点组成多边形
            int pointsCount = points.Count;
            lowestPoint = points[0];
            int index = 0;
            for (int i = 1; i < pointsCount - 1; i++) {
                if (points[i].Y < lowestPoint.Y)  //最低点一定是个凸点
                {
                    lowestPoint = points[i];
                    index++;
                }
            }
            leftPoint = points[(index - 1 + pointsCount) % pointsCount];
            rightPoint = points[(index + 1) % pointsCount];


            XYZ[] tri = new XYZ[3];
            XYZ q = new XYZ();
            tri[0] = lowestPoint; tri[1] = leftPoint; tri[2] = rightPoint;
            double md = 1000000f;
            int in1 = index;
            bool bin = false;

            for (int i = 0; i < pointsCount; i++)                                 //寻找在三角形avb内且离顶点v最近的顶点q   
            {
                if (i == index) continue;
                if (i == (index - 1 + pointsCount) % pointsCount) continue;
                if (i == (index + 1) % pointsCount) continue;
                if (!InsideConvexPolygon(3, tri, points[i])) continue;
                bin = true;
                if ((lowestPoint - points[i]).GetLength() < md) {
                    q = points[i];
                    md = (lowestPoint - q).GetLength();
                }
            }
            if (!bin)                                                         //没有顶点在三角形avb内，返回线段ab中点   
            {
                double x1 = (leftPoint.X + rightPoint.X) / 2;
                double y1 = (leftPoint.Y + rightPoint.Y) / 2;
                return new XYZ(x1, y1, lowestPoint.Z);
            }

            double x = (lowestPoint.X + q.X) / 2;
            double y = (lowestPoint.Y + q.Y) / 2;
            return new XYZ(x, y, lowestPoint.Z);
        }

        bool InsideConvexPolygon(int vcount, XYZ[] polygon, XYZ q) //   可用于三角形！   
        {
            double x = 0, y = 0, z = 0;
            XYZ m, n;
            int i;
            for (i = 0; i < vcount; i++)         //   寻找一个肯定在多边形polygon内的点p：多边形顶点平均值   
            {
                x += polygon[i].X;
                y += polygon[i].Y;
                z = polygon[i].Z;
            }
            x /= vcount;
            y /= vcount;
            XYZ p = new XYZ(x, y, z);

            for (i = 0; i < vcount; i++) {
                m = polygon[i]; n = polygon[(i + 1) % vcount];
                if (multiply(p, m, n) * multiply(q, m, n) < 0)     /*   点p和点q在边l的两侧，说明点q肯定在多边形外         */
                    break;
            }
            return (i == vcount);
        }

        //参考 https://www.cnblogs.com/onegarden/p/5622166.html
        double multiply(XYZ pt1, XYZ pt2, XYZ p0) {
            double mult = (pt1.X - p0.X) * (pt2.Y - p0.Y) - (pt2.X - p0.X) * (pt1.Y - p0.Y);
            return mult;

        }
        #endregion

        #endregion

        #region ChageFloorColor

        public void SetFloorColor(Document doc) {
            FilteredElementCollector fillPatternElementFilter = new FilteredElementCollector(doc);
            fillPatternElementFilter.OfClass(typeof(FillPatternElement));
            //获取实体填充  
            FillPatternElement fillPatternElement = fillPatternElementFilter.First(f => (f as FillPatternElement).GetFillPattern().IsSolidFill) as FillPatternElement;

            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collectors = new FilteredElementCollector(m_document.Document);
            IList<Element> elementLists = collectors.WherePasses(elementCategoryFilter).WhereElementIsNotElementType().ToElements();

            foreach (var item in elementLists) {
                if (m_isClassifyColorByDep) {
                    string name = item.LookupParameter("科室").AsString();

                    if (name != null && m_ColorMappingDic.ContainsKey(name))
                        SetOneFloorColor(doc, fillPatternElement, item.Id, m_ColorMappingDic[name]);
                } else {
                    string name = item.LookupParameter("用途").AsString();
                    if (name != null && (name.Contains("走廊") || name.Equals("走廊") || name.Contains("通道"))) {
                        SetOneFloorColor(doc, fillPatternElement, item.Id, m_corridorColor);
                    } else if (!string.IsNullOrEmpty(name)) {
                        SetOneFloorColor(doc, fillPatternElement, item.Id, m_floorColor);
                    }
                }
            }
        }

        void SetOneFloorColor(Document doc, FillPatternElement fillPatternElement, ElementId floorId, Autodesk.Revit.DB.Color color) {
            OverrideGraphicSettings OverrideGraphicSettings = new OverrideGraphicSettings();
            OverrideGraphicSettings = doc.ActiveView.GetElementOverrides(floorId);
            OverrideGraphicSettings.SetProjectionFillPatternId(fillPatternElement.Id);

            OverrideGraphicSettings.SetProjectionFillColor(color);
            doc.ActiveView.SetElementOverrides(floorId, OverrideGraphicSettings);
        }

        private Dictionary<string, Autodesk.Revit.DB.Color> m_ColorMappingDic = new Dictionary<string, Autodesk.Revit.DB.Color>()
        {
            {"内科",new Color(135,206,250)},
            {"外科",new Color(138,138,186)},
            {"妇产科",new Color(133,193,167)},
            {"儿科",new Color(176,224,230)},
            {"小儿外科",new Color(176,224,230)},
            {"发育儿科",new Color(176,224,230)},
            {"神经内科",new Color(125,170,89)},
            {"感染科",new Color(238,225,202)},
            {"中医科",new Color(178,163,127)},
            {"老年病科",new Color(244,178,152)},
            {"眼科",new Color(242,240,156)},
            {"耳鼻咽喉科",new Color(175,221,175)},
            {"口腔科",new Color(109,160,143)},
            {"肿瘤科",new Color(142,199,237)},
            {"疼痛病房",new Color(115,115,155)},
            {"介入病房",new Color(176,196,222)},
            {"康复中心",new Color(98,173,196)},
            {"内镜诊治中心",new Color(163,209,206)},
            {"宁养病房",new Color(152,165,111)},
            {"睡眠医学中心",new Color(234,220,214)},
            {"其他科室",new Color(232,206,158)},
            {"计算机中心",new Color(239,173,163)},
            {"其他部门",new Color(193,150,120)},
            {"未知",new Color(247,165,129)},
            {"公共区域",new Color(193,193,193)},
            {"总服务台",new Color(114,183,179)},
            {"机电-空调",new Color(130,158,216)},
            {"明喆物业",new Color(88,178,220)},
            {"中国电信",new Color(186,206,175)},
            {"消防监控中心",new Color(141,182,219)},
            {"健身中心",new Color(101,170,101)},
            {"财务处",new Color(234,195,155)},
        };

        #endregion
    }
}