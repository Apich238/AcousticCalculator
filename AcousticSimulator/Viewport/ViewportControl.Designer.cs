namespace Viewport
{
    partial class ViewportControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.glwnd = new OpenTK.GLControl();
            this.MoveTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // glwnd
            // 
            this.glwnd.BackColor = System.Drawing.Color.Black;
            this.glwnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glwnd.Location = new System.Drawing.Point(0, 0);
            this.glwnd.Name = "glwnd";
            this.glwnd.Size = new System.Drawing.Size(127, 127);
            this.glwnd.TabIndex = 0;
            this.glwnd.VSync = false;
            this.glwnd.Click += new System.EventHandler(this.glwnd_Click);
            this.glwnd.DoubleClick += new System.EventHandler(this.glwnd_DoubleClick);
            this.glwnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glwnd_KeyDown);
            this.glwnd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glwnd_KeyUp);
            this.glwnd.Leave += new System.EventHandler(this.glwnd_Leave);
            this.glwnd.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseClick);
            this.glwnd.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseDoubleClick);
            this.glwnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseDown);
            this.glwnd.MouseEnter += new System.EventHandler(this.glwnd_MouseEnter);
            this.glwnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseMove);
            this.glwnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseUp);
            this.glwnd.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glwnd_MouseWheel);
            this.glwnd.Resize += new System.EventHandler(this.glwnd_Resize);
            // 
            // MoveTimer
            // 
            this.MoveTimer.Interval = 40;
            this.MoveTimer.Tick += new System.EventHandler(this.MoveTimer_Tick);
            // 
            // ViewportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glwnd);
            this.Name = "ViewportControl";
            this.Size = new System.Drawing.Size(127, 127);
            this.Load += new System.EventHandler(this.ViewportControl_Load);
            this.ResumeLayout(false);

        }

        private void Glwnd_Resize(System.Object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private OpenTK.GLControl glwnd;
        private System.Windows.Forms.Timer MoveTimer;
    }
}
