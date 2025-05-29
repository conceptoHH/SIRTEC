using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;

namespace SIRTEC.DATOS
{
    internal class CONEXIONMAESTRA
    {
        // Cadena de conexión para SQL Server LocalDB
        public static string conexion = @"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SIRTEC.mdf;Integrated Security=True;Connect Timeout=30";
        
        // Intentar leer configuración desde App.config si existe
        static CONEXIONMAESTRA()
        {
            try
            {
                // Intentar obtener la conexión desde el archivo de configuración
                ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings["SIRTECConnection"];
                if (connectionSettings != null && !string.IsNullOrEmpty(connectionSettings.ConnectionString))
                {
                    conexion = connectionSettings.ConnectionString;
                }
                else
                {
                    // Si no está en la configuración, intentar crear/adjuntar la base de datos
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
                    string dbFolder = Path.Combine(appPath, "Data");
                    
                    if (!Directory.Exists(dbFolder))
                    {
                        Directory.CreateDirectory(dbFolder);
                    }
                    
                    string dbFilePath = Path.Combine(dbFolder, "SIRTEC.mdf");
                    AppDomain.CurrentDomain.SetData("DataDirectory", dbFolder);
                    
                    // Si la base de datos existe localmente, usarla, si no, configurar para crear una nueva
                    conexion = $@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True;Connect Timeout=30";
                }
            }
            catch
            {
                // En caso de error, usar la conexión local predeterminada
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string dbFolder = Path.Combine(appPath, "Data");
                AppDomain.CurrentDomain.SetData("DataDirectory", dbFolder);
            }
        }
        
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
            if (conectar.State == ConnectionState.Open)
            {
                conectar.Close();
            }
        }
    }
}
