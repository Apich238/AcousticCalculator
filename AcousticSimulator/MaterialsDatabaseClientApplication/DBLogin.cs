using MaterialsDatabase;
using System;
using System.Windows.Forms;

namespace MaterialsDatabaseEditor
{
    public partial class DBLogin : Form
    {
        public DBLogin()
        {
            InitializeComponent();
        }
        private ConnectionOptions ops;

        private MaterialsDBClient c;
        public MaterialsDBClient Client { get { return c; } }
        public ConnectionOptions ResultOptions { get { return ops; } }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ServerNameTB.Text.Length == 0)
            {
                MessageBox.Show(this, "Wrong input", "Wrong server name");
                ServerNameTB.Focus();
                return;
            }
            UInt32 PortN;
            if (PortTB.Text.Length == 0 || !UInt32.TryParse(PortTB.Text, out PortN))
            {
                MessageBox.Show(this, "Wrong input", "Wrong port number");
                PortTB.Focus();
                return;
            }
            if (UserNameTB.Text.Length == 0)
            {
                MessageBox.Show(this, "Wrong input", "Wrong user name");
                UserNameTB.Focus();
                return;
            }
            if (UserPassTB.Text.Length == 0)
            {
                MessageBox.Show(this, "Wrong input", "Password cannot be empty");
                UserPassTB.Focus();
                return;
            }
            ops = new ConnectionOptions(UserNameTB.Text, UserPassTB.Text, ServerNameTB.Text, uint.Parse(PortTB.Text));
            try
            {
                c = new MaterialsDBClient(ops.Account, ops.Password, ops.Host, ops.Port);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (AuthenticationException) { MessageBox.Show(this, "Authorisation error", "Wrong user name or password"); }
            catch (DatabaseNotFoundException) { MessageBox.Show(this, "Server error", "Database not found at this server"); }
            catch (ServerException) { MessageBox.Show(this, "Server error", "Server not found or port are closed"); }
            catch (Exception ex) { MessageBox.Show(this, "Exception", ex.Message); }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}