namespace SIRTEC.PRESENTACION
{
    partial class Reinscripcion
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlInfoAlumno = new System.Windows.Forms.Panel();
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblEstadoGeneral = new System.Windows.Forms.Label();
            this.lblSemestreActual = new System.Windows.Forms.Label();
            this.lblNumeroControl = new System.Windows.Forms.Label();
            this.lblNombreAlumno = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDocumentos = new System.Windows.Forms.TabPage();
            this.btnSubirDocumento = new System.Windows.Forms.Button();
            this.lblDocumentosFaltantes = new System.Windows.Forms.Label();
            this.dgvDocumentos = new System.Windows.Forms.DataGridView();
            this.tabMaterias = new System.Windows.Forms.TabPage();
            this.pnlMateriasReprobadas = new System.Windows.Forms.Panel();
            this.dgvMateriasReprobadas = new System.Windows.Forms.DataGridView();
            this.lblMateriasReprobadas = new System.Windows.Forms.Label();
            this.pnlMateriasDisponibles = new System.Windows.Forms.Panel();
            this.lblContadorMaterias = new System.Windows.Forms.Label();
            this.dgvMateriasDisponibles = new System.Windows.Forms.DataGridView();
            this.lblMateriasDisponibles = new System.Windows.Forms.Label();
            this.tabHorario = new System.Windows.Forms.TabPage();
            this.btnConfirmarReinscripcion = new System.Windows.Forms.Button();
            this.btnGenerarPDF = new System.Windows.Forms.Button();
            this.pnlHorario = new System.Windows.Forms.Panel();
            this.dgvHorario = new System.Windows.Forms.DataGridView();
            this.lblVistaPrevia = new System.Windows.Forms.Label();
            this.pnlMensajes = new System.Windows.Forms.Panel();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pnlEncabezado.SuspendLayout();
            this.pnlInfoAlumno.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabDocumentos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentos)).BeginInit();
            this.tabMaterias.SuspendLayout();
            this.pnlMateriasReprobadas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriasReprobadas)).BeginInit();
            this.pnlMateriasDisponibles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriasDisponibles)).BeginInit();
            this.tabHorario.SuspendLayout();
            this.pnlHorario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorario)).BeginInit();
            this.pnlMensajes.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(157)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 1027);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1579, 33);
            this.panel2.TabIndex = 29;
            // 
            // pnlEncabezado
            // 
            this.pnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(157)))));
            this.pnlEncabezado.Controls.Add(this.lblTitulo);
            this.pnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(1579, 60);
            this.pnlEncabezado.TabIndex = 30;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(1579, 60);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "PROCESO DE REINSCRIPCIÓN";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlInfoAlumno
            // 
            this.pnlInfoAlumno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(237)))), ((int)(((byte)(245)))));
            this.pnlInfoAlumno.Controls.Add(this.btnVolver);
            this.pnlInfoAlumno.Controls.Add(this.lblEstadoGeneral);
            this.pnlInfoAlumno.Controls.Add(this.lblSemestreActual);
            this.pnlInfoAlumno.Controls.Add(this.lblNumeroControl);
            this.pnlInfoAlumno.Controls.Add(this.lblNombreAlumno);
            this.pnlInfoAlumno.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfoAlumno.Location = new System.Drawing.Point(0, 60);
            this.pnlInfoAlumno.Name = "pnlInfoAlumno";
            this.pnlInfoAlumno.Size = new System.Drawing.Size(1579, 100);
            this.pnlInfoAlumno.TabIndex = 31;
            // 
            // btnVolver
            // 
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Bahnschrift", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(1425, 30);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(130, 40);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            // 
            // lblEstadoGeneral
            // 
            this.lblEstadoGeneral.AutoSize = true;
            this.lblEstadoGeneral.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoGeneral.ForeColor = System.Drawing.Color.Black;
            this.lblEstadoGeneral.Location = new System.Drawing.Point(25, 55);
            this.lblEstadoGeneral.Name = "lblEstadoGeneral";
            this.lblEstadoGeneral.Size = new System.Drawing.Size(250, 24);
            this.lblEstadoGeneral.TabIndex = 3;
            this.lblEstadoGeneral.Text = "Estado: Proceso pendiente";
            // 
            // lblSemestreActual
            // 
            this.lblSemestreActual.AutoSize = true;
            this.lblSemestreActual.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemestreActual.Location = new System.Drawing.Point(512, 21);
            this.lblSemestreActual.Name = "lblSemestreActual";
            this.lblSemestreActual.Size = new System.Drawing.Size(196, 24);
            this.lblSemestreActual.TabIndex = 2;
            this.lblSemestreActual.Text = "Semestre actual: 1er";
            // 
            // lblNumeroControl
            // 
            this.lblNumeroControl.AutoSize = true;
            this.lblNumeroControl.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroControl.Location = new System.Drawing.Point(512, 55);
            this.lblNumeroControl.Name = "lblNumeroControl";
            this.lblNumeroControl.Size = new System.Drawing.Size(190, 24);
            this.lblNumeroControl.TabIndex = 1;
            this.lblNumeroControl.Text = "Número de control: ";
            // 
            // lblNombreAlumno
            // 
            this.lblNombreAlumno.AutoSize = true;
            this.lblNombreAlumno.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreAlumno.Location = new System.Drawing.Point(25, 21);
            this.lblNombreAlumno.Name = "lblNombreAlumno";
            this.lblNombreAlumno.Size = new System.Drawing.Size(84, 24);
            this.lblNombreAlumno.TabIndex = 0;
            this.lblNombreAlumno.Text = "Alumno:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDocumentos);
            this.tabControl.Controls.Add(this.tabMaterias);
            this.tabControl.Controls.Add(this.tabHorario);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(0, 160);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1579, 867);
            this.tabControl.TabIndex = 32;
            // 
            // tabDocumentos
            // 
            this.tabDocumentos.Controls.Add(this.btnSubirDocumento);
            this.tabDocumentos.Controls.Add(this.lblDocumentosFaltantes);
            this.tabDocumentos.Controls.Add(this.dgvDocumentos);
            this.tabDocumentos.Location = new System.Drawing.Point(4, 30);
            this.tabDocumentos.Name = "tabDocumentos";
            this.tabDocumentos.Padding = new System.Windows.Forms.Padding(3);
            this.tabDocumentos.Size = new System.Drawing.Size(1571, 833);
            this.tabDocumentos.TabIndex = 0;
            this.tabDocumentos.Text = "Verificación de Documentos";
            this.tabDocumentos.UseVisualStyleBackColor = true;
            // 
            // btnSubirDocumento
            // 
            this.btnSubirDocumento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubirDocumento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnSubirDocumento.FlatAppearance.BorderSize = 0;
            this.btnSubirDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubirDocumento.Font = new System.Drawing.Font("Bahnschrift", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubirDocumento.ForeColor = System.Drawing.Color.White;
            this.btnSubirDocumento.Location = new System.Drawing.Point(654, 654);
            this.btnSubirDocumento.Name = "btnSubirDocumento";
            this.btnSubirDocumento.Size = new System.Drawing.Size(262, 50);
            this.btnSubirDocumento.TabIndex = 2;
            this.btnSubirDocumento.Text = "Subir Documento";
            this.btnSubirDocumento.UseVisualStyleBackColor = false;
            // 
            // lblDocumentosFaltantes
            // 
            this.lblDocumentosFaltantes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDocumentosFaltantes.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentosFaltantes.ForeColor = System.Drawing.Color.Red;
            this.lblDocumentosFaltantes.Location = new System.Drawing.Point(3, 3);
            this.lblDocumentosFaltantes.Name = "lblDocumentosFaltantes";
            this.lblDocumentosFaltantes.Size = new System.Drawing.Size(1565, 50);
            this.lblDocumentosFaltantes.TabIndex = 1;
            this.lblDocumentosFaltantes.Text = "DEBES COMPLETAR LOS 4 DOCUMENTOS REQUERIDOS ANTES DE CONTINUAR";
            this.lblDocumentosFaltantes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvDocumentos
            // 
            this.dgvDocumentos.AllowUserToAddRows = false;
            this.dgvDocumentos.AllowUserToDeleteRows = false;
            this.dgvDocumentos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDocumentos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDocumentos.BackgroundColor = System.Drawing.Color.White;
            this.dgvDocumentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocumentos.Location = new System.Drawing.Point(166, 68);
            this.dgvDocumentos.Name = "dgvDocumentos";
            this.dgvDocumentos.ReadOnly = true;
            this.dgvDocumentos.RowHeadersWidth = 51;
            this.dgvDocumentos.RowTemplate.Height = 24;
            this.dgvDocumentos.Size = new System.Drawing.Size(1239, 649);
            this.dgvDocumentos.TabIndex = 0;
            // 
            // tabMaterias
            // 
            this.tabMaterias.Controls.Add(this.pnlMateriasReprobadas);
            this.tabMaterias.Controls.Add(this.pnlMateriasDisponibles);
            this.tabMaterias.Location = new System.Drawing.Point(4, 30);
            this.tabMaterias.Name = "tabMaterias";
            this.tabMaterias.Padding = new System.Windows.Forms.Padding(3);
            this.tabMaterias.Size = new System.Drawing.Size(1571, 833);
            this.tabMaterias.TabIndex = 1;
            this.tabMaterias.Text = "Selección de Materias";
            this.tabMaterias.UseVisualStyleBackColor = true;
            // 
            // pnlMateriasReprobadas
            // 
            this.pnlMateriasReprobadas.Controls.Add(this.dgvMateriasReprobadas);
            this.pnlMateriasReprobadas.Controls.Add(this.lblMateriasReprobadas);
            this.pnlMateriasReprobadas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMateriasReprobadas.Location = new System.Drawing.Point(3, 3);
            this.pnlMateriasReprobadas.Name = "pnlMateriasReprobadas";
            this.pnlMateriasReprobadas.Size = new System.Drawing.Size(1565, 300);
            this.pnlMateriasReprobadas.TabIndex = 1;
            // 
            // dgvMateriasReprobadas
            // 
            this.dgvMateriasReprobadas.AllowUserToAddRows = false;
            this.dgvMateriasReprobadas.AllowUserToDeleteRows = false;
            this.dgvMateriasReprobadas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMateriasReprobadas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMateriasReprobadas.BackgroundColor = System.Drawing.Color.White;
            this.dgvMateriasReprobadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMateriasReprobadas.Location = new System.Drawing.Point(3, 50);
            this.dgvMateriasReprobadas.Name = "dgvMateriasReprobadas";
            this.dgvMateriasReprobadas.ReadOnly = true;
            this.dgvMateriasReprobadas.RowHeadersWidth = 51;
            this.dgvMateriasReprobadas.RowTemplate.Height = 24;
            this.dgvMateriasReprobadas.Size = new System.Drawing.Size(1559, 244);
            this.dgvMateriasReprobadas.TabIndex = 1;
            // 
            // lblMateriasReprobadas
            // 
            this.lblMateriasReprobadas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMateriasReprobadas.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMateriasReprobadas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMateriasReprobadas.Location = new System.Drawing.Point(0, 0);
            this.lblMateriasReprobadas.Name = "lblMateriasReprobadas";
            this.lblMateriasReprobadas.Size = new System.Drawing.Size(1565, 50);
            this.lblMateriasReprobadas.TabIndex = 0;
            this.lblMateriasReprobadas.Text = "MATERIAS REPROBADAS (Inscripción Obligatoria)";
            this.lblMateriasReprobadas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMateriasDisponibles
            // 
            this.pnlMateriasDisponibles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMateriasDisponibles.Controls.Add(this.lblContadorMaterias);
            this.pnlMateriasDisponibles.Controls.Add(this.dgvMateriasDisponibles);
            this.pnlMateriasDisponibles.Controls.Add(this.lblMateriasDisponibles);
            this.pnlMateriasDisponibles.Location = new System.Drawing.Point(3, 309);
            this.pnlMateriasDisponibles.Name = "pnlMateriasDisponibles";
            this.pnlMateriasDisponibles.Size = new System.Drawing.Size(1562, 521);
            this.pnlMateriasDisponibles.TabIndex = 0;
            // 
            // lblContadorMaterias
            // 
            this.lblContadorMaterias.AutoSize = true;
            this.lblContadorMaterias.Font = new System.Drawing.Font("Bahnschrift", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContadorMaterias.Location = new System.Drawing.Point(17, 413);
            this.lblContadorMaterias.Name = "lblContadorMaterias";
            this.lblContadorMaterias.Size = new System.Drawing.Size(300, 22);
            this.lblContadorMaterias.TabIndex = 2;
            this.lblContadorMaterias.Text = "Materias seleccionadas: 0 de 7 max";
            // 
            // dgvMateriasDisponibles
            // 
            this.dgvMateriasDisponibles.AllowUserToAddRows = false;
            this.dgvMateriasDisponibles.AllowUserToDeleteRows = false;
            this.dgvMateriasDisponibles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMateriasDisponibles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMateriasDisponibles.BackgroundColor = System.Drawing.Color.White;
            this.dgvMateriasDisponibles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMateriasDisponibles.Location = new System.Drawing.Point(3, 50);
            this.dgvMateriasDisponibles.Name = "dgvMateriasDisponibles";
            this.dgvMateriasDisponibles.RowHeadersWidth = 51;
            this.dgvMateriasDisponibles.RowTemplate.Height = 24;
            this.dgvMateriasDisponibles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMateriasDisponibles.Size = new System.Drawing.Size(1556, 411);
            this.dgvMateriasDisponibles.TabIndex = 1;
            // 
            // lblMateriasDisponibles
            // 
            this.lblMateriasDisponibles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMateriasDisponibles.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMateriasDisponibles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblMateriasDisponibles.Location = new System.Drawing.Point(0, 0);
            this.lblMateriasDisponibles.Name = "lblMateriasDisponibles";
            this.lblMateriasDisponibles.Size = new System.Drawing.Size(1562, 50);
            this.lblMateriasDisponibles.TabIndex = 0;
            this.lblMateriasDisponibles.Text = "MATERIAS DISPONIBLES PARA EL SIGUIENTE SEMESTRE";
            this.lblMateriasDisponibles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabHorario
            // 
            this.tabHorario.Controls.Add(this.btnConfirmarReinscripcion);
            this.tabHorario.Controls.Add(this.btnGenerarPDF);
            this.tabHorario.Controls.Add(this.pnlHorario);
            this.tabHorario.Controls.Add(this.pnlMensajes);
            this.tabHorario.Location = new System.Drawing.Point(4, 30);
            this.tabHorario.Name = "tabHorario";
            this.tabHorario.Padding = new System.Windows.Forms.Padding(3);
            this.tabHorario.Size = new System.Drawing.Size(1571, 833);
            this.tabHorario.TabIndex = 2;
            this.tabHorario.Text = "Confirmación y Horario";
            this.tabHorario.UseVisualStyleBackColor = true;
            // 
            // btnConfirmarReinscripcion
            // 
            this.btnConfirmarReinscripcion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(74)))));
            this.btnConfirmarReinscripcion.FlatAppearance.BorderSize = 0;
            this.btnConfirmarReinscripcion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarReinscripcion.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmarReinscripcion.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarReinscripcion.Location = new System.Drawing.Point(529, 693);
            this.btnConfirmarReinscripcion.Name = "btnConfirmarReinscripcion";
            this.btnConfirmarReinscripcion.Size = new System.Drawing.Size(300, 50);
            this.btnConfirmarReinscripcion.TabIndex = 3;
            this.btnConfirmarReinscripcion.Text = "Confirmar Reinscripción";
            this.btnConfirmarReinscripcion.UseVisualStyleBackColor = false;
            // 
            // btnGenerarPDF
            // 
            this.btnGenerarPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnGenerarPDF.FlatAppearance.BorderSize = 0;
            this.btnGenerarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarPDF.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarPDF.ForeColor = System.Drawing.Color.White;
            this.btnGenerarPDF.Location = new System.Drawing.Point(861, 693);
            this.btnGenerarPDF.Name = "btnGenerarPDF";
            this.btnGenerarPDF.Size = new System.Drawing.Size(258, 50);
            this.btnGenerarPDF.TabIndex = 2;
            this.btnGenerarPDF.Text = "Generar PDF";
            this.btnGenerarPDF.UseVisualStyleBackColor = false;
            // 
            // pnlHorario
            // 
            this.pnlHorario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHorario.Controls.Add(this.dgvHorario);
            this.pnlHorario.Controls.Add(this.lblVistaPrevia);
            this.pnlHorario.Location = new System.Drawing.Point(3, 125);
            this.pnlHorario.Name = "pnlHorario";
            this.pnlHorario.Size = new System.Drawing.Size(1565, 617);
            this.pnlHorario.TabIndex = 1;
            // 
            // dgvHorario
            // 
            this.dgvHorario.AllowUserToAddRows = false;
            this.dgvHorario.AllowUserToDeleteRows = false;
            this.dgvHorario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHorario.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHorario.BackgroundColor = System.Drawing.Color.White;
            this.dgvHorario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHorario.Location = new System.Drawing.Point(6, 50);
            this.dgvHorario.Name = "dgvHorario";
            this.dgvHorario.ReadOnly = true;
            this.dgvHorario.RowHeadersWidth = 51;
            this.dgvHorario.RowTemplate.Height = 24;
            this.dgvHorario.Size = new System.Drawing.Size(1556, 557);
            this.dgvHorario.TabIndex = 1;
            // 
            // lblVistaPrevia
            // 
            this.lblVistaPrevia.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVistaPrevia.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVistaPrevia.Location = new System.Drawing.Point(0, 0);
            this.lblVistaPrevia.Name = "lblVistaPrevia";
            this.lblVistaPrevia.Size = new System.Drawing.Size(1565, 50);
            this.lblVistaPrevia.TabIndex = 0;
            this.lblVistaPrevia.Text = "VISTA PREVIA DEL HORARIO";
            this.lblVistaPrevia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMensajes
            // 
            this.pnlMensajes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(248)))), ((int)(((byte)(233)))));
            this.pnlMensajes.Controls.Add(this.lblMensajes);
            this.pnlMensajes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMensajes.Location = new System.Drawing.Point(3, 3);
            this.pnlMensajes.Name = "pnlMensajes";
            this.pnlMensajes.Size = new System.Drawing.Size(1565, 100);
            this.pnlMensajes.TabIndex = 0;
            // 
            // lblMensajes
            // 
            this.lblMensajes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMensajes.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensajes.Location = new System.Drawing.Point(0, 0);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(1565, 100);
            this.lblMensajes.TabIndex = 0;
            this.lblMensajes.Text = "Revisa tu horario antes de confirmar la reinscripción";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf|Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*" +
    ".png|Todos los archivos (*.*)|*.*";
            this.openFileDialog.Title = "Seleccionar documento";
            // 
            // Reinscripcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1579, 1060);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlInfoAlumno);
            this.Controls.Add(this.pnlEncabezado);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Reinscripcion";
            this.Text = "Reinscripcion";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlInfoAlumno.ResumeLayout(false);
            this.pnlInfoAlumno.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabDocumentos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentos)).EndInit();
            this.tabMaterias.ResumeLayout(false);
            this.pnlMateriasReprobadas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriasReprobadas)).EndInit();
            this.pnlMateriasDisponibles.ResumeLayout(false);
            this.pnlMateriasDisponibles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriasDisponibles)).EndInit();
            this.tabHorario.ResumeLayout(false);
            this.pnlHorario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorario)).EndInit();
            this.pnlMensajes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlInfoAlumno;
        private System.Windows.Forms.Label lblSemestreActual;
        private System.Windows.Forms.Label lblNumeroControl;
        private System.Windows.Forms.Label lblNombreAlumno;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDocumentos;
        private System.Windows.Forms.TabPage tabMaterias;
        private System.Windows.Forms.TabPage tabHorario;
        private System.Windows.Forms.Label lblEstadoGeneral;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.DataGridView dgvDocumentos;
        private System.Windows.Forms.Button btnSubirDocumento;
        private System.Windows.Forms.Label lblDocumentosFaltantes;
        private System.Windows.Forms.Panel pnlMateriasReprobadas;
        private System.Windows.Forms.DataGridView dgvMateriasReprobadas;
        private System.Windows.Forms.Label lblMateriasReprobadas;
        private System.Windows.Forms.Panel pnlMateriasDisponibles;
        private System.Windows.Forms.Label lblContadorMaterias;
        private System.Windows.Forms.DataGridView dgvMateriasDisponibles;
        private System.Windows.Forms.Label lblMateriasDisponibles;
        private System.Windows.Forms.Panel pnlMensajes;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.Panel pnlHorario;
        private System.Windows.Forms.DataGridView dgvHorario;
        private System.Windows.Forms.Label lblVistaPrevia;
        private System.Windows.Forms.Button btnConfirmarReinscripcion;
        private System.Windows.Forms.Button btnGenerarPDF;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}