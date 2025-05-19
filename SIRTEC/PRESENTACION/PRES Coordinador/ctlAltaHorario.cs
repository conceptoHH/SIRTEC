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

namespace SIRTEC.PRESENTACION.PRES_Coordinador
{
    public partial class ctlAltaHorario : UserControl
    {
        public ctlAltaHorario()
        {
            InitializeComponent();
            CargarDocentes();
            CargarMaterias();
        }

        private void btnGuardarMat_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (RegistrarHorario())
                {
                    MessageBox.Show("Materia registrada correctamente.",
                        "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrEmpty(txtN_materia.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre de la materia", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtN_materia.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtAula.Text))
            {
                MessageBox.Show("Por favor ingrese el aula de la materia", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAula.Focus();
                return false;
            }

            if (cbSemestre.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un semestre", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSemestre.Focus();
                return false;
            }

            if (cbPaquete.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un paquete", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPaquete.Focus();
                return false;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un grupo", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return false;
            }

            if (dgvVerDocentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un docente", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvVerDocentes.Focus();
                return false;
            }

            return true;
        }

        private bool RegistrarHorario()
        {
            bool registroExitoso = false;

            try
            {
                // Obtener el ID del docente seleccionado
                int idDocente = Convert.ToInt32(dgvVerDocentes.SelectedRows[0].Cells["id_docente"].Value);

                // Primero obtenemos el próximo ID para la materia
                DATOS.CONEXIONMAESTRA.abrir();
                string consultaMaxId = "SELECT ISNULL(MAX(id_materia), 0) + 1 FROM Materias";
                SqlCommand cmdMaxId = new SqlCommand(consultaMaxId, CONEXIONMAESTRA.conectar);
                int idMateria = Convert.ToInt32(cmdMaxId.ExecuteScalar());

                // Insertamos la nueva materia
                string consultaMateria = "INSERT INTO Materias(id_materia, n_materia, hora, aula, id_docente, grupo) " +
                                         "VALUES (@id_materia, @n_materia, @hora, @aula, @id_docente, @grupo)";

                using (SqlCommand cmdMateria = new SqlCommand(consultaMateria, CONEXIONMAESTRA.conectar))
                {
                    cmdMateria.Parameters.AddWithValue("@id_materia", idMateria);
                    cmdMateria.Parameters.AddWithValue("@n_materia", txtN_materia.Text);
                    cmdMateria.Parameters.AddWithValue("@hora", dtpHora.Value.TimeOfDay);
                    cmdMateria.Parameters.AddWithValue("@aula", txtAula.Text);
                    cmdMateria.Parameters.AddWithValue("@id_docente", idDocente);
                    cmdMateria.Parameters.AddWithValue("@grupo", comboBox1.SelectedItem.ToString());
                    cmdMateria.ExecuteNonQuery();
                }

                // Insertamos el paquete
                string consultaPaquete = "INSERT INTO Paquetes(id_materia, id_semestre, n_paquete) " +
                                         "VALUES (@id_materia, @id_semestre, @n_paquete)";

                using (SqlCommand cmdPaquete = new SqlCommand(consultaPaquete, CONEXIONMAESTRA.conectar))
                {
                    cmdPaquete.Parameters.AddWithValue("@id_materia", idMateria);
                    cmdPaquete.Parameters.AddWithValue("@id_semestre", cbSemestre.SelectedItem);
                    cmdPaquete.Parameters.AddWithValue("@n_paquete", cbPaquete.SelectedItem);
                    cmdPaquete.ExecuteNonQuery();
                }

                registroExitoso = true;
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el horario: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return registroExitoso;
        }

        private void CargarDocentes()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consultaDocentes = "SELECT id_docente, nombre FROM Docentes";
                SqlDataAdapter adapter = new SqlDataAdapter(consultaDocentes, CONEXIONMAESTRA.conectar);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvVerDocentes.DataSource = dt;
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los docentes: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarMaterias()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consultaMaterias = "SELECT id_materia, n_materia FROM Materias";
                SqlDataAdapter adapter = new SqlDataAdapter(consultaMaterias, CONEXIONMAESTRA.conectar);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvVerMaterias.DataSource = dt;
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las materias: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

        }
    }
}
