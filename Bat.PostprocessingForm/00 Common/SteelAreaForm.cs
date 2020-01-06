using System;
using System.Windows.Forms;

namespace Bat.Shell.PostprocessingForm.Common
{
    public partial class SteelAreaForm : Form
    {
        public double S=0;

        public SteelAreaForm()
        {
            InitializeComponent();
        }

        private void SteelAreaForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
            textBox2.Text ="1";
            this.CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int d = GetNumber(comboBox1.Text);
            S = d * d * Math.PI / 4 * GetNumber(textBox2.Text);
            S = ((int)(S * 10 + 0.5)) / 10;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int GetNumber(string str)
        {
            string s = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    s = s + str[i];
                }
            }
            if (string.IsNullOrEmpty(s)) return 0;
            return int.Parse(s);
        }
    }
}
