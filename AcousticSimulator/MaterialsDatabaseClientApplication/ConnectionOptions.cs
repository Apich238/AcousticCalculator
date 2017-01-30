using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MaterialsDatabaseEditor
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string acc,string pwd,string hst="localhost",uint prt = 3306)
        {
            account = acc;
            password = pwd;
            host = hst;
            port = prt;
        }
        public ConnectionOptions(XDocument xdoc)
        {
            XElement el = xdoc.Root;
            account = el.Element("account").Value;
            password = el.Element("password").Value;
            port = uint.Parse(el.Element("port").Value);
            host = el.Element("host").Value;
        }
        private string account, password, host;
        private uint port;
        public string Account        { get { return account; } }
        public string Password { get { return password; } }
        public string Host { get { return host; } }
        public uint Port { get { return port; } }
        public bool SaveToXML(string fname)
        {
            try {
                XDocument xd = new XDocument();
                xd.Add(new XElement("options"));
                XElement el = xd.Root;
                el.Add(new XElement("account", account));
                el.Add(new XElement("password", password));
                el.Add(new XElement("port", port.ToString()));
                el.Add(new XElement("host", host));
                xd.Save(fname);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
