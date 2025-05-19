using SIRTEC.DATOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIRTEC.PRESENTACION.PRES_Docentes
{
    public partial class calificaciones : Form
    {
        // Variables globales para almacenar los datos del alumno
        private int idAlumno = 0;
        private string nombreAlumno = "";
        private bool todosAprobados = true;
        private bool modoEdicion = false;
        private Dictionary<int, int> materiaHorarioMap = new Dictionary<int, int>(); // Mapea id_materia a id_horario
        private Dictionary<string, int> materiaIdMap = new Dictionary<string, int>(); // Mapea nombre_materia a id_materia
        private Dictionary<string, Dictionary<int, int>> calificacionesOriginales = new Dictionary<string, Dictionary<int, int>>(); // Para controlar cambios
        private int semestreAlumno = 0;

        // Nuevas variables para la lista de materias y alumnos
        private List<KeyValuePair<int, string>> materiasDocente = new List<KeyValuePair<int, string>>();
        private List<KeyValuePair<string, string>> alumnosMateria = new List<KeyValuePair<string, string>>();
        private int idDocenteActual = -1;

        public calificaciones()
        {
            InitializeComponent();
        }

        private void calificaciones_Load(object sender, EventArgs e)
        {
            // Configurar el DataGridView
            ConfigurarDataGridView();

            // Obtener el ID del docente actual
            idDocenteActual = UsuarioActual.IdEspecifico;

            if (idDocenteActual > 0)
            {
                // Mostrar ID del docente para depuración (se puede quitar en producción)
                // MessageBox.Show($"ID del docente actual: {idDocenteActual}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cargar materias del docente y sus alumnos
                CargarMateriasDocente();

                // Si hay materias para mostrar, configurar la interfaz
                if (materiasDocente.Count > 0)
                {
                    ConfigurarInterfazParaListaAlumnos();
                }
                else
                {
                    // Si no hay materias, solo mostrar un mensaje indicativo en la interfaz
                    label2.Text = "No hay materias asignadas";
                    txtNumControl.Visible = false;
                    btnBuscar.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("No se ha podido identificar al docente. Por favor inicie sesión nuevamente.",
                    "Error de identificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ConfigurarInterfazParaListaAlumnos()
        {
            // Ocultar campos de búsqueda por número de control
            label2.Text = "Seleccione una materia:";
            txtNumControl.Visible = false;
            btnBuscar.Visible = false;

            // Crear y configurar ComboBox para materias
            ComboBox cbxMaterias = new ComboBox();
            cbxMaterias.Name = "cbxMaterias";
            cbxMaterias.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            cbxMaterias.Location = new Point(220, 40); // Movido más a la derecha
            cbxMaterias.Size = new Size(280, 32);
            cbxMaterias.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMaterias.Tag = "Materias";

            // Crear y configurar ComboBox para alumnos
            ComboBox cbxAlumnos = new ComboBox();
            cbxAlumnos.Name = "cbxAlumnos";
            cbxAlumnos.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            cbxAlumnos.Location = new Point(670, 40); // Aumentada la distancia, más a la derecha
            cbxAlumnos.Size = new Size(280, 32);
            cbxAlumnos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxAlumnos.Tag = "Alumnos";
            cbxAlumnos.Enabled = false;

            // Etiqueta para combobox de materias
            Label lblMateria = new Label();
            lblMateria.Text = "Materia:";
            lblMateria.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblMateria.Location = new Point(220, 15); // Colocada encima del combobox, más a la derecha
            lblMateria.AutoSize = true;

            // Etiqueta para combobox de alumnos
            Label lblAlumno = new Label();
            lblAlumno.Text = "Alumno:";
            lblAlumno.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblAlumno.Location = new Point(670, 15); // Colocada encima del combobox, más a la derecha
            lblAlumno.AutoSize = true;

            // Agregar controles al panel
            panel1.Controls.Add(cbxMaterias);
            panel1.Controls.Add(cbxAlumnos);
            panel1.Controls.Add(lblMateria);
            panel1.Controls.Add(lblAlumno);

            // Llenar el combobox de materias
            cbxMaterias.DisplayMember = "Value";
            cbxMaterias.ValueMember = "Key";

            foreach (var materia in materiasDocente)
            {
                cbxMaterias.Items.Add(new KeyValuePair<int, string>(materia.Key, materia.Value));
            }

            if (cbxMaterias.Items.Count > 0)
            {
                cbxMaterias.SelectedIndex = 0;
            }

            // Evento para cuando se selecciona una materia
            cbxMaterias.SelectedIndexChanged += (s, args) =>
            {
                if (cbxMaterias.SelectedItem != null)
                {
                    var materiaSeleccionada = (KeyValuePair<int, string>)cbxMaterias.SelectedItem;
                    CargarAlumnosPorMateria(materiaSeleccionada.Key);

                    // Habilitar el combo de alumnos
                    cbxAlumnos.Enabled = true;
                }
            };

            // Evento para cuando se selecciona un alumno
            cbxAlumnos.SelectedIndexChanged += (s, args) =>
            {
                if (cbxAlumnos.SelectedItem != null)
                {
                    var alumnoSeleccionado = (KeyValuePair<string, string>)cbxAlumnos.SelectedItem;
                    string nControl = alumnoSeleccionado.Key;

                    // Llamar a la función de búsqueda de calificaciones con el número de control
                    txtNumControl.Text = nControl;
                    BuscarCalificaciones();
                }
            };
        }

        private void CargarMateriasDocente()
        {
            try
            {
                materiasDocente.Clear();
                CONEXIONMAESTRA.abrir();

                string consulta = @"
                        SELECT 
                            M.id_materia, 
                            M.n_materia
                        FROM 
                            Materias M
                        WHERE 
                            M.id_docente = @id_docente
                        ORDER BY 
                            M.n_materia";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_docente", idDocenteActual);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idMateria = Convert.ToInt32(reader["id_materia"]);
                            string nombreMateria = reader["n_materia"].ToString();

                            materiasDocente.Add(new KeyValuePair<int, string>(idMateria, nombreMateria));
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias del docente: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarAlumnosPorMateria(int idMateria)
        {
            try
            {
                alumnosMateria.Clear();

                // Limpiar el combobox de alumnos
                ComboBox cbxAlumnos = (ComboBox)panel1.Controls["cbxAlumnos"];
                cbxAlumnos.Items.Clear();
                cbxAlumnos.DisplayMember = "Value";
                cbxAlumnos.ValueMember = "Key";

                CONEXIONMAESTRA.abrir();

                string consulta = @"
                        SELECT 
                            A.n_control,
                            A.nombre + ' ' + A.a_paterno + ' ' + A.a_materno AS nombre_completo
                        FROM 
                            Alumnos A
                        INNER JOIN 
                            Horarios H ON A.id_alumno = H.id_alumno
                        WHERE 
                            H.id_materia = @id_materia
                        ORDER BY 
                            A.a_paterno, A.a_materno, A.nombre";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_materia", idMateria);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nControl = reader["n_control"].ToString();
                            string nombreCompleto = reader["nombre_completo"].ToString();

                            KeyValuePair<string, string> alumno = new KeyValuePair<string, string>(nControl, nombreCompleto);
                            alumnosMateria.Add(alumno);
                            cbxAlumnos.Items.Add(alumno);
                        }
                    }
                }

                // Si hay alumnos, seleccionar el primero
                if (cbxAlumnos.Items.Count > 0)
                {
                    cbxAlumnos.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No hay alumnos inscritos en esta materia.",
                        "Sin alumnos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos de la materia: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void ConfigurarDataGridView()
        {
            // Configuración básica
            dgvCalificaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCalificaciones.AllowUserToAddRows = false;
            dgvCalificaciones.AllowUserToDeleteRows = false;
            dgvCalificaciones.ReadOnly = true; // Inicialmente en modo de solo lectura
            dgvCalificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCalificaciones.MultiSelect = false;
            dgvCalificaciones.RowHeadersVisible = false;

            // Establecer el tamaño de fuente para todo el DataGridView
            dgvCalificaciones.DefaultCellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);

            // Estilo para encabezados de columnas
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.Font = new Font("Bahnschrift", 11F, FontStyle.Bold);
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); // Azul oscuro elegante
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCalificaciones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCalificaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCalificaciones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCalificaciones.ColumnHeadersHeight = 35; // Altura para los encabezados

            // Estilo para celdas alternadas
            dgvCalificaciones.RowsDefaultCellStyle.BackColor = Color.White;
            dgvCalificaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242); // Gris muy claro

            // Bordes y estilo de selección
            dgvCalificaciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCalificaciones.GridColor = Color.FromArgb(224, 224, 224); // Gris claro para las líneas

            // Estilo de selección
            dgvCalificaciones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185); // Azul claro
            dgvCalificaciones.DefaultCellStyle.SelectionForeColor = Color.White;

            // Crear columnas
            DataGridViewTextBoxColumn colIdMateria = new DataGridViewTextBoxColumn
            {
                Name = "id_materia",
                HeaderText = "ID Materia",
                Visible = false // Columna oculta para almacenar el ID de la materia
            };

            DataGridViewTextBoxColumn colMateria = new DataGridViewTextBoxColumn
            {
                Name = "materia",
                HeaderText = "Materia",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                ReadOnly = true // La materia siempre es de solo lectura
            };

            DataGridViewTextBoxColumn colUnidad1 = new DataGridViewTextBoxColumn
            {
                Name = "unidad1",
                HeaderText = "Unidad 1",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            DataGridViewTextBoxColumn colUnidad2 = new DataGridViewTextBoxColumn
            {
                Name = "unidad2",
                HeaderText = "Unidad 2",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            DataGridViewTextBoxColumn colUnidad3 = new DataGridViewTextBoxColumn
            {
                Name = "unidad3",
                HeaderText = "Unidad 3",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            DataGridViewTextBoxColumn colPromedio = new DataGridViewTextBoxColumn
            {
                Name = "promedio",
                HeaderText = "Promedio",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Bahnschrift", 10F, FontStyle.Bold)
                }
            };

            DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn
            {
                Name = "estado",
                HeaderText = "Estado",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            // Agregar las columnas al DataGridView
            dgvCalificaciones.Columns.Add(colIdMateria);
            dgvCalificaciones.Columns.Add(colMateria);
            dgvCalificaciones.Columns.Add(colUnidad1);
            dgvCalificaciones.Columns.Add(colUnidad2);
            dgvCalificaciones.Columns.Add(colUnidad3);
            dgvCalificaciones.Columns.Add(colPromedio);
            dgvCalificaciones.Columns.Add(colEstado);
        }


        private void txtNumControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y tecla de control (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            // Si el usuario presiona Enter, buscar calificaciones
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BuscarCalificaciones();
            }
        }

        private void BuscarCalificaciones()
        {
            // Validar que el número de control sea válido
            if (string.IsNullOrWhiteSpace(txtNumControl.Text))
            {
                MessageBox.Show("Por favor ingrese un número de control válido",
                    "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumControl.Focus();
                return;
            }

            // Buscar información del alumno por número de control
            if (BuscarInfoAlumno(txtNumControl.Text))
            {
                // Mostrar información del alumno
                pnlInformacion.Visible = true;
                lblNombreAlumno.Text = "Alumno: " + nombreAlumno;
                lblSemestre.Text = "Semestre: " + semestreAlumno;

                // Limpiar mapeos anteriores
                materiaHorarioMap.Clear();
                materiaIdMap.Clear();
                calificacionesOriginales.Clear();

                // Buscar y mostrar calificaciones del alumno
                CargarCalificaciones();
                
                // Mostrar estado general (aprobado o reprobado)
                if (todosAprobados)
                {
                    lblEstado.Text = "Estado: APROBADO";
                    lblEstado.ForeColor = Color.Green;
                }
                else
                {
                    lblEstado.Text = "Estado: REPROBADO";
                    lblEstado.ForeColor = Color.Red;
                }
            }
            else
            {
                // Ocultar panel de información y limpiar DataGridView
                pnlInformacion.Visible = false;
                dgvCalificaciones.Rows.Clear();
            }
        }

        private bool BuscarInfoAlumno(string numeroControl)
        {
            bool encontrado = false;

            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                            SELECT 
                                A.id_alumno,
                                A.nombre + ' ' + A.a_paterno + ' ' + A.a_materno AS nombre_completo,
                                S.n_semestre
                            FROM 
                                Alumnos A
                            INNER JOIN
                                Semestre S ON A.id_semestre = S.id_semestre
                            WHERE 
                                A.n_control = @n_control";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@n_control", numeroControl);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idAlumno = Convert.ToInt32(reader["id_alumno"]);
                            nombreAlumno = reader["nombre_completo"].ToString();
                            semestreAlumno = Convert.ToInt32(reader["n_semestre"]);
                            encontrado = true;
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún alumno con el número de control proporcionado",
                                "Alumno no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar alumno: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return encontrado;
        }

        private void CargarCalificaciones()
        {
            try
            {
                // Reiniciar variable de control
                todosAprobados = true;

                // Limpiar el DataGridView
                dgvCalificaciones.Rows.Clear();

                CONEXIONMAESTRA.abrir();

                // Primero obtenemos todas las materias del alumno
                string consultaMaterias = @"
                            SELECT 
                                M.id_materia,
                                M.n_materia,
                                H.id_horario
                            FROM 
                                Horarios H
                            INNER JOIN 
                                Materias M ON H.id_materia = M.id_materia
                            WHERE 
                                H.id_alumno = @id_alumno";

                // Si es docente, filtrar solo sus materias
                if (idDocenteActual > 0)
                {
                    consultaMaterias += " AND M.id_docente = @id_docente";
                }

                consultaMaterias += " ORDER BY M.n_materia";

                Dictionary<int, string> materias = new Dictionary<int, string>();

                using (SqlCommand cmd = new SqlCommand(consultaMaterias, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    if (idDocenteActual > 0)
                    {
                        cmd.Parameters.AddWithValue("@id_docente", idDocenteActual);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idMateria = Convert.ToInt32(reader["id_materia"]);
                            string nombreMateria = reader["n_materia"].ToString();
                            int idHorario = Convert.ToInt32(reader["id_horario"]);

                            materias.Add(idMateria, nombreMateria);
                            materiaHorarioMap.Add(idMateria, idHorario);
                            materiaIdMap.Add(nombreMateria, idMateria);
                        }
                    }
                }

                // Ahora para cada materia obtenemos sus calificaciones
                foreach (var materia in materias)
                {
                    int idMateria = materia.Key;
                    string nombreMateria = materia.Value;

                    // Estructura para almacenar calificaciones por unidad
                    double[] calificacionesUnidad = new double[3] { 0, 0, 0 };
                    bool[] unidadesRegistradas = new bool[3] { false, false, false };
                    Dictionary<int, int> calificacionesPorUnidad = new Dictionary<int, int>();

                    // Obtener calificaciones de la materia
                    string consultaCalificaciones = @"
                                SELECT 
                                    id_calificacion,
                                    unidad,
                                    calificacion
                                FROM 
                                    Calificaciones
                                WHERE 
                                    id_alumno = @id_alumno 
                                    AND id_materia = @id_materia";

                    using (SqlCommand cmdCalif = new SqlCommand(consultaCalificaciones, CONEXIONMAESTRA.conectar))
                    {
                        cmdCalif.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmdCalif.Parameters.AddWithValue("@id_materia", idMateria);

                        using (SqlDataReader readerCalif = cmdCalif.ExecuteReader())
                        {
                            while (readerCalif.Read())
                            {
                                int unidad = Convert.ToInt32(readerCalif["unidad"]);
                                double calificacion = Convert.ToDouble(readerCalif["calificacion"]);

                                // Almacenar calificación (ajustando el índice de unidad a base 0)
                                if (unidad >= 1 && unidad <= 3)
                                {
                                    calificacionesUnidad[unidad - 1] = calificacion;
                                    unidadesRegistradas[unidad - 1] = true;
                                    calificacionesPorUnidad[unidad] = (int)calificacion;
                                }
                            }
                        }
                    }

                    // Guardar las calificaciones originales para comparar cambios luego
                    calificacionesOriginales[nombreMateria] = new Dictionary<int, int>(calificacionesPorUnidad);

                    // Calcular promedio solo de las unidades registradas
                    double promedio = 0;
                    int unidadesConCalificacion = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        if (unidadesRegistradas[i])
                        {
                            promedio += calificacionesUnidad[i];
                            unidadesConCalificacion++;
                        }
                    }

                    // Calcular promedio si hay al menos una unidad con calificación
                    if (unidadesConCalificacion > 0)
                    {
                        promedio = Math.Round(promedio / unidadesConCalificacion, 1);
                    }

                    // Determinar si aprobó o no (se requiere 70 para aprobar y todas las unidades registradas deben ser >= 70)
                    bool aprobado = true;
                    for (int i = 0; i < 3; i++)
                    {
                        if (unidadesRegistradas[i] && calificacionesUnidad[i] < 70)
                        {
                            aprobado = false;
                            break;
                        }
                    }

                    // Si el promedio es menor a 70, también se considera reprobado
                    if (promedio < 70)
                    {
                        aprobado = false;
                    }

                    // Si alguna materia está reprobada, el estado general es reprobado
                    if (!aprobado)
                    {
                        todosAprobados = false;
                    }

                    string estado = aprobado ? "APROBADO" : "REPROBADO";

                    // Añadir fila al DataGridView con el id_materia oculto
                    int rowIndex = dgvCalificaciones.Rows.Add(
                        idMateria,
                        nombreMateria,
                        unidadesRegistradas[0] ? calificacionesUnidad[0].ToString() : "N/A",
                        unidadesRegistradas[1] ? calificacionesUnidad[1].ToString() : "N/A",
                        unidadesRegistradas[2] ? calificacionesUnidad[2].ToString() : "N/A",
                        unidadesConCalificacion > 0 ? promedio.ToString() : "N/A",
                        estado);

                    // Colorear estado según aprobado o reprobado
                    DataGridViewCell celda = dgvCalificaciones.Rows[rowIndex].Cells["estado"];
                    celda.Style.ForeColor = aprobado ? Color.Green : Color.Red;
                    celda.Style.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);

                    // Colorear calificaciones reprobatorias en rojo
                    for (int i = 0; i < 3; i++)
                    {
                        if (unidadesRegistradas[i] && calificacionesUnidad[i] < 70)
                        {
                            dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Style.ForeColor = Color.Red;
                        }
                    }

                    // Colorear promedio reprobatorio en rojo
                    if (promedio < 70 && unidadesConCalificacion > 0)
                    {
                        dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Style.ForeColor = Color.Red;
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar calificaciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void chkModoEdicion_CheckedChanged(object sender, EventArgs e)
        {
            modoEdicion = chkModoEdicion.Checked;

            // Actualizar el modo de edición en el DataGridView
            dgvCalificaciones.ReadOnly = !modoEdicion;

            // Si estamos en modo edición, hacer solo editable las columnas de unidades
            if (modoEdicion)
            {
                // Primero, hacer todas las columnas de solo lectura
                foreach (DataGridViewColumn col in dgvCalificaciones.Columns)
                {
                    col.ReadOnly = true;
                }

                // Luego, permitir la edición solo en las columnas de unidades
                dgvCalificaciones.Columns["unidad1"].ReadOnly = false;
                dgvCalificaciones.Columns["unidad2"].ReadOnly = false;
                dgvCalificaciones.Columns["unidad3"].ReadOnly = false;

                // Cambiar el cursor para las celdas editables
                dgvCalificaciones.Cursor = Cursors.Hand;

                // Mostrar botón guardar
                btnGuardar.Visible = true;

                // Cambiar color de fondo para indicar modo edición
                dgvCalificaciones.DefaultCellStyle.BackColor = Color.FromArgb(252, 252, 240);

                // Mostrar mensaje al usuario
                MessageBox.Show("Ahora puede hacer doble clic en las celdas de calificaciones para editarlas.\n" +
                             "Al finalizar, haga clic en 'Guardar' para guardar todos los cambios.",
                             "Modo de Edición Activado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Ocultar botón guardar
                btnGuardar.Visible = false;

                // Restaurar el cursor y el color de fondo
                dgvCalificaciones.Cursor = Cursors.Default;
                dgvCalificaciones.DefaultCellStyle.BackColor = Color.White;
                dgvCalificaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void dgvCalificaciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Solo procesar ediciones en las columnas de calificaciones (unidad1, unidad2, unidad3)
            if (e.RowIndex >= 0 && (e.ColumnIndex == dgvCalificaciones.Columns["unidad1"].Index ||
                                    e.ColumnIndex == dgvCalificaciones.Columns["unidad2"].Index ||
                                    e.ColumnIndex == dgvCalificaciones.Columns["unidad3"].Index))
            {
                // Obtener la celda editada
                DataGridViewCell celda = dgvCalificaciones.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string valorTexto = celda.Value?.ToString() ?? string.Empty;

                // Validar si es un número entre 0 y 100
                if (valorTexto != "N/A" && !string.IsNullOrEmpty(valorTexto))
                {
                    if (!double.TryParse(valorTexto, out double valor) || valor < 0 || valor > 100)
                    {
                        MessageBox.Show("La calificación debe ser un número entre 0 y 100",
                            "Valor inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        celda.Value = "N/A"; // Restaurar valor por defecto
                        return;
                    }

                    // Redondear al entero más cercano
                    celda.Value = Math.Round(valor, 0).ToString();
                }

                // Recalcular promedio y estado
                RecalcularPromedioYEstado(e.RowIndex);
            }
        }

        private void RecalcularPromedioYEstado(int rowIndex)
        {
            // Obtener las calificaciones de las tres unidades
            double[] calificaciones = new double[3];
            bool[] unidadesRegistradas = new bool[3];

            for (int i = 0; i < 3; i++)
            {
                string valorTexto = dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Value?.ToString() ?? string.Empty;

                if (valorTexto != "N/A" && !string.IsNullOrEmpty(valorTexto) && double.TryParse(valorTexto, out double valor))
                {
                    calificaciones[i] = valor;
                    unidadesRegistradas[i] = true;
                }
                else
                {
                    unidadesRegistradas[i] = false;
                }
            }

            // Calcular promedio
            double promedio = 0;
            int unidadesConCalificacion = 0;

            for (int i = 0; i < 3; i++)
            {
                if (unidadesRegistradas[i])
                {
                    promedio += calificaciones[i];
                    unidadesConCalificacion++;
                }
            }

            // Mostrar el promedio
            if (unidadesConCalificacion > 0)
            {
                promedio = Math.Round(promedio / unidadesConCalificacion, 1);
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Value = promedio.ToString();
            }
            else
            {
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Value = "N/A";
            }

            // Determinar si aprobó o no
            bool aprobado = true;
            for (int i = 0; i < 3; i++)
            {
                if (unidadesRegistradas[i] && calificaciones[i] < 70)
                {
                    aprobado = false;
                    dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Style.ForeColor = Color.Red;
                    dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Style.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
                }
                else if (unidadesRegistradas[i])
                {
                    dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Style.ForeColor = Color.Green;
                    dgvCalificaciones.Rows[rowIndex].Cells[$"unidad{i + 1}"].Style.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);
                }
            }

            // Si el promedio es menor a 70, también se considera reprobado
            if (promedio < 70 && unidadesConCalificacion > 0)
            {
                aprobado = false;
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Style.ForeColor = Color.Red;
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Style.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
            }
            else if (unidadesConCalificacion > 0)
            {
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Style.ForeColor = Color.Green;
                dgvCalificaciones.Rows[rowIndex].Cells["promedio"].Style.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);
            }

            // Actualizar estado
            string estado = aprobado ? "APROBADO" : "REPROBADO";
            dgvCalificaciones.Rows[rowIndex].Cells["estado"].Value = estado;
            dgvCalificaciones.Rows[rowIndex].Cells["estado"].Style.ForeColor = aprobado ? Color.Green : Color.Red;
            dgvCalificaciones.Rows[rowIndex].Cells["estado"].Style.Font = new Font("Bahnschrift", 10F, FontStyle.Bold);

            // Resaltar la fila editada para indicar cambios pendientes
            if (modoEdicion)
            {
                dgvCalificaciones.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 254, 217); // Amarillo muy claro
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de guardar los cambios en las calificaciones?",
                "Confirmar guardado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GuardarCalificaciones();
            }
        }

        private void GuardarCalificaciones()
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Lista para almacenar los mensajes de operaciones realizadas
                List<string> operacionesRealizadas = new List<string>();

                // Recorrer cada fila del DataGridView
                foreach (DataGridViewRow row in dgvCalificaciones.Rows)
                {
                    string nombreMateria = row.Cells["materia"].Value.ToString();
                    int idMateria = Convert.ToInt32(row.Cells["id_materia"].Value);

                    // Verificar las tres unidades
                    for (int unidad = 1; unidad <= 3; unidad++)
                    {
                        string valorCelda = row.Cells[$"unidad{unidad}"].Value?.ToString() ?? string.Empty;
                        bool tieneCalificacion = valorCelda != "N/A" && !string.IsNullOrEmpty(valorCelda);

                        // Si tiene calificación, actualizar o insertar
                        if (tieneCalificacion)
                        {
                            int calificacion = Convert.ToInt32(valorCelda);
                            bool calificacionCambiada = true;

                            // Verificar si la calificación ha cambiado
                            if (calificacionesOriginales.ContainsKey(nombreMateria) &&
                                calificacionesOriginales[nombreMateria].ContainsKey(unidad))
                            {
                                int calificacionOriginal = calificacionesOriginales[nombreMateria][unidad];
                                calificacionCambiada = calificacion != calificacionOriginal;
                            }

                            // Solo actualizar si ha cambiado
                            if (calificacionCambiada)
                            {
                                // Verificar si ya existe una calificación para esta unidad
                                string consultaExiste = @"
                                            SELECT COUNT(*) 
                                            FROM Calificaciones 
                                            WHERE id_alumno = @id_alumno 
                                              AND id_materia = @id_materia 
                                              AND unidad = @unidad";

                                int existeCalificacion = 0;

                                using (SqlCommand cmdExiste = new SqlCommand(consultaExiste, CONEXIONMAESTRA.conectar))
                                {
                                    cmdExiste.Parameters.AddWithValue("@id_alumno", idAlumno);
                                    cmdExiste.Parameters.AddWithValue("@id_materia", idMateria);
                                    cmdExiste.Parameters.AddWithValue("@unidad", unidad);
                                    existeCalificacion = Convert.ToInt32(cmdExiste.ExecuteScalar());
                                }

                                // Si existe, actualizar
                                if (existeCalificacion > 0)
                                {
                                    string consultaUpdate = @"
                                                UPDATE Calificaciones 
                                                SET calificacion = @calificacion, 
                                                    fecha_registro = GETDATE() 
                                                WHERE id_alumno = @id_alumno 
                                                  AND id_materia = @id_materia 
                                                  AND unidad = @unidad";

                                    using (SqlCommand cmdUpdate = new SqlCommand(consultaUpdate, CONEXIONMAESTRA.conectar))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@calificacion", calificacion);
                                        cmdUpdate.Parameters.AddWithValue("@id_alumno", idAlumno);
                                        cmdUpdate.Parameters.AddWithValue("@id_materia", idMateria);
                                        cmdUpdate.Parameters.AddWithValue("@unidad", unidad);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    operacionesRealizadas.Add($"Actualizada calificación de {nombreMateria}, Unidad {unidad}: {calificacion}");
                                }
                                // Si no existe, insertar
                                else
                                {
                                    string consultaInsert = @"
                                                INSERT INTO Calificaciones (calificacion, unidad, id_alumno, id_materia, fecha_registro)
                                                VALUES (@calificacion, @unidad, @id_alumno, @id_materia, GETDATE())";

                                    using (SqlCommand cmdInsert = new SqlCommand(consultaInsert, CONEXIONMAESTRA.conectar))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@calificacion", calificacion);
                                        cmdInsert.Parameters.AddWithValue("@unidad", unidad);
                                        cmdInsert.Parameters.AddWithValue("@id_alumno", idAlumno);
                                        cmdInsert.Parameters.AddWithValue("@id_materia", idMateria);
                                        cmdInsert.ExecuteNonQuery();
                                    }

                                    operacionesRealizadas.Add($"Registrada nueva calificación de {nombreMateria}, Unidad {unidad}: {calificacion}");
                                }
                            }
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();

                // Mostrar resumen de operaciones
                if (operacionesRealizadas.Count > 0)
                {
                    string mensaje = "Se realizaron las siguientes operaciones:\n\n" + string.Join("\n", operacionesRealizadas);
                    MessageBox.Show(mensaje, "Operaciones realizadas", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar calificaciones para mostrar los datos actualizados
                    CargarCalificaciones();
                }
                else
                {
                    MessageBox.Show("No se detectaron cambios en las calificaciones",
                        "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Desactivar modo edición
                chkModoEdicion.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar calificaciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Verificar si hay cambios sin guardar cuando estamos en modo edición
            if (modoEdicion && HayCambiosSinGuardar())
            {
                DialogResult resultado = MessageBox.Show(
                    "Hay cambios sin guardar. ¿Desea guardarlos antes de salir?",
                    "Cambios sin guardar",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    GuardarCalificaciones();
                    this.Close();
                }
                else if (resultado == DialogResult.No)
                {
                    this.Close();
                }
                // Si es Cancel, no hacer nada y quedarse en el formulario
            }
            else
            {
                this.Close();
            }
        }

        private bool HayCambiosSinGuardar()
        {
            foreach (DataGridViewRow row in dgvCalificaciones.Rows)
            {
                string nombreMateria = row.Cells["materia"].Value.ToString();

                // Verificar las tres unidades
                for (int unidad = 1; unidad <= 3; unidad++)
                {
                    string valorCelda = row.Cells[$"unidad{unidad}"].Value?.ToString() ?? string.Empty;
                    bool tieneCalificacion = valorCelda != "N/A" && !string.IsNullOrEmpty(valorCelda);

                    if (tieneCalificacion)
                    {
                        int calificacion = Convert.ToInt32(valorCelda);

                        // Verificar si la calificación ha cambiado
                        if (calificacionesOriginales.ContainsKey(nombreMateria) &&
                            calificacionesOriginales[nombreMateria].ContainsKey(unidad))
                        {
                            int calificacionOriginal = calificacionesOriginales[nombreMateria][unidad];
                            if (calificacion != calificacionOriginal)
                                return true;
                        }
                        else if (tieneCalificacion)
                        {
                            // Nueva calificación que no existía antes
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void dgvCalificaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que estamos en modo edición y que las columnas son editables
            if (modoEdicion && e.RowIndex >= 0)
            {
                // Solo permitir editar las columnas de unidades (1, 2, 3)
                if (e.ColumnIndex == dgvCalificaciones.Columns["unidad1"].Index ||
                    e.ColumnIndex == dgvCalificaciones.Columns["unidad2"].Index ||
                    e.ColumnIndex == dgvCalificaciones.Columns["unidad3"].Index)
                {
                    dgvCalificaciones.BeginEdit(true);
                }
            }
        }

        private void dgvCalificaciones_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Solo validar celdas de calificaciones
            if (e.ColumnIndex == dgvCalificaciones.Columns["unidad1"].Index ||
                e.ColumnIndex == dgvCalificaciones.Columns["unidad2"].Index ||
                e.ColumnIndex == dgvCalificaciones.Columns["unidad3"].Index)
            {
                string valorTexto = e.FormattedValue.ToString();

                // Si es N/A o vacío, permitir
                if (valorTexto == "N/A" || string.IsNullOrEmpty(valorTexto))
                {
                    return;
                }

                // Validar que sea un número entre 0 y 100
                if (!double.TryParse(valorTexto, out double valor) || valor < 0 || valor > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("La calificación debe ser un número entre 0 y 100 o 'N/A'",
                        "Valor inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Restaurar el foco a la celda que se está editando
                    dgvCalificaciones.CancelEdit();
                }
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            BuscarCalificaciones();

        }
    }
}
