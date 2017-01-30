using ModelLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace ModelEditor
{
    public partial class BorderEditor : Form
    {
        public BorderEditor(Model m,Border b=null)
        {
            InitializeComponent();
            md = m;
            if (b == null)
            {
                res = new Border();
                res.name = "new border";
            }
            else res = b;



            cbd = new Dictionary<object, int>();
            var ml = m.GetMaterialsDict();
            foreach (var r in ml)
            {
                cbd.Add(r.Value, r.Key);
                ((DataGridViewComboBoxColumn)dg.Columns["Material"]).Items.Add(r.Value);
            }

            foreach (var l in res.layers)
            {
                dg.Rows.Add(ml[l.materialId], l.thickness);
            }



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

            comboBox2.Items.Add("none");
            comboBox3.Items.Add("none");
            foreach (var r in md.rooms)
            {
                comboBox2.Items.Add(r);
                comboBox3.Items.Add(r);
            }
            if (res.room1 == null)
                comboBox2.SelectedItem = comboBox2.Items[0];
            else
                comboBox2.SelectedItem = res.room1;

            if (res.room2 == null)
                comboBox3.SelectedItem = comboBox3.Items[0];
            else
                comboBox3.SelectedItem = res.room2;





        }

        private Dictionary<object, int> cbd;

        private Model md;

        public Border res;

        private void button1_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            res.layers = new List<border_layer>();
            foreach (DataGridViewRow r in dg.Rows)
            {
                try
                {
                    border_layer nl = new border_layer();
                    nl.materialId = cbd[r.Cells["Material"].Value];
                    nl.thickness = double.Parse((r.Cells["Thickness"].Value.ToString()).Replace(',', '.'), NumberFormatInfo.InvariantInfo);
                    res.layers.Add(nl);
                }
                catch (Exception)
                {
                    continue;
                }
            }

        }

        private void button2_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(Object sender, EventArgs e)
        {
            res.name = textBox1.Text;
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
            res.Rectangle.bottomright.Z = (double)numericUpDown7.Value;
        }

        private void comboBox2_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "none")
                res.room1 = null;
            else
                res.room1 = (Room)comboBox2.SelectedItem;
        }

        private void comboBox3_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "none")
                res.room2 = null;
            else
                if (object.ReferenceEquals(res.room1, comboBox3.SelectedItem))
            {
                comboBox3.SelectedItem = comboBox3.Items[0];
                res.room2 = null;
            }
            else
                res.room2 = (Room)comboBox3.SelectedItem;
        }

        private void dg_DataError(Object sender, DataGridViewDataErrorEventArgs e)
        {

        }

 

        private void button3_Click(Object sender, EventArgs e)
        {
            decimal x = numericUpDown4.Value, y = numericUpDown5.Value, z = numericUpDown6.Value;
            numericUpDown4.Value = numericUpDown9.Value;
            numericUpDown5.Value = numericUpDown8.Value;
            numericUpDown6.Value = numericUpDown7.Value;

            numericUpDown9.Value = x;
            numericUpDown8.Value = y;
            numericUpDown7.Value = z;
        }
    }
}
