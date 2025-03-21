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
//using para iText 7
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using Color = iText.Kernel.Colors.Color;

namespace SIRTEC.PRESENTACION
{
    public partial class ctlAltaDocente : UserControl
    {
        int semestre = 1;
        // Lista para almacenar documentos seleccionados temporalmente
        private List<Ldocumentos> documentosSeleccionados = new List<Ldocumentos>();

        public ctlAltaDocente()
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
                int numeroControl = 2532 + idAlumno;

                // Crear la consulta para insertar el alumno
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();

                    // Iniciar una transacción para asegurar integridad
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insertar alumno
                            string query = @"INSERT INTO Alumnos (id_alumno, nombre, a_paterno, a_materno, e_mail, f_nacimiento, n_control, id_semestre)
                                      VALUES (@id_alumno, @nombre, @a_paterno, @a_materno, @e_mail, @f_nacimiento, @n_control, @id_semestre)";

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
            // Simplemente eliminar este control del panel padre
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose();
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
                    //btnGenerarPDF.Click += (sender, e) => GenerarPDF(dtHorario, nombreCompleto, numeroControl, numSemestre);
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
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo PDF|*.pdf",
                    Title = "Guardar horario como PDF",
                    FileName = $"Horario_{numeroControl}.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Crear el documento PDF usando iText 7
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    PdfWriter writer = new PdfWriter(fs);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    // Título del documento
                    Paragraph titulo = new Paragraph("HORARIO DE CLASES")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18);
                    document.Add(titulo);

                    // Información del alumno
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph($"Alumno: {nombreAlumno}").SetFontSize(12));
                    document.Add(new Paragraph($"Número de Control: {numeroControl}").SetFontSize(12));
                    document.Add(new Paragraph($"Semestre: {numSemestre}").SetFontSize(12));
                    document.Add(new Paragraph($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy}").SetFontSize(12));
                    document.Add(new Paragraph("\n"));

                    // Crear tabla para el horario
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 35, 20, 15, 30 }))
                        .UseAllAvailableWidth();

                    // Encabezados de la tabla
                    string[] headers = { "Materia", "Hora", "Aula", "Docente" };
                    foreach (var header in headers)
                    {
                        Cell cell = new Cell().Add(new Paragraph(header))
                            .SetBackgroundColor(new DeviceRgb(220, 220, 220))
                            .SetTextAlignment(TextAlignment.CENTER);
                        table.AddHeaderCell(cell);
                    }

                    // Añadir filas de datos
                    foreach (DataRow row in dtHorario.Rows)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(row["n_materia"].ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(row["hora"].ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(row["aula"].ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(row["docente"].ToString())));
                    }

                    document.Add(table);

                    // Pie de página
                    document.Add(new Paragraph("\n"));
                    Paragraph piePagina = new Paragraph("Este documento es una representación oficial del horario escolar. Guárdelo para futuras referencias.")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(10);
                    document.Add(piePagina);

                    // Cerrar el documento
                    document.Close();
                }

                MessageBox.Show($"El horario se ha guardado correctamente en:\n{saveFileDialog.FileName}",
                    "PDF Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir el archivo PDF generado
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
