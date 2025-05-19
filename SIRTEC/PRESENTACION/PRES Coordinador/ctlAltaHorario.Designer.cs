namespace SIRTEC.PRESENTACION.PRES_Coordinador
{
    partial class ctlAltaHorario
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
            this.btnGuardarMat = new System.Windows.Forms.Button();
            this.txtAula = new System.Windows.Forms.TextBox();
            this.txtN_materia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvVerDocentes = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbPaquete = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbSemestre = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvVerMaterias = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpHora = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVerDocentes)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVerMaterias)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGuardarMat
            // 
            this.btnGuardarMat.Font = new System.Drawing.Font("Bahnschrift Condensed", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarMat.Location = new System.Drawing.Point(52, 668);
            this.btnGuardarMat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGuardarMat.Name = "btnGuardarMat";
            this.btnGuardarMat.Size = new System.Drawing.Size(259, 69);
            this.btnGuardarMat.TabIndex = 18;
            this.btnGuardarMat.Text = "Guardar";
            this.btnGuardarMat.UseVisualStyleBackColor = true;
            this.btnGuardarMat.Click += new System.EventHandler(this.btnGuardarMat_Click);
            // 
            // txtAula
            // 
            this.txtAula.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAula.Location = new System.Drawing.Point(317, 249);
            this.txtAula.Margin = new System.Windows.Forms.Padding(4);
            this.txtAula.Name = "txtAula";
            this.txtAula.Size = new System.Drawing.Size(489, 30);
            this.txtAula.TabIndex = 13;
            // 
            // txtN_materia
            // 
            this.txtN_materia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtN_materia.Location = new System.Drawing.Point(317, 133);
            this.txtN_materia.Margin = new System.Windows.Forms.Padding(4);
            this.txtN_materia.Name = "txtN_materia";
            this.txtN_materia.Size = new System.Drawing.Size(489, 30);
            this.txtN_materia.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(47, 233);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 28);
            this.label4.TabIndex = 9;
            this.label4.Text = "Aula:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(47, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 28);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hora:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 127);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 28);
            this.label1.TabIndex = 11;
            this.label1.Text = "Materia:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvVerDocentes);
            this.panel1.Location = new System.Drawing.Point(317, 306);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 122);
            this.panel1.TabIndex = 19;
            // 
            // dgvVerDocentes
            // 
            this.dgvVerDocentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVerDocentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVerDocentes.Location = new System.Drawing.Point(0, 0);
            this.dgvVerDocentes.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVerDocentes.Name = "dgvVerDocentes";
            this.dgvVerDocentes.RowHeadersWidth = 51;
            this.dgvVerDocentes.Size = new System.Drawing.Size(491, 122);
            this.dgvVerDocentes.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 297);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "Docente:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 524);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(216, 28);
            this.label5.TabIndex = 9;
            this.label5.Text = "Paquete:";
            // 
            // cbPaquete
            // 
            this.cbPaquete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaquete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPaquete.FormattingEnabled = true;
            this.cbPaquete.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.cbPaquete.Location = new System.Drawing.Point(317, 524);
            this.cbPaquete.Margin = new System.Windows.Forms.Padding(4);
            this.cbPaquete.Name = "cbPaquete";
            this.cbPaquete.Size = new System.Drawing.Size(93, 33);
            this.cbPaquete.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(47, 464);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(216, 28);
            this.label6.TabIndex = 9;
            this.label6.Text = "Semestre:";
            // 
            // cbSemestre
            // 
            this.cbSemestre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSemestre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSemestre.FormattingEnabled = true;
            this.cbSemestre.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbSemestre.Location = new System.Drawing.Point(317, 464);
            this.cbSemestre.Margin = new System.Windows.Forms.Padding(4);
            this.cbSemestre.Name = "cbSemestre";
            this.cbSemestre.Size = new System.Drawing.Size(93, 33);
            this.cbSemestre.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(47, 75);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(692, 28);
            this.label7.TabIndex = 11;
            this.label7.Text = "Completa este formulario para dar de alta una materia\r\n";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dgvVerMaterias);
            this.panel2.Location = new System.Drawing.Point(939, 228);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(612, 318);
            this.panel2.TabIndex = 19;
            // 
            // dgvVerMaterias
            // 
            this.dgvVerMaterias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVerMaterias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVerMaterias.Location = new System.Drawing.Point(0, 0);
            this.dgvVerMaterias.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVerMaterias.Name = "dgvVerMaterias";
            this.dgvVerMaterias.RowHeadersWidth = 51;
            this.dgvVerMaterias.Size = new System.Drawing.Size(612, 318);
            this.dgvVerMaterias.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(933, 191);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(692, 28);
            this.label8.TabIndex = 11;
            this.label8.Text = "Materias por paquete:\r\n\r\n\r\n";
            // 
            // dtpHora
            // 
            this.dtpHora.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHora.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHora.Location = new System.Drawing.Point(317, 191);
            this.dtpHora.Margin = new System.Windows.Forms.Padding(4);
            this.dtpHora.Name = "dtpHora";
            this.dtpHora.ShowUpDown = true;
            this.dtpHora.Size = new System.Drawing.Size(489, 30);
            this.dtpHora.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Bahnschrift SemiLight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(47, 581);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(216, 28);
            this.label9.TabIndex = 9;
            this.label9.Text = "Grupo:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "IS1A",
            "IS2A",
            "IS2B",
            "IS2C",
            "IS2D",
            "IS2E",
            "IS2F",
            "IS2G",
            "IS2H",
            "IS2I",
            "IS3A",
            "IS3B",
            "IS4A",
            "IS4B",
            "IS4C",
            "IS4D",
            "IS4E",
            "IS4F"});
            this.comboBox1.Location = new System.Drawing.Point(317, 582);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(93, 33);
            this.comboBox1.TabIndex = 20;
            // 
            // ctlAltaHorario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtpHora);
            this.Controls.Add(this.cbSemestre);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cbPaquete);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnGuardarMat);
            this.Controls.Add(this.txtAula);
            this.Controls.Add(this.txtN_materia);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ctlAltaHorario";
            this.Size = new System.Drawing.Size(1611, 939);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVerDocentes)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVerMaterias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGuardarMat;
        private System.Windows.Forms.TextBox txtAula;
        private System.Windows.Forms.TextBox txtN_materia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvVerDocentes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbPaquete;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbSemestre;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvVerMaterias;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpHora;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
