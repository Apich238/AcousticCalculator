namespace ModelEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noiseSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculationPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reflectingSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linearNoiseSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculatePointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphicsModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialsDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.Modelselection = new System.Windows.Forms.OpenFileDialog();
            this.tv = new System.Windows.Forms.TreeView();
            this.vp1 = new Viewport.ViewportControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.viewMDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveToolStripMenuItem.Text = "Сохранить";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.graphicsModelToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.materialsDBToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(991, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.newToolStripMenuItem.Text = "Новый";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openToolStripMenuItem.Text = "Открыть";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveAsToolStripMenuItem.Text = "Сохранить как";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.addToolStripMenuItem,
            this.calculatePointsToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.editToolStripMenuItem.Text = "Правка";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.undoToolStripMenuItem.Text = "Отменить";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.redoToolStripMenuItem.Text = "Вернуть";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.borderToolStripMenuItem,
            this.noiseSourceToolStripMenuItem,
            this.calculationPointToolStripMenuItem,
            this.roomToolStripMenuItem,
            this.reflectingSurfaceToolStripMenuItem,
            this.linearNoiseSourceToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.addToolStripMenuItem.Text = "Добавить";
            // 
            // borderToolStripMenuItem
            // 
            this.borderToolStripMenuItem.Name = "borderToolStripMenuItem";
            this.borderToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.borderToolStripMenuItem.Text = "Акустическую перегородку";
            this.borderToolStripMenuItem.Click += new System.EventHandler(this.borderToolStripMenuItem_Click);
            // 
            // noiseSourceToolStripMenuItem
            // 
            this.noiseSourceToolStripMenuItem.Name = "noiseSourceToolStripMenuItem";
            this.noiseSourceToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.noiseSourceToolStripMenuItem.Text = "Источник шума";
            this.noiseSourceToolStripMenuItem.Click += new System.EventHandler(this.noiseSourceToolStripMenuItem_Click);
            // 
            // calculationPointToolStripMenuItem
            // 
            this.calculationPointToolStripMenuItem.Name = "calculationPointToolStripMenuItem";
            this.calculationPointToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.calculationPointToolStripMenuItem.Text = "Расчётная точка";
            this.calculationPointToolStripMenuItem.Click += new System.EventHandler(this.calculationPointToolStripMenuItem_Click);
            // 
            // roomToolStripMenuItem
            // 
            this.roomToolStripMenuItem.Name = "roomToolStripMenuItem";
            this.roomToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.roomToolStripMenuItem.Text = "Помещение";
            this.roomToolStripMenuItem.Click += new System.EventHandler(this.roomToolStripMenuItem_Click);
            // 
            // reflectingSurfaceToolStripMenuItem
            // 
            this.reflectingSurfaceToolStripMenuItem.Name = "reflectingSurfaceToolStripMenuItem";
            this.reflectingSurfaceToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.reflectingSurfaceToolStripMenuItem.Text = "Отражающая поверхность";
            this.reflectingSurfaceToolStripMenuItem.Click += new System.EventHandler(this.reflectingSurfaceToolStripMenuItem_Click);
            // 
            // linearNoiseSourceToolStripMenuItem
            // 
            this.linearNoiseSourceToolStripMenuItem.Name = "linearNoiseSourceToolStripMenuItem";
            this.linearNoiseSourceToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.linearNoiseSourceToolStripMenuItem.Text = "Линейный источник шума";
            this.linearNoiseSourceToolStripMenuItem.Click += new System.EventHandler(this.linearNoiseSourceToolStripMenuItem_Click);
            // 
            // calculatePointsToolStripMenuItem
            // 
            this.calculatePointsToolStripMenuItem.Name = "calculatePointsToolStripMenuItem";
            this.calculatePointsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.calculatePointsToolStripMenuItem.Text = "Акустический расчёт";
            this.calculatePointsToolStripMenuItem.Click += new System.EventHandler(this.calculatePointsToolStripMenuItem_Click);
            // 
            // graphicsModelToolStripMenuItem
            // 
            this.graphicsModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.graphicsModelToolStripMenuItem.Name = "graphicsModelToolStripMenuItem";
            this.graphicsModelToolStripMenuItem.Size = new System.Drawing.Size(135, 20);
            this.graphicsModelToolStripMenuItem.Text = "Графическая модель";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.loadToolStripMenuItem.Text = "Загрузить";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.clearToolStripMenuItem.Text = "Убрать";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.helpToolStripMenuItem.Text = "Помощь";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // materialsDBToolStripMenuItem
            // 
            this.materialsDBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMDBToolStripMenuItem});
            this.materialsDBToolStripMenuItem.Name = "materialsDBToolStripMenuItem";
            this.materialsDBToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.materialsDBToolStripMenuItem.Text = "БД материалов";
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "am";
            this.ofd.FileName = "newmodel.am";
            this.ofd.Filter = "\"Mmodel files|*.am|All files|*\"";
            this.ofd.Title = "Open file model";
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "am";
            this.sfd.FileName = "newmodel.am";
            this.sfd.Title = "Save model to file";
            // 
            // Modelselection
            // 
            this.Modelselection.Title = "Select mesh file for model";
            // 
            // tv
            // 
            this.tv.CheckBoxes = true;
            this.tv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv.Enabled = false;
            this.tv.Location = new System.Drawing.Point(0, 0);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(200, 444);
            this.tv.TabIndex = 7;
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_NodeMouseClick);
            this.tv.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_NodeMouseDoubleClick);
            this.tv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tv_KeyDown);
            this.tv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tv_KeyPress);
            // 
            // vp1
            // 
            this.vp1.CameraPosX = 5F;
            this.vp1.CameraPosY = 5F;
            this.vp1.CameraPosZ = 3F;
            this.vp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vp1.Enabled = false;
            this.vp1.Location = new System.Drawing.Point(0, 0);
            this.vp1.Name = "vp1";
            this.vp1.OwnerList = null;
            this.vp1.Size = new System.Drawing.Size(786, 444);
            this.vp1.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tv);
            this.splitContainer1.Panel1MinSize = 60;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vp1);
            this.splitContainer1.Panel2MinSize = 60;
            this.splitContainer1.Size = new System.Drawing.Size(991, 444);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 9;
            // 
            // viewMDBToolStripMenuItem
            // 
            this.viewMDBToolStripMenuItem.Name = "viewMDBToolStripMenuItem";
            this.viewMDBToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.viewMDBToolStripMenuItem.Text = "Просмотр";
            this.viewMDBToolStripMenuItem.Click += new System.EventHandler(this.viewMDBToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 468);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Акустический расчёт железнодорожного вагона";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.OpenFileDialog Modelselection;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noiseSourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculationPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roomToolStripMenuItem;
        private Viewport.ViewportControl vp1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphicsModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculatePointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reflectingSurfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linearNoiseSourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem materialsDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMDBToolStripMenuItem;
    }
}