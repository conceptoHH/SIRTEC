using SIRTEC.DATOS;
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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Globalization;

namespace SIRTEC.PRESENTACION.PRES_Docentes
{
    public partial class DocenteHorario : Form
    {
        // Variables para almacenar la información del docente y su horario
        private int idDocente = -1;
        private string nombreDocente = string.Empty;
        private DataTable dtHorario = new DataTable();

        public DocenteHorario()
        {
            InitializeComponent();

            // Configurar cultura para evitar problemas de formato
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
        }

        private void DocenteHorario_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener el ID del docente del usuario actual
                idDocente = UsuarioActual.IdEspecifico;

                if (idDocente <= 0)
                {
                    MessageBox.Show("No se pudo identificar al docente. Por favor, inicie sesión nuevamente.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Obtener nombre del docente
                ObtenerNombreDocente();

                // Cargar horario del docente
                CargarHorarioDocente();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerNombreDocente()
        {
            try
            {
                CONEXIONMAESTRA.abrir();
                string consulta = "SELECT nombre FROM Docentes WHERE id_docente = @idDocente";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@idDocente", idDocente);
                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        nombreDocente = resultado.ToString();
                        lblNombreDocente.Text = $"Docente: {nombreDocente}";
                    }
                    else
                    {
                        nombreDocente = "Docente no encontrado";
                        lblNombreDocente.Text = $"Docente: {nombreDocente}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el nombre del docente: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void CargarHorarioDocente()
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                // Consulta SQL para obtener el horario del docente
                // Modificamos la consulta para convertir directamente la hora a string en SQL
                string consulta = @"
                    SELECT 
                        M.n_materia AS 'Materia',
                        CONVERT(VARCHAR(5), M.hora, 108) AS 'Hora',
                        M.aula AS 'Aula',
                        M.grupo AS 'Grupo',
                        P.n_paquete AS 'Paquete',
                        S.n_semestre AS 'Semestre'
                    FROM 
                        Materias M
                    INNER JOIN 
                        Paquetes P ON M.id_materia = P.id_materia
                    INNER JOIN 
                        Semestre S ON P.id_semestre = S.id_semestre
                    WHERE 
                        M.id_docente = @idDocente
                    ORDER BY 
                        S.n_semestre, P.n_paquete, M.hora";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@idDocente", idDocente);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    dtHorario = new DataTable();
                    adapter.Fill(dtHorario);

                    if (dtHorario.Rows.Count > 0)
                    {
                        // Mostrar los datos en el DataGridView
                        dgvHorario.DataSource = dtHorario;

                        // Formatear el DataGridView
                        FormatearGridView();

                        // Habilitar el botón para descargar el PDF
                        btnDescargarPDF.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron materias asignadas para este docente.",
                            "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnDescargarPDF.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el horario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDescargarPDF.Enabled = false;
            }
            finally
            {
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void FormatearGridView()
        {
            // Personalizar la apariencia del DataGridView
            dgvHorario.EnableHeadersVisualStyles = false;
            dgvHorario.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 157);
            dgvHorario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHorario.ColumnHeadersDefaultCellStyle.Font = new Font(dgvHorario.Font, FontStyle.Bold);
            dgvHorario.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHorario.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvHorario.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgvHorario.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvHorario.BackgroundColor = Color.White;
            dgvHorario.BorderStyle = BorderStyle.None;
            dgvHorario.RowHeadersVisible = false;

            // Ya no necesitamos formatear la columna hora porque viene como string desde SQL
            // Centrar columnas específicas
            foreach (string columna in new[] { "Hora", "Aula", "Grupo", "Paquete", "Semestre" })
            {
                if (dgvHorario.Columns.Contains(columna))
                {
                    dgvHorario.Columns[columna].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (dtHorario.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para generar el PDF.",
                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostrar diálogo para guardar archivo
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
            saveDialog.Title = "Guardar horario como PDF";
            saveDialog.FileName = $"Horario_{nombreDocente.Replace(" ", "_")}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Asegurarnos de que se pueda crear/sobrescribir el archivo
                    if (File.Exists(saveDialog.FileName))
                    {
                        try
                        {
                            File.Delete(saveDialog.FileName);
                        }
                        catch
                        {
                            MessageBox.Show("El archivo está siendo utilizado por otro programa. Ciérrelo e intente nuevamente.",
                                "Archivo en uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    GenerarPDF(saveDialog.FileName);
                    MessageBox.Show("El PDF ha sido generado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir el PDF generado
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar el PDF: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerarPDF(string rutaArchivo)
        {
            // Establecer codificación para documentos
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Crear documento PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Horario del docente {nombreDocente}";
            document.Info.Subject = "Horario de clases";
            document.Info.Author = "Sistema SIRTEC";

            // Agregar página
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            // Crear gráficos para dibujar en la página
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Definir fuentes
            XFont fontTitulo = new XFont("Arial", 18, XFontStyleEx.Bold);
            XFont fontSubtitulo = new XFont("Arial", 14, XFontStyleEx.Regular);
            XFont fontHeaderTabla = new XFont("Arial", 11, XFontStyleEx.Bold);
            XFont fontCelda = new XFont("Arial", 10, XFontStyleEx.Regular);

            // Dibujar título
            gfx.DrawString("Tecnológico Nacional de México", fontTitulo, XBrushes.DarkBlue,
                new XRect(0, 40, page.Width, 30),
                XStringFormats.TopCenter);

            gfx.DrawString($"Horario del Docente: {nombreDocente}", fontSubtitulo, XBrushes.Black,
                new XRect(0, 80, page.Width, 30),
                XStringFormats.TopCenter);

            gfx.DrawString($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}",
                new XFont("Arial", 10, XFontStyleEx.Italic),
                XBrushes.Black,
                new XRect(0, 110, page.Width, 20),
                XStringFormats.TopCenter);

            // Configurar la tabla
            double margenIzq = 50;
            double margenSup = 140;
            double altoFila = 30;
            double[] anchosColumnas = { 150, 60, 60, 60, 60, 70 }; // Ajustar según las columnas
            double anchoTotal = anchosColumnas.Sum();

            // Dibujar encabezados de la tabla
            string[] encabezados = { "Materia", "Hora", "Aula", "Grupo", "Paquete", "Semestre" };
            double posX = margenIzq;

            // Fondo para los encabezados
            XRect rectEncabezados = new XRect(
                margenIzq, margenSup, anchoTotal, altoFila);
            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 80, 157)),
                rectEncabezados);

            // Escribir los encabezados
            for (int i = 0; i < encabezados.Length; i++)
            {
                XRect rectColHeader = new XRect(
                    posX, margenSup, anchosColumnas[i], altoFila);
                gfx.DrawString(encabezados[i], fontHeaderTabla, XBrushes.White,
                    rectColHeader, XStringFormats.Center);
                posX += anchosColumnas[i];
            }

            // Dibujar las filas de datos
            double posY = margenSup + altoFila;

            for (int fila = 0; fila < dtHorario.Rows.Count; fila++)
            {
                // Alternar colores de fondo para las filas
                XBrush colorFila = fila % 2 == 0
                    ? XBrushes.LightGray
                    : XBrushes.White;

                XRect rectFila = new XRect(
                    margenIzq, posY, anchoTotal, altoFila);
                gfx.DrawRectangle(colorFila, rectFila);

                // Dibujar los datos de cada columna
                posX = margenIzq;
                for (int col = 0; col < encabezados.Length; col++)
                {
                    string valorCelda = dtHorario.Rows[fila][col].ToString();

                    XRect rectCelda = new XRect(
                        posX, posY, anchosColumnas[col], altoFila);

                    // Alineación según el tipo de columna
                    XStringFormat formato = col == 0
                        ? XStringFormats.CenterLeft
                        : XStringFormats.Center;

                    gfx.DrawString(valorCelda, fontCelda, XBrushes.Black,
                        rectCelda, formato);

                    posX += anchosColumnas[col];
                }

                posY += altoFila;

                // Si llegamos al final de la página, crear una nueva
                if (posY > page.Height - 50 && fila < dtHorario.Rows.Count - 1)
                {
                    page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    gfx = XGraphics.FromPdfPage(page);

                    // Título en la nueva página
                    gfx.DrawString("Horario del Docente (continuación)", fontSubtitulo,
                        XBrushes.DarkBlue,
                        new XRect(0, 40, page.Width, 30),
                        XStringFormats.TopCenter);

                    // Reiniciar posición Y para la nueva página
                    posY = 80;

                    // Volver a dibujar los encabezados
                    posX = margenIzq;
                    rectEncabezados = new XRect(
                        margenIzq, posY, anchoTotal, altoFila);
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 80, 157)),
                        rectEncabezados);

                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        XRect rectColHeader = new XRect(
                            posX, posY, anchosColumnas[i], altoFila);
                        gfx.DrawString(encabezados[i], fontHeaderTabla, XBrushes.White,
                            rectColHeader, XStringFormats.Center);
                        posX += anchosColumnas[i];
                    }

                    posY += altoFila;
                }
            }

            // Dibujar el borde exterior de la tabla
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 80, 157), 1),
                new XRect(margenIzq, margenSup, anchoTotal, posY - margenSup));

            // Pie de página
            gfx.DrawString("Sistema Integral de Registro Tecnológico - SIRTEC",
                new XFont("Arial", 8, XFontStyleEx.Italic),
                XBrushes.Gray,
                new XRect(0, page.Height - 30, page.Width, 20),
                XStringFormats.Center);

            // Guardar el documento
            document.Save(rutaArchivo);
        }
    }
}
