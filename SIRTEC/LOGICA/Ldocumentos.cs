using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRTEC.LOGICA
{
    public class Ldocumentos
    {
        public int iddocumento {  get; set; }
        public string n_documento {  get; set; }
        public string extension { get; set; }
        public string tipoDocumento { get; set; }
        public byte[] contenido { get; set; }

    }
}
