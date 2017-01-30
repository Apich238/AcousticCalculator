using MaterialsDatabase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialsDatabaseEditor
{
    public partial class EditItem : Form
    {
        public EditItem(MaterialsDBClient cl)
        {
            InitializeComponent();
            def = new Material();
            ResetFields();
            OKButton.Text = "Добавить";
            Text = "Добавление материала";
            mdb = cl;
        }
        private MaterialsDBClient mdb;
        private List<string> its;
        public EditItem(MaterialsDBClient cl, Material DefaultMaterial)
        {
            InitializeComponent();
            def = DefaultMaterial;
            ResetFields();
            OKButton.Text = "Сохранить";
            Text = "Редактирование материала";
            mdb = cl;
        }
        #region PrivateMethods
        private void ResetFields()
        {
            NameTBox.Text = def.Name;
            NRCTBox.Text = def.gAmbientColorInd.ToString();
      //      fillProbe.ForeColor = Color.FromArgb(def.defaultColor);
        }
        private bool CheckFields()
        {
            if (NameTBox.Text.Length == 0)
            {
                MessageBox.Show(this, "Name cannot be empty.", "Wrong value", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                NameTBox.SelectAll();
                NameTBox.Focus();
                return false;
            }
            //if (double.IsNaN(Helpers.OmniParse(NRCTBox.Text)))
            //{
            //    MessageBox.Show(this, "Wrong value for real number.", "Wrong value", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    NRCTBox.SelectAll();
            //    NRCTBox.Focus();
            //    return false;
            //}

            return true;
        }
        #endregion
        #region PrivateFields
        private Material def;
        #endregion
        #region PublicProperties
        public Material Result
        {
            get
            {
                Material m = new Material();
                m.Name = NameTBox.Text;
                m.absorbparam = double.Parse(textBox1.Text);
                m.reflection = double.Parse(NRCTBox.Text);
                m.gAmbientColor = Color.Gray;
                return m;
            }
        }
        #endregion
        #region EventHandlers
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (!CheckFields()) return;
            DialogResult = DialogResult.OK;

            Close();
        }
        private void CancelButton_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void ResetBtn_Click(object sender, EventArgs e) { ResetFields(); }
        #endregion
    }
}