
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
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;");
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
                li.librosSaga_id,
                li.titulo,
                li.isbn,
                li.año_publicacion,
                li.editorial,
                li.descripcion,
                IFNULL(SUM(stock.cantidad_stock), 0) AS cantidad_total_libros,
                MAX(stock.fecha_entrada) AS fecha_entrada
            FROM 
                sagas li
            LEFT JOIN 
                stock_libros_saga stock ON li.librosSaga_id = stock.librosSaga_id
            GROUP BY 
                li.librosSaga_id, li.titulo, li.isbn, li.año_publicacion, 
                li.editorial, li.descripcion";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

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
            
            if (cbInventario.Text == "Libros")
            {

                txtFechaLibro.Text = "";
                txtCantidadLibro.Text = "";
                txtIDLibro.Text = "";
            }
            else if(cbInventario.Text == "Colecciones")
            {
                txtIDcoleccion.Text = "";
                txtCantidadColeccion.Text = "";
                txtFechaColeccion.Text = "";

            }

           
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBusqueda.Text;
            
            try
            {
                conexion.Open();
                string bu = txtBusqueda.Text;
                if (cbInventario.Text == "Libros")
                {

                    
                    string consulta = @"
            SELECT 
    li.libros_id,
    li.titulo,
    li.isbn,
    li.año_publicacion,
    li.editorial,
    li.descripcion,
    IFNULL(stock.cantidad_stock_libros, 0) AS cantidad_stock_libros,
    stock.fecha_entrada
FROM 
    libros li
LEFT JOIN 
    stock_libros stock ON li.libros_id = stock.libros_id
WHERE 
    li.titulo LIKE CONCAT('%', @criterio, '%') OR 
    li.autor LIKE CONCAT('%', @criterio, '%') OR 
    li.isbn LIKE CONCAT('%', @criterio, '%')
ORDER BY 
    li.titulo ASC; -- Ordenar los resultados
";

                    /*string consulta = "SELECT DISTINCT li.*, stock.cantidad_stock_libros, stock.fecha_entrada  FROM libros li " +
                                      "JOIN stock_libros stock WHERE li.titulo LIKE @criterio OR autor LIKE @criterio OR isbn LIKE @criterio";*/

                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@criterio", "%" + criterioBusqueda + "%");
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    dvgInventarioLibros.DataSource = dataTable;
                   
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    

                    string consulta = @"SELECT 
    li.librosSaga_id,
    li.titulo,
    li.isbn,
    li.año_publicacion,
    li.editorial,
    li.descripcion,
    IFNULL(stock.cantidad_stock, 0) AS cantidad_stock,
    stock.fecha_entrada
FROM 
    sagas li
LEFT JOIN 
    stock_libros_saga stock ON li.librosSaga_id = stock.librosSaga_id
WHERE 
    li.titulo LIKE CONCAT('%', @criterio, '%') OR 
    li.autor LIKE CONCAT('%', @criterio, '%') OR 
    li.isbn LIKE CONCAT('%', @criterio, '%')
ORDER BY 
    li.titulo ASC; -- Ordenar resultados alfabéticamente
";
                    

                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@criterio", "%" + criterioBusqueda + "%");
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    dgvInventarioColecciones.DataSource = dataTable;
                    
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                conexion.Close();
            }
            


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            MostrarLi();
            MostrarColecciones();
            
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                if (cbInventario.Text == "Libros")

                {
                   
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  add_libros(libros_id,cantidad_add_libros,fecha_agregado_libro) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros agregados");
                   
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  add_libros_saga(librosSaga_id,cantidad_add,fecha_agregado) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros agregados");
                   

                }
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {

                conexion.Open();
                if (cbInventario.Text == "Libros")

                {
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into  dell_libros(libros_id,cantidad_borra_libro,fecha_eliminado_libro) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros eliminados");

                    
                }
                else if (cbInventario.Text == "Colecciones")
                {
                    
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("insert into dell_libros_saga(librosSaga_id,cantidad_borra_libro,fecha_eliminado_libro) values(" + id + "," + cant + ",'" + fecha + "');");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros eliminados");
                    

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                conexion.Close();
            }

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                if (cbInventario.Text == "Libros")

                {
                    
                    string fecha = txtFechaLibro.Text;
                    int cant = int.Parse(txtCantidadLibro.Text);
                    int id = int.Parse(txtIDLibro.Text);

                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexion;
                    comando.CommandText = ("UPDATE stock_libros SET libros_id = " + id + ", cantidad_stock_libros = " + cant + ", fecha_entrada = '" + fecha + "' WHERE libros_id = '" + id + "'");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros actualizados");
                  

                }
                else if (cbInventario.Text == "Colecciones")
                {
          
                    string fecha = txtFechaColeccion.Text;
                    int cant = int.Parse(txtCantidadColeccion.Text);
                    int id = int.Parse(txtIDcoleccion.Text);
                    MySqlCommand comando = new MySqlCommand();

                    comando.Connection = conexion;
                    comando.CommandText = ("UPDATE stock_libros_saga SET librosSaga_id = " + id + ", cantidad_stock = " + cant + ", fecha_entrada = '" + fecha + "' WHERE librosSaga_id = '" + id + "'");
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Libros actualizados");
                   

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                conexion.Close();
            }

        }

        private void dgvInventarioColecciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIDcoleccion.Text = dgvInventarioColecciones.CurrentRow.Cells[0].Value.ToString();
            txtCantidadColeccion.Text = dgvInventarioColecciones.CurrentRow.Cells[6].Value.ToString();
            txtFechaColeccion.Text = dgvInventarioColecciones.CurrentRow.Cells[7].Value.ToString();

        }

        private void dvgInventarioLibros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIDLibro.Text = dvgInventarioLibros.CurrentRow.Cells[0].Value.ToString();
            txtCantidadLibro.Text = dvgInventarioLibros.CurrentRow.Cells[6].Value.ToString();
            txtFechaLibro.Text = dvgInventarioLibros.CurrentRow.Cells[7].Value.ToString();

        }
    }
}


