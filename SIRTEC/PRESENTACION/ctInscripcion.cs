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

namespace SIRTEC.PRESENTACION
{
    public partial class ctInscripcion : UserControl
    {
        int semestre = 1;
        // Lista para almacenar documentos seleccionados temporalmente
        private List<Ldocumentos> documentosSeleccionados = new List<Ldocumentos>();

        public ctInscripcion()
        {
            InitializeComponent();
            // Configurar el DataGridView
            ConfigurarDataGridView();
            // Cargar los documentos al iniciar
            CargarDocumentos();

        }

        private void ConfigurarDataGridView()
        {
            dgvDocumentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocumentos.AllowUserToAddRows = false;
            dgvDocumentos.AllowUserToDeleteRows = false;
            dgvDocumentos.ReadOnly = true;
            dgvDocumentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
                        dgvDocumentos.Columns.Add("Nombre", "Nombre del Documento");
                        dgvDocumentos.Columns.Add("Extension", "Extensión");
                        dgvDocumentos.Columns.Add("Tipo", "Tipo de Documento");
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
                    dgvDocumentos.Height = Math.Min(500, 60 + (dgvDocumentos.Rows.Count * dgvDocumentos.RowTemplate.Height));
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
                        // Crear una instancia de Documento
                        Ldocumentos doc = new Ldocumentos
                        {
                            n_documento = Path.GetFileName(file),
                            extension = Path.GetExtension(file),
                            // TipoDocumento se deja vacío para que el usuario lo complete posteriormente
                            tipoDocumento = string.Empty,
                            contenido = File.ReadAllBytes(file)
                        };

                        // Agregar el documento a la lista
                        documentosSeleccionados.Add(doc);

                        // Actualizar la interfaz de usuario (por ejemplo, un ListBox)
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

            if (lbTipoDocu.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un tipo de documento.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            documentosSeleccionados[documentosSeleccionados.Count - 1].tipoDocumento = lbTipoDocu.Text;

            // Guardar los documentos en la base de datos con id_alumno como NULL
            foreach (var docu in documentosSeleccionados)
            {
                InsertarDocumentoEnBD(docu);
            }

            // Cargar los documentos actualizados
            CargarDocumentos();

            MessageBox.Show("Documentos subidos correctamente. Se asociarán al alumno una vez completada la inscripción.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Limpiar la lista y la interfaz
            documentosSeleccionados.Clear();
            txtNombreDocumento.Text = string.Empty;
            pnlDocu.Visible = false;
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

        /// <summary>
        /// Método para asociar documentos con un id_alumno después de la inscripción.
        /// </summary>
        /// <param name="idAlumno">ID del alumno.</param>
        public void AsociarDocumentosAAlumno(int idAlumno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"UPDATE Documentos
                                                 SET id_alumno = @id_alumno
                                                 WHERE id_alumno IS NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@id_alumno", SqlDbType.Int).Value = idAlumno;
                        cmd.ExecuteNonQuery();
                    }
                }

                // Actualizar la vista del DataGridView (ahora estará vacío porque los documentos ya están asociados)
                CargarDocumentos();

                MessageBox.Show("Documentos asociados al alumno correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al asociar los documentos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubirDoc_Click(object sender, EventArgs e)
        {
            pnlDocu.Visible = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar que los campos requeridos estén completos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApaterno.Text) ||
                string.IsNullOrEmpty(txtAmaterno.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dentro de btnGuardar_Click, antes de la inserción
            if (!ExisteSemestre(semestre))
            {
                MessageBox.Show("El semestre 1 no está configurado en el sistema. Por favor, contacte al administrador.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Obtener el siguiente ID disponible
                int idAlumno = ObtenerSiguienteIdAlumno();

                // Generar el número de control (2532 + ID)
                int numeroControl = 2532 + idAlumno;

                // Crear la consulta para insertar el alumno
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {

                    conn.Open();
                    string query = @"INSERT INTO Alumnos (id_alumno, nombre, a_paterno, a_materno, e_mail, f_nacimiento, n_control, id_semestre)
                          VALUES (@id_alumno, @nombre, @a_paterno, @a_materno, @e_mail, @f_nacimiento, @n_control, @id_semestre)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@a_paterno", txtApaterno.Text);

                        cmd.Parameters.AddWithValue("@a_materno", txtAmaterno.Text);
                        cmd.Parameters.AddWithValue("@e_mail", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@f_nacimiento", dtFnacimiento.Value);
                        cmd.Parameters.AddWithValue("@n_control", numeroControl);
                        cmd.Parameters.AddWithValue("@id_semestre", semestre);

                        cmd.ExecuteNonQuery();
                    }

                    // Crear un usuario para el alumno usando el número de control
                    CrearUsuarioParaAlumno(idAlumno, numeroControl.ToString(), conn);
                }

                // Asociar los documentos subidos al alumno
                AsociarDocumentosAAlumno(idAlumno);

                MessageBox.Show($"¡Inscripción completada con éxito!\nNúmero de Control: {numeroControl}\n" +
                               $"Este número será su usuario y contraseña para acceder al sistema.",
                               "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos del formulario
                LimpiarFormulario();
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
        /// Crea un usuario para el alumno recién inscrito utilizando su número de control.
        /// </summary>
        /// <param name="idAlumno">ID del alumno.</param>
        /// <param name="numeroControl">Número de control que servirá como usuario y contraseña.</param>
        /// <param name="conn">Conexión SQL activa.</param>
        private void CrearUsuarioParaAlumno(int idAlumno, string numeroControl, SqlConnection conn)
        {
            try
            {
                // Obtener el siguiente ID de usuario
                int idUsuario = 1;
                string queryId = "SELECT ISNULL(MAX(id_usuarios), 0) + 1 FROM Usuarios";
                using (SqlCommand cmdId = new SqlCommand(queryId, conn))
                {
                    idUsuario = Convert.ToInt32(cmdId.ExecuteScalar());
                }

                // Insertar el usuario usando el número de control como nombre de usuario y contraseña
                string query = @"INSERT INTO Usuarios (id_usuarios, tipo_usuario, username, password, id_alumno, id_docente, id_coordinador)
                       VALUES (@id_usuarios, 'alumno', @user, @password, @id_alumno, NULL, NULL)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_usuarios", idUsuario);
                    cmd.Parameters.AddWithValue("@user", numeroControl);
                    cmd.Parameters.AddWithValue("@password", numeroControl);
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-lanzar la excepción para que se maneje en el método que llama
            }
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
            // Buscar el formulario padre que contiene este control
            Form parentForm = this.FindForm();

            // Crear una nueva instancia del formulario Login
            Login login = new Login();

            // Mostrar el formulario de login
            login.Show();

            // Ocultar o cerrar el formulario padre si existe
            if (parentForm != null)
            {
                parentForm.Hide();
            }
            // No es necesario crear y eliminar una instancia de este control
            // ya que el control actual se eliminará cuando se cierre el formulario
        }
    }
}
