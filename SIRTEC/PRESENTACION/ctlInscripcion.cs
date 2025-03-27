using SIRTEC.DATOS;
using SIRTEC.LOGICA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;


namespace SIRTEC.PRESENTACION
{
    public partial class ctlInscripcion : UserControl
    {
        int semestre = 1;
        // Lista para almacenar documentos seleccionados temporalmente
        private List<Ldocumentos> documentosSeleccionados = new List<Ldocumentos>();

        public ctlInscripcion()
        {
            InitializeComponent();
            // Configurar el DataGridView
            ConfigurarDataGridView();
            // Cargar los documentos al iniciar
            CargarDocumentos();
            // Configurar validaciones de campos
            ConfigurarValidacionesCampos();

            // Agregar manejador para cuando se descarte el control
            this.Disposed += CtlInscripcion_Disposed;
        }

        private void CtlInscripcion_Disposed(object sender, EventArgs e)
        {
            // Si hay documentos sin asociar, eliminarlos al descartar el control
            if (HayDocumentosSinAsociar())
            {
                EliminarDocumentosSinAsociar();
            }
        }

        private void ConfigurarDataGridView()
        {
            // Configuración básica
            dgvDocumentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocumentos.AllowUserToAddRows = false;
            dgvDocumentos.AllowUserToDeleteRows = false;
            dgvDocumentos.ReadOnly = true;
            dgvDocumentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocumentos.MultiSelect = false;
            dgvDocumentos.RowHeadersVisible = false;

            // Establecer el tamaño de fuente para todo el DataGridView
            dgvDocumentos.DefaultCellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);

            // Estilo para encabezados de columnas
            dgvDocumentos.ColumnHeadersDefaultCellStyle.Font = new Font("Bahnschrift", 11F, FontStyle.Bold);
            dgvDocumentos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); // Azul oscuro elegante
            dgvDocumentos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDocumentos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDocumentos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDocumentos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvDocumentos.ColumnHeadersHeight = 35; // Altura más pronunciada para los encabezados

            // Estilo para celdas alternadas
            dgvDocumentos.RowsDefaultCellStyle.BackColor = Color.White;
            dgvDocumentos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242); // Gris muy claro

            // Bordes y estilo de selección
            dgvDocumentos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDocumentos.GridColor = Color.FromArgb(224, 224, 224); // Gris claro para las líneas

            // Estilo de selección
            dgvDocumentos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185); // Azul claro
            dgvDocumentos.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alineación de texto por defecto
            dgvDocumentos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Quitar el efecto focus para que no se vea un borde extra al seleccionar
            dgvDocumentos.StandardTab = true;

            // Agregar un botón contextual para eliminar documentos
            dgvDocumentos.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem eliminarItem = new ToolStripMenuItem("Eliminar documento");
            eliminarItem.Image = Properties.Resources.cancel; // Asegúrate de tener este recurso o cámbialo
            eliminarItem.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);
            eliminarItem.Click += (sender, e) => EliminarDocumentoSeleccionado();
            dgvDocumentos.ContextMenuStrip.Items.Add(eliminarItem);

            // Agregar evento CellFormatting para personalizar cada tipo de documento
            dgvDocumentos.CellFormatting += DgvDocumentos_CellFormatting;

            // Agregar evento para cuando el mouse está sobre una fila
            dgvDocumentos.CellMouseEnter += DgvDocumentos_CellMouseEnter;
            dgvDocumentos.CellMouseLeave += DgvDocumentos_CellMouseLeave;
        }

        private void DgvDocumentos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Si es la columna de "Tipo"
                if (e.ColumnIndex == 2) // Ajusta este índice según tu orden de columnas
                {
                    string tipoDoc = e.Value?.ToString() ?? "";

                    switch (tipoDoc)
                    {
                        case "Acta":
                            e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96); // Verde
                            e.CellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
                            break;
                        case "CURP":
                            e.CellStyle.ForeColor = Color.FromArgb(142, 68, 173); // Morado
                            e.CellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
                            break;
                        case "Certificado":
                            e.CellStyle.ForeColor = Color.FromArgb(211, 84, 0); // Naranja
                            e.CellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
                            break;
                        case "INE":
                            e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185); // Azul
                            e.CellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
                            break;
                        default:
                            e.CellStyle.ForeColor = Color.Black;
                            break;
                    }
                }
            }
        }

        private void DgvDocumentos_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvDocumentos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 245, 251); // Azul muy claro al pasar el mouse
            }
        }

        private void DgvDocumentos_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Restaurar el color original (normal o alternado)
                if (e.RowIndex % 2 == 0)
                    dgvDocumentos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dgvDocumentos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
            }
        }


        private void CargarDocumentos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    // Consulta SQL para obtener documentos que aún no están asociados a un alumno
                    string query = @"SELECT id_documento, n_documento, extension, tipo_documento 
                                       FROM Documentos 
                                       WHERE id_alumno IS NULL";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Configurar las columnas del DataGridView si no están configuradas
                    if (dgvDocumentos.Columns.Count == 0)
                    {
                        // Crear las columnas con estilos personalizados
                        DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn
                        {
                            Name = "Nombre",
                            HeaderText = "Nombre del Documento",
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                            FillWeight = 50
                        };

                        DataGridViewTextBoxColumn colExtension = new DataGridViewTextBoxColumn
                        {
                            Name = "Extension",
                            HeaderText = "Extensión",
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                            FillWeight = 15,
                            DefaultCellStyle = new DataGridViewCellStyle
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        };

                        DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn
                        {
                            Name = "Tipo",
                            HeaderText = "Tipo de Documento",
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                            FillWeight = 35,
                            DefaultCellStyle = new DataGridViewCellStyle
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        };

                        dgvDocumentos.Columns.Add(colNombre);
                        dgvDocumentos.Columns.Add(colExtension);
                        dgvDocumentos.Columns.Add(colTipo);
                    }

                    // Limpiar el DataGridView
                    dgvDocumentos.Rows.Clear();

                    // Agregar las filas al DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvDocumentos.Rows.Add(
                            row["n_documento"],
                            row["extension"],
                            row["tipo_documento"]
                        );
                    }

                    // Ajustar el tamaño del DataGridView según la cantidad de datos
                    int minHeight = 200; // Altura mínima
                    int maxHeight = 500; // Altura máxima
                    int rowsHeight = 60 + (dgvDocumentos.Rows.Count * dgvDocumentos.RowTemplate.Height);

                    dgvDocumentos.Height = Math.Max(minHeight, Math.Min(maxHeight, rowsHeight));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los documentos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar Documento";
                openFileDialog.Filter = "Archivos de Documento|*.pdf;";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        // Verificar si el archivo excede 500 KB (512000 bytes)
                        if (fileInfo.Length > 512000)
                        {
                            MessageBox.Show($"El archivo {fileInfo.Name} excede el tamaño máximo de 500 KB.",
                                "Archivo demasiado grande", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        // Crear una instancia de Documento
                        Ldocumentos doc = new Ldocumentos
                        {
                            n_documento = Path.GetFileName(file),
                            extension = Path.GetExtension(file),
                            tipoDocumento = string.Empty,
                            contenido = File.ReadAllBytes(file)
                        };

                        // Agregar el documento a la lista
                        documentosSeleccionados.Add(doc);
                        txtNombreDocumento.Text = doc.n_documento;
                    }
                }
            }
        }

        private void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            if (documentosSeleccionados.Count == 0)
            {
                MessageBox.Show("No hay documentos para guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Dentro de btnGuardar_Click, antes de la inserción
            if (!ExisteSemestre(semestre))
            {
                // Si el semestre no existe, intentamos crearlo
                int idSemestreCreado = CrearSemestre(semestre);
                if (idSemestreCreado == 0)
                {
                    MessageBox.Show("El semestre 1 no existe y no se pudo crear. Por favor, contacte al administrador.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    semestre = idSemestreCreado; // Usar el ID del semestre creado
                    MessageBox.Show($"Se ha creado automáticamente el semestre {semestre}.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (lbTipoDocu.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un tipo de documento.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asignar el tipo de documento al último documento agregado
            string tipoDocumentoSeleccionado = lbTipoDocu.Text;
            documentosSeleccionados[documentosSeleccionados.Count - 1].tipoDocumento = tipoDocumentoSeleccionado;

            // Verificar si ya existe un documento del mismo tipo
            if (ExisteTipoDocumento(tipoDocumentoSeleccionado))
            {
                MessageBox.Show($"Ya existe un documento de tipo '{tipoDocumentoSeleccionado}'. No se permite subir documentos duplicados del mismo tipo.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar los documentos en la base de datos con id_alumno como NULL
            foreach (var docu in documentosSeleccionados)
            {
                InsertarDocumentoEnBD(docu);
            }

            // Cargar los documentos actualizados
            CargarDocumentos();

            // Verificar cuáles documentos obligatorios faltan por subir
            MostrarDocumentosFaltantes();

            // Limpiar la lista y la interfaz
            documentosSeleccionados.Clear();
            txtNombreDocumento.Text = string.Empty;
            pnlDocu.Visible = false;
        }

        /// <summary>
        /// Muestra los documentos obligatorios que faltan por subir
        /// </summary>
        private void MostrarDocumentosFaltantes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"SELECT tipo_documento FROM Documentos WHERE id_alumno IS NULL";

                    HashSet<string> tiposDocumentos = new HashSet<string>();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tipoDoc = reader["tipo_documento"].ToString();
                                tiposDocumentos.Add(tipoDoc);
                            }
                        }
                    }

                    List<string> documentosFaltantes = new List<string>();

                    if (!tiposDocumentos.Contains("Acta"))
                    {
                        documentosFaltantes.Add("Acta de nacimiento");
                    }

                    if (!tiposDocumentos.Contains("CURP"))
                    {
                        documentosFaltantes.Add("CURP");
                    }

                    if (!tiposDocumentos.Contains("Certificado") && !tiposDocumentos.Contains("INE"))
                    {
                        documentosFaltantes.Add("Certificado o INE");
                    }

                    if (documentosFaltantes.Count > 0)
                    {
                        string mensajeFaltantes = "Documentos subidos correctamente.\n\nDocumentos pendientes por subir:\n- " +
                                                 string.Join("\n- ", documentosFaltantes);
                        MessageBox.Show(mensajeFaltantes, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("¡Todos los documentos obligatorios han sido subidos correctamente!\n" +
                                      "Se asociarán al alumno una vez completada la inscripción.",
                                      "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar documentos faltantes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Método para insertar un documento en la base de datos.
        /// </summary>
        /// <param name="doc">Documento a insertar.</param>
        private void InsertarDocumentoEnBD(Ldocumentos doc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"INSERT INTO Documentos (n_documento, extension, tipo_documento, contenido, id_alumno)
                                                 VALUES (@n_documento, @extension, @tipo_documento, @contenido, NULL)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@n_documento", doc.n_documento);
                        cmd.Parameters.AddWithValue("@extension", doc.extension);
                        cmd.Parameters.AddWithValue("@tipo_documento", doc.tipoDocumento);
                        cmd.Parameters.AddWithValue("@contenido", doc.contenido);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al subir el documento {doc.n_documento}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubirDoc_Click(object sender, EventArgs e)
        {
            pnlDocu.Visible = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar los datos de inscripción
            if (!ValidarDatosInscripcion())
            {
                return; // Si no pasa las validaciones, no continuar
            }

            // Verificar si existe el semestre
            if (!ExisteSemestre(semestre))
            {
                // Si el semestre no existe, intentamos crearlo
                int idSemestreCreado = CrearSemestre(semestre);
                if (idSemestreCreado == 0)
                {
                    MessageBox.Show("El semestre 1 no existe y no se pudo crear. Por favor, contacte al administrador.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    semestre = idSemestreCreado; // Usar el ID del semestre creado
                }
            }

            try
            {
                // Obtener el siguiente ID disponible
                int idAlumno = ObtenerSiguienteIdAlumno();

                // Generar el número de control (2532 + ID)
                int numeroControl = 253200 + idAlumno;

                // Crear la consulta para insertar el alumno
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();

                    // Iniciar una transacción para asegurar integridad
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insertar alumno con los campos de dirección
                            string query = @"INSERT INTO Alumnos (id_alumno, nombre, a_paterno, a_materno, e_mail, f_nacimiento, 
                                      n_control, id_semestre, calle, colonia, codpostal, num_exterior, ciudad, estado)
                                      VALUES (@id_alumno, @nombre, @a_paterno, @a_materno, @e_mail, @f_nacimiento, 
                                      @n_control, @id_semestre, @calle, @colonia, @codigo_postal, @numero, @ciudad, @estado)";

                            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                                cmd.Parameters.AddWithValue("@a_paterno", txtApaterno.Text);
                                cmd.Parameters.AddWithValue("@a_materno", txtAmaterno.Text);
                                cmd.Parameters.AddWithValue("@e_mail", txtEmail.Text);
                                cmd.Parameters.AddWithValue("@f_nacimiento", dtFnacimiento.Value);
                                cmd.Parameters.AddWithValue("@n_control", numeroControl);
                                cmd.Parameters.AddWithValue("@id_semestre", semestre);
                                // Nuevos campos de dirección
                                cmd.Parameters.AddWithValue("@calle", txtCalle.Text);
                                cmd.Parameters.AddWithValue("@colonia", txtColonia.Text);
                                cmd.Parameters.AddWithValue("@numero", txtNumero.Text);
                                cmd.Parameters.AddWithValue("@codigo_postal", txtCodigoP.Text);
                                cmd.Parameters.AddWithValue("@ciudad", txtCiudad.Text);
                                cmd.Parameters.AddWithValue("@estado", cbEstado.SelectedItem.ToString());

                                cmd.ExecuteNonQuery();
                            }

                            // Crear un usuario para el alumno
                            // Obtener el siguiente ID de usuario
                            int idUsuario = 1;
                            string queryId = "SELECT ISNULL(MAX(id_usuarios), 0) + 1 FROM Usuarios";
                            using (SqlCommand cmdId = new SqlCommand(queryId, conn, transaction))
                            {
                                idUsuario = Convert.ToInt32(cmdId.ExecuteScalar());
                            }

                            // Insertar usuario
                            string queryUsuario = @"INSERT INTO Usuarios (id_usuarios, tipo_usuario, username, password, id_alumno, id_docente, id_coordinador)
                                        VALUES (@id_usuarios, 'alumno', @user, @password, @id_alumno, NULL, NULL)";

                            using (SqlCommand cmd = new SqlCommand(queryUsuario, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@id_usuarios", idUsuario);
                                cmd.Parameters.AddWithValue("@user", numeroControl);
                                cmd.Parameters.AddWithValue("@password", numeroControl);
                                cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                                cmd.ExecuteNonQuery();
                            }

                            // Asociar documentos al alumno
                            string queryAsociarDocs = @"UPDATE Documentos
                                                      SET id_alumno = @id_alumno
                                                      WHERE id_alumno IS NULL";

                            using (SqlCommand cmd = new SqlCommand(queryAsociarDocs, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                                cmd.ExecuteNonQuery();
                            }

                            // Confirmar la transacción
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Revertir la transacción en caso de error
                            transaction.Rollback();
                            throw new Exception($"Error en la transacción: {ex.Message}");
                        }
                    }

                    // Actualizar la vista de documentos
                    CargarDocumentos();

                    // Asignar horario al alumno (después de la transacción para no afectarla)
                    bool horarioAsignado = AsignarHorarioAlumno(idAlumno, semestre);

                    MessageBox.Show($"¡Inscripción completada con éxito!\nNúmero de Control: {numeroControl}\n" +
                                   $"Este número será su usuario y contraseña para acceder al sistema.",
                                   "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Si se asignó el horario correctamente, ofrecer ver/descargar el horario
                    if (horarioAsignado)
                    {
                        DialogResult result = MessageBox.Show("¿Desea ver y descargar su horario?",
                            "Horario disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            MostrarVistaPrevia(idAlumno, numeroControl);
                        }
                    }

                    // Limpiar los campos del formulario
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inscribir al alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene el siguiente ID disponible para un nuevo alumno.
        /// </summary>
        private int ObtenerSiguienteIdAlumno()
        {
            int nextId = 1;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "SELECT ISNULL(MAX(id_alumno), 0) + 1 FROM Alumnos";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        nextId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el siguiente ID: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-lanzar la excepción para que se maneje en el método que llama
            }
            return nextId;
        }

        /// <summary>
        /// Limpia todos los campos del formulario después de una inscripción exitosa.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtApaterno.Text = string.Empty;
            txtAmaterno.Text = string.Empty;
            txtEmail.Text = string.Empty;
            dtFnacimiento.Value = DateTime.Now;

            // Limpiar los campos de dirección
            txtCalle.Text = string.Empty;
            txtColonia.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtCodigoP.Text = string.Empty;
            txtCiudad.Text = string.Empty;
            cbEstado.SelectedIndex = -1;

            // Recargar documentos (debería estar vacío ahora)
            CargarDocumentos();
        }
        /// <summary>
        /// Verifica si existe un semestre con el ID especificado.
        /// </summary>
        /// <param name="idSemestre">ID del semestre a verificar.</param>
        /// <returns>True si existe, False en caso contrario.</returns>
        private bool ExisteSemestre(int idSemestre)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Semestre WHERE id_semestre = @id_semestre";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_semestre", idSemestre);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar el semestre: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Preguntar si desea eliminar los documentos sin asociar
            if (HayDocumentosSinAsociar())
            {
                DialogResult resultado = MessageBox.Show("Hay documentos que no han sido asociados a ningún alumno. ¿Desea eliminarlos?",
                    "Documentos sin asociar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    EliminarDocumentosSinAsociar();
                }
            }

            // Eliminar este control del panel padre
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
        }

        /// <summary>
        /// Verifica si hay documentos sin asociar
        /// </summary>
        private bool HayDocumentosSinAsociar()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Documentos WHERE id_alumno IS NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar documentos sin asociar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// Crea un nuevo semestre en la base de datos si no existe.
        /// </summary>
        /// <param name="numeroSemestre">Número del semestre a crear.</param>
        /// <returns>ID del semestre creado o 0 si hubo un error.</returns>
        private int CrearSemestre(int numeroSemestre)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();

                    // Primero verificamos si ya existe
                    string queryVerificar = "SELECT id_semestre FROM Semestre WHERE n_semestre = @n_semestre";
                    using (SqlCommand cmdVerificar = new SqlCommand(queryVerificar, conn))
                    {
                        cmdVerificar.Parameters.AddWithValue("@n_semestre", numeroSemestre);
                        object result = cmdVerificar.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result); // Ya existe, devolver el ID
                        }
                    }

                    // Si no existe, lo creamos
                    string queryInsertar = @"INSERT INTO Semestre (n_semestre, periodo) 
                                       VALUES (@n_semestre, @periodo);
                                       SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, conn))
                    {
                        cmdInsertar.Parameters.AddWithValue("@n_semestre", numeroSemestre);
                        cmdInsertar.Parameters.AddWithValue("@periodo", DateTime.Now); // Usar fecha actual como periodo

                        return Convert.ToInt32(cmdInsertar.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el semestre: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Asigna automáticamente las materias del paquete 1 al alumno recién inscrito.
        /// </summary>
        /// <param name="idAlumno">ID del alumno inscrito.</param>
        /// <param name="idSemestre">ID del semestre del alumno.</param>
        /// <returns>True si se asignó correctamente, False en caso contrario.</returns>
        private bool AsignarHorarioAlumno(int idAlumno, int idSemestre)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();

                    // Obtener las materias del paquete 1 para el semestre indicado
                    string queryMaterias = @"
                        SELECT M.id_materia 
                        FROM Materias M
                        INNER JOIN Paquetes P ON M.id_materia = P.id_materia
                        WHERE P.n_paquete = 1 AND P.id_semestre = @id_semestre";

                    List<int> materiasIds = new List<int>();

                    using (SqlCommand cmd = new SqlCommand(queryMaterias, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_semestre", idSemestre);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materiasIds.Add(Convert.ToInt32(reader["id_materia"]));
                            }
                        }
                    }

                    if (materiasIds.Count == 0)
                    {
                        MessageBox.Show("No se encontraron materias asignadas al paquete 1 para el semestre actual.",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // Asignar cada materia al alumno en la tabla Horarios
                    foreach (int idMateria in materiasIds)
                    {
                        string queryInsertarHorario = @"
                            INSERT INTO Horarios (id_materia, id_alumno, id_semestre, estado)
                            VALUES (@id_materia, @id_alumno, @id_semestre, 'inscrita')";

                        using (SqlCommand cmd = new SqlCommand(queryInsertarHorario, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_materia", idMateria);
                            cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                            cmd.Parameters.AddWithValue("@id_semestre", idSemestre);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar horario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Muestra una vista previa del horario del alumno y permite descargarlo.
        /// </summary>
        /// <param name="idAlumno">ID del alumno.</param>
        /// <param name="numeroControl">Número de control del alumno.</param>
        private void MostrarVistaPrevia(int idAlumno, int numeroControl)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();

                    // Obtener información del alumno
                    string queryAlumno = @"
                        SELECT A.nombre, A.a_paterno, A.a_materno, S.n_semestre
                        FROM Alumnos A
                        INNER JOIN Semestre S ON A.id_semestre = S.id_semestre
                        WHERE A.id_alumno = @id_alumno";

                    string nombreCompleto = "";
                    int numSemestre = 0;

                    using (SqlCommand cmd = new SqlCommand(queryAlumno, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nombreCompleto = $"{reader["nombre"]} {reader["a_paterno"]} {reader["a_materno"]}";
                                numSemestre = Convert.ToInt32(reader["n_semestre"]);
                            }
                        }
                    }

                    // Obtener el horario del alumno
                    string queryHorario = @"
                        SELECT M.n_materia, M.hora, M.aula, D.nombre AS docente
                        FROM Horarios H
                        INNER JOIN Materias M ON H.id_materia = M.id_materia
                        LEFT JOIN Docentes D ON M.id_docente = D.id_docente
                        WHERE H.id_alumno = @id_alumno
                        ORDER BY M.hora";

                    DataTable dtHorario = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryHorario, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dtHorario);
                    }

                    if (dtHorario.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron materias asignadas al alumno.",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Crear un formulario para mostrar la vista previa
                    Form formVistaPrevia = new Form();
                    formVistaPrevia.Text = "Vista Previa de Horario";
                    formVistaPrevia.Size = new Size(800, 600);
                    formVistaPrevia.StartPosition = FormStartPosition.CenterScreen;
                    formVistaPrevia.MinimizeBox = false;
                    formVistaPrevia.MaximizeBox = false;
                    formVistaPrevia.FormBorderStyle = FormBorderStyle.FixedDialog;

                    // Añadir un panel con scroll
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.AutoScroll = true;
                    formVistaPrevia.Controls.Add(panel);

                    // Añadir etiqueta con información del alumno
                    Label lblInfo = new Label();
                    lblInfo.Text = $"Alumno: {nombreCompleto}\r\nNúmero de Control: {numeroControl}\r\nSemestre: {numSemestre}";
                    lblInfo.Font = new Font("Arial", 12, FontStyle.Bold);
                    lblInfo.AutoSize = true;
                    lblInfo.Location = new Point(20, 20);
                    panel.Controls.Add(lblInfo);

                    // Añadir DataGridView con el horario
                    DataGridView dgvHorario = new DataGridView();
                    dgvHorario.DataSource = dtHorario;
                    dgvHorario.Size = new Size(750, 400);
                    dgvHorario.Location = new Point(20, 100);
                    dgvHorario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvHorario.ReadOnly = true;
                    dgvHorario.AllowUserToAddRows = false;
                    dgvHorario.AllowUserToDeleteRows = false;
                    dgvHorario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    panel.Controls.Add(dgvHorario);

                    // Añadir botón para generar PDF
                    Button btnGenerarPDF = new Button();
                    btnGenerarPDF.Text = "Descargar PDF";
                    btnGenerarPDF.Size = new Size(150, 40);
                    btnGenerarPDF.Location = new Point(20, 510);
                    btnGenerarPDF.Click += (sender, e) => GenerarPDF(dtHorario, nombreCompleto, numeroControl, numSemestre);
                    panel.Controls.Add(btnGenerarPDF);

                    // Mostrar el formulario
                    formVistaPrevia.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar vista previa: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Genera un archivo PDF con el horario del alumno utilizando iText 7.
        /// </summary>
        /// <param name="dtHorario">DataTable con la información del horario.</param>
        /// <param name="nombreAlumno">Nombre completo del alumno.</param>
        /// <param name="numeroControl">Número de control del alumno.</param>
        /// <param name="numSemestre">Número de semestre.</param>
        private void GenerarPDF(DataTable dtHorario, string nombreAlumno, int numeroControl, int numSemestre)
        {
            try
            {
                // Primero asegúrate de tener las referencias en la parte superior del archivo:
                // using PdfSharp.Pdf;
                // using PdfSharp.Drawing;
                // using PdfSharp.Drawing.Layout;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo PDF|*.pdf",
                    Title = "Guardar horario como PDF",
                    FileName = $"Horario_{numeroControl}.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Crear documento PDF
                PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
                document.Info.Title = $"Horario de {nombreAlumno}";
                document.Info.Author = "SIRTEC";
                document.Info.Subject = "Horario Escolar";

                // Agregar página
                PdfSharp.Pdf.PdfPage page = document.AddPage();
                page.Size = PdfSharp.PageSize.A4;

                // Crear gráficos
                PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);

                // Definir fuentes
                PdfSharp.Drawing.XFont fontTitulo = new PdfSharp.Drawing.XFont("Arial", 20, PdfSharp.Drawing.XFontStyleEx.Bold);
                PdfSharp.Drawing.XFont fontSubtitulo = new PdfSharp.Drawing.XFont("Arial", 12, PdfSharp.Drawing.XFontStyleEx.Bold);
                PdfSharp.Drawing.XFont fontRegular = new PdfSharp.Drawing.XFont("Arial", 10);
                PdfSharp.Drawing.XFont fontTablaHeader = new PdfSharp.Drawing.XFont("Arial", 11, PdfSharp.Drawing.XFontStyleEx.Bold);
                PdfSharp.Drawing.XFont fontFooter = new PdfSharp.Drawing.XFont("Arial", 8, PdfSharp.Drawing.XFontStyleEx.Italic);

                // Dibujar título
                gfx.DrawString("HORARIO DE CLASES", fontTitulo, PdfSharp.Drawing.XBrushes.Navy,
                              new PdfSharp.Drawing.XRect(0, 40, page.Width, 30),
                              PdfSharp.Drawing.XStringFormats.TopCenter);

                // Dibujar información del alumno
                gfx.DrawString($"Alumno: {nombreAlumno}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 100);
                gfx.DrawString($"Número de Control: {numeroControl}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 120);
                gfx.DrawString($"Semestre: {numSemestre}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 140);
                gfx.DrawString($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 160);

                // Dibujar tabla de horario
                double startY = 200;
                double rowHeight = 30;
                double colWidth1 = 200; // Materia
                double colWidth2 = 100; // Hora
                double colWidth3 = 80;  // Aula
                double colWidth4 = 150; // Docente
                double tableWidth = colWidth1 + colWidth2 + colWidth3 + colWidth4;
                double tableX = 50;

                // Linea superior de la tabla
                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                            tableX, startY,
                            tableX + tableWidth, startY);

                // Dibujar encabezados
                PdfSharp.Drawing.XRect headerRect = new PdfSharp.Drawing.XRect(tableX, startY, tableWidth, rowHeight);
                gfx.DrawRectangle(PdfSharp.Drawing.XBrushes.LightGray, headerRect);
                gfx.DrawRectangle(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1), headerRect);

                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                            tableX + colWidth1, startY,
                            tableX + colWidth1, startY + rowHeight);

                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                            tableX + colWidth1 + colWidth2, startY,
                            tableX + colWidth1 + colWidth2, startY + rowHeight);

                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                            tableX + colWidth1 + colWidth2 + colWidth3, startY,
                            tableX + colWidth1 + colWidth2 + colWidth3, startY + rowHeight);

                gfx.DrawString("Materia", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                              tableX + 5, startY + 20);

                gfx.DrawString("Hora", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                              tableX + colWidth1 + 5, startY + 20);

                gfx.DrawString("Aula", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                              tableX + colWidth1 + colWidth2 + 5, startY + 20);

                gfx.DrawString("Docente", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                              tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);

                startY += rowHeight;

                // Dibujar filas de datos
                foreach (DataRow row in dtHorario.Rows)
                {
                    // Líneas horizontales
                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX, startY,
                                tableX + tableWidth, startY);

                    // Líneas verticales
                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX, startY,
                                tableX, startY + rowHeight);

                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX + colWidth1, startY,
                                tableX + colWidth1, startY + rowHeight);

                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX + colWidth1 + colWidth2, startY,
                                tableX + colWidth1 + colWidth2, startY + rowHeight);

                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX + colWidth1 + colWidth2 + colWidth3, startY,
                                tableX + colWidth1 + colWidth2 + colWidth3, startY + rowHeight);

                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                tableX + tableWidth, startY,
                                tableX + tableWidth, startY + rowHeight);

                    // Datos de las celdas
                    string materia = row["n_materia"].ToString();
                    string hora = row["hora"].ToString();
                    string aula = row["aula"].ToString();
                    string docente = row["docente"] != DBNull.Value ? row["docente"].ToString() : "";

                    // Limitar texto si es demasiado largo
                    if (materia.Length > 35) materia = materia.Substring(0, 32) + "...";
                    if (docente.Length > 25) docente = docente.Substring(0, 22) + "...";

                    gfx.DrawString(materia, fontRegular, PdfSharp.Drawing.XBrushes.Black,
                                  tableX + 5, startY + 20);

                    gfx.DrawString(hora, fontRegular, PdfSharp.Drawing.XBrushes.Black,
                                  tableX + colWidth1 + 5, startY + 20);

                    gfx.DrawString(aula, fontRegular, PdfSharp.Drawing.XBrushes.Black,
                                  tableX + colWidth1 + colWidth2 + 5, startY + 20);

                    gfx.DrawString(docente, fontRegular, PdfSharp.Drawing.XBrushes.Black,
                                  tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);

                    startY += rowHeight;

                    // Si estamos cerca del final de la página, crear una nueva página
                    if (startY > page.Height - 100)
                    {
                        // Línea final de la tabla en la página actual
                        gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                    tableX, startY,
                                    tableX + tableWidth, startY);

                        // Nueva página
                        page = document.AddPage();
                        gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                        startY = 50;

                        // Encabezados en la nueva página
                        gfx.DrawString("HORARIO DE CLASES (Continuación)", fontSubtitulo, PdfSharp.Drawing.XBrushes.Navy,
                                      new PdfSharp.Drawing.XRect(0, 20, page.Width, 20),
                                      PdfSharp.Drawing.XStringFormats.TopCenter);

                        // Repetir encabezados de tabla
                        headerRect = new PdfSharp.Drawing.XRect(tableX, startY, tableWidth, rowHeight);
                        gfx.DrawRectangle(PdfSharp.Drawing.XBrushes.LightGray, headerRect);
                        gfx.DrawRectangle(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1), headerRect);

                        gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                    tableX + colWidth1, startY,
                                    tableX + colWidth1, startY + rowHeight);

                        gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                    tableX + colWidth1 + colWidth2, startY,
                                    tableX + colWidth1 + colWidth2, startY + rowHeight);

                        gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                                    tableX + colWidth1 + colWidth2 + colWidth3, startY,
                                    tableX + colWidth1 + colWidth2 + colWidth3, startY + rowHeight);

                        gfx.DrawString("Materia", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                                      tableX + 5, startY + 20);

                        gfx.DrawString("Hora", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                                      tableX + colWidth1 + 5, startY + 20);

                        gfx.DrawString("Aula", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                                      tableX + colWidth1 + colWidth2 + 5, startY + 20);

                        gfx.DrawString("Docente", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black,
                                      tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);

                        startY += rowHeight;
                    }
                }

                // Línea final de la tabla
                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                            tableX, startY,
                            tableX + tableWidth, startY);

                // Pie de página
                gfx.DrawString("Este documento es una representación oficial del horario escolar. Guárdelo para futuras referencias.",
                              fontFooter, PdfSharp.Drawing.XBrushes.DarkGray,
                              new PdfSharp.Drawing.XRect(0, page.Height - 50, page.Width, 20),
                              PdfSharp.Drawing.XStringFormats.Center);

                // Guardar el documento
                document.Save(saveFileDialog.FileName);

                MessageBox.Show($"El horario se ha guardado correctamente en:\n{saveFileDialog.FileName}",
                    "PDF Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir el archivo PDF
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}\n\nDetalles: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// Realiza todas las validaciones necesarias antes de inscribir al alumno
        /// </summary>
        /// <returns>True si todas las validaciones pasan, False si alguna falla</returns>
        private bool ValidarDatosInscripcion()
        {
            // 1. Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApaterno.Text) ||
                string.IsNullOrEmpty(txtAmaterno.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtCalle.Text) || string.IsNullOrEmpty(txtColonia.Text) ||
                string.IsNullOrEmpty(txtNumero.Text) || string.IsNullOrEmpty(txtCodigoP.Text) ||
                string.IsNullOrEmpty(txtCiudad.Text) || cbEstado.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Validar longitud mínima de los campos (que tenga al menos 3 caracteres)
            if (txtNombre.Text.Length < 3 || txtApaterno.Text.Length < 3 || txtAmaterno.Text.Length < 3 ||
                txtCalle.Text.Length < 3 || txtColonia.Text.Length < 3 || txtCiudad.Text.Length < 3)
            {
                MessageBox.Show("Los campos de texto deben contener al menos 3 caracteres.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 3. Validar la edad (al menos 17 años)
            DateTime fechaNacimiento = dtFnacimiento.Value;
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            // Ajustar la edad si aún no ha cumplido años este año
            if (fechaNacimiento > DateTime.Today.AddYears(-edad))
                edad--;

            if (edad < 17)
            {
                MessageBox.Show("El alumno debe tener al menos 17 años para inscribirse.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 4. Validar formato de correo electrónico
            string email = txtEmail.Text.Trim();
            if (!email.Contains("@") || !email.EndsWith(".com"))
            {
                MessageBox.Show("El correo electrónico debe contener '@' y terminar en '.com'",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 5. Validar que código postal tenga exactamente 5 caracteres
            if (txtCodigoP.Text.Length != 5 || !txtCodigoP.Text.All(char.IsDigit))
            {
                MessageBox.Show("El código postal debe contener exactamente 5 dígitos.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 6. Verificar si ya existe un alumno con el mismo nombre y apellidos
            if (ExisteAlumno(txtNombre.Text.Trim(), txtApaterno.Text.Trim(), txtAmaterno.Text.Trim()))
            {
                MessageBox.Show("Ya existe un alumno registrado con el mismo nombre completo.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 7. Verificar si el correo electrónico ya está registrado
            if (ExisteEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("El correo electrónico ya está registrado en el sistema.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 8. Verificar que se hayan subido al menos 3 documentos obligatorios
            if (!ValidarDocumentosObligatorios())
            {
                MessageBox.Show("Debe subir al menos 3 documentos: Acta, CURP y uno de los siguientes documentos: Certificado o INE.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Todas las validaciones pasaron
            return true;
        }

        /// <summary>
        /// Valida que se hayan subido los documentos obligatorios
        /// </summary>
        /// <returns>True si se han subido los documentos obligatorios, False en caso contrario</returns>
        private bool ValidarDocumentosObligatorios()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    // Consultamos los documentos sin asociar (que serán para este alumno)
                    string query = @"SELECT tipo_documento FROM Documentos WHERE id_alumno IS NULL";

                    HashSet<string> tiposDocumentos = new HashSet<string>();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tipoDoc = reader["tipo_documento"].ToString();
                                tiposDocumentos.Add(tipoDoc);
                            }
                        }
                    }

                    // Verificar si están los documentos obligatorios
                    bool tieneActa = tiposDocumentos.Contains("Acta");
                    bool tieneCURP = tiposDocumentos.Contains("CURP");
                    bool tieneCertificado = tiposDocumentos.Contains("Certificado");
                    bool tieneINE = tiposDocumentos.Contains("INE");

                    // Debe tener Acta, CURP y al menos uno de los siguientes: Certificado o INE
                    return tieneActa && tieneCURP && (tieneCertificado || tieneINE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar documentos obligatorios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Valida que el campo tenga al menos 3 caracteres
        /// </summary>
        private void ValidarLongitudMinima(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length > 0 && textBox.Text.Length < 3)
            {
                MessageBox.Show($"El campo debe contener al menos 3 caracteres.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
            }
        }

        /// <summary>
        /// Verifica si ya existe un alumno con el mismo nombre y apellidos
        /// </summary>
        /// <returns>True si existe, False en caso contrario</returns>
        private bool ExisteAlumno(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM Alumnos 
                                   WHERE LOWER(nombre) = LOWER(@nombre) 
                                   AND LOWER(a_paterno) = LOWER(@a_paterno) 
                                   AND LOWER(a_materno) = LOWER(@a_materno)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@a_paterno", apellidoPaterno);
                        cmd.Parameters.AddWithValue("@a_materno", apellidoMaterno);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar duplicidad de alumno: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // En caso de error, permitimos continuar
            }
        }

        /// <summary>
        /// Verifica si el correo electrónico ya está registrado
        /// </summary>
        /// <returns>True si existe, False en caso contrario</returns>
        private bool ExisteEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Alumnos WHERE LOWER(e_mail) = LOWER(@email)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar duplicidad de email: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // En caso de error, permitimos continuar
            }
        }
        /// <summary>
        /// Configura los manejadores de eventos para validación en tiempo real
        /// </summary>
        private void ConfigurarValidacionesCampos()
        {
            // Validar longitud mínima al cambiar el foco
            txtNombre.Leave += ValidarLongitudMinima;
            txtApaterno.Leave += ValidarLongitudMinima;
            txtAmaterno.Leave += ValidarLongitudMinima;
            txtCalle.Leave += ValidarLongitudMinima;
            txtColonia.Leave += ValidarLongitudMinima;
            txtCiudad.Leave += ValidarLongitudMinima;

            // Validar formato de correo electrónico
            txtEmail.Leave += ValidarFormatoEmail;

            // Validar código postal
            txtCodigoP.KeyPress += SoloNumeros;
            txtCodigoP.Leave += ValidarCodigoPostal;
        }

        /// <summary>
        /// Valida el formato del correo electrónico
        /// </summary>
        private void ValidarFormatoEmail(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
            {
                string email = textBox.Text.Trim();
                if (!email.Contains("@") || !email.EndsWith(".com"))
                {
                    MessageBox.Show("El correo electrónico debe contener '@' y terminar en '.com'",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox.Focus();
                }
            }
        }

        /// <summary>
        /// Permite solo entrada de números
        /// </summary>
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control como backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Valida que el código postal tenga exactamente 5 dígitos
        /// </summary>
        private void ValidarCodigoPostal(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
            {
                string codigoPostal = textBox.Text.Trim();
                if (codigoPostal.Length != 5 || !codigoPostal.All(char.IsDigit))
                {
                    MessageBox.Show("El código postal debe contener exactamente 5 dígitos.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox.Focus();
                }
            }
        }
        /// <summary>
        /// Verifica si ya existe un documento del tipo especificado sin asociar
        /// </summary>
        /// <param name="tipoDocumento">Tipo de documento a verificar</param>
        /// <returns>True si ya existe, False en caso contrario</returns>
        private bool ExisteTipoDocumento(string tipoDocumento)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Documentos WHERE tipo_documento = @tipo_documento AND id_alumno IS NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipo_documento", tipoDocumento);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar tipo de documento: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Elimina todos los documentos sin asociar
        /// </summary>
        private void EliminarDocumentosSinAsociar()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = "DELETE FROM Documentos WHERE id_alumno IS NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            // Solo mostrar mensaje si se eliminaron documentos
                            MessageBox.Show($"Se han eliminado {filasAfectadas} documento(s) que no fueron asociados.",
                                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar documentos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina un documento seleccionado del DataGridView
        /// </summary>
        private void EliminarDocumentoSeleccionado()
        {
            if (dgvDocumentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un documento para eliminar.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtener el nombre del documento seleccionado
                string nombreDocumento = dgvDocumentos.SelectedRows[0].Cells["Nombre"].Value.ToString();
                string tipoDocumento = dgvDocumentos.SelectedRows[0].Cells["Tipo"].Value.ToString();

                DialogResult resultado = MessageBox.Show($"¿Está seguro de eliminar el documento '{nombreDocumento}' de tipo '{tipoDocumento}'?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                    {
                        conn.Open();
                        string query = "DELETE FROM Documentos WHERE n_documento = @nombre AND tipo_documento = @tipo AND id_alumno IS NULL";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", nombreDocumento);
                            cmd.Parameters.AddWithValue("@tipo", tipoDocumento);

                            int filasAfectadas = cmd.ExecuteNonQuery();
                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Documento eliminado correctamente.",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Actualizar la lista de documentos
                                CargarDocumentos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el documento. Es posible que ya esté asociado a un alumno.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el documento: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro de cancelar el proceso de inscripción? Se perderán todos los datos no guardados y se eliminarán los documentos subidos.",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Eliminar documentos sin asociar
                EliminarDocumentosSinAsociar();

                // Limpiar formulario
                LimpiarFormulario();

                // Notificar al usuario
                MessageBox.Show("El proceso de inscripción ha sido cancelado.",
                    "Proceso cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Volver a la pantalla anterior
                if (this.Parent != null)
                {
                    this.Parent.Controls.Remove(this);
                    this.Dispose();
                }
            }
        }
    }
}
