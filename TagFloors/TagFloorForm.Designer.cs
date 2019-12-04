namespace TagFloors
{
    partial class TagFloorForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btn_MarkFloor = new System.Windows.Forms.Button();
            this.btn_AddMark = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.WriteData = new System.Windows.Forms.Button();
            this.db_Path = new System.Windows.Forms.TextBox();
            this.OpenDBBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ParamTrans = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.copyParamBtn = new System.Windows.Forms.Button();
            this.RevitConnectParam = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.DBConnectParam = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.isExistCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Param2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Param1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.IsSettingColor = new System.Windows.Forms.CheckBox();
            this.SettingBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.IsWriteFloorArea = new System.Windows.Forms.CheckBox();
            this.WriteParms = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textPara2 = new System.Windows.Forms.TextBox();
            this.textPara1 = new System.Windows.Forms.TextBox();
            this.LevelTxt = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Floorreappearing = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.Floordetect = new System.Windows.Forms.Button();
            this.OpenExcelBn = new System.Windows.Forms.Button();
            this.excel_Path = new System.Windows.Forms.TextBox();
            this.WriteDatabyExcel = new System.Windows.Forms.Button();
            this.SystemIDBtn = new System.Windows.Forms.Button();
            this.IDText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ParamtersText = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.DestText = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ParamCombineBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(83, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "楼板数据信息写入及标记控件";
            // 
            // btn_MarkFloor
            // 
            this.btn_MarkFloor.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_MarkFloor.Location = new System.Drawing.Point(312, 4);
            this.btn_MarkFloor.Name = "btn_MarkFloor";
            this.btn_MarkFloor.Size = new System.Drawing.Size(151, 31);
            this.btn_MarkFloor.TabIndex = 11;
            this.btn_MarkFloor.Text = "标记楼板序号";
            this.btn_MarkFloor.UseVisualStyleBackColor = false;
            this.btn_MarkFloor.Click += new System.EventHandler(this.btn_MarkFloor_Click);
            // 
            // btn_AddMark
            // 
            this.btn_AddMark.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_AddMark.Location = new System.Drawing.Point(159, 4);
            this.btn_AddMark.Name = "btn_AddMark";
            this.btn_AddMark.Size = new System.Drawing.Size(152, 31);
            this.btn_AddMark.TabIndex = 12;
            this.btn_AddMark.Text = "增加楼板序号";
            this.btn_AddMark.UseVisualStyleBackColor = false;
            this.btn_AddMark.Click += new System.EventHandler(this.btn_AddMark_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_MarkFloor);
            this.panel1.Controls.Add(this.btn_AddMark);
            this.panel1.Location = new System.Drawing.Point(2, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 41);
            this.panel1.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(9, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "创建楼板序号：";
            // 
            // WriteData
            // 
            this.WriteData.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.WriteData.Location = new System.Drawing.Point(239, 148);
            this.WriteData.Name = "WriteData";
            this.WriteData.Size = new System.Drawing.Size(224, 35);
            this.WriteData.TabIndex = 5;
            this.WriteData.Text = "数据库根据楼板序号写入相关信息";
            this.WriteData.UseVisualStyleBackColor = false;
            this.WriteData.Click += new System.EventHandler(this.WriteData_Click);
            // 
            // db_Path
            // 
            this.db_Path.Location = new System.Drawing.Point(111, 14);
            this.db_Path.Name = "db_Path";
            this.db_Path.Size = new System.Drawing.Size(270, 21);
            this.db_Path.TabIndex = 6;
            this.db_Path.TextChanged += new System.EventHandler(this.db_Path_TextChanged);
            // 
            // OpenDBBtn
            // 
            this.OpenDBBtn.Location = new System.Drawing.Point(387, 12);
            this.OpenDBBtn.Name = "OpenDBBtn";
            this.OpenDBBtn.Size = new System.Drawing.Size(75, 23);
            this.OpenDBBtn.TabIndex = 7;
            this.OpenDBBtn.Text = "打开数据库";
            this.OpenDBBtn.UseVisualStyleBackColor = true;
            this.OpenDBBtn.Click += new System.EventHandler(this.OpenDBBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "数据库的路径：";
            // 
            // ParamTrans
            // 
            this.ParamTrans.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ParamTrans.Location = new System.Drawing.Point(239, 346);
            this.ParamTrans.Name = "ParamTrans";
            this.ParamTrans.Size = new System.Drawing.Size(224, 35);
            this.ParamTrans.TabIndex = 26;
            this.ParamTrans.Text = "参数转移";
            this.ParamTrans.UseVisualStyleBackColor = false;
            this.ParamTrans.Click += new System.EventHandler(this.ParamTrans_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(9, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "数据库的表名：";
            // 
            // tableName
            // 
            this.tableName.Location = new System.Drawing.Point(111, 40);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(270, 21);
            this.tableName.TabIndex = 10;
            this.tableName.Text = "总";
            this.tableName.TextChanged += new System.EventHandler(this.tableName_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ParamCombineBtn);
            this.panel2.Controls.Add(this.DestText);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.ParamtersText);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.IDText);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.SystemIDBtn);
            this.panel2.Controls.Add(this.copyParamBtn);
            this.panel2.Controls.Add(this.RevitConnectParam);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.DBConnectParam);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.isExistCheckBox);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.Param2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.Param1);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.IsSettingColor);
            this.panel2.Controls.Add(this.SettingBtn);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.IsWriteFloorArea);
            this.panel2.Controls.Add(this.WriteParms);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.textPara2);
            this.panel2.Controls.Add(this.textPara1);
            this.panel2.Controls.Add(this.LevelTxt);
            this.panel2.Controls.Add(this.tableName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.Floorreappearing);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.Floordetect);
            this.panel2.Controls.Add(this.ParamTrans);
            this.panel2.Controls.Add(this.OpenExcelBn);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.excel_Path);
            this.panel2.Controls.Add(this.OpenDBBtn);
            this.panel2.Controls.Add(this.WriteDatabyExcel);
            this.panel2.Controls.Add(this.db_Path);
            this.panel2.Controls.Add(this.WriteData);
            this.panel2.Location = new System.Drawing.Point(2, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(471, 546);
            this.panel2.TabIndex = 14;
            // 
            // copyParamBtn
            // 
            this.copyParamBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.copyParamBtn.Location = new System.Drawing.Point(6, 346);
            this.copyParamBtn.Name = "copyParamBtn";
            this.copyParamBtn.Size = new System.Drawing.Size(224, 35);
            this.copyParamBtn.TabIndex = 49;
            this.copyParamBtn.Text = "参数复制";
            this.copyParamBtn.UseVisualStyleBackColor = false;
            this.copyParamBtn.Click += new System.EventHandler(this.copyParamBtn_Click);
            // 
            // RevitConnectParam
            // 
            this.RevitConnectParam.Location = new System.Drawing.Point(344, 93);
            this.RevitConnectParam.Name = "RevitConnectParam";
            this.RevitConnectParam.Size = new System.Drawing.Size(121, 21);
            this.RevitConnectParam.TabIndex = 48;
            this.RevitConnectParam.Text = "标记";
            this.RevitConnectParam.TextChanged += new System.EventHandler(this.RevitConnectParam_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(225, 97);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(113, 12);
            this.label17.TabIndex = 47;
            this.label17.Text = "Revit参数(分号):";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(387, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 46;
            this.label16.Text = "（分号隔开）";
            // 
            // DBConnectParam
            // 
            this.DBConnectParam.Location = new System.Drawing.Point(110, 94);
            this.DBConnectParam.Name = "DBConnectParam";
            this.DBConnectParam.Size = new System.Drawing.Size(107, 21);
            this.DBConnectParam.TabIndex = 45;
            this.DBConnectParam.Text = "序号";
            this.DBConnectParam.TextChanged += new System.EventHandler(this.DBConnectParam_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(8, 97);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 12);
            this.label15.TabIndex = 44;
            this.label15.Text = "DB参数(唯一)：";
            // 
            // isExistCheckBox
            // 
            this.isExistCheckBox.AutoSize = true;
            this.isExistCheckBox.Location = new System.Drawing.Point(326, 315);
            this.isExistCheckBox.Name = "isExistCheckBox";
            this.isExistCheckBox.Size = new System.Drawing.Size(120, 16);
            this.isExistCheckBox.TabIndex = 43;
            this.isExistCheckBox.Text = "存在数据则不转移";
            this.isExistCheckBox.UseVisualStyleBackColor = true;
            this.isExistCheckBox.CheckedChanged += new System.EventHandler(this.isExistCheckBox_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Location = new System.Drawing.Point(6, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 35);
            this.button1.TabIndex = 41;
            this.button1.Text = "标记楼板信息";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Param2
            // 
            this.Param2.Location = new System.Drawing.Point(221, 313);
            this.Param2.Name = "Param2";
            this.Param2.Size = new System.Drawing.Size(99, 21);
            this.Param2.TabIndex = 40;
            this.Param2.TextChanged += new System.EventHandler(this.Param2_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(172, 319);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 12);
            this.label5.TabIndex = 39;
            this.label5.Text = "参数2：";
            // 
            // Param1
            // 
            this.Param1.Location = new System.Drawing.Point(63, 313);
            this.Param1.Name = "Param1";
            this.Param1.Size = new System.Drawing.Size(101, 21);
            this.Param1.TabIndex = 38;
            this.Param1.TextChanged += new System.EventHandler(this.Param1_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(14, 318);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 12);
            this.label9.TabIndex = 35;
            this.label9.Text = "参数1：";
            // 
            // IsSettingColor
            // 
            this.IsSettingColor.AutoSize = true;
            this.IsSettingColor.Location = new System.Drawing.Point(191, 123);
            this.IsSettingColor.Name = "IsSettingColor";
            this.IsSettingColor.Size = new System.Drawing.Size(96, 16);
            this.IsSettingColor.TabIndex = 33;
            this.IsSettingColor.Text = "是否设置颜色";
            this.IsSettingColor.UseVisualStyleBackColor = true;
            this.IsSettingColor.CheckedChanged += new System.EventHandler(this.IsSettingColor_CheckedChanged);
            // 
            // SettingBtn
            // 
            this.SettingBtn.Location = new System.Drawing.Point(302, 119);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(144, 23);
            this.SettingBtn.TabIndex = 32;
            this.SettingBtn.Text = "参数为空时颜色设置";
            this.SettingBtn.UseVisualStyleBackColor = true;
            this.SettingBtn.Click += new System.EventHandler(this.SettingBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(9, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "设置：";
            // 
            // IsWriteFloorArea
            // 
            this.IsWriteFloorArea.AutoSize = true;
            this.IsWriteFloorArea.Location = new System.Drawing.Point(53, 123);
            this.IsWriteFloorArea.Name = "IsWriteFloorArea";
            this.IsWriteFloorArea.Size = new System.Drawing.Size(132, 16);
            this.IsWriteFloorArea.TabIndex = 30;
            this.IsWriteFloorArea.Text = "是否写入\"房间面积\"";
            this.IsWriteFloorArea.UseVisualStyleBackColor = true;
            this.IsWriteFloorArea.CheckedChanged += new System.EventHandler(this.IsWriteFloorArea_CheckedChanged);
            // 
            // WriteParms
            // 
            this.WriteParms.Location = new System.Drawing.Point(111, 67);
            this.WriteParms.Name = "WriteParms";
            this.WriteParms.Size = new System.Drawing.Size(270, 21);
            this.WriteParms.TabIndex = 28;
            this.WriteParms.Text = "科室;用途";
            this.WriteParms.TextChanged += new System.EventHandler(this.WriteParms_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(9, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "写入外部参数：";
            // 
            // textPara2
            // 
            this.textPara2.Location = new System.Drawing.Point(239, 245);
            this.textPara2.Name = "textPara2";
            this.textPara2.Size = new System.Drawing.Size(75, 21);
            this.textPara2.TabIndex = 10;
            this.textPara2.Text = "用途";
            this.textPara2.TextChanged += new System.EventHandler(this.textPara2_TextChanged);
            // 
            // textPara1
            // 
            this.textPara1.Location = new System.Drawing.Point(110, 245);
            this.textPara1.Name = "textPara1";
            this.textPara1.Size = new System.Drawing.Size(75, 21);
            this.textPara1.TabIndex = 10;
            this.textPara1.Text = "科室名称";
            this.textPara1.TextChanged += new System.EventHandler(this.textPara1_TextChanged);
            // 
            // LevelTxt
            // 
            this.LevelTxt.Location = new System.Drawing.Point(111, 218);
            this.LevelTxt.Name = "LevelTxt";
            this.LevelTxt.Size = new System.Drawing.Size(246, 21);
            this.LevelTxt.TabIndex = 10;
            this.LevelTxt.Text = "F006";
            this.LevelTxt.TextChanged += new System.EventHandler(this.LevelTxt_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(8, 248);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 12);
            this.label14.TabIndex = 8;
            this.label14.Text = "对应的参数：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(9, 221);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "修改的楼层：";
            // 
            // Floorreappearing
            // 
            this.Floorreappearing.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Floorreappearing.Location = new System.Drawing.Point(239, 387);
            this.Floorreappearing.Name = "Floorreappearing";
            this.Floorreappearing.Size = new System.Drawing.Size(224, 35);
            this.Floorreappearing.TabIndex = 26;
            this.Floorreappearing.Text = "楼板查重";
            this.Floorreappearing.UseVisualStyleBackColor = false;
            this.Floorreappearing.Click += new System.EventHandler(this.Floorreappearing_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(9, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "Excel的路径：";
            // 
            // Floordetect
            // 
            this.Floordetect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Floordetect.Location = new System.Drawing.Point(6, 387);
            this.Floordetect.Name = "Floordetect";
            this.Floordetect.Size = new System.Drawing.Size(224, 35);
            this.Floordetect.TabIndex = 26;
            this.Floordetect.Text = "楼板标记检测";
            this.Floordetect.UseVisualStyleBackColor = false;
            this.Floordetect.Click += new System.EventHandler(this.Floordetect_Click);
            // 
            // OpenExcelBn
            // 
            this.OpenExcelBn.Location = new System.Drawing.Point(364, 189);
            this.OpenExcelBn.Name = "OpenExcelBn";
            this.OpenExcelBn.Size = new System.Drawing.Size(96, 23);
            this.OpenExcelBn.TabIndex = 7;
            this.OpenExcelBn.Text = "打开Excel表\r\n";
            this.OpenExcelBn.UseVisualStyleBackColor = true;
            this.OpenExcelBn.Click += new System.EventHandler(this.OpenExcelBn_Click);
            // 
            // excel_Path
            // 
            this.excel_Path.Location = new System.Drawing.Point(111, 191);
            this.excel_Path.Name = "excel_Path";
            this.excel_Path.Size = new System.Drawing.Size(246, 21);
            this.excel_Path.TabIndex = 6;
            this.excel_Path.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // WriteDatabyExcel
            // 
            this.WriteDatabyExcel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.WriteDatabyExcel.Location = new System.Drawing.Point(6, 272);
            this.WriteDatabyExcel.Name = "WriteDatabyExcel";
            this.WriteDatabyExcel.Size = new System.Drawing.Size(457, 35);
            this.WriteDatabyExcel.TabIndex = 5;
            this.WriteDatabyExcel.Text = "Excel根据楼板序号写入相关信息";
            this.WriteDatabyExcel.UseVisualStyleBackColor = false;
            this.WriteDatabyExcel.Click += new System.EventHandler(this.WriteDatabyExcel_Click);
            // 
            // SystemIDBtn
            // 
            this.SystemIDBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SystemIDBtn.Location = new System.Drawing.Point(238, 428);
            this.SystemIDBtn.Name = "SystemIDBtn";
            this.SystemIDBtn.Size = new System.Drawing.Size(224, 35);
            this.SystemIDBtn.TabIndex = 50;
            this.SystemIDBtn.Text = "系统ID写入";
            this.SystemIDBtn.UseVisualStyleBackColor = false;
            this.SystemIDBtn.Click += new System.EventHandler(this.SystemIDBtn_Click);
            // 
            // IDText
            // 
            this.IDText.Location = new System.Drawing.Point(63, 436);
            this.IDText.Name = "IDText";
            this.IDText.Size = new System.Drawing.Size(167, 21);
            this.IDText.TabIndex = 52;
            this.IDText.TextChanged += new System.EventHandler(this.IDText_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(14, 441);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 12);
            this.label7.TabIndex = 51;
            this.label7.Text = "参数1：";
            // 
            // ParamtersText
            // 
            this.ParamtersText.Location = new System.Drawing.Point(63, 469);
            this.ParamtersText.Name = "ParamtersText";
            this.ParamtersText.Size = new System.Drawing.Size(167, 21);
            this.ParamtersText.TabIndex = 54;
            this.ParamtersText.TextChanged += new System.EventHandler(this.ParamtersText_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(14, 474);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 12);
            this.label12.TabIndex = 53;
            this.label12.Text = "参数1：";
            // 
            // DestText
            // 
            this.DestText.Location = new System.Drawing.Point(293, 471);
            this.DestText.Name = "DestText";
            this.DestText.Size = new System.Drawing.Size(167, 21);
            this.DestText.TabIndex = 56;
            this.DestText.TextChanged += new System.EventHandler(this.DestText_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(244, 476);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 12);
            this.label13.TabIndex = 55;
            this.label13.Text = "参数2：";
            // 
            // ParamCombineBtn
            // 
            this.ParamCombineBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ParamCombineBtn.Location = new System.Drawing.Point(11, 496);
            this.ParamCombineBtn.Name = "ParamCombineBtn";
            this.ParamCombineBtn.Size = new System.Drawing.Size(449, 35);
            this.ParamCombineBtn.TabIndex = 57;
            this.ParamCombineBtn.Text = "参数组合";
            this.ParamCombineBtn.UseVisualStyleBackColor = false;
            this.ParamCombineBtn.Click += new System.EventHandler(this.ParamCombineBtn_Click);
            // 
            // TagFloorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 648);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "TagFloorForm";
            this.Text = "标记楼板及相关参数";
            this.Load += new System.EventHandler(this.TagFloorForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_MarkFloor;
        private System.Windows.Forms.Button btn_AddMark;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button WriteData;
        private System.Windows.Forms.TextBox db_Path;
        private System.Windows.Forms.Button OpenDBBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ParamTrans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox WriteParms;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox IsWriteFloorArea;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button SettingBtn;
        private System.Windows.Forms.CheckBox IsSettingColor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Param1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button OpenExcelBn;
        private System.Windows.Forms.TextBox excel_Path;
        private System.Windows.Forms.Button WriteDatabyExcel;
        private System.Windows.Forms.TextBox LevelTxt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button Floordetect;
        private System.Windows.Forms.Button Floorreappearing;
        private System.Windows.Forms.TextBox textPara1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textPara2;
        private System.Windows.Forms.TextBox Param2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox isExistCheckBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox DBConnectParam;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox RevitConnectParam;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button copyParamBtn;
        private System.Windows.Forms.Button ParamCombineBtn;
        private System.Windows.Forms.TextBox DestText;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox ParamtersText;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox IDText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SystemIDBtn;
    }
}

