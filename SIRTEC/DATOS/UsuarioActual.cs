using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRTEC.DATOS
{
    public static class UsuarioActual
    {
        public static int IdUsuario { get; set; } = -1;
        public static string TipoUsuario { get; set; } = string.Empty;
        public static int IdEspecifico { get; set; } = -1;
        public static string NombreUsuario { get; set; } = string.Empty;

        public static void Limpiar()
        {
            IdUsuario = -1;
            TipoUsuario = string.Empty;
            IdEspecifico = -1;
            NombreUsuario = string.Empty;
        }
    }
}
