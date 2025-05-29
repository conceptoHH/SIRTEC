using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SIRTEC.SetupCustomActions
{
    [RunInstaller(true)]
    public class DatabaseSetup : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);

            try
            {
                // Crear directorio para la base de datos si no existe
                string targetDir = Context.Parameters["TARGETDIR"];
                string dbDir = Path.Combine(targetDir, "Data");

                if (!Directory.Exists(dbDir))
                {
                    Directory.CreateDirectory(dbDir);
                }

                // Ruta del script SQL
                string scriptPath = Path.Combine(targetDir, "SQL", "CreateDatabase.sql");

                if (File.Exists(scriptPath))
                {
                    // Crear/conectar a LocalDB y ejecutar script
                    string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=True;";
                    ExecuteSqlScript(connectionString, scriptPath);

                    // Crear archivo de configuración
                    string dbFilePath = Path.Combine(dbDir, "SIRTEC.mdf");
                    string logFilePath = Path.Combine(dbDir, "SIRTEC_log.ldf");

                    // Crear base de datos física si no existe
                    if (!File.Exists(dbFilePath))
                    {
                        CreateDatabase(connectionString, dbFilePath, logFilePath);
                    }

                    MessageBox.Show("Base de datos SIRTEC configurada correctamente.",
                        "Instalación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encontró el script de creación de base de datos.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar la base de datos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteSqlScript(string connectionString, string scriptPath)
        {
            string script = File.ReadAllText(scriptPath);
            string[] commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (var connection = new SqlConnection(connectionString)) // Fix: Ensure SqlConnection is used correctly
            {
                connection.Open();

                foreach (string commandText in commands)
                {
                    if (!string.IsNullOrWhiteSpace(commandText))
                    {
                        using (SqlCommand command = new SqlCommand(commandText.Trim(), connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                // Ignorar errores como "la base de datos ya existe"
                                if (!ex.Message.Contains("already exists"))
                                    throw;
                            }
                        }
                    }
                }
            }
        }

        private void CreateDatabase(string connectionString, string dbFilePath, string logFilePath)
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) // Fix: Ensure SqlConnection is used correctly
            {
                connection.Open();

                string createDbCommand = $@"
                    CREATE DATABASE SIRTEC ON 
                    PRIMARY (NAME = SIRTEC_Data, 
                    FILENAME = '{dbFilePath}', 
                    SIZE = 5MB, 
                    MAXSIZE = 50MB, 
                    FILEGROWTH = 10%) 
                    LOG ON (NAME = SIRTEC_Log, 
                    FILENAME = '{logFilePath}', 
                    SIZE = 1MB, 
                    MAXSIZE = 5MB, 
                    FILEGROWTH = 10%)";

                using (SqlCommand command = new SqlCommand(createDbCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}