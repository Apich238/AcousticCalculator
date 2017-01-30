namespace ModelEditor
{
    partial class CalculationResults
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg = new System.Windows.Forms.DataGridView();
            this.CalcPointName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f315 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f63 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f125 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f250 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f500 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f1000 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f2000 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f4000 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f8000 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Totaldba = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();
            // 
            // dg
            // 
            this.dg.AllowUserToAddRows = false;
            this.dg.AllowUserToDeleteRows = false;
            this.dg.AllowUserToResizeRows = false;
            this.dg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CalcPointName,
            this.NSName,
            this.f315,
            this.f63,
            this.f125,
            this.f250,
            this.f500,
            this.f1000,
            this.f2000,
            this.f4000,
            this.f8000,
            this.Totaldba,
            this.posx,
            this.posy,
            this.posz});
            this.dg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg.Location = new System.Drawing.Point(0, 0);
            this.dg.Name = "dg";
            this.dg.RowHeadersVisible = false;
            this.dg.Size = new System.Drawing.Size(1098, 346);
            this.dg.TabIndex = 0;
            // 
            // CalcPointName
            // 
            this.CalcPointName.HeaderText = "Название расчётной точки";
            this.CalcPointName.Name = "CalcPointName";
            this.CalcPointName.ReadOnly = true;
            // 
            // NSName
            // 
            this.NSName.HeaderText = "Название источника шума";
            this.NSName.Name = "NSName";
            // 
            // f315
            // 
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = null;
            this.f315.DefaultCellStyle = dataGridViewCellStyle1;
            this.f315.HeaderText = "f=31.5";
            this.f315.Name = "f315";
            this.f315.ReadOnly = true;
            // 
            // f63
            // 
            dataGridViewCellStyle2.Format = "N3";
            dataGridViewCellStyle2.NullValue = null;
            this.f63.DefaultCellStyle = dataGridViewCellStyle2;
            this.f63.HeaderText = "f=63";
            this.f63.Name = "f63";
            this.f63.ReadOnly = true;
            // 
            // f125
            // 
            dataGridViewCellStyle3.Format = "N3";
            dataGridViewCellStyle3.NullValue = null;
            this.f125.DefaultCellStyle = dataGridViewCellStyle3;
            this.f125.HeaderText = "f=125";
            this.f125.Name = "f125";
            this.f125.ReadOnly = true;
            // 
            // f250
            // 
            dataGridViewCellStyle4.Format = "N3";
            this.f250.DefaultCellStyle = dataGridViewCellStyle4;
            this.f250.HeaderText = "f=250";
            this.f250.Name = "f250";
            this.f250.ReadOnly = true;
            // 
            // f500
            // 
            dataGridViewCellStyle5.Format = "N3";
            this.f500.DefaultCellStyle = dataGridViewCellStyle5;
            this.f500.HeaderText = "f=500";
            this.f500.Name = "f500";
            this.f500.ReadOnly = true;
            // 
            // f1000
            // 
            dataGridViewCellStyle6.Format = "N3";
            this.f1000.DefaultCellStyle = dataGridViewCellStyle6;
            this.f1000.HeaderText = "f=1000";
            this.f1000.Name = "f1000";
            this.f1000.ReadOnly = true;
            // 
            // f2000
            // 
            dataGridViewCellStyle7.Format = "N3";
            this.f2000.DefaultCellStyle = dataGridViewCellStyle7;
            this.f2000.HeaderText = "f=2000";
            this.f2000.Name = "f2000";
            this.f2000.ReadOnly = true;
            // 
            // f4000
            // 
            dataGridViewCellStyle8.Format = "N3";
            this.f4000.DefaultCellStyle = dataGridViewCellStyle8;
            this.f4000.HeaderText = "f=4000";
            this.f4000.Name = "f4000";
            this.f4000.ReadOnly = true;
            // 
            // f8000
            // 
            dataGridViewCellStyle9.Format = "N3";
            this.f8000.DefaultCellStyle = dataGridViewCellStyle9;
            this.f8000.HeaderText = "f=8000";
            this.f8000.Name = "f8000";
            this.f8000.ReadOnly = true;
            // 
            // Totaldba
            // 
            dataGridViewCellStyle10.Format = "N3";
            this.Totaldba.DefaultCellStyle = dataGridViewCellStyle10;
            this.Totaldba.HeaderText = "Уровень по шкале А";
            this.Totaldba.Name = "Totaldba";
            this.Totaldba.ReadOnly = true;
            // 
            // posx
            // 
            dataGridViewCellStyle11.Format = "N3";
            this.posx.DefaultCellStyle = dataGridViewCellStyle11;
            this.posx.HeaderText = "Координата x расчётной точки";
            this.posx.Name = "posx";
            this.posx.ReadOnly = true;
            // 
            // posy
            // 
            dataGridViewCellStyle12.Format = "N3";
            this.posy.DefaultCellStyle = dataGridViewCellStyle12;
            this.posy.HeaderText = "Координата y расчётной точки";
            this.posy.Name = "posy";
            this.posy.ReadOnly = true;
            // 
            // posz
            // 
            dataGridViewCellStyle13.Format = "N3";
            this.posz.DefaultCellStyle = dataGridViewCellStyle13;
            this.posz.HeaderText = "Координата z расчётной точки";
            this.posz.Name = "posz";
            this.posz.ReadOnly = true;
            // 
            // CalculationResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 346);
            this.Controls.Add(this.dg);
            this.Name = "CalculationResults";
            this.Text = "Результаты акустического расчёта";
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalcPointName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn f315;
        private System.Windows.Forms.DataGridViewTextBoxColumn f63;
        private System.Windows.Forms.DataGridViewTextBoxColumn f125;
        private System.Windows.Forms.DataGridViewTextBoxColumn f250;
        private System.Windows.Forms.DataGridViewTextBoxColumn f500;
        private System.Windows.Forms.DataGridViewTextBoxColumn f1000;
        private System.Windows.Forms.DataGridViewTextBoxColumn f2000;
        private System.Windows.Forms.DataGridViewTextBoxColumn f4000;
        private System.Windows.Forms.DataGridViewTextBoxColumn f8000;
        private System.Windows.Forms.DataGridViewTextBoxColumn Totaldba;
        private System.Windows.Forms.DataGridViewTextBoxColumn posx;
        private System.Windows.Forms.DataGridViewTextBoxColumn posy;
        private System.Windows.Forms.DataGridViewTextBoxColumn posz;
    }
}