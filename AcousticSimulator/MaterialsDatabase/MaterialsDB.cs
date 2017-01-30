using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;

namespace MaterialsDatabase
{
    #region ExceptionClasses
    public class DBConnectionException : Exception { }
    public class ServerException : DBConnectionException { }
    public class DatabaseNotFoundException : DBConnectionException { }
    public class AuthenticationException : DBConnectionException { }
    #endregion
    public class MaterialsDBClient
    {
        #region PrivateFields
        private MySqlConnection con;
        private MySqlDataAdapter adapter;
        private DataTable table;
        #endregion
        public MaterialsDBClient(string UserName, string UserPassword, string ServerName = "localhost", uint ServerPort = 3306)
        {
        MySqlConnectionStringBuilder conStr = new MySqlConnectionStringBuilder();
            conStr.Database = "materialsdb";
            conStr.Password = UserPassword;
            conStr.Port = ServerPort;
            conStr.Server = ServerName;
            conStr.UserID = UserName;
            con = new MySqlConnection(conStr.ConnectionString);
            adapter = new MySqlDataAdapter("GetAll()", con);
            adapter.SelectCommand.CommandType = CommandType.Text;
            switch (Authorisate())
            {
                case 0: break;
                case 1: throw new ServerException();
                case 2: throw new DatabaseNotFoundException();
                case 3: throw new AuthenticationException();
                default: throw new Exception();
            }
            table = new DataTable();
            Reload();
        }
        private byte Authorisate()
        {
            try
            {
                con.Open();
                con.Close();
                return 0;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Unable to connect")) return 1;
                if (e.Message.Contains("Unknown database")) return 2;
                if (e.Message.Contains("Access denied")) return 3;
                return 4;
            }
        }
        #region Interface
        public enum UserMode
        {
            Viewer,//only view
            Editor,//view and edit
            Corrector,//editor+remove records
        }
        public DataTable Materials { get { return table; } }
        public Material GetMaterialByID(Int32 id)
        {
            Material m = new Material();
            var a = Materials.Rows.Find(id);
            if (a == null) throw new Exception();
            m = new Material();
            m.Name = (string)a["NAME"];
            m.gAmbientColorInd = (Int32)a["AMBIENTCOLOR"];
            m.reflection = (double)a["REFLECTION"];
            m.absorbparam = (double)a["ABSORPTPARAM"];
            return m;
        }
        #region ConnectionProps
        public string ServerName { get { return (new MySqlConnectionStringBuilder(con.ConnectionString)).Server; } }
        public uint ServerPort { get { return (new MySqlConnectionStringBuilder(con.ConnectionString)).Port; } }
        public string UserName { get { return (new MySqlConnectionStringBuilder(con.ConnectionString)).UserID; } }
        public UserMode UserType
        {
            get
            {
                switch (UserName)
                {
                    case "MatDBAdminExt": return UserMode.Corrector;
                    case "MatDBAdmin": return UserMode.Editor;
                    default: return UserMode.Viewer;
                }
            }
        }
        #endregion
        #region DataManipulation
        public void Reload()
        {
            table.Rows.Clear();
            adapter.Fill(table);
            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
            table.Columns["ID"].Unique = true;
        }
        public Int32 AddMaterial(Material m)
        {
            if (UserType == UserMode.Viewer)
                throw new InvalidOperationException(UserName + " is not administrator.");
            MySqlCommand com;
            com = new MySqlCommand("InsertAndReturnKey", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("_name", MySqlDbType.VarChar, 80);
            com.Parameters.Add("_ambcol", MySqlDbType.Int32);
            com.Parameters.Add("_r", MySqlDbType.Double);
            com.Parameters.Add("_a", MySqlDbType.Double);
            com.Parameters["_name"].Value = m.Name;
            com.Parameters["_defcol"].Value = m.gAmbientColorInd;
            com.Parameters["_r"].Value = m.reflection;
            com.Parameters["_a"].Value = m.absorbparam;
            try
            {
                con.Open();
                object o = com.ExecuteScalar().ToString();
                con.Close();
                Int32 p = 0;
                Int32.TryParse(o.ToString(), out p);
                Reload();
                return p;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return 0;
            }
        }
        public bool RemoveOutdatedMaterials()
        {
            if (UserType != UserMode.Corrector)
                throw new InvalidOperationException(UserName + " is not administrator.");
            MySqlCommand c = new MySqlCommand("RemoveOutDated", con);
            try
            {
                con.Open();
                int k = c.ExecuteNonQuery();
                con.Close();
                if (k > 0) Reload();
                return k > 0;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return false;
            }
        }
        public bool UpdMaterialProp(Int32 id, MProps p, object value)
        {
            if (UserType == UserMode.Viewer)
                throw new InvalidOperationException(UserName + " is not administrator.");
            MySqlCommand com = new MySqlCommand();
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("_id", MySqlDbType.Int32);
            com.Parameters["_id"].Value = id;
            com.Connection = con;
            switch (p)
            {
                case MProps.NAME:
                    com.CommandText = "UpdateName";
                    com.Parameters.Add("_name", MySqlDbType.VarChar, 80);
                    com.Parameters["_name"].Value = (string)value;
                    break;
                case MProps.AMBIENTCOLOR:
                    com.CommandText = "UpdateAmbientColor";
                    com.Parameters.Add("_color", MySqlDbType.Int32);
                    com.Parameters["_color"].Value = (Int32)value;
                    break;
                case MProps.OUTDATED: return SetMaterialOutDated(id, (bool)value);
                case MProps.REFLECTION:
                    com.CommandText = "UpdateReflection";
                    com.Parameters.Add("_r", MySqlDbType.Double);
                    com.Parameters["_r"].Value = (double)value;
                    break;
                case MProps.ABSORPTPARAM:
                    com.CommandText = "UpdateAbsorptionParam";
                    com.Parameters.Add("_a", MySqlDbType.Double);
                    com.Parameters["_a"].Value = (double)value;
                    break;
            }
            try
            {
                con.Open();
                int k = com.ExecuteNonQuery();
                con.Close();
                Reload();
                return k > 0;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return false;
            }
        }
        public bool UpdMaterial(Int32 id, Material m)
        {
            Reload();
            try
            {
                Material o = GetMaterialByID(id);
                if (o.Name != m.Name) UpdMaterialProp(id, MProps.NAME, m.Name);
                if (o.gAmbientColorInd != m.gAmbientColorInd) UpdMaterialProp(id, MProps.AMBIENTCOLOR, m.gAmbientColorInd);
                if (o.reflection != m.reflection) UpdMaterialProp(id, MProps.REFLECTION, m.reflection);
                if (o.absorbparam != m.absorbparam) UpdMaterialProp(id, MProps.ABSORPTPARAM, m.absorbparam);
                return true;
            }
            catch (Exception) { return false; }
        }
        public bool SetMaterialOutDated(Int32 id, bool OutDated)
        {
            if (UserType == UserMode.Viewer)
                throw new InvalidOperationException(UserName + " is not administrator.");
            MySqlCommand com = new MySqlCommand();
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("_id", MySqlDbType.Int32);
            com.Parameters["_id"].Value = id;
            com.Connection = con;
            if (OutDated) com.CommandText = "SetOutdatedByID";
            else com.CommandText = "SetNotOutdatedByID";
            try
            {
                con.Open();
                int k = com.ExecuteNonQuery();
                con.Close();
                Reload();
                return k > 0;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return false;
            }
        }
        #endregion
        #endregion
    }
}