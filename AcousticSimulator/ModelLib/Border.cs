using OpenTK;
using System.Collections.Generic;
using System;


namespace ModelLib
{
    public class Border : ModelPart
    {
        public Border() : base()
        {
            Rectangle = new spacerect();
            Rectangle.bottomleft = new Vector3d(0, 0, 0);
            Rectangle.bottomright = new Vector3d(0, 1, 0);
            Rectangle.ontop = new Vector3d(0, 0, 1);
            layers = new List<border_layer>();
            name = "";
            room1 = room2 = null;
        }
        public Border clone()
        {
            var b = new Border();
            b.name = name;
            b.layers = layers;
            b.Rectangle = Rectangle.clone();
            b.room1 = room1;
            b.room2 = room2;
            return b;
        }

        public void flip()
        {
            spacerect s = new spacerect();
            s.ontop = Rectangle.ontop;
            s.bottomleft = Rectangle.bottomright;
            s.bottomright = Rectangle.bottomleft;
            Rectangle = s;
        }

        public spacerect Rectangle;
        public Room room1, room2;
        public List<border_layer> layers;

     //   public bordertype type;

    }

    public class border_layer
    {
        public int materialId;
        public double thickness;
    }

    public class spacerect
    {
        public OpenTK.Vector3d ontop, bottomleft, bottomright;//точки: леж на верхней грани и 2 нижних угла
        public Vector3d lt
        {
            get
            {
                Vector3d v = bottomleft - bottomright;
                double a = Vector3d.Dot(bottomright - ontop, v.Normalized()) + v.Length;
                return ontop + a * v.Normalized();
            }
        }

        public double Square
        {
            get { return (lt - lb).Length * (rb - lb).Length; }
        }

        public double maxlinsz { get { return Math.Max((lt - lb).Length, (rb - lb).Length); } }

        public Vector3d center { get { return (lt + rt + lb + rb) / 4; } }
        public Vector3d facenorm { get { return Vector3d.Cross(bottomleft - ontop, bottomright - ontop).Normalized(); } }//должен указывать внутрь room1
        public spacerect clone()
        {
            spacerect A = new spacerect();
            A.ontop = ontop;
            A.bottomleft = bottomleft;
            A.bottomright = bottomright;
            return A;
        }
        public Vector3d rt
        {
            get
            {
                Vector3d v = bottomleft - bottomright;
                double a = Vector3d.Dot(bottomright - ontop, v.Normalized());
                return ontop + a * v.Normalized();
            }
        }
        public Vector3d lb { get { return bottomleft; } }
        public Vector3d rb { get { return bottomright; } }
    }
}
