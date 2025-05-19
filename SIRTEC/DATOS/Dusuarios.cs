using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRTEC.DATOS
{
    public class Dusuarios
    {
        /// <summary>
        /// Obtiene el tipo de usuario según su ID
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Tipo de usuario o cadena vacía si no existe</returns>
        public string ObtenerTipoUsuario(int idUsuario)
        {
            string tipoUsuario = string.Empty;

            try
            {
                CONEXIONMAESTRA.abrir();

                SqlCommand cmd = new SqlCommand("SELECT tipo_usuario FROM Usuarios WHERE id_usuarios = @IdUsuario", CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    tipoUsuario = resultado.ToString();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception("Error al obtener el tipo de usuario: " + ex.Message);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }

            return tipoUsuario;
        }

        /// <summary>
        /// Obtiene el tipo de usuario según su nombre de usuario
        /// </summary>
        /// <param name="nombreUsuario">Nombre del usuario</param>
        /// <returns>Tipo de usuario o cadena vacía si no existe</returns>
        public string ObtenerTipoUsuarioPorNombre(string nombreUsuario)
        {
            string tipoUsuario = string.Empty;

            try
            {
                CONEXIONMAESTRA.abrir();

                SqlCommand cmd = new SqlCommand("SELECT tipo_usuario FROM Usuarios WHERE username = @NombreUsuario", CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    tipoUsuario = resultado.ToString();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception("Error al obtener el tipo de usuario: " + ex.Message);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }

            return tipoUsuario;
        }
        /// <summary>
        /// Obtiene el ID de un usuario según su nombre de usuario
        /// </summary>
        /// <param name="nombreUsuario">Nombre del usuario</param>
        /// <returns>ID del usuario o -1 si no existe</returns>
        public int ObtenerIdUsuarioPorNombre(string nombreUsuario)
        {
            int idUsuario = -1;

            try
            {
                CONEXIONMAESTRA.abrir();

                SqlCommand cmd = new SqlCommand("SELECT id_usuarios FROM Usuarios WHERE username = @NombreUsuario", CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ID del usuario: " + ex.Message);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }

            return idUsuario;
        }

        /// <summary>
        /// Obtiene el ID de usuario basado en el tipo de usuario y su ID específico
        /// </summary>
        /// <param name="tipoUsuario">Tipo de usuario (alumno, docente, coordinador)</param>
        /// <param name="idEspecifico">ID específico (id_alumno, id_docente, id_coordinador)</param>
        /// <returns>ID del usuario o -1 si no existe</returns>
        public int ObtenerIdUsuarioPorTipoYReferencia(string tipoUsuario, int idEspecifico)
        {
            int idUsuario = -1;
            string campoReferencia = "";

            // Determinar qué campo de referencia usar según el tipo
            switch (tipoUsuario.ToLower())
            {
                case "alumno":
                    campoReferencia = "id_alumno";
                    break;
                case "docente":
                    campoReferencia = "id_docente";
                    break;
                case "coordinador":
                    campoReferencia = "id_coordinador";
                    break;
                default:
                    throw new ArgumentException("Tipo de usuario no válido");
            }

            try
            {
                CONEXIONMAESTRA.abrir();

                string query = $"SELECT id_usuarios FROM Usuarios WHERE {campoReferencia} = @IdEspecifico";
                SqlCommand cmd = new SqlCommand(query, CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdEspecifico", idEspecifico);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ID del usuario: " + ex.Message);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }

            return idUsuario;
        }

        /// <summary>
        /// Verifica si existen credenciales válidas y devuelve el ID del usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>ID del usuario o -1 si las credenciales son inválidas</returns>
        public int ValidarCredenciales(string username, string password)
        {
            int idUsuario = -1;

            try
            {
                CONEXIONMAESTRA.abrir();

                SqlCommand cmd = new SqlCommand("SELECT id_usuarios FROM Usuarios WHERE username = @Username AND password = @Password",
                                              CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar credenciales: " + ex.Message);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }

            return idUsuario;
        }
    }
}
