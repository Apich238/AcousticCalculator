using MaterialsDatabase;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MaterialsDatabaseEditor
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            try
            {
                c = new ConnectionOptions(XDocument.Load("MDBConnOpts.xml"));
                db=new MaterialsDBClient(c.Account, c.Password, c.Host, c.Port);
            }
            catch (Exception)
            {
                DBLogin d = new DBLogin();
                d.ShowDialog(this);
                if (d.DialogResult == DialogResult.Cancel) db = null;
                else db = d.Client;
            }

        }
        public ConnectionOptions c;
        public MaterialsDBClient db;
        #region PrivateMethods
        #region GetPropsByRowIndex
        private Int32 GetIDByRowIndex(Int32 Index) { return (Int32)DG.Rows[Index].Cells["ID"].Value; }
        private bool GetOutDatedByRowIndex(Int32 Index) { return 1 == (byte)DG.Rows[Index].Cells["OUTDATED"].Value; }
        private Material GetMaterialByRowIndex(Int32 Index)
        {
            return db.GetMaterialByID(GetIDByRowIndex(Index));
        }
        #endregion
        #region GetSelected_Props
        private Int32 GetSelectedRowIndex() { return DG.SelectedRows[0].Index; }
        private Int32 GetSelectedID() { return GetIDByRowIndex(GetSelectedRowIndex()); }
        private bool GetSelectedOutDated() { return GetOutDatedByRowIndex(GetSelectedRowIndex()); }
        private Material GetSelectedMaterial() { return GetMaterialByRowIndex(GetSelectedRowIndex()); }
        #endregion
        #region GetPropsByID
        private Int32 GetRowIndexByID(Int32 ID)
        {
            for (int i = 0; i < DG.Rows.Count; i++) if (GetIDByRowIndex(i) == ID) return i;
            return -1;
        }
        private bool GetDeleteMarkByID(Int32 ID) { return GetOutDatedByRowIndex(GetSelectedRowIndex()); }
        #endregion
        #region ViewControl
        private void upd()
        {
            DG.DataSource = db.Materials;
            UpdateDGColumnsView();
            UpdateDGRowsView();
            UpdateButtons();
        }
        private void UpdateDGColumnsView()
        {
            DG.Columns["OUTDATED"].DisplayIndex = 1;
            DG.Columns["OUTDATED"].HeaderText = "Outdated";
            DG.Columns["OUTDATED"].HeaderCell.ToolTipText = "Marked items are outdated";
            DG.Columns["NAME"].HeaderText = "Name";
            DG.Columns["NAME"].HeaderCell.ToolTipText = "Name of material";
            DG.Columns["AMBIENTCOLOR"].DefaultCellStyle.Format = "X6";
            DG.Columns["AMBIENTCOLOR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DG.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn c in DG.Columns) c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void UpdateDGRowsView()
        {
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                if (GetOutDatedByRowIndex(i)) DG.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                else DG.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                DG.Rows[i].Cells["AMBIENTCOLOR"].Style.ForeColor = Color.FromArgb((Int32)DG.Rows[i].Cells["AMBIENTCOLOR"].Value);
                Color c = Color.FromArgb((Int32)DG.Rows[i].Cells["AMBIENTCOLOR"].Value);
                DG.Rows[i].Cells["AMBIENTCOLOR"].Style.BackColor =
    ((c.R * 5 + c.G * 6 + c.B * 4) > 1500) ? Color.Black : Color.White;
            }
        }
        private void UpdateButtons()
        {
            removeToolStripMenuItem.Enabled = db.UserType == MaterialsDBClient.UserMode.Corrector;
            newToolStripMenuItem.Enabled = db.UserType != MaterialsDBClient.UserMode.Viewer;
            copyToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled =
                outdateswitchToolStripMenuItem.Enabled = (DG.SelectedRows.Count == 1) && newToolStripMenuItem.Enabled;
            if (DG.SelectedRows.Count == 1)
                outdateswitchToolStripMenuItem.Text = GetSelectedOutDated() ? "Set not outdated" : "Set outdated";
        }
        #endregion
        private int SearchName(string searchstr)
        {
            searchstr = searchstr.ToUpperInvariant();
            string nm;
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                nm = GetMaterialByRowIndex(i).Name.ToUpperInvariant();
                if (nm.Length < searchstr.Length) continue;
                if (nm.Substring(0, searchstr.Length) == searchstr) return i;
            }
            return -1;
        }
        #endregion
        #region EventHandlers
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (db != null)
                File.WriteAllText(Application.StartupPath + "//LastLogin.txt",
                db.ServerName + "\n" + db.ServerPort.ToString() + "\n" + db.UserName,
                System.Text.Encoding.Default);
        }
        private bool searchmode;
        private void DG_SelectionChanged(object sender, EventArgs e)
        {
            if (DG.SelectedRows.Count == 0)
                if (DG.Rows.Count > 0) DG.Rows[0].Selected = true;
                else return;
            UpdateButtons();
            int p = GetSelectedRowIndex();
            if (!searchmode) SearchTBox.Text = "";
            DG.Rows[p].Selected = true;
        }
        private void SearchTBox_TextChanged(object sender, EventArgs e)
        {
            searchmode = true;
            if (SearchTBox.Text.Length == 0) { DG.Rows[0].Selected = true; return; }
            int k = SearchName(SearchTBox.Text);
            if (k == -1) DG.Rows[0].Selected = true;
            else DG.Rows[k].Selected = true;
            searchmode = false;
        }
        private void Editor_Load(object sender, EventArgs e) { if (db == null) Close(); else upd(); }
        private void reconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialsDBClient c = db;
            DBLogin log = new DBLogin();
            if (log.ShowDialog(this) != DialogResult.OK) return;
            db = log.Client;
            upd();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditItem ElemEdit = new EditItem(db);
            ElemEdit.ShowDialog(this);
            if (ElemEdit.DialogResult != DialogResult.OK) return;
            Int32 id = 0;
            try
            {
                id = db.AddMaterial(ElemEdit.Result);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, "An error occured when tried to add new material: " + ex.Message, "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DG.Rows[GetRowIndexByID(id)].Selected = true;
            upd();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Material m = GetSelectedMaterial();
            EditItem ElemEdit = new EditItem(db, m);
            ElemEdit.ShowDialog(this);
            if (ElemEdit.DialogResult != DialogResult.OK) return;
            Int32 id = 0;
            try
            {
                id = db.AddMaterial(ElemEdit.Result);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, "An error occured when tried to add new material: " + ex.Message, "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DG.Rows[GetRowIndexByID(id)].Selected = true;
            upd();
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Material m0 = GetSelectedMaterial();
            Int32 id = GetSelectedID();
            EditItem ElemEdit = new EditItem(db, m0);
            ElemEdit.ShowDialog(this);
            try
            {
                if (ElemEdit.DialogResult != System.Windows.Forms.DialogResult.OK) return;
                Material m1 = ElemEdit.Result;
                db.UpdMaterial(id, m1);
                DG.Rows[GetRowIndexByID(id)].Selected = true;
                upd();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, "An error occured when tried to edit material property: " + ex.Message, "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 id = GetSelectedID();
            try
            {
                db.SetMaterialOutDated(id, !GetDeleteMarkByID(id));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, "An error occured when tried to edit material property: " + ex.Message, "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DG.Rows[GetRowIndexByID(id)].Selected = true;
            upd();
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            db.RemoveOutdatedMaterials();
            upd();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        private void updateTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DG.SelectedRows.Count == 0) DG.Rows[0].Selected = true;
            Int32 id = GetSelectedID();
            db.Reload();
            upd();
            DG.Rows[GetRowIndexByID(id)].Selected = true;
        }
        #endregion
        private void DG_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            upd();
        }
    }
}