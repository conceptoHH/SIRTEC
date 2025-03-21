namespace SIRTEC.PRESENTACION
{
    partial class ctInscripcion
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtApaterno = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmaterno = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtFnacimiento = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvDocumentos = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnSubirDoc = new System.Windows.Forms.Button();
            this.pnlDocu = new System.Windows.Forms.Panel();
            this.lbTipoDocu = new System.Windows.Forms.ListBox();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.btnGuardarDoc = new System.Windows.Forms.Button();
            this.txtNombreDocumento = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentos)).BeginInit();
            this.pnlDocu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre(s):";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(677, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Completa el siguiente formulario para realizar tu proceso de reinscripcion";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(254, 121);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(368, 26);
            this.txtNombre.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Apellido Paterno:";
            // 
            // txtApaterno
            // 
            this.txtApaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApaterno.Location = new System.Drawing.Point(254, 164);
            this.txtApaterno.Name = "txtApaterno";
            this.txtApaterno.Size = new System.Drawing.Size(368, 26);
            this.txtApaterno.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Apellido Materno:";
            // 
            // txtAmaterno
            // 
            this.txtAmaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmaterno.Location = new System.Drawing.Point(254, 207);
            this.txtAmaterno.Name = "txtAmaterno";
            this.txtAmaterno.Size = new System.Drawing.Size(368, 26);
            this.txtAmaterno.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(51, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Fecha de Nacimiento:";
            // 
            // dtFnacimiento
            // 
            this.dtFnacimiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFnacimiento.Location = new System.Drawing.Point(254, 248);
            this.dtFnacimiento.Name = "dtFnacimiento";
            this.dtFnacimiento.Size = new System.Drawing.Size(368, 26);
            this.dtFnacimiento.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(51, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(197, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "E-mail:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(254, 287);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(368, 26);
            this.txtEmail.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvDocumentos);
            this.panel1.Location = new System.Drawing.Point(664, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 524);
            this.panel1.TabIndex = 3;
            // 
            // dgvDocumentos
            // 
            this.dgvDocumentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocumentos.Location = new System.Drawing.Point(12, 16);
            this.dgvDocumentos.Name = "dgvDocumentos";
            this.dgvDocumentos.RowHeadersWidth = 51;
            this.dgvDocumentos.Size = new System.Drawing.Size(537, 30);
            this.dgvDocumentos.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(660, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(212, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Documentos subidos:";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.Location = new System.Drawing.Point(55, 345);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(194, 56);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnSubirDoc
            // 
            this.btnSubirDoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSubirDoc.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubirDoc.Location = new System.Drawing.Point(427, 345);
            this.btnSubirDoc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSubirDoc.Name = "btnSubirDoc";
            this.btnSubirDoc.Size = new System.Drawing.Size(195, 56);
            this.btnSubirDoc.TabIndex = 6;
            this.btnSubirDoc.Text = "Subir Documentos";
            this.btnSubirDoc.UseVisualStyleBackColor = false;
            this.btnSubirDoc.Click += new System.EventHandler(this.btnSubirDoc_Click);
            // 
            // pnlDocu
            // 
            this.pnlDocu.Controls.Add(this.lbTipoDocu);
            this.pnlDocu.Controls.Add(this.btnExaminar);
            this.pnlDocu.Controls.Add(this.btnGuardarDoc);
            this.pnlDocu.Controls.Add(this.txtNombreDocumento);
            this.pnlDocu.Controls.Add(this.label10);
            this.pnlDocu.Controls.Add(this.label12);
            this.pnlDocu.Location = new System.Drawing.Point(55, 406);
            this.pnlDocu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlDocu.Name = "pnlDocu";
            this.pnlDocu.Size = new System.Drawing.Size(757, 510);
            this.pnlDocu.TabIndex = 7;
            this.pnlDocu.Visible = false;
            // 
            // lbTipoDocu
            // 
            this.lbTipoDocu.FormattingEnabled = true;
            this.lbTipoDocu.Items.AddRange(new object[] {
            "Acta",
            "CURP",
            "INE",
            "Certificado"});
            this.lbTipoDocu.Location = new System.Drawing.Point(227, 75);
            this.lbTipoDocu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lbTipoDocu.Name = "lbTipoDocu";
            this.lbTipoDocu.Size = new System.Drawing.Size(368, 69);
            this.lbTipoDocu.TabIndex = 15;
            // 
            // btnExaminar
            // 
            this.btnExaminar.Font = new System.Drawing.Font("Bahnschrift Condensed", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExaminar.Location = new System.Drawing.Point(609, 58);
            this.btnExaminar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(127, 29);
            this.btnExaminar.TabIndex = 14;
            this.btnExaminar.Text = "Examinar";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // btnGuardarDoc
            // 
            this.btnGuardarDoc.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarDoc.Location = new System.Drawing.Point(28, 183);
            this.btnGuardarDoc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGuardarDoc.Name = "btnGuardarDoc";
            this.btnGuardarDoc.Size = new System.Drawing.Size(194, 56);
            this.btnGuardarDoc.TabIndex = 14;
            this.btnGuardarDoc.Text = "Guardar";
            this.btnGuardarDoc.UseVisualStyleBackColor = true;
            this.btnGuardarDoc.Click += new System.EventHandler(this.btnGuardarDoc_Click);
            // 
            // txtNombreDocumento
            // 
            this.txtNombreDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombreDocumento.Location = new System.Drawing.Point(227, 35);
            this.txtNombreDocumento.Name = "txtNombreDocumento";
            this.txtNombreDocumento.Size = new System.Drawing.Size(368, 26);
            this.txtNombreDocumento.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(24, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(197, 23);
            this.label10.TabIndex = 5;
            this.label10.Text = "Tipo de Documento:";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(24, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(211, 23);
            this.label12.TabIndex = 7;
            this.label12.Text = "Nombre del Documento:";
            // 
            // btnVolver
            // 
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.BackgroundImage = global::SIRTEC.Properties.Resources.cancel;
            this.btnVolver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(1156, 17);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(70, 65);
            this.btnVolver.TabIndex = 6;
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // ctInscripcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlDocu);
            this.Controls.Add(this.btnSubirDoc);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dtFnacimiento);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtAmaterno);
            this.Controls.Add(this.txtApaterno);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "ctInscripcion";
            this.Size = new System.Drawing.Size(1251, 711);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentos)).EndInit();
            this.pnlDocu.ResumeLayout(false);
            this.pnlDocu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtApaterno;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmaterno;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtFnacimiento;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvDocumentos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnSubirDoc;
        private System.Windows.Forms.Panel pnlDocu;
        private System.Windows.Forms.TextBox txtNombreDocumento;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnGuardarDoc;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.ListBox lbTipoDocu;
        private System.Windows.Forms.Button btnVolver;
    }
}
