using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIRTEC.PRESENTACION;

namespace SIRTEC.PRESENTACION
{
    public partial class ModuloPrincipal : Form
    {
        private string _tipoUsuario;

        // Constructor original para compatibilidad
        public ModuloPrincipal()
        {
            InitializeComponent();
            ConfigurarVistaSegunTipoUsuario("coordinador"); // Por defecto
        }

        // Nuevo constructor que acepta el tipo de usuario
        public ModuloPrincipal(string tipoUsuario)
        {
            InitializeComponent();
            _tipoUsuario = tipoUsuario.ToLower();
            ConfigurarVistaSegunTipoUsuario(_tipoUsuario);
        }

        private void ConfigurarVistaSegunTipoUsuario(string tipoUsuario)
        {
            // Ocultar todos los botones primero
            btnInsc.Visible = false;
            btnCoord.Visible = false;
            btnDocentes.Visible = false;
            btnReinsc.Visible = false;

            // Mostrar solo los correspondientes al tipo de usuario
            switch (tipoUsuario)
            {
                case "alumno":
                    btnInsc.Visible = true;
                    btnReinsc.Visible = true;
                    // Aquí puedes añadir más botones específicos para alumnos
                    this.Text = "Sistema SIRTEC - Módulo Alumno";
                    break;
                case "docente":
                    btnDocentes.Visible = true;
                    // Aquí mostrarías los botones para docentes
                    // Por ejemplo: btnDocente.Visible = true;
                    this.Text = "Sistema SIRTEC - Módulo Docente";
                    break;
                case "coordinador":
                    btnInsc.Visible = true;
                    btnReinsc.Visible = true;
                    btnCoord.Visible = true;
                    // Mostrar todos los módulos para el coordinador
                    this.Text = "Sistema SIRTEC - Módulo Coordinador";
                    break;
                default:
                    MessageBox.Show("Tipo de usuario no reconocido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        private void btnInsc_Click(object sender, EventArgs e)
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

        private void btnCoord_Click(object sender, EventArgs e)
        {
            Cordinador cordinador = new Cordinador();
            cordinador.Show();
            this.Hide();
        }
    }
}
