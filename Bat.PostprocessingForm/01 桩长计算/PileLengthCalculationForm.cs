using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bat.UI;

namespace Bat.Shell.PostprocessingForm
{
    [BatShellFormAttribute("桩长计算")]
    public partial class PileLengthCalculationForm : Form
    {
        public class Soillayer
        {
            public double Height = 0;//层厚
            public double Qik = 0;//摩阻力
            public double Fa0 = 0;//端承力
            public double Frk = 0;//饱和抗压强度
            public double Gama = 0;//土重度
            public string Describe;//土层描述

            public Soillayer()
            {
            }

            public Soillayer(double height, double qik, double fa0, double gama, string describe)
            {
                Height = height;//层厚
                Qik = qik;//摩阻力
                Fa0 = fa0;//端承力
                Gama = gama;//土重度
                Describe = describe;//土层描述
            }

            /// <summary>
            /// 得到桩侧阻力
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="u"></param>
            /// <returns></returns>
            public static double GetSumQik(List<Soillayer> list, double len, double u, double depth)
            {
                double ret = 0;
                double lastHeight = 0;
                foreach (var item in list)
                {
                    //总层厚小于埋深
                    if (lastHeight + item.Height < depth)
                    {
                        lastHeight = lastHeight + item.Height;
                        continue;
                    }
                    //总层厚小于埋深小于桩长+埋深
                    else
                    {
                        if (item.Height > len && lastHeight < depth)//整根桩长位于当前层
                        {
                            return 0.5 * u * len * item.Qik;
                        }
                        else
                        {
                            //桩顶至当前层底层的距离
                            if (lastHeight + item.Height < len + depth)
                            {
                                //为第一层
                                if (lastHeight < depth)
                                {
                                    double h0 = lastHeight + item.Height - depth;
                                    ret = ret + (0.5 * u * h0 * item.Qik);
                                }
                                else
                                {
                                    ret = ret + (0.5 * u * item.Height * item.Qik);
                                }
                                lastHeight = lastHeight + item.Height;
                            }
                            else//为最后一层
                            {
                                ret = ret + (0.5 * u * (len + depth - lastHeight) * item.Qik);
                                return ret;
                            }
                        }
                    }
                }
                throw new Exception("钻孔长度不够");
            }

            /// <summary>
            /// 计算桩端力
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="a"></param>
            /// <param name="depth"></param>
            /// <returns></returns>
            public static double GetQr(List<Soillayer> list, double len, double a, double depth, double waterdepth, double k2)
            {
                double gama2 = GetSoilWeight(list, len, depth, waterdepth);
                double m0 = 0.7;
                double gama = 0.65;
                double h = len + depth - waterdepth;
                if (h > 40) h = 40;
                double fa0 = Getfa0(list, len, depth);
                return a * (m0 * gama * (fa0 + k2 * gama2 * (h - 3)));
            }

            /// <summary>
            /// 得到fa0
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="depth"></param>
            /// <returns></returns>
            public static double Getfa0(List<Soillayer> list, double len, double depth)
            {
                double lastHeight = 0;
                foreach (var item in list)
                {
                    if (lastHeight + item.Height < len + depth)
                    {

                        lastHeight = lastHeight + item.Height;
                    }
                    else
                    {
                        return item.Fa0;
                    }
                }
                throw new Exception("计算fa0参数有误");
            }

            /// <summary>
            /// 得到frk
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="depth"></param>
            /// <returns></returns>
            public static double GetFrk(List<Soillayer> list, double len, double depth)
            {
                double lastHeight = 0;
                foreach (var item in list)
                {
                    if (lastHeight + item.Height < len + depth)
                    {

                        lastHeight = lastHeight + item.Height;
                    }
                    else
                    {
                        return item.Frk;
                    }
                }
                throw new Exception("计算frk参数有误");
            }

            /// <summary>
            /// 得到桩端以上土层重度
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="depth"></param>
            /// <param name="waterdepth"></param>
            /// <returns></returns>
            public static double GetSoilWeight(List<Soillayer> list, double len, double depth, double waterdepth)
            {
                double ret = 0;
                double lastHeight = 0;
                foreach (var item in list)
                {
                    //总层厚小于埋深+桩长
                    if (lastHeight + item.Height < len + depth)
                    {
                        //水在当前层层底以下
                        if (lastHeight + item.Height < waterdepth)
                        {
                            ret = ret + item.Height * (item.Gama-10);
                        }
                        //水在当前层层顶以上
                        else if (lastHeight >= waterdepth)
                        {
                            ret = ret + item.Height * item.Gama;
                        }
                        else
                        {
                            ret = ret + (item.Height - (waterdepth - lastHeight)) * (item.Gama - 10);
                            ret = ret + (waterdepth - lastHeight) * item.Gama;
                        }
                        lastHeight = lastHeight + item.Height;
                    }
                    //为最后一层
                    else
                    {
                        //水在当前层层底以下
                        if (lastHeight + item.Height < waterdepth)
                        {
                            ret = ret + (len + depth-lastHeight) * (item.Gama-10);
                        }
                        //水在当前层层顶以上
                        else if (lastHeight > waterdepth)
                        {
                            ret = ret + (len + depth-lastHeight) * item.Gama;
                        }
                        else
                        {
                            if (len + depth < waterdepth)
                            {
                                ret = ret + (len + depth - lastHeight) * (item.Gama - 10);
                            }
                            else
                            {
                                ret = ret + (waterdepth - lastHeight) * (item.Gama - 10);
                                ret = ret + (len + depth - waterdepth) * item.Gama;
                            }
                        }
                        break;
                    }
                }
                ret = ret / (len + depth);//平均重度
                return ret;
            }

            /// <summary>
            /// 得到桩侧阻力
            /// </summary>
            /// <param name="list"></param>
            /// <param name="len"></param>
            /// <param name="u"></param>
            /// <returns></returns>
            public static double GetSumFrk(List<Soillayer> list, double len, double u, double depth)
            {
                double ret = 0;
                double lastHeight = 0;
                double c2 = 0.03 * 0.8;
                foreach (var item in list)
                {
                    //总层厚小于埋深
                    if (lastHeight + item.Height < depth)
                    {
                        lastHeight = lastHeight + item.Height;
                        continue;
                    }
                    //总层厚小于埋深小于桩长+埋深
                    else
                    {
                        if (item.Height > len && lastHeight < depth)//整根桩长位于当前层
                        {
                            return c2 * u * len * item.Frk*1000;
                        }
                        else
                        {
                            //总高大于埋深，但是小于桩长+埋深，为中间层
                            if (lastHeight + item.Height < len + depth)
                            {
                                //第一层
                                if (lastHeight < depth)
                                {
                                    double h0 = lastHeight + item.Height - depth;
                                    ret = ret + (c2 * u * h0 * item.Frk*1000);
                                }
                                //中间层
                                else
                                {
                                    ret = ret + (c2 * u * item.Height * item.Frk*1000);
                                }
                                lastHeight = lastHeight + item.Height;
                            }
                            else//为最后一层
                            {
                                ret = ret + (c2 * u * (len + depth - lastHeight) * item.Frk*1000);
                                return ret;
                            }
                        }
                    }
                }
                throw new Exception("计算侧阻力参数有误");
            }
        }

        public enum PileLengthCalculationType
        {
            摩擦桩 = 1,
            嵌岩桩 = 2
        };

        PileLengthCalculationType mPileLengthCalculationType = PileLengthCalculationType.摩擦桩;

        string mCalculationText = "计算过程无";//计算过程

        public PileLengthCalculationForm()
        {
            InitializeComponent();
            this.Icon = Bat.UI.ProtectionLockHelper.GetGlobalFormIcon();
        }


        private void PileLengthCalculationForm_Load(object sender, EventArgs e)
        {
            //this.CenterToScreen();
            textBox1.Text = "10000";
            textBox2.Text = "25";
            textBox3.Text = "0";
            textBox4.Text = "2.2";
            textBox5.Text = "0";
            List<Soillayer> list = new List<Soillayer>();
            list.Add(new Soillayer(1.2,60,240,19,"粉质黏土"));
            list.Add(new Soillayer(1.8, 100, 400, 19, "碎石土"));
            list.Add(new Soillayer(0.9, 60, 240, 19, "粉质黏土"));
            list.Add(new Soillayer(4.2, 140, 550, 19, "碎石土"));
            list.Add(new Soillayer(19.3, 350, 1500, 23, "中风化灰岩"));
            list.Add(new Soillayer(0.5, 200, 800, 23, "中风化砾岩"));
            UpdateGridView(list);
        }

        /// <summary>
        /// 得到土层
        /// </summary>
        /// <param name="batGridView"></param>
        /// <returns></returns>
        private List<Soillayer> GetSoillayerList(BatGridView batGridView)
        {
            List<Soillayer> ret = new List<Soillayer>();
            for (int i = 0; i < batGridView.Rows.Count; i++)
            {
                DataGridViewRow item = batGridView.Rows[i];
                Soillayer soillayer = new Soillayer();
                //排除最后一行，并且如果钢束名称为空，则删除此钢束
                if (item.Cells[0].Value != null && !string.IsNullOrEmpty(item.Cells[0].Value.ToString()))
                {
                    //如果是新增加的行，还没有与之关联的对象，则new并关联
                    soillayer.Height = double.Parse(item.Cells[0].Value.ToString());
                    soillayer.Qik = double.Parse(item.Cells[1].Value.ToString());
                    soillayer.Fa0 = double.Parse(item.Cells[2].Value.ToString());
                    soillayer.Frk = double.Parse(item.Cells[2].Value.ToString());
                    soillayer.Gama = double.Parse(item.Cells[3].Value.ToString());
                    if (item.Cells[4].Value!=null) soillayer.Describe = item.Cells[4].Value.ToString();
                    ret.Add(soillayer);
                }
            }
            return ret;
        }

        private void UpdateGridView(List<Soillayer> list)
        {
            if (list != null)
            {
                batGridView1.Rows.Clear();
                foreach (var item in list)
                {
                    batGridView1.Rows.Add(item.Height,item.Qik,item.Fa0,item.Gama,item.Describe);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<Soillayer> listSoillayer = GetSoillayerList(this.batGridView1);
                
                if (listSoillayer.Count > 0)
                {
                    double fn = double.Parse(textBox1.Text);
                    double designLength = double.Parse(textBox2.Text);
                    double depth = double.Parse(textBox3.Text);
                    if (listSoillayer.Select(l => l.Height).Sum() < (designLength + depth))
                    {
                        MessageBox.Show("钻孔深度不足", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    double diametra = double.Parse(textBox4.Text);
                    double waterdepth = double.Parse(textBox5.Text);
                    if (mPileLengthCalculationType == PileLengthCalculationType.摩擦桩)
                    {
                        mCalculationText = "按摩擦桩计算：";
                        double f1 = Soillayer.GetSumQik(listSoillayer, designLength, diametra * Math.PI, depth);
                        mCalculationText += "\nΣQikL=" + S0(f1) + "kN";
                        double gama2 = Soillayer.GetSoilWeight(listSoillayer, designLength, depth, waterdepth);
                        mCalculationText += "\n加权土重=" + S1(gama2) + "kN/m3";
                        double m0 = double.Parse(textBox11.Text);
                        double gama = double.Parse(textBox10.Text);
                        double k2 = double.Parse(textBox9.Text);
                        double h = designLength + depth;
                        if (h > 40) h = 40;
                        mCalculationText += "\n桩端埋置深度=" + S1(designLength + depth) + "m";
                        double fa0 = Soillayer.Getfa0(listSoillayer, designLength, depth);
                        mCalculationText += "\nfa0=" + S0(fa0) + "kPa";
                        double f2 = (diametra * diametra * Math.PI / 4) * (m0 * gama * (fa0 + k2 * gama2 * (h - 3)));
                        mCalculationText += "\nApQr=" + S0(f2) + "kN";
                        //桩自重与置换土层的差值
                        double pileW = 25 * designLength * diametra * diametra * Math.PI / 4;
                        mCalculationText += "\n桩自重=" + S0(pileW) + "kN";

                        double soilW = gama2 * (designLength + depth) * diametra * diametra * Math.PI / 4;
                        mCalculationText += "\n置换土重=" + S0(soilW) + "kN";

                        double dw = pileW - soilW;
                        textBox6.Text = S0(fn + dw);
                        textBox7.Text = S0(f1 + f2);
                        textBox8.Text = S1((f1 + f2) / (fn + dw));
                    }
                    else if (mPileLengthCalculationType == PileLengthCalculationType.嵌岩桩)
                    {
                        mCalculationText = "按嵌岩桩计算：";
                        double c2 = double.Parse(textBox13.Text);
                        double c1 = double.Parse(textBox12.Text);
                        mCalculationText += "\n桩端埋置深度=" + S1(designLength + depth) + "m";
                        double frk = Soillayer.GetFrk(listSoillayer, designLength, depth);
                        mCalculationText += "\nfrk=" + S0(frk) + "Mpa";
                        double f1 = c1 * diametra * diametra * Math.PI / 4 * frk * 1000;
                        mCalculationText += "\nc1Apfrk=" + S0(f1);
                        double ls = 0.8;
                        if (frk > 30)
                        {
                            ls = 0.2;
                        }
                        else if (frk > 15)
                        {
                            ls = 0.5;
                        }

                        mCalculationText += "\n侧阻力发挥系数=" + S1(ls);
                        double f2 = Soillayer.GetSumQik(listSoillayer, designLength, diametra * Math.PI, depth) * ls;
                        mCalculationText += "\nΣQikl=" + S0(f2) + "kN";

                        double f3 = Soillayer.GetSumFrk(listSoillayer, designLength, diametra * Math.PI, depth);
                        mCalculationText += "\nΣfrkh=" + S0(f3) + "kN";


                        //桩自重与置换土层的差值
                        double pileW = 25 * designLength * diametra * diametra * Math.PI / 4;
                        mCalculationText += "\n桩自重=" + S0(pileW) + "kN";

                        double g = Soillayer.GetSoilWeight(listSoillayer, designLength, depth, waterdepth);
                        mCalculationText += "\n加权土重=" + S1(g) + "kN/m3";
 
                        double soilW = g * (designLength + depth) * diametra * diametra * Math.PI / 4;
                        mCalculationText += "\n置换土重=" + S0(soilW) + "kN";
                        double dw = pileW - soilW;

                        textBox6.Text = S0(fn + dw);
                        textBox7.Text = S0(f1 + f2 + f3);
                        textBox8.Text = S1((f1 + f2 + f3) / (fn + dw));
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public string S0(double value)
        {
            return string.Format("{0:0}", value);
        }

        public string S1(double value)
        {
            return string.Format("{0:0.00}", value);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            mPileLengthCalculationType = PileLengthCalculationType.摩擦桩;
            batGridView1.Columns[2].HeaderText = "[fa0](kPa)";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            mPileLengthCalculationType = PileLengthCalculationType.嵌岩桩;
            batGridView1.Columns[2].HeaderText = "frk(MPa)";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(mCalculationText, "中间结果", MessageBoxButtons.OK);
        }

        private void PileLengthCalculationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
