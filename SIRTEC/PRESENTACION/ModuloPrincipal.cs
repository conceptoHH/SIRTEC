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
        public ModuloPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
