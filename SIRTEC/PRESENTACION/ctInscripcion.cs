using SIRTEC.DATOS;
using SIRTEC.LOGICA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIRTEC.PRESENTACION
{
    public partial class ctInscripcion : UserControl
    {
        // Lista para almacenar documentos seleccionados temporalmente
        private List<Ldocumentos> documentosSeleccionados = new List<Ldocumentos>();

        public ctInscripcion()
        {
            InitializeComponent();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar Documento";
                openFileDialog.Filter = "Archivos de Documento|*.pdf;";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        // Crear una instancia de Documento
                        Ldocumentos doc = new Ldocumentos
                        {
                            n_documento = Path.GetFileName(file),
                            extension = Path.GetExtension(file),
                            // TipoDocumento se deja vacío para que el usuario lo complete posteriormente
                            tipoDocumento = string.Empty,
                            contenido = File.ReadAllBytes(file)
                        };

                        // Agregar el documento a la lista
                        documentosSeleccionados.Add(doc);

                        // Actualizar la interfaz de usuario (por ejemplo, un ListBox)
                        txtNombreDocumento.Text = doc.n_documento;

                    }
                }
            }
        }

        private void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            documentosSeleccionados[documentosSeleccionados.Count - 1].tipoDocumento = lbTipoDocu.Text;

            if (documentosSeleccionados.Count == 0)
            {
                MessageBox.Show("No hay documentos para guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Guardar los documentos en la base de datos con id_alumno como NULL
            foreach (var docu in documentosSeleccionados)
            {
                InsertarDocumentoEnBD(docu);
            }

            MessageBox.Show("Documentos subidos correctamente. Se asociarán al alumno una vez completada la inscripción.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Limpiar la lista y la interfaz
            documentosSeleccionados.Clear();
            pnlDocu.Visible = false;
        }

        /// <summary>
        /// Método para insertar un documento en la base de datos.
        /// </summary>
        /// <param name="doc">Documento a insertar.</param>
        private void InsertarDocumentoEnBD(Ldocumentos doc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"INSERT INTO Documentos (n_documento, extension, tipo_documento, contenido, id_alumno)
                                             VALUES (@n_documento, @extension, @tipo_documento, @contenido, NULL)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@n_documento", doc.n_documento);
                        cmd.Parameters.AddWithValue("@extension", doc.extension);
                        cmd.Parameters.AddWithValue("@tipo_documento", doc.tipoDocumento);
                        cmd.Parameters.AddWithValue("@contenido", doc.contenido);

                        //cmd.Parameters.Add("@n_documento", SqlDbType.NVarChar, 255).Value = doc.n_documento;
                        //cmd.Parameters.Add("@extension", SqlDbType.NVarChar, 10).Value = doc.extension;
                        //cmd.Parameters.Add("@tipo_documento", SqlDbType.NVarChar, 50).Value = doc.tipoDocumento;
                        //cmd.Parameters.Add("@contenido", SqlDbType.VarBinary, -1).Value = doc.contenido;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al subir el documento {doc.n_documento}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Método para asociar documentos con un id_alumno después de la inscripción.
        /// </summary>
        /// <param name="idAlumno">ID del alumno.</param>
        public void AsociarDocumentosAAlumno(int idAlumno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXIONMAESTRA.conexion))
                {
                    conn.Open();
                    string query = @"UPDATE Documentos
                                             SET id_alumno = @id_alumno
                                             WHERE id_alumno IS NULL";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@id_alumno", SqlDbType.Int).Value = idAlumno;
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Documentos asociados al alumno correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al asociar los documentos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubirDoc_Click(object sender, EventArgs e)
        {
            pnlDocu.Visible = true;
        }
    }
}
