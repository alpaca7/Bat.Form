using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bat.Shell;

namespace Bat.Shell.PostprocessingForm
{
    [BatShellFormAttribute("风荷载计算")]
    public partial class WindLoadForm : Form
    {
        public WindLoadForm()
        {
            InitializeComponent();
            this.Icon = Bat.UI.ProtectionLockHelper.GetGlobalFormIcon();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = radioButton5.Checked;
            textBox6.Enabled = radioButton5.Checked;
            textBox8.Enabled = radioButton6.Checked;
            textBox9.Enabled = radioButton6.Checked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            double u10 = double.Parse(textBox4.Text);//基本风速
            double kt = double.Parse(textBox1.Text);//地形条件系数
            double alfa0 = double.Parse(textBox2.Text);//地表粗糙度系数，根据表4.2.1取值
            double kf = double.Parse(textBox11.Text);//抗风风险系数
            double gv = double.Parse(textBox7.Text);//静阵系数
            double z = double.Parse(textBox3.Text);//基准高度
            double ch = double.Parse(textBox5.Text);//横向力系数
            double cd = double.Parse(textBox9.Text);//构件阻力系数
            double d = double.Parse(textBox6.Text);//主梁高度
            double an = double.Parse(textBox8.Text);//投影面积
            //根据《公路桥梁抗风设计规范》JTG/T 3360-01-2018 第4.2.6条 
            double kc = 1.174;//根据表4.2.4取值
            if (radioButton2.Checked)
            {
                alfa0 = 0.16;
                kc = 1.0;
            }
            else if (radioButton3.Checked)
            {
                alfa0 = 0.22;
                kc = 0.785;
            }
            else if (radioButton4.Checked)
            {
                alfa0 = 0.30;
                kc = 0.564;
            }
            //构件基准高度Z处的设计基准风速m/s
            double ud = kf * kt * Math.Pow(z / 10.0, alfa0) * kc * u10;
            //第5.2.1 等效静阵风风速m/s
            double ug = gv*ud;
            double fg = 0;
            //第5.3.1 主梁上的等效静阵风荷载
            if (radioButton5.Checked)
            {
                fg = 0.5 * 1.25 * ug * ug * ch * d;
            }
            //第5.4.1 桥墩、桥塔上的等效静阵风荷载
            else if (radioButton6.Checked)
            {
                fg = 0.5 * 1.25 * ug * ug * cd * an;
            }
            textBox10.Text = string.Format("{0:000}", fg);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox11.Enabled = !checkBox1.Checked;
            if (checkBox1.Checked)
            {
                double u10 = double.Parse(textBox4.Text);//基本风速
                if (u10 > 32.6)
                {
                    textBox11.Text = "1.05";
                }
                else if (u10 > 24.5)
                {
                    textBox11.Text = "1.02";
                }
                else
                {
                    textBox11.Text = "1.00";
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = !checkBox2.Checked;
            if (checkBox2.Checked)
            {
                if (radioButton1.Checked)
                {
                    textBox2.Text = "0.12";
                }
                else if (radioButton2.Checked)
                {
                    textBox2.Text = "0.16";
                }
                else if (radioButton3.Checked)
                {
                    textBox2.Text = "0.22";
                }
                else if (radioButton4.Checked)
                {
                    textBox2.Text = "0.30";
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (radioButton1.Checked)
                {
                    textBox2.Text = "0.12";
                }
                else if (radioButton2.Checked)
                {
                    textBox2.Text = "0.16";
                }
                else if (radioButton3.Checked)
                {
                    textBox2.Text = "0.22";
                }
                else if (radioButton4.Checked)
                {
                    textBox2.Text = "0.30";
                }
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                double u10 = double.Parse(textBox4.Text);//基本风速
                if (u10 > 32.6)
                {
                    textBox11.Text = "1.05";
                }
                else if (u10 > 24.5)
                {
                    textBox11.Text = "1.02";
                }
                else
                {
                    textBox11.Text = "1.00";
                }
            }
        }
    }
}
