using SIRTEC.PRESENTACION.PRES_Coordinador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIRTEC.PRESENTACION
{
    public partial class Cordinador: Form
    {
        public Cordinador()
        {
            InitializeComponent();
        }

        private void btnAltaDocente_Click(object sender, EventArgs e)
        {
            // Ocultar los controles actuales del panel padre
            foreach (Control control in pnlPadre.Controls)
            {
                control.Visible = false;
            }

            // Crear una nueva instancia del control de inscripción
            PRES_Coordinador.ctlAltaDocente ins = new PRES_Coordinador.ctlAltaDocente();
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

        private void btnHorarios_Click(object sender, EventArgs e)
        {
            // Ocultar los controles actuales del panel padre
            foreach (Control control in pnlPadre.Controls)
            {
                control.Visible = false;
            }

            // Crear una nueva instancia del control de inscripción
            ctlAltaHorario ins = new ctlAltaHorario();
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

        private void Cordinador_Load(object sender, EventArgs e)
        {

        }

        private void btnConsultaAlumnos_Click(object sender, EventArgs e)
        {
            // Ocultar los controles actuales del panel padre
            foreach (Control control in pnlPadre.Controls)
            {
                control.Visible = false;
            }

            // Crear una nueva instancia del control de inscripción
            ctlConsultaAlumnos ins = new ctlConsultaAlumnos();
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
    }
}
