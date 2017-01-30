using ModelLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using Viewport;
using System.Linq;

namespace ModelEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            closeonload = false;

            GraphicsOptions.LoadFromXMLFile("GraphicsOptions.xml");
            try
            {
                c = new ConnectionOptions(XDocument.Load("MDBConnOpts.xml"));
                doc=new Document(c.Host,c.Port,c.Account,c.Password);
            }
            catch (Exception)
            {
                DBLogin d = new DBLogin();
                d.ShowDialog(this);
                if (d.DialogResult == DialogResult.Cancel)
                {
                    closeonload = true;
                    return;
                }
                doc = null;
                while(doc==null)
                {
                    try
                    {
                        doc = new Document(d.ResultOptions.Host, d.ResultOptions.Port, d.ResultOptions.Account, d.ResultOptions.Password);
                    }
                    catch(Exception ex)
                    {
                        doc = null;
                        if(DialogResult.Cancel== MessageBox.Show(string.Format("db connection exception{0}", ex.Message)))
                        {
                            closeonload = true;
                            return;
                        }
                    }
                }
            }
            vps = new ViewportList(doc);
            vps.Add(vp1);

            vp1.CameraPitch = Angle.FromDegrees(-23f);
            vp1.CameraYaw = Angle.FromDegrees(135f);

            story = new Stack<storyrecord>();
            ftr = new Stack<storyrecord>();
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
            AttachAllViewportsToDoc();
            updTitle();
            unlock();
            updtpd();
        }
        private bool closeonload;
        private ViewportList vps;

        ConnectionOptions c;
        private Document doc;
        //    private Viewport.ViewportControl vp1;
        Dictionary<TreeNode, ModelPart> tpd1;
        Dictionary<ModelPart, TreeNode> tpd2;
        void updtpd()
        {
            tpd1 = new Dictionary<TreeNode, ModelPart>();
            tpd2 = new Dictionary<ModelPart, TreeNode>();
            Model m = doc.Model;
            tv.Nodes.Clear();
            var mn=new TreeNode("Полигональная модель");
            tpd1.Add(mn, m.MeshModel);
            tpd2.Add( m.MeshModel,mn);
            tv.Nodes.Add(mn);
            tv.Nodes[0].Checked = m.MeshModel.visible;

            foreach (var s in m.MeshModel.meshes)
            {
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                mn.Nodes.Add(sn);
            }
            foreach (var s in m.reflectors)
            {
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                mn.Nodes.Add(sn);
            }


            foreach (var s in m.sources)
            {
                if (s.room != null) continue;
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                tv.Nodes.Add(sn);
            }
            foreach (var s in m.borders)
            {
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                tv.Nodes.Add(sn);
            }
            foreach (var s in m.calcpts)
            {
                if (s.room != null) continue;
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                tv.Nodes.Add(sn);
            }
            foreach (var s in m.rooms)
            {
                var sn = new TreeNode(s.ToString());
                sn.Checked = s.visible;
                tpd1.Add(sn, s);
                tpd2.Add(s, sn);
                tv.Nodes.Add(sn);
                foreach (var c in m.calcpts)
                    if (c.room != null && object.ReferenceEquals(c.room, s))
                    {
                        var cn = new TreeNode(c.ToString());
                        cn.Checked = c.visible;
                        tpd1.Add(cn, c);
                        tpd2.Add(c, cn);
                        sn.Nodes.Add(cn);
                    }
                foreach (var c in m.sources)
                    if (c.room != null && object.ReferenceEquals(c.room, s))
                    {
                        var cn = new TreeNode(c.ToString());
                        cn.Checked = c.visible;
                        tpd1.Add(cn, c);
                        tpd2.Add(c, cn);
                        sn.Nodes.Add(cn);
                    }
            }
        }
        private void AttachAllViewportsToDoc()
        {
            vps.ownDoc = doc;
        }
        private void updTitle()
        {
            if (doc.Fname == "") Text = "Редактор акустической модели - newmodel.am";
            else Text = string.Format("Редактор акустической модели - {0}", doc.Fname);
            if (!doc.Saved) Text += "*";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close current doc if isnt saved

            if (tv.Enabled)
            {
                if (!doc.Saved)
                    switch (MessageBox.Show(this, "save?", "Saving", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Yes:
                            if (doc.Fname == "")
                            {
                                if (sfd.ShowDialog(this) == DialogResult.Cancel) return;
                                else doc.save(sfd.FileName);
                            }
                            else doc.save(sfd.FileName);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
            }
            if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
            try
            {
                doc.Load(ofd.FileName);


                unlock();
                AttachAllViewportsToDoc();
                updTitle();
                updtpd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("an error occured when tried to load model:{0}",ex.Message), "error");
            }
        }

        void unlock()
        {
            foreach (var v in vps) v.Enabled = true;
            editToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            tv.Enabled = true;

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close current doc if isnt saved
            if (tv.Enabled)
            {
                if (!doc.Saved)
                    switch (MessageBox.Show(this, "save?", "Saving", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Yes:
                            if (doc.Fname == "")
                            {
                                if (sfd.ShowDialog(this) == DialogResult.Cancel) return;
                                else doc.save(sfd.FileName);
                            }
                            else doc.save(sfd.FileName);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
            }
                doc = new Document(c.Host, c.Port, c.Account, c.Password);

            AttachAllViewportsToDoc();
            unlock();
            updTitle();
            updtpd();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (doc.Fname == "")
            {
                if (sfd.ShowDialog(this) == DialogResult.Cancel) return;
                else doc.save(sfd.FileName);
            }
            else
            doc.save(sfd.FileName);
            updTitle();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!doc.Saved) saveToolStripMenuItem_Click(sender, e);
            Close();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (closeonload) { Close(); return; }
        }
        
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ev = story.Pop();
            ftr.Push(ev);
            ev.undo(doc.Model, tpd1, tpd2, tv);
            redoToolStripMenuItem.Enabled = ftr.Count > 0;
            undoToolStripMenuItem.Enabled = story.Count > 0;
            vps.Redraw();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ev = ftr.Pop();
            story.Push(ev);
            ev.redo(doc.Model, tpd1, tpd2, tv);
            redoToolStripMenuItem.Enabled = ftr.Count > 0;
            undoToolStripMenuItem.Enabled = story.Count > 0;
            vps.Redraw();
        }


        private void borderToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            BorderEditor nedit = new BorderEditor(doc.Model);
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.AddBorder(nedit.res, nedit.res.room1, nedit.res.room2);

            var sn = new TreeNode(nedit.res.ToString());
            tv.Nodes.Add(sn);
            sn.Checked = nedit.res.visible;
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            vps.Redraw();

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }

        private void noiseSourceToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            NoiseSourceEdit nedit = new NoiseSourceEdit(doc.Model);
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.sources.Add(nedit.res);

            var sn = new TreeNode(nedit.res.ToString());
            if (nedit.res.room != null)
                tpd2[nedit.res.room].Nodes.Add(sn);
            else
                tv.Nodes.Add(sn);
            sn.Checked = nedit.res.visible;
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            vps.Redraw();

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }


        private void roomToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            RoomEditor nedit = new RoomEditor();
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.rooms.Add(nedit.res);

            var sn = new TreeNode(nedit.res.ToString());
            tv.Nodes.Add(sn);
            sn.Checked = nedit.res.visible;
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            vps.Redraw();

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }

        private void tv_NodeMouseDoubleClick(Object sender, TreeNodeMouseClickEventArgs e)
        {

            if (tpd1.ContainsKey(e.Node))
            {
                e.Node.Checked = tpd1[e.Node].visible;
                vps.Redraw();
            }

            if (!tpd1.ContainsKey(e.Node)) return;
            var n = tpd1[e.Node];
            var nc = storyrecord.cloneitem(n);
            DialogResult dr = DialogResult.Cancel;
            if (n is NoiseSource)
                if (n is LinearNoiseSrc)
                    dr = new LinearNSEditor((LinearNoiseSrc)n).ShowDialog(this);
                else dr = new NoiseSourceEdit(doc.Model, (NoiseSource)n).ShowDialog(this);
            if (n is calcpoint) dr = new CalcPointEdit(doc.Model, (calcpoint)n).ShowDialog(this);
            if (n is Border) dr = new BorderEditor(doc.Model, (Border)n).ShowDialog(this);
            if (n is Reflector) dr = new ReflectorEditor((Reflector)n).ShowDialog(this);
            if (n is Room) dr = new RoomEditor((Room)n).ShowDialog(this);
            if (dr != DialogResult.Cancel)
            {
                storyrecord srec = storyrecord.RecordUpdate(nc, n);
                story.Push(srec);
                ftr.Clear();
                undoToolStripMenuItem.Enabled = true;
                redoToolStripMenuItem.Enabled = false;
            }

            e.Node.Checked = tpd1[e.Node].visible;
            e.Node.Text = tpd1[e.Node].ToString();

            var mp = tpd1[e.Node];
            if ((mp is Mesh) || mp is PolygonalModel) return;
            Room newroom = null, oldroom = null;
            if (mp is pointModelPart) newroom = ((pointModelPart)mp).room;
            if (e.Node.Parent == null) oldroom = null;
            else oldroom = (Room)tpd1[e.Node.Parent];

            if (newroom == null)
            {
                if (oldroom != null)
                {
                    tpd2[oldroom].Nodes.Remove(e.Node);
                    tv.Nodes.Add(e.Node);
                }
            }
            else
            {
                if (oldroom == null)
                {
                    tv.Nodes.Remove(e.Node);
                    tpd2[newroom].Nodes.Add(e.Node);
                }
                else if (!object.ReferenceEquals(oldroom, newroom))
                {
                    tpd2[oldroom].Nodes.Remove(e.Node);
                    tpd2[newroom].Nodes.Add(e.Node);
                }
            }

            vps.Redraw();

        }
        

        enum action { add, delete, update }
        class storyrecord
        {
            public action a;
            public ModelLib.ModelPart olditem;
            public ModelLib.ModelPart newitem;
            public void undo(Model m, Dictionary<TreeNode, ModelPart> tpd1, Dictionary<ModelPart, TreeNode> tpd2, TreeView tv)
            {
                switch (a)
                {
                    case action.add:
                        if (newitem is NoiseSource)
                        {
                            m.sources.Remove((NoiseSource)newitem);
                            TreeNode n = tpd2[newitem];
                            tv.Nodes.Remove(n);
                            tpd1.Remove(tpd2[newitem]);
                            tpd2.Remove(newitem);
                        }
                        if (newitem is Border)
                        {
                            m.borders.Remove((Border)newitem);
                            TreeNode n = tpd2[newitem];
                            tv.Nodes.Remove(n);
                            tpd1.Remove(tpd2[newitem]);
                            tpd2.Remove(newitem);
                        }
                        if (newitem is Room)
                        {
                            m.rooms.Remove((Room)newitem);
                            TreeNode n = tpd2[newitem];
                            tv.Nodes.Remove(n);
                            tpd1.Remove(tpd2[newitem]);
                            tpd2.Remove(newitem);
                        }
                        if (newitem is calcpoint)
                        {
                            m.calcpts.Remove((calcpoint)newitem);
                            TreeNode n = tpd2[newitem];
                            tv.Nodes.Remove(n);
                            tpd1.Remove(tpd2[newitem]);
                            tpd2.Remove(newitem);
                        }
                        break;
                    case action.delete:
                        if (olditem is NoiseSource)
                        {
                            m.sources.Add((NoiseSource)olditem);

                            TreeNode n = new TreeNode(olditem.ToString());
                            tpd1.Add(n, (NoiseSource)olditem);
                            tpd2.Add((NoiseSource)olditem, n);
                            tv.Nodes.Add(n);
                        }
                        if (olditem is Border)
                        {
                            m.AddBorder(((Border)olditem), ((Border)olditem).room1, ((Border)olditem).room2);

                            TreeNode n = new TreeNode(olditem.ToString());
                            tpd1.Add(n, (Border)olditem);
                            tpd2.Add((Border)olditem, n);
                            tv.Nodes.Add(n);
                        }
                        if (olditem is Room)
                        {
                            m.rooms.Add((Room)olditem);

                            TreeNode n = new TreeNode(olditem.ToString());
                            tpd1.Add(n, (Room)olditem);
                            tpd2.Add((Room)olditem, n);
                            tv.Nodes.Add(n);
                        }
                        if (olditem is calcpoint)
                        {
                            m.calcpts.Add((calcpoint)olditem);

                            TreeNode n = new TreeNode(olditem.ToString());
                            tpd1.Add(n, (calcpoint)olditem);
                            tpd2.Add((calcpoint)olditem, n);
                            tv.Nodes.Add(n);
                        }
                        break;
                    case action.update:
                        //newitem=olditem
                        if (olditem is NoiseSource)
                        {
                            var tmp = ((NoiseSource)newitem).clone();

                            ((NoiseSource)newitem).name = ((NoiseSource)olditem).name;
                            ((NoiseSource)newitem).power = ((NoiseSource)olditem).power;
                            ((NoiseSource)newitem).pos = ((NoiseSource)olditem).pos;
                            ((NoiseSource)newitem).room = ((NoiseSource)olditem).room;

                            olditem = newitem;
                            newitem = tmp;
                        }
                        if (olditem is Border)
                        {
                            var tmp = ((Border)newitem).clone();

                            ((Border)newitem).name = ((Border)olditem).name;
                            ((Border)newitem).layers = ((Border)olditem).layers;
                            ((Border)newitem).Rectangle = ((Border)olditem).Rectangle;
                            ((Border)newitem).room1 = ((Border)olditem).room1;
                            ((Border)newitem).room2 = ((Border)olditem).room2;

                            olditem = newitem;
                            newitem = tmp;
                        }
                        if (olditem is Room)
                        {
                            var tmp = ((Room)newitem).clone();

                            ((Room)newitem).name = ((Room)olditem).name;

                            olditem = newitem;
                            newitem = tmp;
                        }
                        if (olditem is calcpoint)
                        {
                            var tmp = ((calcpoint)newitem).clone();

                            ((calcpoint)newitem).name = ((calcpoint)olditem).name;
                            ((calcpoint)newitem).pos = ((calcpoint)olditem).pos;
                            ((calcpoint)newitem).room = ((calcpoint)olditem).room;

                            olditem = newitem;
                            newitem = tmp;
                        }
                        break;
                }
            }
            public void redo(Model m, Dictionary<TreeNode, ModelPart> tpd1, Dictionary<ModelPart, TreeNode> tpd2, TreeView tv)
            {
                switch (a)
                {
                    case action.add:
                        if (newitem is calcpoint)
                        {
                            m.calcpts.Add((calcpoint)newitem);
                        }
                        if (newitem is Border)
                        {
                            m.borders.Add((Border)newitem);
                        }
                        if (newitem is Room)
                        {
                            m.rooms.Add((Room)newitem);
                        }
                        if (newitem is NoiseSource)
                        {
                            m.sources.Add((NoiseSource)newitem);
                        }
                        var sn = new TreeNode(newitem.ToString());
                        tv.Nodes.Add(sn);
                        tpd1.Add(sn, newitem);
                        tpd2.Add(newitem, sn);
                        break;
                    case action.delete:
                        if (olditem is calcpoint)
                        {
                            m.calcpts.Remove((calcpoint)olditem);
                        }
                        if (olditem is Border)
                        {
                            m.borders.Remove((Border)olditem);
                        }
                        if (olditem is Room)
                        {
                            m.rooms.Remove((Room)olditem);
                        }
                        if (olditem is NoiseSource)
                        {
                            m.sources.Remove((NoiseSource)olditem);
                        }
                        TreeNode n = tpd2[olditem];
                        tv.Nodes.Remove(n);
                        tpd1.Remove(tpd2[olditem]);
                        tpd2.Remove(olditem);
                        break;
                    case action.update:
                        if (olditem is calcpoint)
                        {
                            var tmp = ((calcpoint)olditem).clone();

                            ((calcpoint)olditem).name = ((calcpoint)newitem).name;
                            ((calcpoint)olditem).pos = ((calcpoint)newitem).pos;
                            ((calcpoint)olditem).room = ((calcpoint)newitem).room;

                            newitem = olditem;
                            olditem = tmp;
                        }
                        if (olditem is Border)
                        {
                            var tmp = ((Border)olditem).clone();

                            ((Border)olditem).name = ((Border)newitem).name;
                            ((Border)olditem).layers = ((Border)newitem).layers;
                            ((Border)olditem).Rectangle = ((Border)newitem).Rectangle;
                            ((Border)olditem).room1 = ((Border)newitem).room1;
                            ((Border)olditem).room2 = ((Border)newitem).room2;

                            newitem = olditem;
                            olditem = tmp;
                        }
                        if (olditem is Reflector)
                        {
                            var tmp = ((Reflector)olditem).clone();

                            ((Reflector)olditem).name = ((Reflector)newitem).name;
                            ((Reflector)olditem).matid = ((Reflector)newitem).matid;
                            ((Reflector)olditem).Rectangle = ((Reflector)newitem).Rectangle;

                            newitem = olditem;
                            olditem = tmp;
                        }
                        if (olditem is Room)
                        {
                            var tmp = ((Room)olditem).clone();

                            ((Room)olditem).name = ((Room)newitem).name;

                            newitem = olditem;
                            olditem = tmp;
                        }
                        if (olditem is NoiseSource)
                        {
                            var tmp = ((NoiseSource)olditem).clone();

                            ((NoiseSource)olditem).name = ((NoiseSource)newitem).name;
                            ((NoiseSource)olditem).power = ((NoiseSource)newitem).power.clone();
                            ((NoiseSource)olditem).pos = ((NoiseSource)newitem).pos;
                            ((NoiseSource)olditem).room = ((NoiseSource)newitem).room;

                            newitem = olditem;
                            olditem = tmp;
                        }
                        break;
                }
            }
            public static storyrecord RecordAdd(ModelLib.ModelPart item)
            {
                storyrecord s = new storyrecord();
                s.a = action.add;
                s.newitem = item;
                s.olditem = null;
                return s;
            }
            public static storyrecord RecordRemove(ModelLib.ModelPart item)
            {
                storyrecord s = new storyrecord();
                s.a = action.delete;
                s.olditem = item;
                s.newitem = null;
                return s;
            }
            public static storyrecord RecordUpdate(ModelLib.ModelPart itemOld, ModelLib.ModelPart itemNew)
            {
                storyrecord s = new storyrecord();
                s.a = action.update;
                s.olditem = itemOld;//this is backup
                s.newitem = itemNew;
                return s;
            }
            public static ModelLib.ModelPart cloneitem(ModelLib.ModelPart orig)
            {
                if (orig is NoiseSource)
                {
                    return ((NoiseSource)orig).clone();
                }
                if (orig is Border)
                {
                    return ((Border)orig).clone();
                }
                if (orig is Reflector)
                {
                    return ((Reflector)orig).clone();
                }
                if (orig is calcpoint)
                {
                    return ((calcpoint)orig).clone();
                }
                if (orig is Room)
                {
                    return ((Room)orig).clone();
                }
                return null;
            }
        }

        Stack<storyrecord> story;
        Stack<storyrecord> ftr;


        private void calculationPointToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            CalcPointEdit nedit = new CalcPointEdit(doc.Model);
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.calcpts.Add(nedit.res);

            var sn = new TreeNode(nedit.res.ToString());
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            if (nedit.res.room != null)
                tpd2[nedit.res.room].Nodes.Add(sn);
            else
                tv.Nodes.Add(sn);
            vps.Redraw();

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }

        private void tv_NodeMouseClick(Object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tpd1.ContainsKey(e.Node))
                tpd1[e.Node].visible = e.Node.Checked;
            foreach(var p in tpd1) p.Value.selected = tpd2[p.Value].IsSelected;
            vps.Redraw();
        }

        private void aboutToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog(this);
        }

        private void loadToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            if (Modelselection.ShowDialog() == DialogResult.Cancel) return;
            try
            {
                doc.Model.MeshModel = PolygonalModel.LoadFromFile(Modelselection.FileName);
            }
            catch(Exception )
            {
                MessageBox.Show(string.Format("error occured when tried to load meshed model"));
            }
            vps.Redraw();
            updtpd();
        }

        private void clearToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            doc.Model.MeshModel = new PolygonalModel();
            vps.Redraw();
            updtpd();
        }

        private void tv_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            foreach (var p in tpd1) p.Value.selected = tpd2[p.Value].IsSelected;
            vps.Redraw();
        }

        private void calculatePointsToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            var r= doc.Model.calc();
            CalculationResults rf = new CalculationResults(r);
            rf.ShowDialog(this);
        }

        private void tv_KeyPress(Object sender, KeyPressEventArgs e)
        {
        }

        private void tv_KeyDown(Object sender, KeyEventArgs e)
        {
            var s = tv.SelectedNode;
            if(e.KeyCode==Keys.Delete)
            {
                var mp = tpd1[s];
                if ((mp is PolygonalModel) || (mp is Mesh)) return;
                s.Remove();
                tpd2.Remove(mp);
                tpd1.Remove(s);
                if (mp is Border) doc.Model.borders.Remove((Border)mp);
                if (mp is calcpoint) doc.Model.calcpts.Remove((calcpoint)mp);
                if (mp is NoiseSource) doc.Model.sources.Remove((NoiseSource)mp);
                if (mp is Reflector) doc.Model.reflectors.Remove((Reflector)mp);
                if (mp is Room)
                {
                    doc.Model.rooms.Remove((Room)mp);
                    foreach (var b in doc.Model.borders.Where(p => object.ReferenceEquals(mp, p.room1) || object.ReferenceEquals(mp, p.room2)))
                        if (object.ReferenceEquals(b.room1, mp)) b.room1 = null; else b.room2 = null;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            if (sfd.ShowDialog(this) == DialogResult.Cancel) return;
            doc.saveas(sfd.FileName);
            updTitle();
        }

        private void reflectingSurfaceToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            ReflectorEditor nedit = new ReflectorEditor();
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.reflectors.Add(nedit.res);

            var sn = new TreeNode(nedit.res.ToString());
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            vps.Redraw();

            tv.Nodes.Add(sn);

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }

        private void linearNoiseSourceToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            LinearNSEditor nedit = new LinearNSEditor();
            if (nedit.ShowDialog(this) == DialogResult.Cancel) return;
            doc.Model.sources.Add(nedit.res);

            var sn = new TreeNode(nedit.res.ToString());
            tpd1.Add(sn, nedit.res);
            tpd2.Add(nedit.res, sn);
            vps.Redraw();

            tv.Nodes.Add(sn);

            storyrecord sr = storyrecord.RecordAdd(nedit.res);
            story.Push(sr);
            ftr.Clear();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
        }

        private void viewMDBToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            MaterialsDatabaseEditor.Editor ed = new MaterialsDatabaseEditor.Editor();
            ed.ShowDialog(this);
        }
    }
}