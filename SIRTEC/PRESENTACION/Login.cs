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
            pnlPadre.Controls.Clear();
            ctInscripcion ins = new ctInscripcion();
            ins.Dock = DockStyle.Fill;
            pnlPadre.Controls.Add(ins);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsuario.Text;
            string password = txtContraseña.Text;

            if (ValidarCredenciales(username, password))
            {
                // Credenciales válidas, redirigir al usuario a la siguiente pantalla
                MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Aquí puedes redirigir al usuario a la pantalla principal
                ModuloPrincipal principal = new ModuloPrincipal();
                principal.Show();
                this.Hide();
            }
            else
            {
                // Credenciales inválidas, mostrar mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCredenciales(string username, string password)
        {
            bool credencialesValidas = false;

            try
            {
                CONEXIONMAESTRA.abrir();
                {
                    string consulta = "SELECT COUNT(*) FROM Usuarios WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                        credencialesValidas = cantidad > 0;
                    }
                    CONEXIONMAESTRA.cerrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar credenciales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }

            return credencialesValidas;
        }
    }
}
