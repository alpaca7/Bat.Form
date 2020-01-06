using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bat.Shell.PostprocessingForm
{
    public partial class SectionAcorForm : Form
    {
        /// <summary>
        /// 核心高度
        /// </summary>
        public double Hcor
        {
            get
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    return double.Parse(textBox1.Text);
                }
                else return 0;
            }
            set { textBox1.Text = value.ToString(); }
        }

        /// <summary>
        /// 核心宽度
        /// </summary>
        public double Bcor
        {
            get
            {
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    return double.Parse(textBox2.Text);
                }
                else return 0;
            }
            set { textBox2.Text = value.ToString(); }
        }

        public SectionAcorForm()
        {
            InitializeComponent();
        }

        private void SteelAreaForm_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
