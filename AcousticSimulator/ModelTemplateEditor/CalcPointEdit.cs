using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelLib;

namespace ModelEditor
{
    public partial class CalcPointEdit : Form
    {
        public CalcPointEdit(Model m,calcpoint cp=null)
        {
            InitializeComponent();
            res = cp;
            if(res==null)
            {
                res = new calcpoint();
                textBox1.Text = "new calculation point";
            }
            textBox1.Text = res.name;
            numericUpDown12.Value = (decimal)res.pos.X;
            numericUpDown11.Value = (decimal)res.pos.Y;
            numericUpDown13.Value = (decimal)res.pos.Z;

            comboBox2.Items.Add("none");
            foreach (var r in m.rooms)
            {
                comboBox2.Items.Add(r);
            }
            if (res.room == null)
                comboBox2.SelectedItem = comboBox2.Items[0];
            else
                comboBox2.SelectedItem = res.room;


        }
        public calcpoint res;


        private void numericUpDown12_ValueChanged(Object sender, EventArgs e)
        {
            res.pos.X = (double)numericUpDown12.Value;
        }

        private void numericUpDown11_ValueChanged(Object sender, EventArgs e)
        {
            res.pos.Y = (double)numericUpDown11.Value;
        }

        private void numericUpDown13_ValueChanged(Object sender, EventArgs e)
        {
            res.pos.Z = (double)numericUpDown13.Value;
        }

        private void textBox1_TextChanged(Object sender, EventArgs e)
        {
            res.name = textBox1.Text;
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void comboBox2_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "none")
                res.room = null;
            else res.room = (Room)comboBox2.SelectedItem;
        }
    }
}