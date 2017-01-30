using System;
using System.Drawing;
using System.Xml.Linq;
using System.Globalization;

namespace Viewport
{
    public static class GraphicsOptions
    {
        public static Color BackgroundColor;
        public static Color HighGridColor;
        public static Color LowGridColor;
        public static Color SelectedItemColor;
        public static Color AxisXColor;
        public static Color AxisYColor;
        public static Color AxisZColor;
        public static Color CactPointDefault;
        public static Color NoiseSourceDefault;
        public static void SetDefaults()
        {
            BackgroundColor = Color.Gray;
            HighGridColor = Color.White;
            LowGridColor = Color.LightGray;
            SelectedItemColor = Color.YellowGreen;
            AxisXColor = Color.Red;
            AxisYColor = Color.Green;
            AxisZColor = Color.Blue;
            CactPointDefault = Color.Yellow;
            NoiseSourceDefault = Color.White;
    }
        public static bool LoadFromXMLFile(string pth)
        {
            try
            {
                XElement rt = XDocument.Load(pth).Root;
                XElement el;
                SetDefaults();
                if (null != (el = rt.Element("BackgroundColor"))) BackgroundColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("HighGridColor"))) HighGridColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("LowGridColor"))) LowGridColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("SelectedItemColor"))) SelectedItemColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("AxisXColor"))) AxisXColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("AxisYColor"))) AxisYColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("AxisZColor"))) AxisZColor = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("CactPointDefault"))) CactPointDefault = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                if (null != (el = rt.Element("NoiseSourceDefault"))) NoiseSourceDefault = Color.FromArgb(Int32.Parse(el.Value, NumberStyles.HexNumber));
                return true;
            }
            catch (Exception) { return false; }
        }
        public static bool SaveToXMLFile(string pth)
        {
            XElement el = new XElement("GraphicsOptions");
            if (BackgroundColor != Color.Gray)
                el.Add(new XElement("BackgroundColor", BackgroundColor.ToArgb().ToString("X")));
            if (HighGridColor != Color.White)
                el.Add(new XElement("HighGridColor", HighGridColor.ToArgb().ToString("X")));
            if (LowGridColor != Color.LightGray)
                el.Add(new XElement("LowGridColor", LowGridColor.ToArgb().ToString("X")));
            if (SelectedItemColor != Color.YellowGreen)
                el.Add(new XElement("SelectedItemColor", SelectedItemColor.ToArgb().ToString("X")));
            if (AxisXColor != Color.Red)
                el.Add(new XElement("AxisXColor", AxisXColor.ToArgb().ToString("X")));
            if (AxisYColor != Color.Green)
                el.Add(new XElement("AxisYColor", AxisYColor.ToArgb().ToString("X")));
            if (AxisZColor != Color.Green)
                el.Add(new XElement("AxisZColor", AxisZColor.ToArgb().ToString("X")));
            if (AxisZColor != Color.Green)
                el.Add(new XElement("CactPointDefault", AxisZColor.ToArgb().ToString("X")));
            if (AxisZColor != Color.Green)
                el.Add(new XElement("NoiseSourceDefault", AxisZColor.ToArgb().ToString("X")));
            XDocument d = new XDocument(el);
            try
            {
                d.Save(pth);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
