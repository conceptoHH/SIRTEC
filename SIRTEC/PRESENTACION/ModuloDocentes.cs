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
    public partial class ModuloDocentes : Form
    {
        public ModuloDocentes()
        {
            InitializeComponent();
        }

        private void btnSubirCal_Click(object sender, EventArgs e)
        {
            PRES_Docentes.calificaciones formCalificaciones = new PRES_Docentes.calificaciones();
            formCalificaciones.ShowDialog();
        }
        private void btnHorarios_Click(object sender, EventArgs e)
        {
            try
            {
                PRES_Docentes.DocenteHorario formHorario = new PRES_Docentes.DocenteHorario();
                formHorario.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el horario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnListas_Click(object sender, EventArgs e)
        {
            try
            {
                PRES_Docentes.DocenteListas formListas = new PRES_Docentes.DocenteListas();
                formListas.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir las listas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
