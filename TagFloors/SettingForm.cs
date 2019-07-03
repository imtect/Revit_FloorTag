using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagFloors {
    public partial class SettingForm : Form {

        string param;

        Dictionary<string, Color> colorDic;

        public SettingForm(string param) {
            InitializeComponent();

            this.param = param;

            Init();
        }

        public void Init() {

            colorDic = new Dictionary<string, Color>();

            this.Size = new Size(250, 186);

            ParseParam();

        }

        void ParseParam() {
            if (string.IsNullOrEmpty(param)) return;

            string[] paramCode;
            if (param.Contains(";"))
                paramCode = param.Split(';');
            else
                paramCode = new string[] { param };

            int index = 0;

            foreach (var item in paramCode) {
                Color color = CreateButtonColor(item, index++);
                if (!colorDic.ContainsKey(param))
                    colorDic.Add(item, color);
            }
        }

        Color CreateButtonColor(string param, int index) {
            Label label = new Label();
            label.Text = param + ":";
            label.Size = new Size(65, 12);
            label.Location = new Point(13, index * 28 + 13);
            this.Controls.Add(label);

            Button button = new Button();
            button.Name = param;
            button.Size = new Size(138, 23);
            button.Location = new Point(84, index * 28 + 8);
            button.BackColor = Color.Gray;

            button.Click += (object sender, EventArgs e) => {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.ShowDialog();
                button.BackColor = colorDialog.Color;
                if (colorDic.ContainsKey(button.Name))
                    colorDic[button.Name] = button.BackColor;
            };

            this.Controls.Add(button);

            return button.BackColor;
        }

        public Dictionary<string, Color> GetParamSettingColor() {
            return colorDic;
        }

        private void button1_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
