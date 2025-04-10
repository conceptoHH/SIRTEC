using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIRTEC.DATOS;
using System.Data.SqlClient;
using System.IO;

namespace SIRTEC.PRESENTACION.PRES_Coordinador
{
    public partial class ctlConsultaAlumnos : UserControl
    {
        // Variables globales
        private int idAlumnoSeleccionado = 0;
        private bool modoEdicion = false;

        public ctlConsultaAlumnos()
        {
            InitializeComponent();
        }

        private void ctlConsultaAlumnos_Load(object sender, EventArgs e)
        {
            // Configurar DataGridView
            ConfigurarDataGridView();

            // Cargar alumnos al iniciar
            CargarAlumnos();

            // Ocultar panel de edición inicialmente
            pnlEdicion.Visible = false;

            // Configurar ComboBox de estados
            CargarEstados();
        }

        private void ConfigurarDataGridView()
        {
            // Configuración básica
            dgvAlumnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlumnos.AllowUserToAddRows = false;
            dgvAlumnos.AllowUserToDeleteRows = false;
            dgvAlumnos.ReadOnly = true;
            dgvAlumnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlumnos.MultiSelect = false;
            dgvAlumnos.RowHeadersVisible = false;

            // Establecer el tamaño de fuente para todo el DataGridView
            dgvAlumnos.DefaultCellStyle.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);

            // Estilo para encabezados de columnas
            dgvAlumnos.ColumnHeadersDefaultCellStyle.Font = new Font("Bahnschrift", 11F, FontStyle.Bold);
            dgvAlumnos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); // Azul oscuro elegante
            dgvAlumnos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAlumnos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAlumnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAlumnos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvAlumnos.ColumnHeadersHeight = 35; // Altura más pronunciada para los encabezados

            // Estilo para celdas alternadas
            dgvAlumnos.RowsDefaultCellStyle.BackColor = Color.White;
            dgvAlumnos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242); // Gris muy claro

            // Bordes y estilo de selección
            dgvAlumnos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAlumnos.GridColor = Color.FromArgb(224, 224, 224); // Gris claro para las líneas

            // Estilo de selección
            dgvAlumnos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185); // Azul claro
            dgvAlumnos.DefaultCellStyle.SelectionForeColor = Color.White;

            // Crear columnas
            DataGridViewTextBoxColumn colControl = new DataGridViewTextBoxColumn
            {
                Name = "n_control",
                HeaderText = "No. Control",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 15
            };

            DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn
            {
                Name = "nombre_completo",
                HeaderText = "Nombre Completo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 35
            };

            DataGridViewTextBoxColumn colEmail = new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 25
            };

            DataGridViewTextBoxColumn colSemestre = new DataGridViewTextBoxColumn
            {
                Name = "semestre",
                HeaderText = "Semestre",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            // Agregar botón de editar
            DataGridViewButtonColumn colEditar = new DataGridViewButtonColumn
            {
                Name = "editar",
                HeaderText = "Editar",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(52, 152, 219), // Azul
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(41, 128, 185),
                    SelectionForeColor = Color.White
                }
            };

            // Agregar botón de eliminar
            DataGridViewButtonColumn colEliminar = new DataGridViewButtonColumn
            {
                Name = "eliminar",
                HeaderText = "Eliminar",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(231, 76, 60), // Rojo
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(192, 57, 43),
                    SelectionForeColor = Color.White
                }
            };

            // Agregar las columnas al DataGridView
            dgvAlumnos.Columns.Add(colControl);
            dgvAlumnos.Columns.Add(colNombre);
            dgvAlumnos.Columns.Add(colEmail);
            dgvAlumnos.Columns.Add(colSemestre);
            dgvAlumnos.Columns.Add(colEditar);
            dgvAlumnos.Columns.Add(colEliminar);

            // Agregar evento para el clic en botones
            dgvAlumnos.CellClick += DgvAlumnos_CellClick;
        }

        private void DgvAlumnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que se haya hecho clic en una fila válida
            if (e.RowIndex < 0) return;

            // Obtener el ID del alumno seleccionado (guardado en el Tag de la fila)
            idAlumnoSeleccionado = Convert.ToInt32(dgvAlumnos.Rows[e.RowIndex].Tag);

            // Si se hizo clic en el botón "Editar"
            if (e.ColumnIndex == dgvAlumnos.Columns["editar"].Index)
            {
                // Cargar datos del alumno en el panel de edición
                CargarDatosAlumno(idAlumnoSeleccionado);
                // Mostrar panel de edición
                pnlEdicion.Visible = true;
                pnlEdicion.Dock = DockStyle.Fill;
                pnlBuscar.Visible = false;
                modoEdicion = true;
                // Ocultar DataGridView
                dgvAlumnos.Visible = false;
                btnBuscar.Visible = false;
                txtBusqueda.Visible = false;
                lblBuscar.Visible = false;
            }
            // Si se hizo clic en el botón "Eliminar"
            else if (e.ColumnIndex == dgvAlumnos.Columns["eliminar"].Index)
            {
                // Confirmar eliminación
                ConfirmarEliminarAlumno(idAlumnoSeleccionado);
            }
        }

        private void CargarAlumnos()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            A.id_alumno,
                            A.n_control,
                            A.nombre + ' ' + A.a_paterno + ' ' + A.a_materno AS nombre_completo,
                            A.e_mail,
                            S.n_semestre
                        FROM 
                            Alumnos A
                        INNER JOIN 
                            Semestre S ON A.id_semestre = S.id_semestre
                        ORDER BY 
                            A.id_alumno DESC";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Limpiar filas existentes
                    dgvAlumnos.Rows.Clear();

                    // Agregar datos al DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvAlumnos.Rows.Add(
                            row["n_control"],
                            row["nombre_completo"],
                            row["e_mail"],
                            row["n_semestre"]);

                        // Guardar el ID del alumno en el Tag de la fila para uso posterior
                        dgvAlumnos.Rows[rowIndex].Tag = row["id_alumno"];
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarDatosAlumno(int idAlumno)
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            A.nombre,
                            A.a_paterno,
                            A.a_materno,
                            A.e_mail,
                            A.f_nacimiento,
                            A.calle,
                            A.colonia,
                            A.codpostal,
                            A.num_exterior,
                            A.ciudad,
                            A.estado,
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

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Llenar campos del formulario con datos del alumno
                        txtNombre.Text = reader["nombre"].ToString();
                        txtApellidoPaterno.Text = reader["a_paterno"].ToString();
                        txtApellidoMaterno.Text = reader["a_materno"].ToString();
                        txtEmail.Text = reader["e_mail"].ToString();
                        dtpFechaNacimiento.Value = Convert.ToDateTime(reader["f_nacimiento"]);
                        txtCalle.Text = reader["calle"].ToString();
                        txtColonia.Text = reader["colonia"].ToString();
                        txtCP.Text = reader["codpostal"].ToString();
                        txtNumero.Text = reader["num_exterior"].ToString();
                        txtCiudad.Text = reader["ciudad"].ToString();
                        cbEstado.Text = reader["estado"].ToString();
                        cbSemestre.Text = reader["n_semestre"].ToString();

                        // Guardar el ID del semestre
                        cbSemestre.Tag = reader["id_semestre"];
                    }
                    reader.Close();
                }

                // Cargar documentos del alumno
                CargarDocumentosAlumno(idAlumno);

                // Cargar horario del alumno
                CargarHorarioAlumno(idAlumno);

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del alumno: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarDocumentosAlumno(int idAlumno)
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            id_documento,
                            n_documento,
                            extension,
                            tipo_documento
                        FROM 
                            Documentos
                        WHERE 
                            id_alumno = @id_alumno";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Asignar datos al DataGridView de documentos
                    dgvDocumentos.DataSource = dt;

                    // Configurar columnas del DataGridView
                    if (dgvDocumentos.Columns.Count > 0)
                    {
                        dgvDocumentos.Columns["id_documento"].Visible = false;
                        dgvDocumentos.Columns["n_documento"].HeaderText = "Nombre";
                        dgvDocumentos.Columns["extension"].HeaderText = "Extensión";
                        dgvDocumentos.Columns["tipo_documento"].HeaderText = "Tipo";
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar documentos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarHorarioAlumno(int idAlumno)
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            M.n_materia AS Materia,
                            M.hora AS Hora,
                            M.aula AS Aula,
                            D.nombre AS Docente
                        FROM 
                            Horarios H
                        INNER JOIN 
                            Materias M ON H.id_materia = M.id_materia
                        LEFT JOIN 
                            Docentes D ON M.id_docente = D.id_docente
                        WHERE 
                            H.id_alumno = @id_alumno
                        ORDER BY 
                            M.hora";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Asignar datos al DataGridView de horario
                    dgvHorario.DataSource = dt;
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar horario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarEstados()
        {
            // Lista de estados de México
            string[] estados = new string[] {
                    "Aguascalientes", "Baja California", "Baja California Sur", "Campeche", "Chiapas",
                    "Chihuahua", "Ciudad de México", "Coahuila", "Colima", "Durango", "Estado de México",
                    "Guanajuato", "Guerrero", "Hidalgo", "Jalisco", "Michoacán", "Morelos", "Nayarit",
                    "Nuevo León", "Oaxaca", "Puebla", "Querétaro", "Quintana Roo", "San Luis Potosí",
                    "Sinaloa", "Sonora", "Tabasco", "Tamaulipas", "Tlaxcala", "Veracruz", "Yucatán", "Zacatecas"
                };

            // Asignar al combobox
            cbEstado.Items.AddRange(estados);

            // Cargar semestres
            CargarSemestres();
        }

        private void CargarSemestres()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = "SELECT id_semestre, n_semestre FROM Semestre ORDER BY n_semestre";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Limpiar items existentes
                    cbSemestre.Items.Clear();
                    cbSemestre.DisplayMember = "Text";
                    cbSemestre.ValueMember = "Value";

                    // Agregar semestres al combobox
                    while (reader.Read())
                    {
                        cbSemestre.Items.Add(new { Text = reader["n_semestre"].ToString(), Value = reader["id_semestre"] });
                    }
                    reader.Close();
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar semestres: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                GuardarCambiosAlumno();
            }
        }

        private bool ValidarDatos()
        {
            // Validar que los campos obligatorios no estén vacíos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellidoPaterno.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || cbSemestre.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor complete los campos obligatorios.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar formato de correo electrónico
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.EndsWith(".com"))
            {
                MessageBox.Show("El correo electrónico debe contener '@' y terminar en '.com'",
                    "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void GuardarCambiosAlumno()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        UPDATE Alumnos SET 
                            nombre = @nombre,
                            a_paterno = @a_paterno,
                            a_materno = @a_materno,
                            e_mail = @e_mail,
                            f_nacimiento = @f_nacimiento,
                            calle = @calle,
                            colonia = @colonia,
                            codpostal = @codpostal,
                            num_exterior = @num_exterior,
                            ciudad = @ciudad,
                            estado = @estado,
                            id_semestre = @id_semestre
                        WHERE 
                            id_alumno = @id_alumno";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@a_paterno", txtApellidoPaterno.Text);
                    cmd.Parameters.AddWithValue("@a_materno", txtApellidoMaterno.Text);
                    cmd.Parameters.AddWithValue("@e_mail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@f_nacimiento", dtpFechaNacimiento.Value);
                    cmd.Parameters.AddWithValue("@calle", txtCalle.Text);
                    cmd.Parameters.AddWithValue("@colonia", txtColonia.Text);
                    cmd.Parameters.AddWithValue("@codpostal", txtCP.Text);
                    cmd.Parameters.AddWithValue("@num_exterior", txtNumero.Text);
                    cmd.Parameters.AddWithValue("@ciudad", txtCiudad.Text);
                    cmd.Parameters.AddWithValue("@estado", cbEstado.Text);

                    // Obtener id_semestre del combobox
                    dynamic selectedItem = cbSemestre.SelectedItem;
                    int idSemestre = selectedItem != null ? Convert.ToInt32(selectedItem.Value) : Convert.ToInt32(cbSemestre.Tag);

                    cmd.Parameters.AddWithValue("@id_semestre", idSemestre);
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumnoSeleccionado);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Volver a la vista de lista
                VolverALista();

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void VolverALista()
        {
            // Ocultar panel de edición
            pnlEdicion.Visible = false;
            // Mostrar lista de alumnos
            dgvAlumnos.Visible = true;
            btnBuscar.Visible = true;
            txtBusqueda.Visible = true;
            lblBuscar.Visible = true;
            // Recargar datos
            CargarAlumnos();
            // Limpiar selección
            idAlumnoSeleccionado = 0;
            modoEdicion = false;
        }

        private void ConfirmarEliminarAlumno(int idAlumno)
        {
            // Obtener información del alumno
            string nombreAlumno = "";
            string numControl = "";

            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            nombre + ' ' + a_paterno + ' ' + a_materno AS nombre_completo,
                            n_control
                        FROM 
                            Alumnos
                        WHERE 
                            id_alumno = @id_alumno";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        nombreAlumno = reader["nombre_completo"].ToString();
                        numControl = reader["n_control"].ToString();
                    }
                    reader.Close();
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener información del alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
                return;
            }

            // Mostrar mensaje de confirmación
            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro de eliminar al alumno {nombreAlumno} (Control: {numControl})?\n\n" +
                "Esta acción también eliminará todos sus documentos y registros de horarios.\n\n" +
                "Esta operación no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                EliminarAlumno(idAlumno);
            }
        }

        private void EliminarAlumno(int idAlumno)
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Iniciar una transacción para garantizar la integridad de los datos
                SqlTransaction transaction = CONEXIONMAESTRA.conectar.BeginTransaction();

                try
                {
                    // 1. Eliminar registros en tabla Horarios
                    string consultaHorarios = "DELETE FROM Horarios WHERE id_alumno = @id_alumno";
                    using (SqlCommand cmd = new SqlCommand(consultaHorarios, CONEXIONMAESTRA.conectar, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Eliminar documentos
                    string consultaDocumentos = "DELETE FROM Documentos WHERE id_alumno = @id_alumno";
                    using (SqlCommand cmd = new SqlCommand(consultaDocumentos, CONEXIONMAESTRA.conectar, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Eliminar usuario
                    string consultaUsuario = "DELETE FROM Usuarios WHERE id_alumno = @id_alumno";
                    using (SqlCommand cmd = new SqlCommand(consultaUsuario, CONEXIONMAESTRA.conectar, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.ExecuteNonQuery();
                    }

                    // 4. Finalmente, eliminar el alumno
                    string consultaAlumno = "DELETE FROM Alumnos WHERE id_alumno = @id_alumno";
                    using (SqlCommand cmd = new SqlCommand(consultaAlumno, CONEXIONMAESTRA.conectar, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                        cmd.ExecuteNonQuery();
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    MessageBox.Show("Alumno eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar lista de alumnos
                    CargarAlumnos();
                }
                catch (Exception ex)
                {
                    // Revertir la transacción en caso de error
                    transaction.Rollback();
                    throw new Exception($"Error en la transacción: {ex.Message}");
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Volver a la vista de lista sin guardar cambios
            VolverALista();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Eliminar este control del panel padre
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            if (string.IsNullOrEmpty(textoBusqueda))
            {
                // Si no hay texto de búsqueda, cargar todos los alumnos
                CargarAlumnos();
                return;
            }

            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = @"
                        SELECT 
                            A.id_alumno,
                            A.n_control,
                            A.nombre + ' ' + A.a_paterno + ' ' + A.a_materno AS nombre_completo,
                            A.e_mail,
                            S.n_semestre
                        FROM 
                            Alumnos A
                        INNER JOIN 
                            Semestre S ON A.id_semestre = S.id_semestre
                        WHERE 
                            A.n_control LIKE @busqueda OR
                            A.nombre LIKE @busqueda OR
                            A.a_paterno LIKE @busqueda OR
                            A.a_materno LIKE @busqueda OR
                            A.e_mail LIKE @busqueda
                        ORDER BY 
                            A.id_alumno DESC";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@busqueda", $"%{textoBusqueda}%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Limpiar filas existentes
                    dgvAlumnos.Rows.Clear();

                    // Agregar datos al DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvAlumnos.Rows.Add(
                            row["n_control"],
                            row["nombre_completo"],
                            row["e_mail"],
                            row["n_semestre"]);

                        // Guardar el ID del alumno en el Tag de la fila para uso posterior
                        dgvAlumnos.Rows[rowIndex].Tag = row["id_alumno"];
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar alumnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void dgvDocumentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Obtener ID del documento seleccionado
            int idDocumento = Convert.ToInt32(dgvDocumentos.Rows[e.RowIndex].Cells["id_documento"].Value);

            // Mostrar documento
            MostrarDocumento(idDocumento);
        }

        private void MostrarDocumento(int idDocumento)
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = "SELECT n_documento, contenido, extension FROM Documentos WHERE id_documento = @id_documento";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_documento", idDocumento);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nombreArchivo = reader["n_documento"].ToString();
                            byte[] contenido = (byte[])reader["contenido"];
                            string extension = reader["extension"].ToString();

                            // Guardar temporalmente el archivo para abrirlo
                            string tempPath = Path.Combine(Path.GetTempPath(), nombreArchivo);
                            File.WriteAllBytes(tempPath, contenido);

                            // Abrir el archivo con la aplicación predeterminada
                            System.Diagnostics.Process.Start(tempPath);
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir documento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }
    }
}
