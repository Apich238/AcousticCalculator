using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ModelLib
{
    public class Reflector : ModelPart
    {
        public spacerect Rectangle;
        public int matid;
        public Reflector()
        {
            matid = 0;
            name = "";
            Rectangle = new spacerect();
        }

        public ModelPart clone()
        {
            var r = new Reflector()                ;
            r.matid = matid;
            r.name = name;
            r.Rectangle = Rectangle;
            return r;
        }
    }

    public class reflmat
    {
        public string name;
        public double d315, d63, d125, d250, d500, d1000, d2000, d4000, d8000;
        public Color col;
        public override String ToString()
        {
            return name;
        }
    }

    public static class ReflMatColl
    {
        public static Dictionary<int, reflmat> d;
        public static bool inited = false;
        public static void init()

        {
            if (inited) return;
            d = new Dictionary<int, reflmat>();
            reflmat m = new reflmat();
            m.name = "асфальт, бетон";
            m.d315 = 0.02;
            m.d63 = 0.02;
            m.d125 = 0.02;
            m.d250 = 0.03;
            m.d500 = 0.03;
            m.d1000 = 0.04;
            m.d2000 = 0.05;
            m.d4000 = 0.06;
            m.d8000 = 0.06;
            m.col = Color.Gray;
            d.Add(0, m);
            m = new reflmat();
            m.name = "трава, песок";
            m.d315 = 0.1;
            m.d63 = 0.1;
            m.d125 = 0.33;
            m.d250 = 0.4;
            m.d500 = 0.8;
            m.d1000 = 0.8;
            m.d2000 = 0.8;
            m.d4000 = 0.8;
            m.d8000 = 0.7;
            m.col = Color.Green;
            d.Add(1, m);
            inited = true;
        }


    }

}