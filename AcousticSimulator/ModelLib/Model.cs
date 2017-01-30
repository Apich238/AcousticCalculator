using MaterialsDatabase;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ModelLib
{
    public abstract class ModelPart
    {
        public ModelPart() { visible = true; }
        public bool visible;
        public bool selected;
        public string name;
        public override String ToString()
        {
            return name;

        }
    }

    public class Model
    {
        public Model(IMatDBDict m)
        {
            mdb = m;
            MeshModel = new PolygonalModel();
            calcpts = new List<calcpoint>();
            rooms = new List<Room>();
            reflectors = new List<Reflector>();
            borders = new List<Border>();
            sources = new List<NoiseSource>();
        }

        public void AddBorder(Border b, Room r1 = null, Room r2 = null)
        {
            b.room1 = (r1 == null) ? r2 : r1;
            b.room2 = (r1 == null) ? null : r2;

            borders.Add(b);
        }

        public IMatDBDict mdb;
        public PolygonalModel MeshModel;
        public List<calcpoint> calcpts;
        public List<Room> rooms;
        public List<Border> borders;
        public List<Reflector> reflectors;

        public List<NoiseSource> sources;
        //public void calc(calcpoint p)//расчитать л-ую точку в calcpts
        //{
        //    if (sources.Count == 0) { p.result = new noise(); return; }
        //    noise r = sources[0].GetLevel(p.pos);
        //    for (int i = 1; i < sources.Count; i++)
        //        r += sources[i].GetLevel(p.pos);
        //    p.result = r;
        //}

        public void SaveToFile(string filename)
        {
            using (var sw = new BinaryWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                sw.Write(rooms.Count);
                foreach (var r in rooms)
                {
                    var b = Encoding.UTF8.GetBytes(r.name);
                    sw.Write(b.Length);
                    sw.Write(b);
                }

                sw.Write(borders.Count);
                foreach (var b in borders)
                {
                    var bt = Encoding.UTF8.GetBytes(b.name);
                    sw.Write(bt.Length);
                    sw.Write(bt);
                    

                    sw.Write((b.room1 == null) ? -1 : rooms.IndexOf(b.room1));
                    sw.Write((b.room2 == null) ? -1 : rooms.IndexOf(b.room2));

                    sw.Write(b.Rectangle.ontop.X);
                    sw.Write(b.Rectangle.ontop.Y);
                    sw.Write(b.Rectangle.ontop.Z);
                    sw.Write(b.Rectangle.bottomleft.X);
                    sw.Write(b.Rectangle.bottomleft.Y);
                    sw.Write(b.Rectangle.bottomleft.Z);
                    sw.Write(b.Rectangle.bottomright.X);
                    sw.Write(b.Rectangle.bottomright.Y);
                    sw.Write(b.Rectangle.bottomright.Z);

                    sw.Write(b.layers.Count);
                    foreach (var l in b.layers)
                    {
                        sw.Write(l.materialId);
                        sw.Write(l.thickness);
                    }
                }

                sw.Write(reflectors.Count);
                foreach (var b in reflectors)
                {
                    var bt = Encoding.UTF8.GetBytes(b.name);
                    sw.Write(bt.Length);
                    sw.Write(bt);
                    sw.Write(b.matid);
                    sw.Write(b.Rectangle.ontop.X);
                    sw.Write(b.Rectangle.ontop.Y);
                    sw.Write(b.Rectangle.ontop.Z);
                    sw.Write(b.Rectangle.bottomleft.X);
                    sw.Write(b.Rectangle.bottomleft.Y);
                    sw.Write(b.Rectangle.bottomleft.Z);
                    sw.Write(b.Rectangle.bottomright.X);
                    sw.Write(b.Rectangle.bottomright.Y);
                    sw.Write(b.Rectangle.bottomright.Z);
                }
                
                sw.Write(sources.Count);
                foreach (var s in sources)
                {
                    var b = Encoding.UTF8.GetBytes(s.name);
                    sw.Write(b.Length);
                    sw.Write(b);

                    sw.Write((s.room == null) ? -1 : rooms.IndexOf(s.room));

                    sw.Write(s.power.lvl315.value);
                    sw.Write(s.power.lvl63.value);
                    sw.Write(s.power.lvl125.value);
                    sw.Write(s.power.lvl250.value);
                    sw.Write(s.power.lvl500.value);
                    sw.Write(s.power.lvl1000.value);
                    sw.Write(s.power.lvl2000.value);
                    sw.Write(s.power.lvl4000.value);
                    sw.Write(s.power.lvl8000.value);

                    sw.Write((bool)(s is LinearNoiseSrc));
                    if (s is LinearNoiseSrc)
                    {
                        sw.Write((s as LinearNoiseSrc).A.X);
                        sw.Write((s as LinearNoiseSrc).A.Y);
                        sw.Write((s as LinearNoiseSrc).A.Z);
                        sw.Write((s as LinearNoiseSrc).B.X);
                        sw.Write((s as LinearNoiseSrc).B.Y);
                        sw.Write((s as LinearNoiseSrc).B.Z);
                    }
                    else
                    {
                        sw.Write(s.pos.X);
                        sw.Write(s.pos.Y);
                        sw.Write(s.pos.Z);
                    }

                }
                sw.Write(calcpts.Count);
                foreach (var c in calcpts)
                {
                    var bt = Encoding.UTF8.GetBytes(c.name);
                    sw.Write(bt.Length);
                    sw.Write(bt);

                    sw.Write((c.room == null) ? -1 : rooms.IndexOf(c.room));

                    sw.Write(c.pos.X);
                    sw.Write(c.pos.Y);
                    sw.Write(c.pos.Z);

                }
            }
        }

        public Dictionary<int,string> GetMaterialsDict()
        {
            Dictionary<int, string> d = new Dictionary<int, string>();
            foreach (var k in mdb.Keys) d.Add(k, mdb[k].Name);
            return d;
        }

        public Dictionary<calcpoint,Dictionary<NoiseSource,noise>> calc()
        {
            NoiseCalculator c = new NoiseCalculator(this);
            c.calc();
            return c.res;
        }

        public static Model LoadFromFile(IMatDBDict md, string filename)
        {
            Model m = new Model(md);
            using (var sw = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                m.MeshModel = new PolygonalModel();
                int c;
                int l;
                byte[] b;
                m.rooms = new List<Room>();
                c = sw.ReadInt32();
                for (int i = 0; i < c; i++)
                {
                    Room rm = new Room();
                    l = sw.ReadInt32();
                    b = sw.ReadBytes(l);
                    rm.name = Encoding.UTF8.GetString(b);
                    m.rooms.Add(rm);
                }
                int ri;
                m.borders = new List<Border>();
                c = sw.ReadInt32();
                for (int i = 0; i < c; i++)
                {
                    Border x = new Border();
                    l = sw.ReadInt32();
                    b = sw.ReadBytes(l);
                    x.name = Encoding.UTF8.GetString(b);
                    

                    ri = sw.ReadInt32();
                    if (ri == -1) x.room1 = null;
                    else x.room1 = m.rooms[ri];

                    ri = sw.ReadInt32();
                    if (ri == -1) x.room2 = null;
                    else x.room2 = m.rooms[ri];

                    x.Rectangle = new spacerect();
                    x.Rectangle.ontop = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                    x.Rectangle.bottomleft = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                    x.Rectangle.bottomright = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());

                    int cc = sw.ReadInt32();
                    x.layers = new List<border_layer>();
                    for (int j = 0; j < cc; j++)
                    {
                        border_layer lr = new border_layer();
                        lr.materialId = sw.ReadInt32();
                        lr.thickness = sw.ReadDouble();
                        x.layers.Add(lr);
                    }

                    m.borders.Add(x);
                }

                m.reflectors = new List<Reflector>();
                c = sw.ReadInt32();
                for (int i = 0; i < c; i++)
                {
                    Reflector x = new Reflector();
                    l = sw.ReadInt32();
                    b = sw.ReadBytes(l);
                    x.name = Encoding.UTF8.GetString(b);

                    x.matid = sw.ReadInt32();

                    x.Rectangle = new spacerect();
                    x.Rectangle.ontop = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                    x.Rectangle.bottomleft = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                    x.Rectangle.bottomright = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());

                    m.reflectors.Add(x);

                }

                c = sw.ReadInt32();
                m.sources = new List<NoiseSource>();
                for (int i = 0; i < c; i++)
                {
                    NoiseSource x = new NoiseSource();
                    l = sw.ReadInt32();
                    b = sw.ReadBytes(l);
                    x.name = Encoding.UTF8.GetString(b);

                    ri = sw.ReadInt32();
                    if (ri == -1) x.room = null;
                    else x.room = m.rooms[ri];

                    x.power = new noise();
                    x.power.lvl315.value = sw.ReadDouble();
                    x.power.lvl63.value = sw.ReadDouble();
                    x.power.lvl125.value = sw.ReadDouble();
                    x.power.lvl250.value = sw.ReadDouble();
                    x.power.lvl500.value = sw.ReadDouble();
                    x.power.lvl1000.value = sw.ReadDouble();
                    x.power.lvl2000.value = sw.ReadDouble();
                    x.power.lvl4000.value = sw.ReadDouble();
                    x.power.lvl8000.value = sw.ReadDouble();

                    if (sw.ReadBoolean())
                    {
                        var xs = new LinearNoiseSrc();
                        xs.name = x.name;
                        xs.power = x.power;
                        xs.room = null;
                        xs.A = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                        xs.B = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                        m.sources.Add(xs);
                    }
                    else
                    {
                        x.pos = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());
                        m.sources.Add(x);
                    }

                }
                m.calcpts = new List<calcpoint>();
                c = sw.ReadInt32();
                for (int i = 0; i < c; i++)
                {
                    calcpoint x = new calcpoint();
                    l = sw.ReadInt32();
                    b = sw.ReadBytes(l);
                    x.name = Encoding.UTF8.GetString(b);

                    ri = sw.ReadInt32();
                    if (ri == -1) x.room = null;
                    else x.room = m.rooms[ri];

                    x.pos = new Vector3d(sw.ReadDouble(), sw.ReadDouble(), sw.ReadDouble());

                    m.calcpts.Add(x);
                }
            }
            return m;
        }

        public Color GetMaterialColor(int mid)
        {
            try { return mdb[mid].gAmbientColor; }
            catch (KeyNotFoundException) { return Color.Red; }
        }
    }

    public class pointModelPart:ModelPart
    {
        public Room room;
        public Vector3d pos;
    }
}