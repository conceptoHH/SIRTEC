﻿using System;
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
            pnlPadre.Controls.Clear();
            ctInscripcion ins = new ctInscripcion();
            ins.Dock = DockStyle.Fill;
            pnlPadre.Controls.Add(ins);
        }
    }
}
