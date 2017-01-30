using Assimp;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ModelLib
{
    public class PolygonalModel : ModelPart
    {
        public PolygonalModel()
        {
            meshes = new List<Mesh>();
            mats = new List<gmaterial>();
            name = "model";
        }
        private bool load(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException();
            Scene s = null;
            var imp = new AssimpImporter();
            if (!imp.IsImportFormatSupported(Path.GetExtension(filename)))
            {
                throw new ArgumentException("Model format " + Path.GetExtension(filename) + " is not supported!  Cannot load {1}", "filename");
            }
            s = imp.ImportFile(filename, PostProcessSteps.Triangulate | PostProcessSteps.PreTransformVertices);
            mats = new List<gmaterial>();
            name = filename;
            foreach (var m in s.Materials)
            {
                gmaterial gm = new gmaterial();
                gm.colAmbient = Color.FromArgb((byte)(255 * m.ColorAmbient.A),
                    (byte)(255 * m.ColorAmbient.R),
                    (byte)(255 * m.ColorAmbient.G),
                    (byte)(255 * m.ColorAmbient.B));
                gm.colDiffuse = Color.FromArgb((byte)(255 * m.ColorDiffuse.A),
                    (byte)(255 * m.ColorDiffuse.R),
                    (byte)(255 * m.ColorDiffuse.G),
                    (byte)(255 * m.ColorDiffuse.B));
                gm.colSpecular = Color.FromArgb((byte)(255 * m.ColorSpecular.A),
                    (byte)(255 * m.ColorSpecular.R),
                    (byte)(255 * m.ColorSpecular.G),
                    (byte)(255 * m.ColorSpecular.B));
                mats.Add(gm);
            }
            foreach (var m in s.Meshes)
            {
                Mesh ms = new Mesh();
                var vs = m.Vertices;
                var fs = m.Faces;
                var ns = m.Normals;
                ms.name = m.Name;
                foreach (var v in vs)
                {
                    ms.vertices.Add(new OpenTK.Vector3d(v.X, -v.Z, v.Y));
                }
                foreach (var n in ns)
                {
                    ms.normals.Add(new OpenTK.Vector3d(n.X, -n.Z, n.Y));
                }
                foreach (var f in fs)
                {
                    ms.surface.Add(new Tuple<int, int, int>((int)f.Indices[0], (int)f.Indices[1], (int)f.Indices[2]));
                }
                ms.matind = m.MaterialIndex;
                meshes.Add(ms);
            }
            return true;
        }
        public List<Mesh> meshes;
        public List<gmaterial> mats;
        public static PolygonalModel LoadFromFile(string filename)
        {
            var x = new PolygonalModel();
            x.load(filename);
            return x;
        }

    }

    public class gmaterial
    {
        public Color colAmbient;
        public Color colDiffuse;
        public Color colSpecular;

    }


    public class Mesh:ModelPart
    {
        public Mesh()
        {
            vertices = new List<Vector3d>();
            surface = new List<Tuple<int, int, int>>();//координаты вершин и соответствующие индексы нормалей
            normals = new List<Vector3d>();
            
        }
        public List<Vector3d> vertices;//vertices
        public List<Tuple<int, int, int>> surface;//surface is list of triangles, this is list of surfaces.
        public List<Vector3d> normals;
        public int matind;
    }
}
