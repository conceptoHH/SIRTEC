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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Verificar si existe algún coordinador al cargar el formulario
            if (!ExisteCoordinador())
            {
                // No existe coordinador, mostrar formulario de registro
                MostrarFormularioRegistroCoordinador();
            }
        }

        private bool ExisteCoordinador()
        {
            bool existeCoordinador = false;

            try
            {
                CONEXIONMAESTRA.abrir();
                {
                    string consulta = "SELECT COUNT(*) FROM Usuarios WHERE tipo_usuario = 'coordinador'";

                    using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                    {
                        int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                        existeCoordinador = cantidad > 0;
                    }
                    CONEXIONMAESTRA.cerrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar coordinadores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return existeCoordinador;
        }

        private void MostrarFormularioRegistroCoordinador()
        {
            AltaCoordinadorPrimeraVez formRegistro = new AltaCoordinadorPrimeraVez();
            DialogResult resultado = formRegistro.ShowDialog();
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Ocultar los controles actuales del panel padre
            foreach (Control control in pnlPadre.Controls)
            {
                control.Visible = false;
            }

            // Crear una nueva instancia del control de inscripción
            ctlInscripcion ins = new ctlInscripcion();
            ins.Dock = DockStyle.Fill;

            // Agregar el control al panel
            pnlPadre.Controls.Add(ins);

            // Asegurarse de que el botón "Volver" en ctInscripcion funcione correctamente
            // Para esto, podemos manejar el evento Disposed del control
            ins.Disposed += (s, args) =>
            {
                // Cuando el control se elimine, mostrar de nuevo todos los controles del login
                foreach (Control control in pnlPadre.Controls)
                {
                    if (control != ins)
                    {
                        control.Visible = true;
                    }
                }
            };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsuario.Text;
            string password = txtContraseña.Text;

            string tipoUsuario = ValidarCredenciales(username, password);

            if (!string.IsNullOrEmpty(tipoUsuario))
            {
                // Credenciales válidas, redirigir al usuario a la siguiente pantalla
                MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Pasar el tipo de usuario al formulario principal
                ModuloPrincipal principal = new ModuloPrincipal(tipoUsuario);
                principal.Show();
                this.Hide();
            }
            else
            {
                // Credenciales inválidas, mostrar mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ValidarCredenciales(string username, string password)
        {
            string tipoUsuario = "";

            try
            {
                CONEXIONMAESTRA.abrir();
                {
                    string consulta = "SELECT tipo_usuario FROM Usuarios WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tipoUsuario = reader["tipo_usuario"].ToString();
                            }
                        }
                    }
                    CONEXIONMAESTRA.cerrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar credenciales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return tipoUsuario;
        }
    }
}
