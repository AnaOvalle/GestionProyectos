
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{

    public partial class Inventario : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123");
        public Inventario()
        {
            InitializeComponent();
            dgvInventarioColecciones.Visible = false;
            dvgInventarioLibros.Visible = false;
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbInventario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbInventario.Text == "Libros")
                {
                    txtIDcoleccion.Enabled = false;
                    txtCantidadColeccion.Enabled = false;
                    txtFechaColeccion.Enabled = false;
                    txtFechaLibro.Enabled = true;
                    txtCantidadLibro.Enabled = true;
                    txtIDLibro.Enabled = true;
                    dgvInventarioColecciones.Visible = false;
                    dvgInventarioLibros.Visible = true;
                    MostrarLi();
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    txtIDcoleccion.Enabled = true;
                    txtCantidadColeccion.Enabled = true;
                    txtFechaColeccion.Enabled = true;
                    txtFechaLibro.Enabled = false;
                    txtCantidadLibro.Enabled = false;
                    txtIDLibro.Enabled = false;
                    dgvInventarioColecciones.Visible = true;
                    dvgInventarioLibros.Visible = false;
                    MostrarColecciones();  // Cambié el nombre de la función aquí
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MostrarLi()
        {
            try
            {
                conexion.Open();
                string consulta = @"
            SELECT 
                li.libros_id,
                li.titulo,
                li.isbn,
                li.año_publicacion,
                li.editorial,
                li.descripcion,
                IFNULL(SUM(stock.cantidad_stock_libros), 0) AS cantidad_total_libros,
                MAX(stock.fecha_entrada) AS fecha_entrada
            FROM 
                libros li
            LEFT JOIN 
                stock_libros stock ON li.libros_id = stock.libros_id
            GROUP BY 
                li.libros_id, li.titulo, li.isbn, li.año_publicacion, 
                li.editorial, li.descripcion";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                dvgInventarioLibros.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        public void MostrarColecciones()
        {
            try
            {
                conexion.Open();
                string consulta = @"
       
SELECT 
    MIN(sa.sagas_id) AS sagas_id,    -- Usa el ID mínimo o máximo en el grupo de nombres
    sa.nombre, 
    IFNULL(SUM(stock.cantidad_stock), 0) AS cantidad_total_stock, 
    MAX(stock.fecha_entrada) AS ultima_fecha
FROM 
    sagas sa
LEFT JOIN 
    stock_libros_saga stock ON sa.sagas_id = stock.librosSaga_id
GROUP BY 
    sa.nombre
ORDER BY 
    sa.nombre;";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                // Mostrar las sagas que tienen 0 stock
                foreach (DataRow row in dataTable.Rows)
                {
                    if (Convert.ToInt32(row["cantidad_total_stock"]) == 0)
                    {
                        // Acción a tomar si el stock es 0 (ej. agregar stock o marcar de alguna forma)
                        row["nombre"] = row["nombre"] + " (Sin stock)";
                    }
                }

                dgvInventarioColecciones.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }



        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dgvInventarioColecciones.Visible = false;
            dvgInventarioLibros.Visible = false;

            txtIDcoleccion.Text = "";
            txtCantidadColeccion.Text = "";
            txtFechaColeccion.Text = "";
            txtFechaLibro.Text = "";
            txtCantidadLibro.Text = "";
            txtIDLibro.Text = "";
            txtBusqueda.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string bu = txtBusqueda.Text;
                if (cbInventario.Text == "Libros")
                {

                    conexion.Open();
                    string consulta = "SELECT DISTINCT li.*, stock.cantidad_stock_libros, stock.fecha_entrada  FROM libros li " +
                                      "JOIN stock_libros stock WHERE li.titulo = '" + bu + "'";

                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    dvgInventarioLibros.DataSource = dataTable;
                    conexion.Close();
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    conexion.Open();
                    string consulta = "SELECT DISTINCT sa.*, stock.cantidad_stock, stock.fecha_entrada FROM sagas sa " +
                                      "JOIN stock_libros_saga stock ON sa.librosSaga_id = stock.librosSaga_id " +
                                      "WHERE sa.nombre = '" + bu + "'";  // Esta es la consulta corregida

                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    dgvInventarioColecciones.DataSource = dataTable;
                    conexion.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            MostrarLi();
            MostrarColecciones();
            MessageBox.Show("Actualizado");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbInventario.Text == "Libros")

                {
                    conexion.Open();
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  add_libros(libros_id,cantidad_add_libros,fecha_agregado_libro) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    conexion.Open();
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  add_libros_saga(librosSaga_id,cantidad_add,fecha_agregado) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbInventario.Text == "Libros")

                {
                    conexion.Open();
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  dell_libros(libros_id,cantidad_borra_libro,fecha_eliminado_libro) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    /*conexion.Open();
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  add_libros_saga(librosSaga_id,cantidad_add,fecha_agregado) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();*/

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbInventario.Text == "Libros")

                {
                    conexion.Open();
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);

                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("UPDATE stock_libros SET libros_id = " + id + ", cantidad_stock_libros = " + cant + ", fecha_entrada = '" + fecha + "' WHERE libros_id = '" + id + "'");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();

                }
                else if (cbInventario.Text == "Colecciones")
                {
                    conexion.Open();
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();

                    comando.Connection = conexion;
                    comando.CommandText = ("UPDATE stock_libros_saga SET librosSaga_id = " + id + ", cantidad_stock = " + cant + ", fecha_entrada = '" + fecha + "' WHERE librosSaga_id = '" + id + "'");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("mensaje enviado");
                    conexion.Close();

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}


