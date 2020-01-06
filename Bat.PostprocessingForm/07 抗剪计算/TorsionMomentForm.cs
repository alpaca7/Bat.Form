using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bat.Fea.Postprocessing.RtilarSection.TorsionalCapacity;

namespace Bat.Shell.PostprocessingForm
{
    public partial class TorsionMomentForm : Form
    {
        Dictionary<int, Image> _imagelist = new Dictionary<int, Image>();
        TorsionMomentMember torsionMomentMember;



        public TorsionMomentForm()
        {
            InitializeComponent();

        }

        private void Form5_5_4_Load(object sender, EventArgs e)
        {
            _imagelist.Add(0, global::Bat.Shell.Properties.Resources.rec);
            _imagelist.Add(1, global::Bat.Shell.Properties.Resources.box);
            comboBox3.SelectedIndex = 0;
            comboBox1.SelectedIndex = 5;
            comboBox2.SelectedIndex = 2;
            pictureBox1.Image = global::Bat.Shell.Properties.Resources.rec;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = _imagelist[comboBox3.SelectedIndex];
            if (comboBox3.SelectedIndex == 0)
            {
                textBox3.Visible = false;
                textBox4.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
            }
            else
            {
                textBox3.Visible = true;
                textBox4.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            torsionMomentMember = new TorsionMomentMember(comboBox1.Text, comboBox2.Text, comboBox3.Text);
            textBox31.Text = torsionMomentMember.fcd.ToString();
            textBox32.Text = torsionMomentMember.ftd.ToString();
            textBox33.Text = torsionMomentMember.εcu.ToString();
            textBox34.Text = torsionMomentMember.β.ToString();
            textBox14.Text = torsionMomentMember.ξb.ToString();
            textBox15.Text = torsionMomentMember.Es.ToString();
            textBox16.Text = torsionMomentMember.fsd.ToString();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S>0) textBox11.Text = dlg.S.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dlg = new SectionAcorForm();
            dlg.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void UpdateForm(TorsionMomentMember data)
        {
            ;
        }
    }
}
