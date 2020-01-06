using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bat.Fea.Postprocessing.RtilarSection.CracksChecking;

namespace Bat.Shell.PostprocessingForm
{
    [BatShellFormAttribute("裂缝宽度计算")]
    public partial class CracksCheckingForm : Form
    {
        public CracksCheckingForm()
        {
            InitializeComponent();
            this.Icon = Bat.UI.ProtectionLockHelper.GetGlobalFormIcon();
        }

        private void CracksCheckingForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            batGridView1.InitColoumnTitle(typeof(CracksChecking));
            batGridView1.UpdateView<CracksChecking>(new List<CracksChecking> { GetInitData(new CracksChecking(4500, 2250)) });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label7.Enabled = true;
                textBox8.Enabled = true;
            }
            else
            {
                label7.Enabled = false;
                textBox8.Enabled = false;
            }
        }

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            List<CracksChecking> listRectType = batGridView1.CastDataSource<CracksChecking>().Select(l => GetInitData(l)).ToList();
            batGridView1.UpdateView<CracksChecking>(listRectType);
        }

        /// <summary>
        /// 得到界面数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private CracksChecking GetInitData(CracksChecking item)
        {
            item.m_C1 = double.Parse(textBox1.Text);
            item.m_C2 = double.Parse(textBox2.Text);
            item.m_C3 = double.Parse(textBox3.Text);
            item.m_c = double.Parse(textBox4.Text);
            item.m_d = double.Parse(textBox5.Text);
            item.m_As = double.Parse(textBox6.Text);
            item.m_h = double.Parse(textBox7.Text);
            item.m_b = double.Parse(textBox9.Text);
            item.m_as = double.Parse(textBox10.Text);
            item.m_l0 = double.Parse(textBox8.Text);
            item.m_SectionType = radioButton1.Checked ? 0 : 1;
            item.m_ElementType = comboBox1.SelectedIndex;
            return item;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S > 0) textBox6.Text = dlg.S.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
