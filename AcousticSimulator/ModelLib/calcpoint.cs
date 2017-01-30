using MaterialsDatabase;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ModelLib
{

    public class calcpoint:pointModelPart
    {
        public calcpoint()
        {
        }
        public calcpoint clone()
        {
            calcpoint A = new calcpoint();
            A.name = name;
            A.pos = pos;
            A.room = room;
            return A;
        }
    }
}
