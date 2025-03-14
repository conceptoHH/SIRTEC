using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRTEC.LOGICA
{
    public class Lusuarios
    {
        public int idUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int? IdAlumno { get; set; }
        public int? IdDocente { get; set; }
        public int? IdCoordinador { get; set; }
    }
}
