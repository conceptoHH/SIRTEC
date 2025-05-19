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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSubirCal_Click(object sender, EventArgs e)
        {
            PRES_Docentes.calificaciones formCalificaciones = new PRES_Docentes.calificaciones();
            formCalificaciones.ShowDialog();
        }
    }
}
