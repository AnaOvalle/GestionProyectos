using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec.wmf;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProyectoBiblioteca
{
    public partial class Reportes : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=localhost; Database=BibliotecaGestion; Uid=root; Pwd=SB1299; Port = 3306;");

        public Reportes()
        {
            InitializeComponent();
        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            cbTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList; //Evita ingresar información manualmente al combobox
            cbPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;

            cbTipoReporte.Items.Add("Prestamos"); 
            cbTipoReporte.Items.Add("Devoluciones"); 
            cbTipoReporte.Items.Add("Perdidas"); 
            cbTipoReporte.Items.Add("Usuarios"); 

            cbPeriodo.Items.Add("Enero"); 
            cbPeriodo.Items.Add("Febrero"); 
            cbPeriodo.Items.Add("Marzo"); 
            cbPeriodo.Items.Add("Abril"); 
            cbPeriodo.Items.Add("Mayo"); 
            cbPeriodo.Items.Add("Junio"); 
            cbPeriodo.Items.Add("Julio"); 
            cbPeriodo.Items.Add("Agosto"); 
            cbPeriodo.Items.Add("Septiembre"); 
            cbPeriodo.Items.Add("Octubre"); 
            cbPeriodo.Items.Add("Noviembre"); 
            cbPeriodo.Items.Add("Diciembre"); 
        }


        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Se aceptan solo numeros 
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255)) //Valores sacados de tabla ascii donde no aceptara ni letras ni caracteres como puntos, comas, guiones etc
            {
                e.Handled = true; // si es verdad, no deja introducirlos 
            }
        }

        public void ConexionGeneral(string consulta) //Abrir y cerrar conexion con la base de datos
        {
            conexion.Open();

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            DataTable dataTable = new DataTable();

            adaptador.Fill(dataTable);

            dgvReporte1.DataSource = dataTable;
            conexion.Close();
        }

        

        public void PeriodoPrestamos() //Procesar los prestamos por periodos (12 meses)
        {
            Dictionary<string, int> Meses = new Dictionary<string, int>() //
            {
                {"Enero", 1},
                {"Febrero", 2 },
                {"Marzo", 3 },
                {"Abril", 4 },
                {"Mayo", 5 },
                {"Junio", 6 },
                {"Julio", 7 },
                {"Agosto", 8 },
                {"Septiembre", 9 },
                {"Octubre", 10 },
                {"Noviembre", 11 },
                {"Diciembre", 12}
            };
           

            if (cbTipoReporte.Text == "Prestamos" && Meses.ContainsKey(cbPeriodo.Text) && string.IsNullOrWhiteSpace(txtID.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT * FROM prestamos WHERE MONTH(fecha_prestamo) = {mesNumero} AND YEAR(fecha_prestamo) = 2024";

                try
                {
                    
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else if (cbTipoReporte.Text == "Prestamos" && !string.IsNullOrWhiteSpace(txtID.Text) && int.TryParse(txtID.Text, out int id) && Meses.ContainsKey(cbPeriodo.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text]; 
  
                string consulta = $"SELECT * FROM prestamos WHERE MONTH(fecha_prestamo) = {mesNumero} AND YEAR(fecha_prestamo) AND prestamos_id = {id}";

                try
                {                  
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }
    
            }
            else
            {
                MessageBox.Show("Selecciona un tipo de reporte válido y un periodo adecuado.");
            }


        }

        public void PeriodoDevoluciones()
        {
            Dictionary<string, int> Meses = new Dictionary<string, int>() //
            {
                {"Enero", 1},
                {"Febrero", 2 },
                {"Marzo", 3 },
                {"Abril", 4 },
                {"Mayo", 5 },
                {"Junio", 6 },
                {"Julio", 7 },
                {"Agosto", 8 },
                {"Septiembre", 9 },
                {"Octubre", 10 },
                {"Noviembre", 11 },
                {"Diciembre", 12}
            };

            if (cbTipoReporte.Text == "Devoluciones" && Meses.ContainsKey(cbPeriodo.Text) && string.IsNullOrWhiteSpace(txtID.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT d.devolucion_id, d.estado AS Estado_Devolucion, p.fecha_devolucion, p.fecha_devuelto, p.estado AS estado_prestamo FROM  devoluciones d JOIN prestamos p ON d.prestamos_id = p.prestamos_id WHERE d.estado = 'devuelto' AND MONTH(p.fecha_prestamo) = {mesNumero}  AND YEAR(p.fecha_prestamo); ";

                try
                {                    
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else if (cbTipoReporte.Text == "Devoluciones" && !string.IsNullOrWhiteSpace(txtID.Text) && int.TryParse(txtID.Text, out int id) && Meses.ContainsKey(cbPeriodo.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text];

                string consulta = $"SELECT d.devolucion_id, d.estado AS Estado_Devolucion, p.fecha_devolucion, p.fecha_devuelto, p.estado AS estado_prestamo FROM  devoluciones d JOIN prestamos p ON d.prestamos_id = p.prestamos_id WHERE d.estado = 'devuelto' AND MONTH(p.fecha_prestamo) = {mesNumero}  AND YEAR(p.fecha_prestamo) AND prestamos_id = {id};";

                try
                {
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Selecciona un tipo de reporte válido y un periodo adecuado.");
            }

        }

        public void PeriodoPerdidas()
        {
            Dictionary<string, int> Meses = new Dictionary<string, int>() //
            {
                {"Enero", 1},
                {"Febrero", 2 },
                {"Marzo", 3 },
                {"Abril", 4 },
                {"Mayo", 5 },
                {"Junio", 6 },
                {"Julio", 7 },
                {"Agosto", 8 },
                {"Septiembre", 9 },
                {"Octubre", 10 },
                {"Noviembre", 11 },
                {"Diciembre", 12}
            };

            if (cbTipoReporte.Text == "Perdidas" && Meses.ContainsKey(cbPeriodo.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT * FROM perdidas WHERE MONTH(fecha_perdida) = {mesNumero} AND YEAR(fecha_perdida) ";

                try
                {
                    // Intentar ejecutar la consulta
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    // Si hay un error, mostrar un mensaje de error detallado
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else if(cbTipoReporte.Text == "Perdidas" && Meses.ContainsKey(cbPeriodo.Text) && string.IsNullOrWhiteSpace(txtID.Text) && int.TryParse(txtID.Text, out int id))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT * FROM perdidas WHERE MONTH(fecha_perdida) = {mesNumero} AND YEAR(fecha_perdida) AND prestamos_id = {id}";

                try
                {
                    // Intentar ejecutar la consulta
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    // Si hay un error, mostrar un mensaje de error detallado
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Selecciona un tipo de reporte válido y un periodo adecuado.");
            }


        }

        public void PeriodoUsuarios()
        {
            Dictionary<string, int> Meses = new Dictionary<string, int>() //
            {
                {"Enero", 1},
                {"Febrero", 2 },
                {"Marzo", 3 },
                {"Abril", 4 },
                {"Mayo", 5 },
                {"Junio", 6 },
                {"Julio", 7 },
                {"Agosto", 8 },
                {"Septiembre", 9 },
                {"Octubre", 10 },
                {"Noviembre", 11 },
                {"Diciembre", 12}
            };


            if (cbTipoReporte.Text == "Usuarios" && Meses.ContainsKey(cbPeriodo.Text))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT * FROM usuarios WHERE MONTH(fecha_registro) = {mesNumero} AND YEAR(fecha_registro)";

                try
                {
                    // Intentar ejecutar la consulta
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    // Si hay un error, mostrar un mensaje de error detallado
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }

            }
            else if (cbTipoReporte.Text == "Usuarios" && Meses.ContainsKey(cbPeriodo.Text) && string.IsNullOrWhiteSpace(txtID.Text) && int.TryParse(txtID.Text, out int id))
            {
                int mesNumero = Meses[cbPeriodo.Text]; // Obtener el número de mes
                string consulta = $"SELECT * FROM usuarios WHERE MONTH(fecha_registro) = {mesNumero} AND YEAR(fecha_registro) AND prestamos_id = {id}";

                try
                {
                    // Intentar ejecutar la consulta
                    ConexionGeneral(consulta);
                }
                catch (Exception ex)
                {
                    // Si hay un error, mostrar un mensaje de error detallado
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un tipo de reporte válido y un periodo adecuado.");
            }

        }

        private void btnReportes_Click(object sender, EventArgs e) 
        {
            if(cbTipoReporte.Text == "Prestamos")
            {
                PeriodoPrestamos();
            }
            else if (cbTipoReporte.Text == "Devoluciones")
            {
                PeriodoDevoluciones();
            }
            else if (cbTipoReporte.Text == "Perdidas")
            {
                PeriodoPerdidas();
            }
            else if (cbTipoReporte.Text == "Usuarios")
            {
                PeriodoUsuarios();
            }
 
        }

        private void btnImprimir_Click(object sender, EventArgs e) 
        {
            if (dgvReporte1.Rows.Count > 0)
            {
                SaveFileDialog Guardar = new SaveFileDialog();
                Guardar.Filter = "PDF (*.pdf)|*.pdf";
                Guardar.FileName = "Resultado.pdf";
                bool ErrorMessage = false;
                if (Guardar.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(Guardar.FileName))
                    {
                        try
                        {
                            File.Delete(Guardar.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Ocurrio un error al eliminar el archivo. " + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {


                        try
                        {
                            PdfPTable pdftabla = new PdfPTable(dgvReporte1.Columns.Count);
                            pdftabla.DefaultCell.Padding = 2;
                            pdftabla.WidthPercentage = 100;
                            pdftabla.HorizontalAlignment = Element.ALIGN_LEFT;                   

                            foreach (DataGridViewColumn columna in dgvReporte1.Columns) 
                            {
                                PdfPCell columnaCelda = new PdfPCell(new Phrase(columna.HeaderText));
                                pdftabla.AddCell(columnaCelda);
                            }
                            foreach (DataGridViewRow fila in dgvReporte1.Rows) 
                            {
                                
                                foreach (DataGridViewCell filaCelda in fila.Cells)
                                {
                                    var celdaValor = filaCelda.Value?.ToString() ?? "N/A"; 
                                    pdftabla.AddCell(celdaValor);
                                }
                            }

                            using (FileStream flujoDeArchivo = new FileStream(Guardar.FileName, FileMode.Create))
                            {
                                Document documento = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(documento, flujoDeArchivo);
                                documento.Open();

                                /*Font textoFuente = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
                                Paragraph texto = new Paragraph("Reporte", textoFuente);
                                documento.Add(new Paragraph("\n\n"));
                                documento.Add(texto);*/

                                documento.Add(pdftabla);
                                documento.Close();
                            }
                            MessageBox.Show("Exportación de datos exitosa.");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error de exportacion de los datos" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No se encontró ningun registro.");
                
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) //Limpiar dgvReportes
        {
            dgvReporte1.DataSource = null;
        }

    }
}

