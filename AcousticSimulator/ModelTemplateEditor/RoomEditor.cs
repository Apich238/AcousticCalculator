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
    public partial class RoomEditor : Form
    {
        public RoomEditor(Room r=null)
        {
            InitializeComponent();
            res = r;
            if (res == null)
            {
                res = new Room();
                res.name = "new room";
            }

            textBox1.Text = res.name;

        }
        public Room res;
        private void textBox1_TextChanged(Object sender, EventArgs e)
        {
            res.name = textBox1.Text;
        }
        

        private void button1_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
