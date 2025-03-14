using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SIRTEC.DATOS
{
    internal class CONEXIONMAESTRA
    {
        static string name = Environment.MachineName;
        public static string conexion = $@"Server={name}\SQLEXPRESS; Initial Catalog=SIRTEC;Integrated Security=True;";
        public static SqlConnection conectar = new SqlConnection(conexion);
        public static void abrir()
        {
            if (conectar.State == ConnectionState.Closed)
            {
                conectar.Open();
            }
        }
        public static void cerrar()
        {
            if (conectar.State == ConnectionState.Closed)
            {
                conectar.Close();
            }
        }
    }
}
