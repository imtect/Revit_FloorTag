using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagFloors
{
    public partial class TagFloorForm : Form
    {
        public Command m_instance;

        //private bool isSameLevel = false;

        private string m_param1;
        private string m_param2;

        public TagFloorForm(Command instance)
        {
            InitializeComponent();
            m_instance = instance;

            Init();
        }

        public void Init()
        {
            m_instance.m_tableName = tableName.Text;
            m_instance.m_codeName = WriteParms.Text;
            m_instance.m_writeParams = WriteParms.Text;
            m_instance.m_dbConnectParams = DBConnectParam.Text.ToString();
            m_instance.m_revitConnectParams = RevitConnectParam.Text.ToString();

            m_param1 = Param1.Text;
            m_param2 = Param2.Text;

            m_instance.m_isWriteFloorArea = IsWriteFloorArea.Checked;
            m_instance.m_isSettingColor = IsSettingColor.Checked;
            m_instance.isExistParam = isExistCheckBox.Checked;

            m_instance.m_floorColor = new Autodesk.Revit.DB.Color(128, 128, 128);
            m_instance.m_corridorColor = new Autodesk.Revit.DB.Color(192, 192, 192);
        }

        private void btn_MarkFloor_Click(object sender, EventArgs e) {
            m_instance.CreateLevelFloorTags(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_AddMark_Click(object sender, EventArgs e) {
            m_instance.AddFloorTags(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OpenDBBtn_Click(object sender, EventArgs e) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择DB文件";
            fileDialog.Filter = "db文件|*.db";
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                string file = fileDialog.FileName;
                db_Path.Text = file;
                m_instance.m_sqliteFilePath = file;
            }
        }

        private void tableName_TextChanged(object sender, EventArgs e) {
            m_instance.m_tableName = tableName.Text.ToString();
        }

        private void WriteParms_TextChanged(object sender, EventArgs e) {
            m_instance.m_writeParams = WriteParms.Text.ToString();
        }

        private void IsWriteFloorArea_CheckedChanged(object sender, EventArgs e) {
            if (IsWriteFloorArea.CheckState == CheckState.Checked)
                m_instance.m_isWriteFloorArea = true;
            else
                m_instance.m_isWriteFloorArea = false;
        }

        private void IsSettingColor_CheckedChanged(object sender, EventArgs e) {
            if (IsSettingColor.CheckState == CheckState.Checked)
                m_instance.m_isSettingColor = true;
            else
                m_instance.m_isSettingColor = false;
        }

        private void SettingBtn_Click(object sender, EventArgs e) {
            SettingForm form = new SettingForm(WriteParms.Text);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK) {
                foreach (var item in form.GetParamSettingColor()) {
                    if (!m_instance.m_paramColor.ContainsKey(item.Key)) {
                        Color dialogColor = item.Value;
                        Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(dialogColor.R,
                        dialogColor.G, dialogColor.B);

                        m_instance.m_paramColor.Add(item.Key, color);
                    }
                }
            }
        }


        private void WriteData_Click(object sender, EventArgs e) {
            m_instance.WriteDataIntoRevit(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Param1_TextChanged(object sender, EventArgs e) {
            m_param1 = Param1.Text;
        }

        private void Param2_TextChanged(object sender, EventArgs e) {
            m_param2 = Param2.Text;
        }




        private void ParamTrans_Click(object sender, EventArgs e) {
            m_instance.TransferParam(m_instance.m_document.Document, m_param1, m_param2);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TagFloorForm_Load(object sender, EventArgs e)
        {
            m_instance.levelName = LevelTxt.Text.ToString();
            m_instance.para1txt = textPara1.Text.ToString();
            m_instance.para2txt = textPara2.Text.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void db_Path_TextChanged(object sender, EventArgs e)
        {

        }

        private void OpenExcelBn_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void WriteDatabyExcel_Click(object sender, EventArgs e)
        {
            
            m_instance.WriteDataIntoRevitbyExcel(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OpenExcelBn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择EXCEL文件";
            fileDialog.Filter = "EXCEL文件|*.xlsx";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                excel_Path.Text = file;
                m_instance.excelPath = file;
            }
        }

        private void LevelTxt_TextChanged(object sender, EventArgs e)
        {
            m_instance.levelName = LevelTxt.Text.ToString();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Floordetect_Click(object sender, EventArgs e)
        {
            m_instance.detectFloors(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Floorreappearing_Click(object sender, EventArgs e)
        {
            m_instance.FindReappearingFloor(m_instance.m_document.Document);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textPara1_TextChanged(object sender, EventArgs e)
        {
            m_instance.para1txt = textPara1.Text.ToString();
        }

        private void textPara2_TextChanged(object sender, EventArgs e)
        {
            m_instance.para2txt = textPara2.Text.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_instance.MarkFloorInfo(m_instance.m_document.Document, false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void isExistCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (isExistCheckBox.CheckState == CheckState.Checked)
                m_instance.isExistParam = true;
            else
                m_instance.isExistParam = false;
        }

        private void DBConnectParam_TextChanged(object sender, EventArgs e) {
            m_instance.m_dbConnectParams = DBConnectParam.Text.ToString();
        }

        private void RevitConnectParam_TextChanged(object sender, EventArgs e) {
            m_instance.m_revitConnectParams = RevitConnectParam.Text.ToString();
        }
    }
}
