using System;
using MaterialsDatabase;

namespace ModelLib
{
    public class Document
    {


        public Document(String host, UInt32 port, String account, String password, string filename = "")
        {
            fname = filename;
            saved = fname != "";
            if (!saved)
                model = new Model(new MatDBDict(account, password, host, port));
            else
                model = Model.LoadFromFile(new MatDBDict(account, password, host, port), fname);
        }
        #region fields
        private Model model;
        private string fname;
        private bool saved;
        #endregion
        #region props
        public string Fname { get { return fname; } set { fname = value; } }
        public Model Model { get { return model; } }
        public bool Saved { get { return saved; } }
        #endregion

        public bool save(string filename)
        {
            try
            {
                Model.SaveToFile(filename);
                Fname = filename;
                return saved = true;
            }
            catch (Exception) { return saved = false; }
        }
        public bool saveas(string filename)
        {
            try
            {
                Model.SaveToFile(filename);
                return true;
            }
            catch (Exception) { return false; }
        }
        public bool save() { if (Fname != "") return save(Fname); else throw new ArgumentException("filename required"); }
        public void Load(string filename)
        {
            try
            {
                model = Model.LoadFromFile(model.mdb, filename);
                fname = filename;
                saved = true;
            }
            catch (Exception) { }
        }
    }
}