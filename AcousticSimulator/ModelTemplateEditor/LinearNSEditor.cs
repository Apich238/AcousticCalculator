using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using System.Windows.Forms;

namespace ModelEditor
{
    public partial class LinearNSEditor : Form
    {
        public LinearNSEditor(LinearNoiseSrc src=null)
        {
            InitializeComponent();
            if (src == null)
            {
                res = new LinearNoiseSrc();
                res.name = "new noise source";
                res.power = new noise();
            }
            else res = src;
            
            textBox1.Text = res.name;
            trackBar2.Value = (int)res.power.lvl315.value;
            trackBar3.Value = (int)res.power.lvl63.value;
            trackBar4.Value = (int)res.power.lvl125.value;
            trackBar5.Value = (int)res.power.lvl250.value;
            trackBar6.Value = (int)res.power.lvl500.value;
            trackBar7.Value = (int)res.power.lvl1000.value;
            trackBar8.Value = (int)res.power.lvl2000.value;
            trackBar9.Value = (int)res.power.lvl4000.value;
            trackBar10.Value = (int)res.power.lvl8000.value;
            numericUpDown2.Value = (int)res.power.lvl315.value;
            numericUpDown3.Value = (int)res.power.lvl63.value;
            numericUpDown4.Value = (int)res.power.lvl125.value;
            numericUpDown5.Value = (int)res.power.lvl250.value;
            numericUpDown6.Value = (int)res.power.lvl500.value;
            numericUpDown7.Value = (int)res.power.lvl1000.value;
            numericUpDown8.Value = (int)res.power.lvl2000.value;
            numericUpDown9.Value = (int)res.power.lvl4000.value;
            numericUpDown10.Value = (int)res.power.lvl8000.value;

            numericUpDown12.Value = (decimal)res.A.X;
            numericUpDown11.Value = (decimal)res.A.Y;
            numericUpDown13.Value = (decimal)res.A.Z;

            numericUpDown1.Value = (decimal)res.B.X;
            numericUpDown14.Value = (decimal)res.B.Y;
            numericUpDown15.Value = (decimal)res.B.Z;

            

        }

        public LinearNoiseSrc res;

        private void textBox1_TextChanged(Object sender, EventArgs e)
        {
            res.name = textBox1.Text;
        }

        private void trackBar2_Scroll(Object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
            res.power.lvl315.value = trackBar2.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar3_Scroll(Object sender, EventArgs e)
        {
            numericUpDown3.Value = trackBar3.Value;
            res.power.lvl63.value = trackBar3.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar4_Scroll(Object sender, EventArgs e)
        {
            numericUpDown4.Value = trackBar4.Value;
            res.power.lvl125.value = trackBar4.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar5_Scroll(Object sender, EventArgs e)
        {
            numericUpDown5.Value = trackBar5.Value;
            res.power.lvl250.value = trackBar5.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar6_Scroll(Object sender, EventArgs e)
        {
            numericUpDown6.Value = trackBar6.Value;
            res.power.lvl500.value = trackBar6.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar7_Scroll(Object sender, EventArgs e)
        {
            numericUpDown7.Value = trackBar7.Value;
            res.power.lvl1000.value = trackBar7.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar8_Scroll(Object sender, EventArgs e)
        {
            numericUpDown8.Value = trackBar8.Value;
            res.power.lvl2000.value = trackBar8.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar9_Scroll(Object sender, EventArgs e)
        {
            numericUpDown9.Value = trackBar9.Value;
            res.power.lvl4000.value = trackBar9.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void trackBar10_Scroll(Object sender, EventArgs e)
        {
            numericUpDown10.Value = trackBar10.Value;
            res.power.lvl8000.value = trackBar10.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }


        private void numericUpDown2_ValueChanged(Object sender, EventArgs e)
        {

            trackBar2.Value = (int)numericUpDown2.Value;
            res.power.lvl315.value = trackBar2.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown3_ValueChanged(Object sender, EventArgs e)
        {

            trackBar3.Value = (int)numericUpDown3.Value;
            res.power.lvl63.value = trackBar3.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown4_ValueChanged(Object sender, EventArgs e)
        {

            trackBar4.Value = (int)numericUpDown4.Value;
            res.power.lvl125.value = trackBar4.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown5_ValueChanged(Object sender, EventArgs e)
        {

            trackBar5.Value = (int)numericUpDown5.Value;
            res.power.lvl250.value = trackBar5.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown6_ValueChanged(Object sender, EventArgs e)
        {

            trackBar6.Value = (int)numericUpDown6.Value;
            res.power.lvl500.value = trackBar6.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown7_ValueChanged(Object sender, EventArgs e)
        {

            trackBar7.Value = (int)numericUpDown7.Value;
            res.power.lvl1000.value = trackBar7.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown8_ValueChanged(Object sender, EventArgs e)
        {

            trackBar8.Value = (int)numericUpDown8.Value;
            res.power.lvl2000.value = trackBar8.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown9_ValueChanged(Object sender, EventArgs e)
        {

            trackBar9.Value = (int)numericUpDown9.Value;
            res.power.lvl4000.value = trackBar9.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown10_ValueChanged(Object sender, EventArgs e)
        {

            trackBar10.Value = (int)numericUpDown10.Value;
            res.power.lvl8000.value = trackBar10.Value;
            label12.Text = string.Format("Полный уровень по шкале А, dBA={0}", res.power.dBA.value);
        }

        private void numericUpDown12_ValueChanged(Object sender, EventArgs e)
        {
            res.A.X =(double) numericUpDown12.Value;
        }

        private void numericUpDown11_ValueChanged(Object sender, EventArgs e)
        {
            res.A.Y = (double)numericUpDown11.Value;
        }

        private void numericUpDown13_ValueChanged(Object sender, EventArgs e)
        {
            res.A.Z = (double)numericUpDown13.Value;
        }

        private void numericUpDown1_ValueChanged(Object sender, EventArgs e)
        {
            res.B.X = (double)numericUpDown1.Value;
        }

        private void numericUpDown14_ValueChanged(Object sender, EventArgs e)
        {
            res.B.Y = (double)numericUpDown14.Value;
        }

        private void numericUpDown15_ValueChanged(Object sender, EventArgs e)
        {
            res.B.Z = (double)numericUpDown15.Value;
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
