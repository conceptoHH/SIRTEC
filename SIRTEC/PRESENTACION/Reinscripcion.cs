using SIRTEC.DATOS;
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
    public partial class Reinscripcion : Form
    {
        // Variables para gestionar el proceso de reinscripción
        private int idAlumno;
        private string numeroControl;
        private string nombreCompleto;
        private int idSemestre;
        private int semestreActual;
        private bool documentosCompletos = false;
        private bool tieneMateriasReprobadas = false;
        private List<int> materiasSeleccionadas = new List<int>();
        private List<int> materiasReprobadas = new List<int>();
        private int contadorMaterias = 0;
        private const int MAX_MATERIAS = 7;
        private bool horarioValido = false;

        public Reinscripcion()
        {
            InitializeComponent();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            // Configurar eventos para los botones
            btnSubirDocumento.Click += new EventHandler(btnSubirDocumento_Click);
            btnConfirmarReinscripcion.Click += new EventHandler(btnConfirmarReinscripcion_Click);
            btnGenerarPDF.Click += new EventHandler(btnGenerarPDF_Click);
            btnVolver.Click += new EventHandler(btnVolver_Click);

            // Configurar evento para selección de materias
            dgvMateriasDisponibles.CellContentClick += new DataGridViewCellEventHandler(dgvMateriasDisponibles_CellContentClick);

            // Configurar evento para cargar el formulario
            this.Load += new EventHandler(Reinscripcion_Load);
        }

        private void Reinscripcion_Load(object sender, EventArgs e)
        {
            // 1. Obtener información del alumno logueado
            ObtenerInformacionAlumno();

            // 2. Mostrar información en la interfaz
            MostrarInformacionAlumno();

            // 3. Verificar documentos
            VerificarDocumentos();

            // 4. Configurar pestañas según el estado
            ConfigurarPestañas();
        }

        private void ObtenerInformacionAlumno()
        {
            try
            {
                // Obtener ID del alumno desde la clase estática
                idAlumno = UsuarioActual.IdEspecifico;

                if (idAlumno <= 0)
                {
                    MessageBox.Show("No se pudo identificar al alumno. Por favor inicie sesión nuevamente.",
                        "Error de identificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            A.n_control,
                            A.nombre + ' ' + A.a_paterno + ' ' + A.a_materno AS nombre_completo,
                            A.id_semestre,
                            S.n_semestre
                        FROM 
                            Alumnos A
                        INNER JOIN
                            Semestre S ON A.id_semestre = S.id_semestre
                        WHERE 
                            A.id_alumno = @id_alumno";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            numeroControl = reader["n_control"].ToString();
                            nombreCompleto = reader["nombre_completo"].ToString();
                            idSemestre = Convert.ToInt32(reader["id_semestre"]);
                            semestreActual = Convert.ToInt32(reader["n_semestre"]);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró información del alumno.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener información del alumno: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
                this.Close();
            }
        }

        private void MostrarInformacionAlumno()
        {
            lblNombreAlumno.Text = $"Alumno: {nombreCompleto}";
            lblNumeroControl.Text = $"Número de control: {numeroControl}";
            lblSemestreActual.Text = $"Semestre actual: {semestreActual}°";
            lblEstadoGeneral.Text = "Estado: Verificando documentos...";
            lblEstadoGeneral.ForeColor = Color.DarkOrange;
        }

        private void VerificarDocumentos()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT COUNT(*)
                        FROM Documentos
                        WHERE id_alumno = @id_alumno";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                    int cantidadDocumentos = Convert.ToInt32(cmd.ExecuteScalar());

                    documentosCompletos = (cantidadDocumentos >= 4);
                }
                CONEXIONMAESTRA.cerrar();

                // Cargar documentos en DataGridView
                CargarDocumentos();

                // Actualizar interfaz según estado de documentos
                if (documentosCompletos)
                {
                    lblDocumentosFaltantes.Text = "DOCUMENTOS COMPLETOS - Puedes continuar con la reinscripción";
                    lblDocumentosFaltantes.ForeColor = Color.Green;
                    VerificarMateriasReprobadas();
                }
                else
                {
                    lblDocumentosFaltantes.Text = $"DEBES COMPLETAR LOS 4 DOCUMENTOS REQUERIDOS ANTES DE CONTINUAR (Actualmente: {dgvDocumentos.Rows.Count}/4)";
                    lblDocumentosFaltantes.ForeColor = Color.Red;
                    lblEstadoGeneral.Text = "Estado: Documentos incompletos";
                    lblEstadoGeneral.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar documentos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarDocumentos()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT
                            id_documento,
                            n_documento AS 'Nombre',
                            tipo_documento AS 'Tipo de Documento',
                            extension AS 'Formato'
                        FROM 
                            Documentos
                        WHERE 
                            id_alumno = @id_alumno
                        ORDER BY 
                            Id_documento ASC";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvDocumentos.DataSource = dt;

                    // Ocultar columna de ID
                    if (dgvDocumentos.Columns.Contains("id_documento"))
                    {
                        dgvDocumentos.Columns["id_documento"].Visible = false;
                    }
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar documentos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void VerificarMateriasReprobadas()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT DISTINCT M.id_materia
                        FROM Calificaciones C
                        INNER JOIN Materias M ON C.id_materia = M.id_materia
                        INNER JOIN Horarios H ON M.id_materia = H.id_materia AND H.id_alumno = C.id_alumno
                        WHERE C.id_alumno = @id_alumno AND C.calificacion < 70";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        tieneMateriasReprobadas = reader.HasRows;
                    }
                }
                CONEXIONMAESTRA.cerrar();

                if (tieneMateriasReprobadas)
                {
                    lblEstadoGeneral.Text = "Estado: Tiene materias reprobadas";
                    lblEstadoGeneral.ForeColor = Color.DarkOrange;
                    CargarMateriasReprobadas();
                }
                else if (documentosCompletos)
                {
                    lblEstadoGeneral.Text = "Estado: Listo para reinscripción";
                    lblEstadoGeneral.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar materias reprobadas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarMateriasReprobadas()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT DISTINCT 
                            M.id_materia,
                            M.n_materia AS 'Materia',
                            CONVERT(VARCHAR, M.hora, 108) AS 'Hora',
                            M.aula AS 'Aula',
                            D.nombre AS 'Docente',
                            (SELECT AVG(C2.calificacion) 
                             FROM Calificaciones C2 
                             WHERE C2.id_alumno = @id_alumno AND C2.id_materia = M.id_materia) AS 'Promedio'
                        FROM 
                            Calificaciones C
                        INNER JOIN 
                            Materias M ON C.id_materia = M.id_materia
                        INNER JOIN 
                            Horarios H ON M.id_materia = H.id_materia AND H.id_alumno = C.id_alumno
                        LEFT JOIN 
                            Docentes D ON M.id_docente = D.id_docente
                        WHERE 
                            C.id_alumno = @id_alumno AND C.calificacion < 70
                        GROUP BY 
                            M.id_materia, M.n_materia, M.hora, M.aula, D.nombre";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvMateriasReprobadas.DataSource = dt;

                    // Ocultar columna de ID
                    if (dgvMateriasReprobadas.Columns.Contains("id_materia"))
                    {
                        dgvMateriasReprobadas.Columns["id_materia"].Visible = false;
                    }

                    // Guardar IDs de materias reprobadas
                    materiasReprobadas.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        materiasReprobadas.Add(Convert.ToInt32(row["id_materia"]));
                    }

                    // Agregar materias reprobadas a la lista de seleccionadas automáticamente
                    materiasSeleccionadas.AddRange(materiasReprobadas);

                    // Actualizar contador
                    contadorMaterias = materiasReprobadas.Count;
                    lblContadorMaterias.Text = $"Materias seleccionadas: {contadorMaterias} de {MAX_MATERIAS} max";
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias reprobadas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarMateriasDisponibles()
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Determinar el siguiente semestre
                int siguienteSemestre = semestreActual + 1;
                if (siguienteSemestre > 8) siguienteSemestre = 8; // Limitar a 8 semestres

                string consulta = @"
                        SELECT 
                            M.id_materia,
                            M.n_materia AS 'Materia',
                            CONVERT(VARCHAR, M.hora, 108) AS 'Hora',
                            M.aula AS 'Aula',
                            D.nombre AS 'Docente',
                            P.n_paquete AS 'Paquete'
                        FROM 
                            Materias M
                        INNER JOIN 
                            Paquetes P ON M.id_materia = P.id_materia
                        INNER JOIN 
                            Semestre S ON P.id_semestre = S.id_semestre
                        LEFT JOIN 
                            Docentes D ON M.id_docente = D.id_docente
                        WHERE 
                            S.n_semestre = @siguiente_semestre
                            AND M.id_materia NOT IN (
                                -- Materias que ya tiene el alumno en su horario
                                SELECT H.id_materia
                                FROM Horarios H
                                WHERE H.id_alumno = @id_alumno
                            )
                            AND M.id_materia NOT IN (
                                -- Materias reprobadas (identificadas desde Calificaciones)
                                SELECT DISTINCT C.id_materia
                                FROM Calificaciones C
                                WHERE C.id_alumno = @id_alumno 
                                AND C.calificacion < 70
                            )";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                    cmd.Parameters.AddWithValue("@siguiente_semestre", siguienteSemestre);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Añadir columna para botón de selección
                    dt.Columns.Add("Seleccionar", typeof(bool));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Seleccionar"] = false;
                    }

                    dgvMateriasDisponibles.DataSource = dt;

                    // Ocultar columna de ID
                    if (dgvMateriasDisponibles.Columns.Contains("id_materia"))
                    {
                        dgvMateriasDisponibles.Columns["id_materia"].Visible = false;
                    }

                    // Configurar columna de selección como checkbox
                    if (dgvMateriasDisponibles.Columns.Contains("Seleccionar"))
                    {
                        DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                        checkColumn.Name = "Seleccionar";
                        checkColumn.HeaderText = "Seleccionar";
                        checkColumn.Width = 70;
                        checkColumn.ReadOnly = false;

                        // Reemplazar la columna existente
                        int index = dgvMateriasDisponibles.Columns["Seleccionar"].Index;
                        dgvMateriasDisponibles.Columns.RemoveAt(index);
                        dgvMateriasDisponibles.Columns.Insert(index, checkColumn);
                    }
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias disponibles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void ConfigurarPestañas()
        {
            // Deshabilitar todas las pestañas excepto documentos
            tabMaterias.Enabled = false;
            tabHorario.Enabled = false;

            // Si los documentos están completos, habilitar la pestaña de materias
            if (documentosCompletos)
            {
                tabMaterias.Enabled = true;
                CargarMateriasDisponibles();

                // Si no tiene materias reprobadas, ocultar panel
                if (!tieneMateriasReprobadas)
                {
                    pnlMateriasReprobadas.Visible = false;
                }
            }

            // Deshabilitar botones en la pestaña de horario
            btnConfirmarReinscripcion.Enabled = false;
            btnGenerarPDF.Enabled = false;
        }

        private void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = openFileDialog.FileName;
                    string nombreArchivo = Path.GetFileName(rutaArchivo);
                    string extension = Path.GetExtension(rutaArchivo);

                    // Mostrar diálogo para seleccionar tipo de documento
                    string tipoDocumento = ObtenerTipoDocumento();
                    if (string.IsNullOrEmpty(tipoDocumento)) return;

                    // Leer el archivo como bytes
                    byte[] contenidoArchivo = File.ReadAllBytes(rutaArchivo);

                    // Guardar documento en la base de datos
                    GuardarDocumento(nombreArchivo, tipoDocumento, extension, contenidoArchivo);

                    // Recargar documentos
                    VerificarDocumentos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al subir documento: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerTipoDocumento()
        {
            // Crear formulario simple para seleccionar tipo de documento
            Form formTipo = new Form();
            formTipo.Text = "Seleccionar tipo de documento";
            formTipo.Size = new Size(400, 200);
            formTipo.FormBorderStyle = FormBorderStyle.FixedDialog;
            formTipo.StartPosition = FormStartPosition.CenterParent;
            formTipo.MaximizeBox = false;
            formTipo.MinimizeBox = false;

            ComboBox comboTipos = new ComboBox();
            comboTipos.Items.AddRange(new string[] {
                    "Acta",
                    "Certificado",
                    "CURP",
                    "INE"
                });
            comboTipos.Location = new Point(100, 50);
            comboTipos.Size = new Size(200, 30);
            comboTipos.DropDownStyle = ComboBoxStyle.DropDownList;

            Button btnAceptar = new Button();
            btnAceptar.Text = "Aceptar";
            btnAceptar.Location = new Point(150, 100);
            btnAceptar.DialogResult = DialogResult.OK;

            formTipo.Controls.Add(comboTipos);
            formTipo.Controls.Add(btnAceptar);
            formTipo.AcceptButton = btnAceptar;

            if (formTipo.ShowDialog() == DialogResult.OK && comboTipos.SelectedItem != null)
            {
                return comboTipos.SelectedItem.ToString();
            }

            return string.Empty;
        }

        private void GuardarDocumento(string nombre, string tipo, string extension, byte[] contenido)
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        INSERT INTO Documentos (n_documento, tipo_documento, extension, contenido, id_alumno)
                        VALUES (@nombre, @tipo, @extension, @contenido, @id_alumno)";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@extension", extension);
                    cmd.Parameters.AddWithValue("@contenido", contenido);
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Documento subido correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar documento: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void dgvMateriasDisponibles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si se hizo clic en la columna "Seleccionar"
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvMateriasDisponibles.Columns["Seleccionar"].Index)
            {
                // Obtener el valor actual del checkbox
                bool seleccionado = Convert.ToBoolean(dgvMateriasDisponibles.Rows[e.RowIndex].Cells["Seleccionar"].Value);

                // Cambiar el valor
                seleccionado = !seleccionado;
                dgvMateriasDisponibles.Rows[e.RowIndex].Cells["Seleccionar"].Value = seleccionado;

                // Obtener id de materia
                int idMateria = Convert.ToInt32(dgvMateriasDisponibles.Rows[e.RowIndex].Cells["id_materia"].Value);

                // Actualizar lista de materias seleccionadas
                if (seleccionado)
                {
                    // Verificar que no exceda el límite
                    if (contadorMaterias >= MAX_MATERIAS)
                    {
                        MessageBox.Show($"No puedes seleccionar más de {MAX_MATERIAS} materias",
                            "Límite alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Desmarcar el checkbox
                        dgvMateriasDisponibles.Rows[e.RowIndex].Cells["Seleccionar"].Value = false;
                        return;
                    }

                    // Verificar si no hay conflicto de horarios
                    if (VerificarConflictoHorario(idMateria))
                    {
                        MessageBox.Show("Esta materia tiene conflicto de horario con otra ya seleccionada",
                            "Conflicto de horario", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Desmarcar el checkbox
                        dgvMateriasDisponibles.Rows[e.RowIndex].Cells["Seleccionar"].Value = false;
                        return;
                    }

                    // Agregar a la lista
                    materiasSeleccionadas.Add(idMateria);
                    contadorMaterias++;
                }
                else
                {
                    // Quitar de la lista
                    materiasSeleccionadas.Remove(idMateria);
                    contadorMaterias--;
                }

                // Actualizar contador
                lblContadorMaterias.Text = $"Materias seleccionadas: {contadorMaterias} de {MAX_MATERIAS} max";

                // Si ya hay materias seleccionadas, habilitar pestaña de horario
                if (contadorMaterias > 0)
                {
                    tabHorario.Enabled = true;
                    CargarVistaHorario();
                }
                else
                {
                    tabHorario.Enabled = false;
                }
            }
        }

        private bool VerificarConflictoHorario(int idMateriaNueva)
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Obtener hora de la materia nueva (tipo time en SQL, TimeSpan en .NET)
                string consultaHoraNueva = @"SELECT hora FROM Materias WHERE id_materia = @id_materia";
                TimeSpan horaNueva = TimeSpan.Zero;

                using (SqlCommand cmd = new SqlCommand(consultaHoraNueva, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_materia", idMateriaNueva);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        horaNueva = (TimeSpan)result;
                    }
                }

                // Si no se pudo obtener la hora, no hay conflicto
                if (horaNueva == TimeSpan.Zero)
                {
                    CONEXIONMAESTRA.cerrar();
                    return false;
                }

                // Verificar conflicto con materias ya seleccionadas
                bool hayConflicto = false;

                foreach (int idMateria in materiasSeleccionadas)
                {
                    string consultaHora = @"SELECT hora FROM Materias WHERE id_materia = @id_materia";
                    TimeSpan hora = TimeSpan.Zero;

                    using (SqlCommand cmd = new SqlCommand(consultaHora, CONEXIONMAESTRA.conectar))
                    {
                        cmd.Parameters.AddWithValue("@id_materia", idMateria);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            hora = (TimeSpan)result;

                            // Considerar conflicto si las horas son iguales
                            if (hora == horaNueva)
                            {
                                hayConflicto = true;
                                break;
                            }
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
                return hayConflicto;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar conflicto de horario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
                return true; // Asumir conflicto en caso de error
            }
        }

        private void CargarVistaHorario()
        {
            try
            {
                // Crear tabla para mostrar horario
                DataTable dtHorario = new DataTable();
                dtHorario.Columns.Add("ID", typeof(int));
                dtHorario.Columns.Add("Materia", typeof(string));
                dtHorario.Columns.Add("Hora", typeof(string));
                dtHorario.Columns.Add("Aula", typeof(string));
                dtHorario.Columns.Add("Docente", typeof(string));
                dtHorario.Columns.Add("Tipo", typeof(string));

                CONEXIONMAESTRA.abrir();

                // Cargar materias reprobadas
                foreach (int idMateria in materiasReprobadas)
                {
                    string consulta = @"
                            SELECT 
                                M.id_materia,
                                M.n_materia,
                                CONVERT(VARCHAR, M.hora, 108) AS hora,
                                M.aula,
                                D.nombre
                            FROM 
                                Materias M
                            LEFT JOIN 
                                Docentes D ON M.id_docente = D.id_docente
                            WHERE 
                                M.id_materia = @id_materia";

                    using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                    {
                        cmd.Parameters.AddWithValue("@id_materia", idMateria);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dtHorario.Rows.Add(
                                    reader["id_materia"],
                                    reader["n_materia"],
                                    reader["hora"],
                                    reader["aula"],
                                    reader["nombre"],
                                    "Reprobada (Obligatoria)"
                                );
                            }
                        }
                    }
                }

                // Cargar materias seleccionadas (que no sean reprobadas)
                foreach (int idMateria in materiasSeleccionadas)
                {
                    if (!materiasReprobadas.Contains(idMateria))
                    {
                        string consulta = @"
                                SELECT 
                                    M.id_materia,
                                    M.n_materia,
                                    CONVERT(VARCHAR, M.hora, 108) AS hora,
                                    M.aula,
                                    D.nombre
                                FROM 
                                    Materias M
                                LEFT JOIN 
                                    Docentes D ON M.id_docente = D.id_docente
                                WHERE 
                                    M.id_materia = @id_materia";

                        using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                        {
                            cmd.Parameters.AddWithValue("@id_materia", idMateria);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    dtHorario.Rows.Add(
                                        reader["id_materia"],
                                        reader["n_materia"],
                                        reader["hora"],
                                        reader["aula"],
                                        reader["nombre"],
                                        "Nueva"
                                    );
                                }
                            }
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();

                // Mostrar en DataGridView
                dgvHorario.DataSource = dtHorario;

                // Ocultar columna ID
                if (dgvHorario.Columns.Contains("ID"))
                {
                    dgvHorario.Columns["ID"].Visible = false;
                }

                // Configurar colores según tipo
                foreach (DataGridViewRow row in dgvHorario.Rows)
                {
                    if (row.Cells["Tipo"].Value.ToString() == "Reprobada (Obligatoria)")
                    {
                        row.DefaultCellStyle.BackColor = Color.MistyRose;
                    }
                    else if (row.Cells["Tipo"].Value.ToString() == "Nueva")
                    {
                        row.DefaultCellStyle.BackColor = Color.Honeydew;
                    }
                }

                // Habilitar botón de confirmación
                btnConfirmarReinscripcion.Enabled = true;

                // Mostrar mensaje confirmando que el horario es válido
                lblMensajes.Text = "El horario está listo para ser confirmado. Revisa los detalles antes de proceder.";
                horarioValido = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vista previa del horario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                horarioValido = false;
            }
        }

        private void btnConfirmarReinscripcion_Click(object sender, EventArgs e)
        {

        }

        private bool RealizarReinscripcion()
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Iniciar transacción
                SqlTransaction transaction = CONEXIONMAESTRA.conectar.BeginTransaction();

                try
                {
                    // 1. Actualizar semestre del alumno
                    int nuevoIdSemestre = idSemestre + 1;
                    if (nuevoIdSemestre > 8) nuevoIdSemestre = 8; // Limitar a 8 semestres

                    string consultaActualizarSemestre = @"
                            UPDATE Alumnos 
                            SET id_semestre = @nuevo_semestre 
                            WHERE id_alumno = @id_alumno";

                    using (SqlCommand cmd = new SqlCommand(consultaActualizarSemestre, CONEXIONMAESTRA.conectar, transaction))
                    {
                        cmd.Parameters.AddWithValue("@nuevo_semestre", nuevoIdSemestre);
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Registrar nuevas materias en Horarios
                    foreach (int idMateria in materiasSeleccionadas)
                    {
                        // Verificar si ya está registrada (por ser reprobada)
                        string consultaExiste = @"
                                SELECT COUNT(*) FROM Horarios 
                                WHERE id_alumno = @id_alumno AND id_materia = @id_materia";

                        int existente = 0;

                        using (SqlCommand cmd = new SqlCommand(consultaExiste, CONEXIONMAESTRA.conectar, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                            cmd.Parameters.AddWithValue("@id_materia", idMateria);
                            existente = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Si no existe, insertarla
                        if (existente == 0)
                        {
                            string consultaInsertarHorario = @"
                                    INSERT INTO Horarios (id_alumno, id_materia)
                                    VALUES (@id_alumno, @id_materia)";

                            using (SqlCommand cmd = new SqlCommand(consultaInsertarHorario, CONEXIONMAESTRA.conectar, transaction))
                            {
                                cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                                cmd.Parameters.AddWithValue("@id_materia", idMateria);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Confirmar transacción
                    transaction.Commit();
                    CONEXIONMAESTRA.cerrar();
                    return true;
                }
                catch (Exception ex)
                {
                    // Revertir transacción en caso de error
                    transaction.Rollback();
                    MessageBox.Show($"Error al realizar reinscripción: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CONEXIONMAESTRA.cerrar();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar proceso de reinscripción: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
                return false;
            }
        }

        private void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
            saveDialog.Title = "Guardar horario como PDF";
            saveDialog.FileName = $"Horario_{numeroControl}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GenerarPDFHorario(saveDialog.FileName);
                    MessageBox.Show("PDF generado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir el PDF
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Reemplaza el método GenerarPDFHorario por el siguiente:

        private void GenerarPDFHorario(string rutaArchivo)
        {
            try
            {
                // Convertir el DataGridView a DataTable para reutilizar la lógica
                DataTable dtHorario = new DataTable();
                foreach (DataGridViewColumn col in dgvHorario.Columns)
                {
                    dtHorario.Columns.Add(col.HeaderText, typeof(string));
                }
                foreach (DataGridViewRow row in dgvHorario.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dtHorario.NewRow();
                        for (int i = 0; i < dgvHorario.Columns.Count; i++)
                        {
                            dr[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        dtHorario.Rows.Add(dr);
                    }
                }

                // Crear documento PDF
                PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
                document.Info.Title = $"Horario de {nombreCompleto}";
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
                gfx.DrawString($"Alumno: {nombreCompleto}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 100);
                gfx.DrawString($"Número de Control: {numeroControl}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 120);
                gfx.DrawString($"Semestre: {semestreActual + 1}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 140);
                gfx.DrawString($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy}", fontSubtitulo, PdfSharp.Drawing.XBrushes.Black, 50, 160);

                // Definir columnas de la tabla
                double startY = 200;
                double rowHeight = 30;
                double colWidth1 = 180; // Materia
                double colWidth2 = 70;  // Hora
                double colWidth3 = 60;  // Aula
                double colWidth4 = 130; // Docente
                double colWidth5 = 90;  // Tipo
                double tableWidth = colWidth1 + colWidth2 + colWidth3 + colWidth4 + colWidth5;
                double tableX = 40;

                // Línea superior de la tabla
                gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                    tableX, startY,
                    tableX + tableWidth, startY);

                // Dibujar encabezados
                PdfSharp.Drawing.XRect headerRect = new PdfSharp.Drawing.XRect(tableX, startY, tableWidth, rowHeight);
                gfx.DrawRectangle(PdfSharp.Drawing.XBrushes.LightGray, headerRect);
                gfx.DrawRectangle(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1), headerRect);

                // Líneas verticales para separar encabezados
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
                    tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY,
                    tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY + rowHeight);

                // Textos de encabezados
                gfx.DrawString("Materia", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + 5, startY + 20);
                gfx.DrawString("Hora", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + 5, startY + 20);
                gfx.DrawString("Aula", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + 5, startY + 20);
                gfx.DrawString("Docente", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);
                gfx.DrawString("Tipo", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4 + 5, startY + 20);

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
                        tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY,
                        tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY + rowHeight);

                    gfx.DrawLine(new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 1),
                        tableX + tableWidth, startY,
                        tableX + tableWidth, startY + rowHeight);

                    // Datos de las celdas
                    string materia = row["Materia"].ToString();
                    string hora = row["Hora"].ToString();
                    string aula = row["Aula"].ToString();
                    string docente = row["Docente"] != DBNull.Value ? row["Docente"].ToString() : "";
                    string tipo = row.Table.Columns.Contains("Tipo") ? row["Tipo"].ToString() : "";

                    // Limitar texto si es demasiado largo
                    if (materia.Length > 30) materia = materia.Substring(0, 27) + "...";
                    if (docente.Length > 20) docente = docente.Substring(0, 17) + "...";

                    gfx.DrawString(materia, fontRegular, PdfSharp.Drawing.XBrushes.Black, tableX + 5, startY + 20);
                    gfx.DrawString(hora, fontRegular, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + 5, startY + 20);
                    gfx.DrawString(aula, fontRegular, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + 5, startY + 20);
                    gfx.DrawString(docente, fontRegular, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);
                    gfx.DrawString(tipo, fontRegular, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4 + 5, startY + 20);

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

                        // Líneas verticales para separar encabezados
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
                            tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY,
                            tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4, startY + rowHeight);

                        // Textos de encabezados
                        gfx.DrawString("Materia", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + 5, startY + 20);
                        gfx.DrawString("Hora", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + 5, startY + 20);
                        gfx.DrawString("Aula", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + 5, startY + 20);
                        gfx.DrawString("Docente", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + 5, startY + 20);
                        gfx.DrawString("Tipo", fontTablaHeader, PdfSharp.Drawing.XBrushes.Black, tableX + colWidth1 + colWidth2 + colWidth3 + colWidth4 + 5, startY + 20);

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
                document.Save(rutaArchivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}\n\nDetalles: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de volver al menú principal?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
