using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using ModelLib;

namespace Viewport
{
    public partial class ViewportControl : UserControl
    {
        public ViewportControl() : this(null) { }
        public ViewportControl(ViewportList Container)
        {
            InitializeComponent();
            container = Container;
            if (container != null) container.Add(this);
            initialGraphicsSetup();
        }

        private void ViewportControlMF_LostFocus(Object sender, EventArgs e)
        {
            MoveTimer.Stop();
        }
        #region ViewportList members
        private ViewportList container;
        public ViewportList OwnerList
        {
            get
            {
                return container;
            }
            set
            {
                if (value != null && !(value is ViewportList)) throw new ArgumentException();
                if (container == null)//if this is nowhere
                {
                    if (value == null) return;//nothing changes
                    if (!value.Contains(this)) value.Add(this);//self insertion
                    container = value;
                }
                else
                {
                    if (value == null)//remove from owner list
                    {
                        container.Remove(this);
                        container = null;
                    }
                    else
                    {//add to a new list
                        OwnerList = null;
                        container = value;
                        container.Add(this);
                    }
                }
                Invalidate();
            }
        }
        public Document Document { get { return container == null ? null : container.ownDoc; } }
        #endregion
        #region Drawing 
        private void initialGraphicsSetup()
        {
            cam = new camera(Angle.FromDegrees(40.0f), 1.0f);
            Invalidate();
        }
        private camera cam;
        private class camera//представляет камеру и её атрибуты в классе viewport
        {
            public camera(Angle _fovW, float WdH)
            {
                fovW = _fovW;
                WdivH = WdH;
                p = new Position(Vector3.Zero, 0.0f, 0.0f, 0.0f);
                target = Vector3.Zero;
                UpdLocAxis();
            }
            #region fields
            private Vector3 target;//точка, являющаяся центром наблюдения. вращение "вокруг объекта" происходит вокруг неё
            private Position p;//положение камеры в пространстве
            public Angle fovW;//угол обзора по горизонтали
            public float WdivH;//отношение углов обзора, вертикального к горизонтальному
            public Angle fovH { get { return fovW / WdivH; } }//угол озора по вертикали
            public Vector3 _Forward, _Top, _Right;//направления локальных осей координат
            private void UpdLocAxis()
            {
                _Forward = p.FromLoclToGlob(Vector3.UnitY);
                _Top = p.FromLoclToGlob(Vector3.UnitZ);
                _Right = p.FromLoclToGlob(Vector3.UnitX);
            }

            public float PosX { get { return p.X; } set { p.X = value; } }
            public float PosY { get { return p.Y; } set { p.Y = value; } }
            public float PosZ { get { return p.Z; } set { p.Z = value; } }
            public Angle PosYaw { get { return p.Yaw; } set { p.Yaw = value; UpdLocAxis(); } }
            public Angle PosPitch { get { return p.Pitch; } set { p.Pitch = value; UpdLocAxis(); } }
            public Angle PosRoll { get { return p.Roll; } set { p.Roll = value; UpdLocAxis(); } }
            #endregion
            public enum destination
            {
                nowhere = 0x00,
                right = 0x11,
                left = 0x10,
                front = 0x22,
                back = 0x20,
                top = 0x44,
                down = 0x40
            }//направления движения камеры
            public enum polarrotdest { top, down, left, right }//направления движения камеры при её вращении вокруг точки target(направления полярных координат)
            public void ApplyMatrices()//применение матриц проецирования и просмотра к текущему контексту
            {
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                Matrix4 q = Matrix4.CreatePerspectiveFieldOfView((float)(fovH.Radians > Math.PI ? 3.1415f : fovH.Radians), WdivH, 0.01f, 100000.0f);
                GL.LoadMatrix(ref q);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                q = Matrix4.LookAt(p.Pos, p.Pos + _Forward, _Top);
                GL.LoadMatrix(ref q);
            }
            public void Move(destination dst, float dist)//движение камеры в собственной системе координат в указанном направлении на указанное расстояние
            {
                if (dst == destination.nowhere) return;
                if (dst.HasFlag(destination.right)) p.Pos += dist * _Right;
                else if (dst.HasFlag(destination.left)) p.Pos -= dist * _Right;
                if (dst.HasFlag(destination.front)) p.Pos += dist * _Forward;
                else if (dst.HasFlag(destination.back)) p.Pos -= dist * _Forward;
                if (dst.HasFlag(destination.top)) p.Pos += dist * _Top;
                else if (dst.HasFlag(destination.down)) p.Pos -= dist * _Top;
            }
            public void Move(Vector3 dest)//движение камеры на указанный вектор в глобальной системе координат.
            {
                p.Pos += dest;
            }

        }
        private class Position//используется в camera. определяет местоположение и поворот предмета в пространстве
        {
            private Vector3 pos;
            private Quaternion rotq;
            private Angle yaw, pitch, roll;
            public Position(Vector3 ps, Angle y, Angle p, Angle r)
            {
                pos = ps;
                yaw = y;
                pitch = p;
                roll = r;
                SetQByAngs();
            }
            private void SetQByAngs()
            {
                Matrix3 A = Matrix3.CreateRotationY(roll),
                    B = Matrix3.CreateRotationX(-pitch),
                    C = Matrix3.CreateRotationZ(-yaw);
                Matrix3 r = C * B * A;
                rotq = Quaternion.FromMatrix(r);
            }
            public float X { get { return pos.X; } set { pos.X = value; } }
            public float Y { get { return pos.Y; } set { pos.Y = value; } }
            public float Z { get { return pos.Z; } set { pos.Z = value; } }
            public Vector3 Pos { get { return pos; } set { pos = value; } }
            public Angle Yaw { get { return yaw; } set { yaw = value; SetQByAngs(); } }
            public Angle Pitch { get { return pitch; } set { pitch = value; SetQByAngs(); } }
            public Angle Roll { get { return roll; } set { roll = value; SetQByAngs(); } }
            public Quaternion RotationQ { get { return rotq; } }
            public void Rotate(Vector3 axis, Angle value, Vector3 c)//поворот вокруг точки с на угол value по оси axis
            {
                pos -= c;
                Quaternion q1 = Quaternion.FromAxisAngle(axis, value);
                Quaternion q = rotq * q1;
                rotq = q;
                yaw = (float)Math.Atan2(2 * q.X * q.Y + 2 * q.W * q.Z, q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);
                pitch = -(float)Math.Asin(2 * q.W * q.Y - 2 * q.X * q.Z);
                roll = -(float)Math.Atan2(2 * q.Y * q.Z + 2 * q.W * q.X, -q.W * q.W + q.X * q.X + q.Y * q.Y - q.Z * q.Z);
                pos = Vector3.Transform(pos, q1) + c;
            }
            public Vector3 FromLoclToGlob(Vector3 v) { return Vector3.Transform(v, rotq); }
        }
        #region fields
        //private Vector3 ViewTarget;
        //private Angle APolar, AAzimuthal;
        //private projectionMode prmode;
        //private Angle FOV_w;
        //private Angle FOV_h { get { return FOV_w / glwnd.AspectRatio; } }
        //private float DistToTarget;
        //private Angle FOV_x;
        //private Angle FOV_y { get { return FOV_x / glwnd.AspectRatio; } }
        #endregion
        #region interface
        public float CameraPosX
        {
            get { return cam.PosX; }
            set { cam.PosX = value; Invalidate(); }
        }
        public float CameraPosY { get { return cam.PosY; } set { cam.PosY = value; Invalidate(); } }
        public float CameraPosZ
        {
            get { return cam.PosZ; }
            set { cam.PosZ = value; Invalidate(); }
        }
        public Angle CameraYaw { get { return cam.PosYaw; } set { cam.PosYaw = value; Invalidate(); } }
        public Angle CameraPitch { get { return cam.PosPitch; } set { cam.PosPitch = value; Invalidate(); } }
        public Angle CameraRoll { get { return cam.PosRoll; } set { cam.PosRoll = value; Invalidate(); } }
        //public projectionMode Prmode
        //{
        //    get { return prmode; }
        //    set
        //    {
        //        if (prmode != value)
        //        { prmode = value; Redraw(); }
        //    }
        //}
        //public defaultView DefaultView
        //{
        //    get
        //    {
        //        if (APolar.Degrees == 90.0)
        //        {
        //            if (AAzimuthal.Degrees == 0.0) return defaultView.Back;
        //            if (AAzimuthal.Degrees == 90.0) return defaultView.Right;
        //            if (AAzimuthal.Degrees == 180.0) return defaultView.Front;
        //            if (AAzimuthal.Degrees == 270.0) return defaultView.Left;
        //        }
        //        if (APolar.Degrees == 0.0 && AAzimuthal.Degrees == 0.0) return defaultView.Top;
        //        if (APolar.Degrees == 180.0 && AAzimuthal.Degrees == 180.0) return defaultView.Bottom;
        //        return defaultView.Custom;
        //    }
        //    set
        //    {
        //        switch (value)
        //        {
        //            case defaultView.Right: APolar.Degrees = 90.0f; AAzimuthal.Degrees = 90.0f; break;
        //            case defaultView.Front: APolar.Degrees = 90.0f; AAzimuthal.Degrees = 180.0f; break;
        //            case defaultView.Left: APolar.Degrees = 90.0f; AAzimuthal.Degrees = 270.0f; break;
        //            case defaultView.Back: APolar.Degrees = 90.0f; AAzimuthal.Degrees = 0.0f; break;
        //            case defaultView.Top: APolar.Degrees = 0.0f; AAzimuthal.Degrees = 0.0f; break;
        //            case defaultView.Bottom: APolar.Degrees = 180.0f; AAzimuthal.Degrees = 180.0f; break;
        //            default: DefaultView = defaultView.Top; break;
        //        }
        //        Invalidate();
        //    }
        //}
        //public Angle azim { get { return AAzimuthal; } set { AAzimuthal = value; Redraw(); } }
        //public Angle apol { get { return APolar; } set { APolar = value; Redraw(); } }
        //public void SetView(Angle pol, Angle azi)
        //{
        //    APolar = pol;
        //    AAzimuthal = azi;
        //    Invalidate();
        //}
        //public void SetView(Angle pol, Angle azi, Vector3 VTarget)
        //{
        //    SetView(pol, azi);
        //    ViewTarget = VTarget;
        //    Invalidate();
        //}
        #endregion
        #region drawing functions
        protected override void OnPaint(PaintEventArgs e)
        {
#if (DesignMode)
            return;
#else
            if (container == null) return;
            if (Document != null) Redraw();
#endif
        }
        public void Redraw()
        {
#if(DesignMode)
            return;
#else
            if (container == null) return;
            SetContext();
            DrawGrid();
            DrawGeometry();
            DrawAxis();
            GL.Flush();
            glwnd.SwapBuffers();
#endif
        }
        private void DrawGrid()
        {
            GL.LineWidth(1.0f);
            GL.Color3(GraphicsOptions.HighGridColor);
            GL.Begin(PrimitiveType.Lines);
            for (int i = -10; i <= 10; i++)
            {
                GL.Vertex2(new Vector2d(-10, i));
                GL.Vertex2(new Vector2d(10, i));
                GL.Vertex2(new Vector2d(i, -10));
                GL.Vertex2(new Vector2d(i, 10));
            }
            GL.End();
        }
        private void DrawGeometry()
        {
            Document d;
            try
            {
                d = Document;
                if (null == d) return;
            }
            catch (Exception) { return; }
            DrawMeshedModel();
            DrawsoundBorders();
            DrawReflectors();
            DrawRooms();
            DrawNoisesources();
            DrawCalcPoints();


        }

        private void DrawRooms()
        {
            foreach (var p in Document.Model.rooms)
            {
                if (!p.visible || !p.selected) continue;
                GL.Color3(Color.White);

                Vector3d min = new Vector3d(0, 0, 0), max = new Vector3d(0, 0, 0);
                bool f = true;
                foreach (var x in Document.Model.calcpts)
                    if (x.room != null && object.ReferenceEquals(x.room, p))
                    {
                        if (f || x.pos.X < min.X) min.X = x.pos.X;
                        if (f || x.pos.Y < min.Y) min.Y = x.pos.Y;
                        if (f || x.pos.Z < min.Z) min.Z = x.pos.Z;
                        if (f || x.pos.X > max.X) max.X = x.pos.X;
                        if (f || x.pos.Y > max.Y) max.Y = x.pos.Y;
                        if (f || x.pos.Z > max.Z) max.Z = x.pos.Z;
                        f = false;
                    }
                foreach (var x in Document.Model.sources)
                    if (x.room != null && object.ReferenceEquals(x.room, p))
                    {
                        if (f || x.pos.X < min.X) min.X = x.pos.X;
                        if (f || x.pos.Y < min.Y) min.Y = x.pos.Y;
                        if (f || x.pos.Z < min.Z) min.Z = x.pos.Z;
                        if (f || x.pos.X > max.X) max.X = x.pos.X;
                        if (f || x.pos.Y > max.Y) max.Y = x.pos.Y;
                        if (f || x.pos.Z > max.Z) max.Z = x.pos.Z;
                        f = false;
                    }
                foreach (var x in Document.Model.borders)
                    if (x.room1 != null && object.ReferenceEquals(x.room1, p) || x.room2 != null && object.ReferenceEquals(x.room2, p))
                    {
                        if(f)
                        {
                            min.X = x.Rectangle.lt.X;
                            min.Y = x.Rectangle.lt.Y;
                            min.Z = x.Rectangle.lt.Z;
                            max.X = x.Rectangle.lt.X;
                            max.Y = x.Rectangle.lt.Y;
                            max.Z = x.Rectangle.lt.Z;
                            f = false;
                        }
                        if (x.Rectangle.lt.X < min.X) min.X = x.Rectangle.lt.X;
                        if (x.Rectangle.lt.Y < min.Y) min.Y = x.Rectangle.lt.Y;
                        if (x.Rectangle.lt.Z < min.Z) min.Z = x.Rectangle.lt.Z;
                        if (x.Rectangle.lt.X > max.X) max.X = x.Rectangle.lt.X;
                        if (x.Rectangle.lt.Y > max.Y) max.Y = x.Rectangle.lt.Y;
                        if (x.Rectangle.lt.Z > max.Z) max.Z = x.Rectangle.lt.Z;

                        if (x.Rectangle.rt.X < min.X) min.X = x.Rectangle.rt.X;
                        if (x.Rectangle.rt.Y < min.Y) min.Y = x.Rectangle.rt.Y;
                        if (x.Rectangle.rt.Z < min.Z) min.Z = x.Rectangle.rt.Z;
                        if (x.Rectangle.rt.X > max.X) max.X = x.Rectangle.rt.X;
                        if (x.Rectangle.rt.Y > max.Y) max.Y = x.Rectangle.rt.Y;
                        if (x.Rectangle.rt.Z > max.Z) max.Z = x.Rectangle.rt.Z;

                        if (x.Rectangle.lb.X < min.X) min.X = x.Rectangle.lb.X;
                        if (x.Rectangle.lb.Y < min.Y) min.Y = x.Rectangle.lb.Y;
                        if (x.Rectangle.lb.Z < min.Z) min.Z = x.Rectangle.lb.Z;
                        if (x.Rectangle.lb.X > max.X) max.X = x.Rectangle.lb.X;
                        if (x.Rectangle.lb.Y > max.Y) max.Y = x.Rectangle.lb.Y;
                        if (x.Rectangle.lb.Z > max.Z) max.Z = x.Rectangle.lb.Z;

                        if (x.Rectangle.rb.X < min.X) min.X = x.Rectangle.rb.X;
                        if (x.Rectangle.rb.Y < min.Y) min.Y = x.Rectangle.rb.Y;
                        if (x.Rectangle.rb.Z < min.Z) min.Z = x.Rectangle.rb.Z;
                        if (x.Rectangle.rb.X > max.X) max.X = x.Rectangle.rb.X;
                        if (x.Rectangle.rb.Y > max.Y) max.Y = x.Rectangle.rb.Y;
                        if (x.Rectangle.rb.Z > max.Z) max.Z = x.Rectangle.rb.Z;
                    }

                Vector3d[] bx = new Vector3d[] {
                    new Vector3d(min.X,min.Y,min.Z),
                    new Vector3d(min.X,max.Y,min.Z),
                    new Vector3d(max.X,max.Y,min.Z),
                    new Vector3d(max.X,min.Y,min.Z),
                    new Vector3d(min.X,min.Y,max.Z),
                    new Vector3d(min.X,max.Y,max.Z),
                    new Vector3d(max.X,max.Y,max.Z),
                    new Vector3d(max.X,min.Y,max.Z), };
                double q = 0.2, t = 1 - q;
                GL.Begin(PrimitiveType.Lines);
                int a = 0, z = 1; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 4; z = 5; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 3; z = 2; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 7; z = 6; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 1; z = 2; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 0; z = 3; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 5; z = 6; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 4; z = 7; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 0; z = 4; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 1; z = 5; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 2; z = 6; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                a = 3; z = 7; GL.Vertex3(bx[a]); GL.Vertex3(bx[a] * t + bx[z] * q); GL.Vertex3(bx[z] * t + bx[a] * q); GL.Vertex3(bx[z]);
                GL.End();
            }
        }
        

        private void DrawCalcPoints()
        {
            foreach (var p in Document.Model.calcpts)
            {
                if (!p.visible)                    continue;
                GL.Color3(GraphicsOptions.CactPointDefault);
                GL.Disable(EnableCap.DepthTest);
                GL.PointSize(7f);
                GL.Begin(PrimitiveType.Points);
                GL.Vertex3(p.pos);
                GL.End();
            }
        }

        private Color GetColorByPointResult(noise result)
        {
            return Color.FromArgb(
                (byte)(Math.Min(255, 255 * (result.dBA.value / 210))),
                (byte)(Math.Min(255, 255 * (result.dBA.value / 210))),
                (byte)(Math.Min(255, 255 * (result.dBA.value / 210))));
        }

        private void DrawNoisesources()
        {
            foreach (var p in Document.Model.sources)
            {
                if (!p.visible) continue;
                GL.PointSize(7f);
                GL.Color3(GraphicsOptions.NoiseSourceDefault);
                GL.Disable(EnableCap.DepthTest);
                if (p is LinearNoiseSrc)
                {
                    var q = (LinearNoiseSrc)p;
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(q.A);
                    GL.Vertex3(q.B);
                    GL.End();
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(q.A);
                    GL.Vertex3(q.B);
                    GL.End();
                }
                else
                {
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(p.pos);
                    GL.End();
                }
            }
        }

        private void DrawsoundBorders()
        {
            foreach (var p in Document.Model.borders)
            {
                if (!p.visible) continue;
                if (p.layers.Count > 0)
                {
                    GL.Enable(EnableCap.DepthTest);
                    GL.Color3(Document.Model.GetMaterialColor(p.layers[0].materialId));
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex3(p.Rectangle.lt);
                    GL.Vertex3(p.Rectangle.rt);
                    GL.Vertex3(p.Rectangle.rb);
                    GL.Vertex3(p.Rectangle.lb);
                    GL.End();
                    GL.LineWidth(3);
                    GL.Color3(Color.Green);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(p.Rectangle.center);
                    GL.Vertex3(p.Rectangle.center+p.Rectangle.facenorm/4);
                    GL.End();

                }
                GL.Disable(EnableCap.DepthTest);
                GL.LineWidth(1);
                GL.Color3(p.selected ? Color.White : Color.Red);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(p.Rectangle.lt);
                GL.Vertex3(p.Rectangle.rt);
                GL.Vertex3(p.Rectangle.rb);
                GL.Vertex3(p.Rectangle.lb);
                GL.End();

            }
        }
        private void DrawReflectors()
        {
            foreach (var p in Document.Model.reflectors)
            {
                if (!p.visible) continue;
                ReflMatColl.init();
                GL.Enable(EnableCap.DepthTest);
                GL.Color3(ReflMatColl.d[p.matid].col);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex3(p.Rectangle.lt);
                GL.Vertex3(p.Rectangle.rt);
                GL.Vertex3(p.Rectangle.rb);
                GL.Vertex3(p.Rectangle.lb);
                GL.End();
                GL.LineWidth(3);
                GL.Color3(Color.Green);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(p.Rectangle.center);
                GL.Vertex3(p.Rectangle.center + p.Rectangle.facenorm / 4);
                GL.End();

            }
        }

        private void DrawMeshedModel()
        {
            PolygonalModel m;
            try
            {
                m = Document.Model.MeshModel;
            }
            catch (Exception) { return; }

            if (!m.visible) return;

            if (m.meshes.Count == 0) return;
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            Vector3 lightdirection =(cam._Right*0.5f + cam._Top*0.5f - cam._Forward).Normalized();
            Color diffuselight = Color.White;
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuselight);
            GL.Light(LightName.Light0, LightParameter.Position,new Vector4(lightdirection));
            GL.Light(LightName.Light0, LightParameter.Ambient, Color.Black);

            foreach (var ms in m.meshes)
            {

                if (!ms.visible) continue;

                var mat = m.mats[ms.matind];

                GL.Material(MaterialFace.Front, MaterialParameter.Ambient, mat.colAmbient);
                GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, mat.colDiffuse);

                GL.Color3(mat.colAmbient);
                GL.Begin(PrimitiveType.Triangles);
                foreach (var p in ms.surface)
                {
                    GL.Normal3(ms.normals[p.Item1]);
                    GL.Vertex3(ms.vertices[p.Item1]);
                    GL.Normal3(ms.normals[p.Item2]);
                    GL.Vertex3(ms.vertices[p.Item2]);
                    GL.Normal3(ms.normals[p.Item3]);
                    GL.Vertex3(ms.vertices[p.Item3]);
                }
                GL.End();

            }
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Normalize);
        }

        private void DrawAxis()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(GraphicsOptions.AxisXColor);
            GL.Vertex3(Vector3d.Zero);
            GL.Vertex3(Vector3d.UnitX / 2);
            GL.Color3(GraphicsOptions.AxisYColor);
            GL.Vertex3(Vector3d.Zero);
            GL.Vertex3(Vector3d.UnitY / 2);
            GL.Color3(GraphicsOptions.AxisZColor);
            GL.Vertex3(Vector3d.Zero);
            GL.Vertex3(Vector3d.UnitZ / 2);
            GL.End();
        }
        private void SetContext()
        {
            glwnd.MakeCurrent();
            GL.Viewport(glwnd.Size);
            GL.Enable(EnableCap.DepthTest);
            cam.ApplyMatrices();
            //if (prmode == projectionMode.axonometric) SetProjOrtho();
            //else SetProjPersp();
            GL.ClearColor(GraphicsOptions.BackgroundColor);
            GL.ClearDepth(1.0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        //private void SetProjPersp()
        //{
        //    GL.MatrixMode(MatrixMode.Projection);
        //    GL.LoadIdentity();
        //    Matrix4 q = Matrix4.CreatePerspectiveFieldOfView((float)FOV_y.Radians, glwnd.AspectRatio, 0.01f, 1000.0f);
        //    GL.LoadMatrix(ref q);
        //    GL.MatrixMode(MatrixMode.Modelview);
        //    GL.LoadIdentity();
        //    Vector3 eye = Vector3.UnitZ;
        //    q = Matrix4.CreateRotationX(-(float)apol.Radians);
        //    eye = Vector3.TransformVector(eye, q);
        //    q = Matrix4.CreateRotationZ((float)(Angle.pi - azim.Radians));
        //    eye = Vector3.TransformVector(eye, q);
        //    eye = DistToTarget * eye;
        //    q = Matrix4.LookAt(eye, Vector3.Zero, Vector3.UnitZ);
        //    GL.LoadMatrix(ref q);
        //}
        //private void SetProjOrtho()
        //{
        //    GL.MatrixMode(MatrixMode.Projection);
        //    GL.LoadIdentity();
        //    Matrix4 q = Matrix4.CreateOrthographic((float)FOV_w, (float)FOV_h, -1000.0f, 1000.0f);
        //    GL.LoadMatrix(ref q);
        //    GL.Rotate(-APolar.Degrees, Vector3d.UnitX);
        //    GL.Rotate(-AAzimuthal.Degrees, Vector3d.UnitZ);
        //}
        #endregion
        #region itemselection
        private int GetCursorTargetID()
        {
            //selection render
            //get probe from cursor position
            //try ret id from it
            return -1;//no target
        }
        #endregion
        #endregion
        #region MouseEvents
        private bool selfrotation,moving;
        private Point lastmouselocation;
        private void glwnd_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            cam.Move(camera.destination.front, 0.03f*e.Delta);
            Redraw();
        }
        private void glwnd_Click(Object sender, EventArgs e)
        {
        }
        private void glwnd_DoubleClick(Object sender, EventArgs e)
        {

        }
        private void glwnd_MouseClick(Object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        private void glwnd_MouseDoubleClick(Object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }
        private void glwnd_MouseDown(Object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (!selfrotation && e.Button == MouseButtons.Right)
            {
                selfrotation = true;
                lastmouselocation = e.Location;
            }
            if(!moving && e.Button==MouseButtons.Middle)
            {
                moving = true;
                lastmouselocation = e.Location;
            }
        }
        private void glwnd_MouseEnter(Object sender, EventArgs e)
        {

        }
        private void glwnd_MouseMove(Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (selfrotation)
            {
                var d = new Point((e.Location.X - lastmouselocation.X) % Width, (e.Location.Y - lastmouselocation.Y) % Height);
                cam.PosYaw += (((float)d.X) / Width) * cam.fovW * 5f;
                cam.PosPitch += (((float)d.Y) / Height) * cam.fovH * 5f;
                lastmouselocation = e.Location;
                Redraw();

            }
            if(moving)
            {
                cam.Move(camera.destination.right, -(e.Location.X - lastmouselocation.X) * 0.03f);
                cam.Move(camera.destination.top, (e.Location.Y - lastmouselocation.Y) * 0.03f);
                lastmouselocation = e.Location;
                Redraw();
            }

            lastmouselocation = e.Location;
            Redraw();
        }

        private void glwnd_MouseUp(Object sender, System.Windows.Forms.MouseEventArgs e)
        {

            selfrotation = false;
            moving = false;
        }

        private void glwnd_Resize(Object sender, EventArgs e)
        {
            if (cam != null) cam.WdivH = glwnd.AspectRatio;
            Redraw();
        }

        private bool _w, _s, _a, _d, _f, _r;//current destination of camera moving

        private void ViewportControl_Load(Object sender, EventArgs e)
        {
            FindForm().LostFocus += ViewportControlMF_LostFocus;
        }

        private void glwnd_Leave(Object sender, EventArgs e)
        {
            MoveTimer.Stop();
            Capture = false;
            selfrotation = false;
        }

        private void UpdKeysStates()
        {
            _w = Keyboard.IsKeyDown(Key.W);
            _s = Keyboard.IsKeyDown(Key.S);
            _a = Keyboard.IsKeyDown(Key.A);
            _d = Keyboard.IsKeyDown(Key.D);
            _f = Keyboard.IsKeyDown(Key.F);
            _r = Keyboard.IsKeyDown(Key.R);
        }

        private void glwnd_KeyDown(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            UpdKeysStates();
            MoveTimer.Start();
        }

        private void glwnd_KeyUp(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            UpdKeysStates();
            if (!(_w || _s || _a || _d || _f || _r))
                MoveTimer.Stop();
        }
        private void MoveTimer_Tick(Object sender, EventArgs e)
        {
            camera.destination d = 0;
            if (_w && !_s) d |= camera.destination.front;
            if (!_w && _s) d |= camera.destination.back;
            if (_a && !_d) d |= camera.destination.left;
            if (!_a && _d) d |= camera.destination.right;
            if (_f && !_r) d |= camera.destination.down;
            if (!_f && _r) d |= camera.destination.top;
            cam.Move(d, 0.05f);
            Redraw();
            UpdKeysStates();
            if (!(_w || _s || _a || _d || _f || _r)) MoveTimer.Stop();
        }
        #endregion
    }
    public enum projectionMode { axonometric = 0, perspective = 1 }
    public enum defaultView
    {
        Custom = 0,
        Bottom = 2,
        Top = 3,
        Left = 4,
        Right = 5,
        Back = 8,
        Front = 9,
    }
}