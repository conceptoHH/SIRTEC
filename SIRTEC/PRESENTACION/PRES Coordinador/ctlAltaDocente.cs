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
    public partial class ctlAltaDocente : UserControl
    {
        public ctlAltaDocente()
        {
            InitializeComponent();
        }

        private void btnRegistrarDoc_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (RegistrarDocentes())
                {
                    MessageBox.Show("Docente registrado correctamente. Ahora puede iniciar sesión.",
                        "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrEmpty(txtNomDoc.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre del docente", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomDoc.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtUsuarioDoc.Text))
            {
                MessageBox.Show("Por favor ingrese un nombre de usuario", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuarioDoc.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtContrasenaDoc.Text))
            {
                MessageBox.Show("Por favor ingrese una contraseña", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasenaDoc.Focus();
                return false;
            }

            // Validar longitud mínima de los campos (que tenga al menos 3 caracteres)
            if (txtNomDoc.Text.Length < 3 || txtUsuarioDoc.Text.Length < 3 || txtContrasenaDoc.Text.Length < 3)
            {
                MessageBox.Show("Los campos de texto deben contener al menos 3 caracteres.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar formato de correo electrónico en el campo de usuario
            string email = txtUsuarioDoc.Text.Trim();
            if (!email.Contains("@") || !email.EndsWith(".com"))
            {
                MessageBox.Show("El nombre de usuario debe contener '@' y terminar en '.com'",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar si el usuario ya existe
            try
            {
                CONEXIONMAESTRA.abrir();
                string consultaUsuarioExistente = "SELECT COUNT(*) FROM Usuarios WHERE username = @user";
                using (SqlCommand cmd = new SqlCommand(consultaUsuarioExistente, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsuarioDoc.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("El nombre de usuario ya existe. Por favor elija otro.",
                            "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsuarioDoc.Focus();
                        CONEXIONMAESTRA.cerrar();
                        return false;
                    }
                }
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar usuario: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
                return false;
            }

            return true;
        }

        private bool RegistrarDocentes()
        {
            bool registroExitoso = false;
            int idDocente = 0;
            bool habilitado = false;

            try
            {
                // Primero obtenemos el próximo ID para el coordinador
                CONEXIONMAESTRA.abrir();
                string consultaMaxId = "SELECT ISNULL(MAX(id_docente), 0) + 1 FROM Docentes";
                SqlCommand cmdMaxId = new SqlCommand(consultaMaxId, CONEXIONMAESTRA.conectar);
                idDocente = Convert.ToInt32(cmdMaxId.ExecuteScalar());

                // Insertamos el nuevo coordinador
                string consultaCoordinador = "INSERT INTO Docentes(nombre, habilitado) " +
                                            "VALUES (@nombre, @habilitado)";

                using (SqlCommand cmdCoordinador = new SqlCommand(consultaCoordinador, CONEXIONMAESTRA.conectar))
                {
                    cmdCoordinador.Parameters.AddWithValue("@nombre", txtNomDoc.Text);
                    cmdCoordinador.Parameters.AddWithValue("@habilitado", habilitado);
                    cmdCoordinador.ExecuteNonQuery();
                }

                // Ahora obtenemos el próximo ID para el usuario
                string consultaMaxIdUsuario = "SELECT ISNULL(MAX(id_usuarios), 0) + 1 FROM Usuarios";
                SqlCommand cmdMaxIdUsuario = new SqlCommand(consultaMaxIdUsuario, CONEXIONMAESTRA.conectar);
                int idUsuario = Convert.ToInt32(cmdMaxIdUsuario.ExecuteScalar());

                // Insertamos el nuevo usuario asociado al coordinador
                string consultaUsuario = "INSERT INTO Usuarios(id_usuarios, tipo_usuario, username, password, id_docente) " +
                                       "VALUES (@id_usuarios, @tipo_usuario, @username, @password, @id_docente)";

                using (SqlCommand cmdUsuario = new SqlCommand(consultaUsuario, CONEXIONMAESTRA.conectar))
                {
                    cmdUsuario.Parameters.AddWithValue("@id_usuarios", idUsuario);
                    cmdUsuario.Parameters.AddWithValue("@tipo_usuario", "docente");
                    cmdUsuario.Parameters.AddWithValue("@username", txtUsuarioDoc.Text);
                    cmdUsuario.Parameters.AddWithValue("@password", txtContrasenaDoc.Text);
                    cmdUsuario.Parameters.AddWithValue("@id_docente", idDocente);
                    cmdUsuario.ExecuteNonQuery();
                }

                registroExitoso = true;
                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar coordinador: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return registroExitoso;
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

        private void ctlAltaDocente_Load(object sender, EventArgs e)
        {

        }
    }
}
