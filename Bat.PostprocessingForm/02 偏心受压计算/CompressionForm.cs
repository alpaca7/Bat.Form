using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bat.Fea.Postprocessing.RtilarSection.AxialCapacity;

namespace Bat.Shell.PostprocessingForm
{
    [BatShellFormAttribute("偏压构件计算")]
    public partial class CompressionForm : Form
    {
        Dictionary<int, Image> _imagelist = new Dictionary<int, Image>();
        bool _isLoaded = false;

        public CompressionForm()
        {
            InitializeComponent();
            this.Icon = Bat.UI.ProtectionLockHelper.GetGlobalFormIcon();
        }

        private void CompressionForm_Load(object sender, EventArgs e)
        {
            if (!_isLoaded)
            {
                _imagelist.Add(0, global::Bat.Shell.Properties.Resources.rec);
                _imagelist.Add(1, global::Bat.Shell.Properties.Resources.box);
                comboBox3.SelectedIndex = 0;
                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 1;
                comboBox4.SelectedIndex = 0;
                pictureBox1.Image = global::Bat.Shell.Properties.Resources.rec;
                batGridView1.InitColoumnTitle(typeof(RectSection));
                batGridView1.UpdateView<RectSection>(new List<RectSection> { GetInitData(new RectSection(4500,2250,1500, 200, 200))});
                _isLoaded = true;
            }
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

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

  

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //矩形截面
            if (comboBox3.SelectedIndex == 0)
            {
                List<RectSection> listRectType = batGridView1.CastDataSource<RectSection>().Select(l => GetInitData(l)).ToList();
                batGridView1.UpdateView<RectSection>(listRectType);
            }
            //箱形截面
            if (comboBox3.SelectedIndex == 1)
            {
                List<BoxSection> listRectType = batGridView1.CastDataSource<BoxSection>().Select(l => GetInitData(l)).ToList();
                batGridView1.UpdateView<BoxSection>(listRectType);
            }
        }
        
    
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S > 0) textBox11.Text = dlg.S.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S > 0) textBox13.Text = dlg.S.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S > 0) textBox15.Text = dlg.S.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var dlg = new Common.SteelAreaForm();
            dlg.ShowDialog();
            if (dlg.S > 0) textBox17.Text = dlg.S.ToString();
        }

        private void batGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

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

        /// <summary>
        /// 从界面输入参数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private RectSection GetInitData(RectSection item)
        {
            //截面尺寸及计算高度
            double height = double.Parse(textBox1.Text);//
            double width = double.Parse(textBox2.Text);//
            double ly0 = double.Parse(textBox5.Text);//
            double lz0 = double.Parse(textBox6.Text);//
            //顺桥向配筋
            double AreaUp = double.Parse(textBox11.Text);
            double OffsetUp = double.Parse(textBox12.Text);
            double AreaDown = double.Parse(textBox13.Text);
            double OffsetDown = double.Parse(textBox14.Text);
            //横桥向配筋
            double AreaLeft = double.Parse(textBox15.Text);
            double OffsetLeft = double.Parse(textBox16.Text);
            double AreaRight = double.Parse(textBox17.Text);
            double OffsetRight = double.Parse(textBox18.Text);
            //截面尺寸及计算高度
            item.Concgrade = GetNumber(comboBox1.Text);
            item.Steelgrade = GetNumber(comboBox2.Text);
            item.SafetyFactor = GetNumber(comboBox4.Text) / 10.0;
            item.m_Width = width;
            item.m_Height = height;
            item.m_Ly0 = ly0;
            item.m_Lz0 = lz0;
            //顺桥向配筋
            item.m_AreaUp = AreaUp;
            item.m_OffsetUp = OffsetUp;
            item.m_AreaDown = AreaDown;
            item.m_OffsetDown = OffsetDown;
            //横桥向配筋
            item.m_AreaLeft = AreaLeft;
            item.m_OffsetLeft = OffsetLeft;
            item.m_AreaRight = AreaRight;
            item.m_OffsetRight = OffsetRight;
            //
            return item;
        }

        /// <summary>
        /// 从界面输入参数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private BoxSection GetInitData(BoxSection item)
        {
            //截面尺寸及计算高度
            double height = double.Parse(textBox1.Text);//
            double width = double.Parse(textBox2.Text);//
            double tw = double.Parse(textBox3.Text);//
            double tf = double.Parse(textBox4.Text);//
            double ly0 = double.Parse(textBox5.Text);//
            double lz0 = double.Parse(textBox6.Text);//
            //顺桥向配筋
            double AreaUp = double.Parse(textBox11.Text);
            double OffsetUp = double.Parse(textBox12.Text);
            double AreaDown = double.Parse(textBox13.Text);
            double OffsetDown = double.Parse(textBox14.Text);
            //横桥向配筋
            double AreaLeft = double.Parse(textBox15.Text);
            double OffsetLeft = double.Parse(textBox16.Text);
            double AreaRight = double.Parse(textBox17.Text);
            double OffsetRight = double.Parse(textBox18.Text);
            //截面尺寸及计算高度
            item.Concgrade = GetNumber(comboBox1.Text);
            item.Steelgrade = GetNumber(comboBox2.Text);
            item.SafetyFactor = GetNumber(comboBox4.Text) / 10.0;
            item.m_Width = width;
            item.m_Height = height;
            item.m_Tw = tw;
            item.m_Tf = tf;
            item.m_Ly0 = ly0;
            item.m_Lz0 = lz0;
            //顺桥向配筋
            item.m_AreaUp = AreaUp;
            item.m_OffsetUp = OffsetUp;
            item.m_AreaDown = AreaDown;
            item.m_OffsetDown = OffsetDown;
            //横桥向配筋
            item.m_AreaLeft = AreaLeft;
            item.m_OffsetLeft = OffsetLeft;
            item.m_AreaRight = AreaRight;
            item.m_OffsetRight = OffsetRight;
            //
            return item;
        }
    }
}
