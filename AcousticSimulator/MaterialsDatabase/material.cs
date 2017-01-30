using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MaterialsDatabase
{
    //g* - graphics attributes
    //a* - acoustic attributes
    //without prefix - common attribute
    //in comment of a field writen property name in database
#pragma warning disable CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    public class Material
#pragma warning restore CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    {
        public Int32 gAmbientColorInd;//AMBIENTCOLOR
        public Color gAmbientColor { get { return Color.FromArgb(gAmbientColorInd); } set { gAmbientColorInd = value.ToArgb(); } }
        public string Name;//NAME
        public double reflection;//REFLECTION
        public double absorbparam;//ABSORPTPARAM
        public override Boolean Equals(Object obj)
        {
            if (obj is Material)
            {
                var m = (Material)obj;
                return m.reflection == reflection && m.absorbparam==absorbparam &&
                    m.gAmbientColorInd == gAmbientColorInd &&
                    m.Name == Name;
            }
            return false;
        }
    }
    public enum MProps//updatable material props
    {
        NAME,
        AMBIENTCOLOR,
        OUTDATED,
        REFLECTION,
        ABSORPTPARAM
    }
}
