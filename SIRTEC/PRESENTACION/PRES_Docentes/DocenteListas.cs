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
    public partial class DocenteListas : Form
    {
        // Variables para almacenar la información del docente y sus materias
        private int idDocente = -1;
        private string nombreDocente = string.Empty;
        private List<KeyValuePair<int, string>> materiasDocente = new List<KeyValuePair<int, string>>();
        private DataTable dtAlumnos = new DataTable();
        private string nombreMateriaActual = string.Empty;
        private string horaMateriaActual = string.Empty;
        private string aulaMateriaActual = string.Empty;
        private string grupoActual = string.Empty;
        private int idMateriaActual = -1;

        public DocenteListas()
        {
            InitializeComponent();

            // Configurar cultura para evitar problemas de formato
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
        }

        private void DocenteListas_Load(object sender, EventArgs e)
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

                // Cargar materias del docente
                CargarMateriasDocente();

                // Configurar el DataGridView
                ConfigurarDataGridView();

                // Deshabilitar botón exportar hasta que se seleccione una materia
                btnExportarPDF.Enabled = false;
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

        private void CargarMateriasDocente()
        {
            try
            {
                materiasDocente.Clear();
                cbMaterias.Items.Clear();

                CONEXIONMAESTRA.abrir();

                string consulta = @"
                    SELECT 
                        M.id_materia, 
                        M.n_materia,
                        CONVERT(VARCHAR(5), M.hora, 108) AS hora,
                        M.aula,
                        M.grupo,
                        S.n_semestre,
                        P.n_paquete
                    FROM 
                        Materias M
                    INNER JOIN 
                        Paquetes P ON M.id_materia = P.id_materia
                    INNER JOIN 
                        Semestre S ON P.id_semestre = S.id_semestre
                    WHERE 
                        M.id_docente = @id_docente
                    ORDER BY 
                        S.n_semestre, P.n_paquete, M.n_materia";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_docente", idDocente);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idMateria = Convert.ToInt32(reader["id_materia"]);
                            string nombreMateria = reader["n_materia"].ToString();
                            string hora = reader["hora"].ToString();
                            string aula = reader["aula"].ToString();
                            string grupo = reader["grupo"].ToString();
                            int semestre = Convert.ToInt32(reader["n_semestre"]);
                            int paquete = Convert.ToInt32(reader["n_paquete"]);

                            // Crear un texto descriptivo para el ComboBox
                            string textoMateria = $"{nombreMateria} - Sem: {semestre} - Grupo: {grupo} - {hora}";

                            // Guardar la información de la materia para uso posterior
                            materiasDocente.Add(new KeyValuePair<int, string>(idMateria, textoMateria));

                            // Añadir al ComboBox
                            cbMaterias.Items.Add(textoMateria);
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();

                // Si hay materias, seleccionar la primera por defecto
                if (cbMaterias.Items.Count > 0)
                {
                    cbMaterias.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No se encontraron materias asignadas para este docente.",
                        "Sin materias", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las materias del docente: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void ConfigurarDataGridView()
        {
            // Personalizar la apariencia del DataGridView
            dgvAlumnos.EnableHeadersVisualStyles = false;
            dgvAlumnos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 157);
            dgvAlumnos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAlumnos.ColumnHeadersDefaultCellStyle.Font = new Font(dgvAlumnos.Font, FontStyle.Bold);
            dgvAlumnos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvAlumnos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvAlumnos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgvAlumnos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAlumnos.BackgroundColor = Color.White;
            dgvAlumnos.BorderStyle = BorderStyle.None;
            dgvAlumnos.RowHeadersVisible = false;
        }

        private void cbMaterias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaterias.SelectedIndex >= 0)
            {
                // Obtener el ID de la materia seleccionada
                idMateriaActual = materiasDocente[cbMaterias.SelectedIndex].Key;

                // Cargar los alumnos de esta materia
                CargarAlumnosMateria(idMateriaActual);

                // Obtener información adicional de la materia para el PDF
                ObtenerDetallesMateria(idMateriaActual);

                // Habilitar el botón para exportar a PDF
                btnExportarPDF.Enabled = true;
            }
        }

        private void CargarAlumnosMateria(int idMateria)
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                string consulta = @"
                    SELECT 
                        A.n_control AS 'Núm. Control',
                        A.a_paterno AS 'Apellido Paterno',
                        A.a_materno AS 'Apellido Materno',
                        A.nombre AS 'Nombre',
                        S.n_semestre AS 'Semestre',
                        CASE
                            WHEN AVG(C.calificacion) IS NULL THEN 'Sin evaluar'
                            WHEN AVG(C.calificacion) >= 70 THEN 'Aprobado'
                            ELSE 'Reprobado'
                        END AS 'Estado',
                        ISNULL(AVG(C.calificacion), 0) AS 'Promedio'
                    FROM 
                        Alumnos A
                    INNER JOIN 
                        Horarios H ON A.id_alumno = H.id_alumno
                    INNER JOIN
                        Semestre S ON A.id_semestre = S.id_semestre
                    LEFT JOIN
                        Calificaciones C ON A.id_alumno = C.id_alumno AND C.id_materia = H.id_materia
                    WHERE 
                        H.id_materia = @id_materia
                    GROUP BY
                        A.n_control, A.a_paterno, A.a_materno, A.nombre, S.n_semestre
                    ORDER BY 
                        A.a_paterno, A.a_materno, A.nombre";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_materia", idMateria);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    dtAlumnos = new DataTable();
                    adapter.Fill(dtAlumnos);

                    dgvAlumnos.DataSource = dtAlumnos;

                    // Actualizar contador de alumnos
                    lblTotalAlumnos.Text = $"Total de alumnos: {dtAlumnos.Rows.Count}";

                    // Dar formato a las columnas numéricas
                    if (dgvAlumnos.Columns.Contains("Promedio"))
                    {
                        dgvAlumnos.Columns["Promedio"].DefaultCellStyle.Format = "N1";
                        dgvAlumnos.Columns["Promedio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // Centrar algunas columnas
                    if (dgvAlumnos.Columns.Contains("Núm. Control"))
                        dgvAlumnos.Columns["Núm. Control"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    if (dgvAlumnos.Columns.Contains("Semestre"))
                        dgvAlumnos.Columns["Semestre"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    if (dgvAlumnos.Columns.Contains("Estado"))
                    {
                        dgvAlumnos.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Colorear celdas según el estado
                        foreach (DataGridViewRow row in dgvAlumnos.Rows)
                        {
                            if (row.Cells["Estado"].Value.ToString() == "Aprobado")
                                row.Cells["Estado"].Style.ForeColor = Color.Green;
                            else if (row.Cells["Estado"].Value.ToString() == "Reprobado")
                                row.Cells["Estado"].Style.ForeColor = Color.Red;
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista de alumnos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void ObtenerDetallesMateria(int idMateria)
        {
            try
            {
                CONEXIONMAESTRA.abrir();

                string consulta = @"
                    SELECT 
                        M.n_materia,
                        CONVERT(VARCHAR(5), M.hora, 108) AS hora,
                        M.aula,
                        M.grupo
                    FROM 
                        Materias M
                    WHERE 
                        M.id_materia = @id_materia";

                using (SqlCommand cmd = new SqlCommand(consulta, CONEXIONMAESTRA.conectar))
                {
                    cmd.Parameters.AddWithValue("@id_materia", idMateria);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nombreMateriaActual = reader["n_materia"].ToString();
                            horaMateriaActual = reader["hora"].ToString();
                            aulaMateriaActual = reader["aula"].ToString();
                            grupoActual = reader["grupo"].ToString();
                        }
                    }
                }

                CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener detalles de la materia: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CONEXIONMAESTRA.cerrar();
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            if (dtAlumnos.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para generar el PDF.",
                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostrar diálogo para guardar archivo
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
            saveDialog.Title = "Guardar lista como PDF";
            saveDialog.FileName = $"Lista_{nombreMateriaActual.Replace(" ", "_")}_{grupoActual}.pdf";

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
            // Configurar el encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Crear documento PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Lista de alumnos - {nombreMateriaActual}";
            document.Info.Subject = "Lista de alumnos";
            document.Info.Author = "Sistema SIRTEC";

            // Agregar página
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            // Obtener dimensiones de la página en unidades
            double pageWidth = page.Width.Point;
            double pageHeight = page.Height.Point;

            // Establecer márgenes
            double margenIzq = 30;
            double margenDer = 30;
            double margenSup = 30;

            // Calcular ancho disponible para la tabla
            double anchoDisponible = pageWidth - margenIzq - margenDer;

            // Crear gráficos para dibujar en la página
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Definir fuentes
            XFont fontTitulo = new XFont("Arial", 16, XFontStyleEx.Bold);
            XFont fontSubtitulo = new XFont("Arial", 12, XFontStyleEx.Regular);
            XFont fontDetalles = new XFont("Arial", 10, XFontStyleEx.Regular);
            XFont fontHeaderTabla = new XFont("Arial", 9, XFontStyleEx.Bold);
            XFont fontCelda = new XFont("Arial", 8, XFontStyleEx.Regular);
            XFont fontFooter = new XFont("Arial", 8, XFontStyleEx.Italic);

            // Dibujar título
            gfx.DrawString("Tecnológico Nacional de México", fontTitulo, XBrushes.DarkBlue,
                new XRect(0, 40, pageWidth, 30),
                XStringFormats.TopCenter);

            // Dibujar información del curso
            gfx.DrawString($"Lista de Alumnos", fontSubtitulo, XBrushes.Black,
                new XRect(0, 80, pageWidth, 30),
                XStringFormats.TopCenter);

            // Información de la materia
            double yPos = 120;
            gfx.DrawString($"Materia: {nombreMateriaActual}", fontDetalles, XBrushes.Black, margenIzq, yPos);
            yPos += 20;
            gfx.DrawString($"Docente: {nombreDocente}", fontDetalles, XBrushes.Black, margenIzq, yPos);
            yPos += 20;
            gfx.DrawString($"Grupo: {grupoActual}", fontDetalles, XBrushes.Black, margenIzq, yPos);
            gfx.DrawString($"Hora: {horaMateriaActual}", fontDetalles, XBrushes.Black, margenIzq + 200, yPos);
            gfx.DrawString($"Aula: {aulaMateriaActual}", fontDetalles, XBrushes.Black, margenIzq + 350, yPos);
            yPos += 20;
            gfx.DrawString($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}", fontDetalles, XBrushes.Black, margenIzq, yPos);

            // Configurar la tabla
            double inicioTabla = 200;
            double altoFila = 20;

            // Calcular anchos proporcionales para las columnas dentro del ancho disponible
            // Definir porcentajes para cada columna
            double[] porcentajesColumnas = { 0.12, 0.16, 0.16, 0.20, 0.08, 0.15, 0.13 }; // Porcentajes de ancho total
            double[] anchosColumnas = new double[porcentajesColumnas.Length];

            // Calcular anchos reales de columnas basados en el ancho disponible
            for (int i = 0; i < anchosColumnas.Length; i++)
            {
                anchosColumnas[i] = anchoDisponible * porcentajesColumnas[i];
            }

            // Dibujar encabezados de la tabla
            string[] encabezados = { "Núm. Control", "Ap. Paterno", "Ap. Materno", "Nombre", "Sem.", "Estado", "Prom." };
            double posX = margenIzq;

            // Fondo para los encabezados
            XRect rectEncabezados = new XRect(margenIzq, inicioTabla, anchoDisponible, altoFila);
            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 80, 157)), rectEncabezados);

            // Escribir los encabezados
            for (int i = 0; i < encabezados.Length; i++)
            {
                XRect rectColHeader = new XRect(posX, inicioTabla, anchosColumnas[i], altoFila);
                gfx.DrawString(encabezados[i], fontHeaderTabla, XBrushes.White,
                    rectColHeader, XStringFormats.Center);
                posX += anchosColumnas[i];
            }

            // Dibujar las filas de datos
            double posY = inicioTabla + altoFila;

            for (int fila = 0; fila < dtAlumnos.Rows.Count; fila++)
            {
                // Alternar colores de fondo para las filas
                XBrush colorFila = fila % 2 == 0
                    ? XBrushes.LightGray
                    : XBrushes.White;

                XRect rectFila = new XRect(margenIzq, posY, anchoDisponible, altoFila);
                gfx.DrawRectangle(colorFila, rectFila);

                // Dibujar los datos de cada columna
                posX = margenIzq;
                for (int col = 0; col < dtAlumnos.Columns.Count; col++)
                {
                    string valorCelda = dtAlumnos.Rows[fila][col].ToString();

                    XRect rectCelda = new XRect(posX, posY, anchosColumnas[col], altoFila);

                    // Alineación diferente según el tipo de columna
                    XStringFormat formato;

                    // Centrar número de control, semestre, estado y promedio
                    if (col == 0 || col == 4 || col == 5 || col == 6)
                        formato = XStringFormats.Center;
                    else
                        formato = XStringFormats.CenterLeft;

                    // Color especial para estado
                    XBrush colorTexto = XBrushes.Black;
                    if (col == 5) // Columna Estado
                    {
                        if (valorCelda == "Aprobado")
                            colorTexto = XBrushes.DarkGreen;
                        else if (valorCelda == "Reprobado")
                            colorTexto = XBrushes.DarkRed;
                    }

                    gfx.DrawString(valorCelda, fontCelda, colorTexto, rectCelda, formato);

                    posX += anchosColumnas[col];
                }

                posY += altoFila;

                // Si llegamos al final de la página, crear una nueva
                if (posY > pageHeight - 50 && fila < dtAlumnos.Rows.Count - 1)
                {
                    page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    gfx = XGraphics.FromPdfPage(page);

                    // Título en la nueva página
                    gfx.DrawString("Lista de Alumnos (continuación)", fontSubtitulo, XBrushes.DarkBlue,
                        new XRect(0, 40, pageWidth, 30), XStringFormats.TopCenter);

                    gfx.DrawString($"Materia: {nombreMateriaActual} - Grupo: {grupoActual}", fontDetalles, XBrushes.Black,
                        new XRect(0, 70, pageWidth, 20), XStringFormats.TopCenter);

                    // Reiniciar posición Y para la nueva página
                    posY = 100;

                    // Volver a dibujar los encabezados
                    posX = margenIzq;
                    rectEncabezados = new XRect(margenIzq, posY, anchoDisponible, altoFila);
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 80, 157)), rectEncabezados);

                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        XRect rectColHeader = new XRect(posX, posY, anchosColumnas[i], altoFila);
                        gfx.DrawString(encabezados[i], fontHeaderTabla, XBrushes.White,
                            rectColHeader, XStringFormats.Center);
                        posX += anchosColumnas[i];
                    }

                    posY += altoFila;
                }
            }

            // Dibujar el borde exterior de la tabla
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 80, 157), 1),
                new XRect(margenIzq, inicioTabla, anchoDisponible, posY - inicioTabla));

            // Pie de página
            gfx.DrawString("Sistema Integral de Registro Tecnológico - SIRTEC",
                fontFooter, XBrushes.Gray,
                new XRect(0, pageHeight - 30, pageWidth, 20), XStringFormats.Center);

            // Guardar el documento
            document.Save(rutaArchivo);
        }
    }
}