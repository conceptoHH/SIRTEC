namespace SIRTEC.PRESENTACION.PRES_Coordinador
{
    partial class ctlAltaDocente
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRegistrarDoc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUsuarioDoc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtContrasenaDoc = new System.Windows.Forms.TextBox();
            this.txtNomDoc = new System.Windows.Forms.TextBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(497, 174);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 29);
            this.label3.TabIndex = 32;
            this.label3.Text = "Nombre:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Bahnschrift Condensed", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1405, 66);
            this.label1.TabIndex = 31;
            this.label1.Text = "Alta de Docentes";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRegistrarDoc
            // 
            this.btnRegistrarDoc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRegistrarDoc.AutoEllipsis = true;
            this.btnRegistrarDoc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRegistrarDoc.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarDoc.Location = new System.Drawing.Point(599, 641);
            this.btnRegistrarDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRegistrarDoc.Name = "btnRegistrarDoc";
            this.btnRegistrarDoc.Size = new System.Drawing.Size(200, 87);
            this.btnRegistrarDoc.TabIndex = 30;
            this.btnRegistrarDoc.Text = "Registrar Docentes";
            this.btnRegistrarDoc.UseVisualStyleBackColor = true;
            this.btnRegistrarDoc.Click += new System.EventHandler(this.btnRegistrarDoc_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label4.Location = new System.Drawing.Point(497, 325);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 29);
            this.label4.TabIndex = 28;
            this.label4.Text = "Usuario:";
            // 
            // txtUsuarioDoc
            // 
            this.txtUsuarioDoc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtUsuarioDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuarioDoc.Location = new System.Drawing.Point(501, 384);
            this.txtUsuarioDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUsuarioDoc.Name = "txtUsuarioDoc";
            this.txtUsuarioDoc.Size = new System.Drawing.Size(395, 34);
            this.txtUsuarioDoc.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label5.Location = new System.Drawing.Point(497, 466);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 29);
            this.label5.TabIndex = 29;
            this.label5.Text = "Contraseña:";
            // 
            // txtContrasenaDoc
            // 
            this.txtContrasenaDoc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtContrasenaDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContrasenaDoc.Location = new System.Drawing.Point(501, 530);
            this.txtContrasenaDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtContrasenaDoc.Name = "txtContrasenaDoc";
            this.txtContrasenaDoc.Size = new System.Drawing.Size(395, 34);
            this.txtContrasenaDoc.TabIndex = 27;
            // 
            // txtNomDoc
            // 
            this.txtNomDoc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtNomDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomDoc.Location = new System.Drawing.Point(501, 236);
            this.txtNomDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNomDoc.Name = "txtNomDoc";
            this.txtNomDoc.Size = new System.Drawing.Size(395, 34);
            this.txtNomDoc.TabIndex = 25;
            this.txtNomDoc.Tag = "";
            // 
            // btnVolver
            // 
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.BackgroundImage = global::SIRTEC.Properties.Resources.cancel;
            this.btnVolver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(1143, 81);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(93, 80);
            this.btnVolver.TabIndex = 33;
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // ctlAltaDocente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRegistrarDoc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUsuarioDoc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtContrasenaDoc);
            this.Controls.Add(this.txtNomDoc);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ctlAltaDocente";
            this.Size = new System.Drawing.Size(1405, 896);
            this.Load += new System.EventHandler(this.ctlAltaDocente_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRegistrarDoc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUsuarioDoc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtContrasenaDoc;
        private System.Windows.Forms.TextBox txtNomDoc;
        private System.Windows.Forms.Button btnVolver;
    }
}
