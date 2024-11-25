
using System;
using System.Data;
using MySql.Data.MySqlClient; // Cambiar SqlClient a MySqlClient
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class RegistroLibros : Form
    {
        MySqlConnection conexion = new MySqlConnection("Server=127.0.0.1;Database=Biblio2;Uid=root;Pwd=hola123;");

        public RegistroLibros()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            MySqlCommand altas = new MySqlCommand("INSERT INTO libros (titulo, autor, isbn, editorial, año_publicacion, genero_id, categoria_id) VALUES (@titulo, @autor, @isbn, @editorial, @año_publicacion, @genero_id, @categoria_id)", conexion);

            altas.Parameters.AddWithValue("@titulo", txtTitulo.Text);
            altas.Parameters.AddWithValue("@autor", txtApellido.Text);
            altas.Parameters.AddWithValue("@isbn", txtIBSN.Text);
            altas.Parameters.AddWithValue("@editorial", txtEditorial.Text);
            altas.Parameters.AddWithValue("@año_publicacion", txtAño.Text);
            altas.Parameters.AddWithValue("@genero_id", cbGenero.SelectedValue);
            altas.Parameters.AddWithValue("@categoria_id", cbCategoria.SelectedValue);

            try
            {
                conexion.Open();
                altas.ExecuteNonQuery();
                MessageBox.Show("Libro registrado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conexion.Close();
                RefrescarLibros();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int idLibro;

            if (int.TryParse(txtID.Text, out idLibro))
            {
                MySqlCommand actualizar = new MySqlCommand(
                    "UPDATE libros SET titulo = @titulo, autor = @autor, isbn = @isbn, editorial = @editorial, año_publicacion = @año_publicacion, genero_id = @genero_id, categoria_id = @categoria_id WHERE libros_id = @id",
                    conexion
                );

                actualizar.Parameters.AddWithValue("@titulo", txtTitulo.Text);
                actualizar.Parameters.AddWithValue("@autor", txtApellido.Text);
                actualizar.Parameters.AddWithValue("@isbn", txtIBSN.Text);
                actualizar.Parameters.AddWithValue("@editorial", txtEditorial.Text);
                actualizar.Parameters.AddWithValue("@año_publicacion", txtAño.Text);
                actualizar.Parameters.AddWithValue("@genero_id", cbGenero.SelectedValue);
                actualizar.Parameters.AddWithValue("@categoria_id", cbCategoria.SelectedValue);
                actualizar.Parameters.AddWithValue("@id", idLibro);

                try
                {
                    conexion.Open();
                    int filasAfectadas = actualizar.ExecuteNonQuery();
                    MessageBox.Show(filasAfectadas > 0 ? "Libro actualizado correctamente." : "No se encontró un libro con ese ID.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                    RefrescarLibros();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int idLibro;

            if (int.TryParse(txtID.Text, out idLibro))
            {
                MySqlCommand eliminar = new MySqlCommand("DELETE FROM libros WHERE libros_id = @id", conexion);
                eliminar.Parameters.AddWithValue("@id", idLibro);

                try
                {
                    conexion.Open();
                    int filasAfectadas = eliminar.ExecuteNonQuery();
                    MessageBox.Show(filasAfectadas > 0 ? "Libro eliminado correctamente." : "No se encontró un libro con ese ID.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                    RefrescarLibros();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.");
            }
        }

        private void RefrescarLibros()
        {
            try
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM libros", conexion);

                DataTable dataTable = new DataTable();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }

                DGVLibros.DataSource = dataTable;
                DGVLibros.BackgroundColor = Color.White;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void CargarGeneros()
        {
            try
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT genero_id, nombre FROM generos", conexion);

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                cbGenero.DataSource = dt;
                cbGenero.DisplayMember = "nombre";
                cbGenero.ValueMember = "genero_id";
                cbGenero.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar géneros: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void CargarCategorias()
        {
            try
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT categorias_id, nombre FROM categorias", conexion);

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                cbCategoria.DataSource = dt;
                cbCategoria.DisplayMember = "nombre";
                cbCategoria.ValueMember = "categorias_id";
                cbCategoria.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBuscar.Text;

            if (!string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                string query = "SELECT * FROM libros WHERE titulo LIKE @criterio OR autor LIKE @criterio OR isbn LIKE @criterio";

                try
                {
                    conexion.Open();
                    MySqlCommand buscar = new MySqlCommand(query, conexion);
                    buscar.Parameters.AddWithValue("@criterio", "%" + criterioBusqueda + "%");

                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = buscar.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    DGVLibros.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar los datos: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un criterio de búsqueda.");
            }
        }

        

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {
           
        }
        private void RegistroLibros_Load(object sender, EventArgs e)
        {
            CargarGeneros();
            CargarCategorias();
        }

        private void txtAutor_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
