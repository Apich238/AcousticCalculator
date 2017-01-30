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
    public partial class ReflectorEditor : Form
    {
        public ReflectorEditor(Reflector val=null)
        {
            InitializeComponent();
            if (val == null) res = new Reflector();
            else res = val;

            textBox1.Text = res.name;

            numericUpDown1.Value = (decimal)res.Rectangle.ontop.X;
            numericUpDown2.Value = (decimal)res.Rectangle.ontop.Y;
            numericUpDown3.Value = (decimal)res.Rectangle.ontop.Z;

            numericUpDown4.Value = (decimal)res.Rectangle.bottomleft.X;
            numericUpDown5.Value = (decimal)res.Rectangle.bottomleft.Y;
            numericUpDown6.Value = (decimal)res.Rectangle.bottomleft.Z;

            numericUpDown9.Value = (decimal)res.Rectangle.bottomright.X;
            numericUpDown8.Value = (decimal)res.Rectangle.bottomright.Y;
            numericUpDown7.Value = (decimal)res.Rectangle.bottomright.Z;

            ReflMatColl.init();
            foreach (var p in ReflMatColl.d)
                comboBox1.Items.Add(p.Value);

            comboBox1.SelectedItem = ReflMatColl.d[res.matid];

        }

        public Reflector res;

        private void button1_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void numericUpDown1_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.ontop.X = (double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.ontop.Y = (double)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.ontop.Z = (double)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomleft.X = (double)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomleft.Y = (double)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomleft.Z = (double)numericUpDown6.Value;
        }

        private void numericUpDown9_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomright.X = (double)numericUpDown9.Value;
        }

        private void numericUpDown8_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomright.Y = (double)numericUpDown8.Value;
        }

        private void numericUpDown7_ValueChanged(Object sender, EventArgs e)
        {
            res.Rectangle.bottomright.Z =(double) numericUpDown7.Value;
        }

        private void comboBox1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            res.matid = ReflMatColl.d.First(p => (p.Value == comboBox1.SelectedItem)).Key;
        }

        private void textBox1_TextChanged(Object sender, EventArgs e)
        {
            res.name = textBox1.Text;
        }
    }
}
