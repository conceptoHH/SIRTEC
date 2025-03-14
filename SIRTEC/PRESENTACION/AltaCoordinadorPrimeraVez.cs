using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using SIRTEC.DATOS;

namespace SIRTEC.PRESENTACION
{
    public partial class AltaCoordinadorPrimeraVez : Form
    {
        public AltaCoordinadorPrimeraVez()
        {
            InitializeComponent();
        }

        // Este método sería llamado desde un botón para registrar el coordinador
        private void btnRegistrarCoordinador_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (RegistrarCoordinador())
                {
                    MessageBox.Show("Coordinador registrado correctamente. Ahora puede iniciar sesión.",
                        "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cerrar el formulario y regresar al login
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrEmpty(txtNomCoord.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre del coordinador", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomCoord.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtUsuarioCoord.Text))
            {
                MessageBox.Show("Por favor ingrese un nombre de usuario", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuarioCoord.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtContrasenaCoord.Text))
            {
                MessageBox.Show("Por favor ingrese una contraseña", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasenaCoord.Focus();
                return false;
            }

            // Verificar si el usuario ya existe
            try
            {
                CONEXIONMAESTRA.abrir();
                string consultaUsuarioExistente = "SELECT COUNT(*) FROM Usuarios WHERE username = @user";
                using (SqlCommand cmd = new SqlCommand(consultaUsuarioExistente, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsuarioCoord.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("El nombre de usuario ya existe. Por favor elija otro.",
                            "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsuarioCoord.Focus();
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

        private bool RegistrarCoordinador()
        {
            bool registroExitoso = false;
            int idCoordinador = 0;

            try
            {
                // Primero obtenemos el próximo ID para el coordinador
                CONEXIONMAESTRA.abrir();
                string consultaMaxId = "SELECT ISNULL(MAX(id_coordinador), 0) + 1 FROM Coordinadores";
                SqlCommand cmdMaxId = new SqlCommand(consultaMaxId, CONEXIONMAESTRA.conectar);
                idCoordinador = Convert.ToInt32(cmdMaxId.ExecuteScalar());

                // Insertamos el nuevo coordinador
                string consultaCoordinador = "INSERT INTO Coordinadores(id_coordinador, nombre) " +
                                            "VALUES (@id_coordinador, @nombre)";

                using (SqlCommand cmdCoordinador = new SqlCommand(consultaCoordinador, CONEXIONMAESTRA.conectar))
                {
                    cmdCoordinador.Parameters.AddWithValue("@id_coordinador", idCoordinador);
                    cmdCoordinador.Parameters.AddWithValue("@nombre", txtNomCoord.Text);
                    cmdCoordinador.ExecuteNonQuery();
                }

                // Ahora obtenemos el próximo ID para el usuario
                string consultaMaxIdUsuario = "SELECT ISNULL(MAX(id_usuarios), 0) + 1 FROM Usuarios";
                SqlCommand cmdMaxIdUsuario = new SqlCommand(consultaMaxIdUsuario, CONEXIONMAESTRA.conectar);
                int idUsuario = Convert.ToInt32(cmdMaxIdUsuario.ExecuteScalar());

                // Insertamos el nuevo usuario asociado al coordinador
                string consultaUsuario = "INSERT INTO Usuarios(id_usuarios, tipo_usuario, username, password, id_coordinador) " +
                                       "VALUES (@id_usuarios, @tipo_usuario, @username, @password, @id_coordinador)";

                using (SqlCommand cmdUsuario = new SqlCommand(consultaUsuario, CONEXIONMAESTRA.conectar))
                {
                    cmdUsuario.Parameters.AddWithValue("@id_usuarios", idUsuario);
                    cmdUsuario.Parameters.AddWithValue("@tipo_usuario", "coordinador");
                    cmdUsuario.Parameters.AddWithValue("@username", txtUsuarioCoord.Text);
                    cmdUsuario.Parameters.AddWithValue("@password", txtContrasenaCoord.Text);
                    cmdUsuario.Parameters.AddWithValue("@id_coordinador", idCoordinador);
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
    }
}
