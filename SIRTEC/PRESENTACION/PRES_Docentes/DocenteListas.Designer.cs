namespace SIRTEC.PRESENTACION.PRES_Docentes
{
    partial class DocenteListas
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNombreDocente = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelSeleccion = new System.Windows.Forms.Panel();
            this.lblSeleccionMateria = new System.Windows.Forms.Label();
            this.cbMaterias = new System.Windows.Forms.ComboBox();
            this.dgvAlumnos = new System.Windows.Forms.DataGridView();
            this.btnExportarPDF = new System.Windows.Forms.Button();
            this.lblTotalAlumnos = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelSeleccion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(157)))));
            this.panel1.Controls.Add(this.lblNombreDocente);
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 80);
            this.panel1.TabIndex = 0;
            // 
            // lblNombreDocente
            // 
            this.lblNombreDocente.AutoSize = true;
            this.lblNombreDocente.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreDocente.ForeColor = System.Drawing.Color.White;
            this.lblNombreDocente.Location = new System.Drawing.Point(12, 47);
            this.lblNombreDocente.Name = "lblNombreDocente";
            this.lblNombreDocente.Size = new System.Drawing.Size(80, 18);
            this.lblNombreDocente.TabIndex = 1;
            this.lblNombreDocente.Text = "Docente: -";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(157, 24);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Listas de Grupo";
            // 
            // panelSeleccion
            // 
            this.panelSeleccion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelSeleccion.Controls.Add(this.lblSeleccionMateria);
            this.panelSeleccion.Controls.Add(this.cbMaterias);
            this.panelSeleccion.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeleccion.Location = new System.Drawing.Point(0, 80);
            this.panelSeleccion.Name = "panelSeleccion";
            this.panelSeleccion.Size = new System.Drawing.Size(800, 60);
            this.panelSeleccion.TabIndex = 1;
            // 
            // lblSeleccionMateria
            // 
            this.lblSeleccionMateria.AutoSize = true;
            this.lblSeleccionMateria.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeleccionMateria.Location = new System.Drawing.Point(12, 20);
            this.lblSeleccionMateria.Name = "lblSeleccionMateria";
            this.lblSeleccionMateria.Size = new System.Drawing.Size(143, 18);
            this.lblSeleccionMateria.TabIndex = 1;
            this.lblSeleccionMateria.Text = "Seleccione Materia:";
            // 
            // cbMaterias
            // 
            this.cbMaterias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterias.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMaterias.FormattingEnabled = true;
            this.cbMaterias.Location = new System.Drawing.Point(161, 17);
            this.cbMaterias.Name = "cbMaterias";
            this.cbMaterias.Size = new System.Drawing.Size(343, 26);
            this.cbMaterias.TabIndex = 0;
            this.cbMaterias.SelectedIndexChanged += new System.EventHandler(this.cbMaterias_SelectedIndexChanged);
            // 
            // dgvAlumnos
            // 
            this.dgvAlumnos.AllowUserToAddRows = false;
            this.dgvAlumnos.AllowUserToDeleteRows = false;
            this.dgvAlumnos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlumnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlumnos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlumnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnos.Location = new System.Drawing.Point(15, 146);
            this.dgvAlumnos.Name = "dgvAlumnos";
            this.dgvAlumnos.ReadOnly = true;
            this.dgvAlumnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlumnos.Size = new System.Drawing.Size(773, 251);
            this.dgvAlumnos.TabIndex = 2;
            // 
            // btnExportarPDF
            // 
            this.btnExportarPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(157)))));
            this.btnExportarPDF.FlatAppearance.BorderSize = 0;
            this.btnExportarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportarPDF.Location = new System.Drawing.Point(633, 403);
            this.btnExportarPDF.Name = "btnExportarPDF";
            this.btnExportarPDF.Size = new System.Drawing.Size(155, 35);
            this.btnExportarPDF.TabIndex = 3;
            this.btnExportarPDF.Text = "Exportar a PDF";
            this.btnExportarPDF.UseVisualStyleBackColor = false;
            this.btnExportarPDF.Click += new System.EventHandler(this.btnExportarPDF_Click);
            // 
            // lblTotalAlumnos
            // 
            this.lblTotalAlumnos.AutoSize = true;
            this.lblTotalAlumnos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAlumnos.Location = new System.Drawing.Point(12, 412);
            this.lblTotalAlumnos.Name = "lblTotalAlumnos";
            this.lblTotalAlumnos.Size = new System.Drawing.Size(138, 17);
            this.lblTotalAlumnos.TabIndex = 4;
            this.lblTotalAlumnos.Text = "Total de alumnos: 0";
            // 
            // DocenteListas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblTotalAlumnos);
            this.Controls.Add(this.btnExportarPDF);
            this.Controls.Add(this.dgvAlumnos);
            this.Controls.Add(this.panelSeleccion);
            this.Controls.Add(this.panel1);
            this.Name = "DocenteListas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Listas de Alumnos";
            this.Load += new System.EventHandler(this.DocenteListas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelSeleccion.ResumeLayout(false);
            this.panelSeleccion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNombreDocente;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelSeleccion;
        private System.Windows.Forms.Label lblSeleccionMateria;
        private System.Windows.Forms.ComboBox cbMaterias;
        private System.Windows.Forms.DataGridView dgvAlumnos;
        private System.Windows.Forms.Button btnExportarPDF;
        private System.Windows.Forms.Label lblTotalAlumnos;
    }
}