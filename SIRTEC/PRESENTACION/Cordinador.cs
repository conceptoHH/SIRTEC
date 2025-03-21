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
            pnlPadre.Controls.Clear();
            ctlAltaDocente ctlAltaDocente = new ctlAltaDocente();
            ctlAltaDocente.Dock = DockStyle.Fill;
            pnlPadre.Controls.Add(ctlAltaDocente);
        }

        private void btnHorarios_Click(object sender, EventArgs e)
        {
            pnlPadre.Controls.Clear();
            ctlAltaHorario horario = new ctlAltaHorario();
            horario.Dock = DockStyle.Fill;
            pnlPadre .Controls.Add(horario);
        }
    }
}
