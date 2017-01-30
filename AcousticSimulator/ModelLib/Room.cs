using Assimp;
using MaterialsDatabase;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ModelLib
{

    public class Room : ModelPart
    {
        public Room()
        {
            name = "";
        }
        public Room clone()
        {
            Room r = new Room();
            return r;
        }
    }
}
