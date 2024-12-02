
using System;
using System.Data;
using MySql.Data.MySqlClient; // Cambiar SqlClient a MySqlClient
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Bunifu.UI.WinForms;

namespace ProyectoBiblioteca
{
    public partial class RegistroLibros : Form
    {
        MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;");

        public RegistroLibros()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Imagen.Image.Save(ms, ImageFormat.Jpeg);
            byte[] data = ms.ToArray();
            string estado = "Activo";
            MySqlCommand altas = new MySqlCommand("INSERT INTO libros (titulo, autor, isbn, editorial, año_publicacion, genero_id, categoria_id, descripcion, imagen, estado) VALUES (@titulo, @autor, @isbn, @editorial, @año_publicacion, @genero_id, @categoria_id,@descripcion, @imagen, @estado)", conexion);

            altas.Parameters.AddWithValue("@titulo", txtTitulo.Text);
            altas.Parameters.AddWithValue("@autor", txtAutor.Text);
            altas.Parameters.AddWithValue("@isbn", txtIBSN.Text);
            altas.Parameters.AddWithValue("@editorial", txtEditorial.Text);
            altas.Parameters.AddWithValue("@año_publicacion", txtAño.Text);
            altas.Parameters.AddWithValue("@genero_id", cbGenero.SelectedValue);
            altas.Parameters.AddWithValue("@categoria_id", cbCategoria.SelectedValue);
            altas.Parameters.AddWithValue("@descripcion", txtDescrip.Text);
            altas.Parameters.AddWithValue("@imagen", data);
            altas.Parameters.AddWithValue("@estado", estado);

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
            MemoryStream ms = new MemoryStream();
            Imagen.Image.Save(ms, ImageFormat.Jpeg);
            byte[] data = ms.ToArray();

            MySqlCommand actualizar = new MySqlCommand(
                "UPDATE libros SET titulo = @titulo, autor = @autor, isbn = @isbn, editorial = @editorial, año_publicacion = @año_publicacion, genero_id = @genero_id, categoria_id = @categoria_id, descripcion = @descripcion, imagen = @imagen WHERE titulo = @titulo",
                conexion
            );
            actualizar.Parameters.AddWithValue("@titulo", txtTitulo.Text);
            actualizar.Parameters.AddWithValue("@autor", txtAutor.Text);
            actualizar.Parameters.AddWithValue("@isbn", txtIBSN.Text);
            actualizar.Parameters.AddWithValue("@editorial", txtEditorial.Text);
            actualizar.Parameters.AddWithValue("@año_publicacion", txtAño.Text);
            actualizar.Parameters.AddWithValue("@genero_id", cbGenero.SelectedValue);
            actualizar.Parameters.AddWithValue("@categoria_id", cbCategoria.SelectedValue);
            actualizar.Parameters.AddWithValue("@descripcion", txtDescrip.Text);
            actualizar.Parameters.AddWithValue("@imagen", data);

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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string estado = "Inactivo";
            MySqlCommand eliminar = new MySqlCommand("UPDATE libros SET estado = @estado WHERE titulo = @titulo ", conexion);
            eliminar.Parameters.AddWithValue("@estado", estado);
            eliminar.Parameters.AddWithValue("@titulo", txtTitulo.Text);
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

        private void RefrescarLibros()
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.libros_id, li.titulo, li.isbn, li.año_publicacion, li.editorial, li.descripcion, li.autor,  gen.nombre AS genero, ca.nombre AS categoria, li.estado " +
                                  "FROM libros li " +
                                  "JOIN generos gen ON li.genero_id = gen.genero_id " +
                                  "JOIN categorias ca ON li.categoria_id = ca.categorias_id";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                DGVLibros.DataSource = dataTable;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(buscar);
                    DataTable dataTable = new DataTable();
                    adaptador.Fill(dataTable);
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
            RefrescarLibros();
        }

        private void txtAutor_TextChanged(object sender, EventArgs e)
        {

        }

        private void DGVLibros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTitulo.Text = DGVLibros.CurrentRow.Cells[1].Value.ToString();
            txtAutor.Text = DGVLibros.CurrentRow.Cells[6].Value.ToString();
            txtIBSN.Text = DGVLibros.CurrentRow.Cells[2].Value.ToString();
            txtEditorial.Text = DGVLibros.CurrentRow.Cells[4].Value.ToString();
            txtAño.Text = DGVLibros.CurrentRow.Cells[3].Value.ToString();
            cbGenero.Text = DGVLibros.CurrentRow.Cells[7].Value.ToString();
            cbCategoria.Text = DGVLibros.CurrentRow.Cells[8].Value.ToString();
            txtDescrip.Text = DGVLibros.CurrentRow.Cells[5].Value.ToString();
            int id = int.Parse(DGVLibros.CurrentRow.Cells[0].Value.ToString());
            conexion.Open();
            try
            {
                
                string sql = "SELECT imagen FROM libros WHERE libros_id='" + id + "'";
                MySqlCommand comando = new MySqlCommand(sql, conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    MemoryStream ms = new MemoryStream((byte[])reader["imagen"]);
                    Bitmap bm = new Bitmap(ms);
                    Imagen.Visible = true;
                    Imagen.Image = bm;
                }
                else
                {
                    MessageBox.Show("No se cuenta con imagen");
                    Imagen.Visible = false;
                }
            }
            catch
            {
                MessageBox.Show("No se cuenta con imagene");
                Imagen.Visible = false;
            }
            conexion.Close();

        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            Imagen.Visible = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagenes|*.jpg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Title = "Seleccionar imagen";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Imagen.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void DGVLibros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DGVLibros.Columns[e.ColumnIndex].Name == "estado")
            {
                string estado = e.Value?.ToString();

                if (estado == "Inactivo")
                {
                    // Cambia el color de fondo de toda la fila
                    DGVLibros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    DGVLibros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
                else if (estado == "Activo")
                {
                    // Restaurar los colores si es necesario
                    DGVLibros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    DGVLibros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtTitulo.Text = "";
            txtAutor.Text = "";
            txtIBSN.Text = "";
            txtEditorial.Text = "";
            txtAño.Text = "";
            cbGenero.Text = "";
            cbCategoria.Text = "";
            txtDescrip.Text = "";
            Imagen.Image = null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefrescarLibros();
        }
    }
}
