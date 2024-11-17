using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class Libros : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");
        public int bandera = 0;
        public Libros()
        {
            InitializeComponent();
            MostrarLi();
        }
        public void MostrarLi()
        {
            conexion.Open();
            try
            {
               
                if (bandera == 0)
                {
                    
                    string consulta = "SELECT  li.libros_id, li.titulo, li.isbn, li.año_publicacion, li.editorial," +
                        " li.descripcion, li.autor, ge.nombre AS Genero, ca.nombre AS Categoria  FROM libros li JOIN generos ge " +
                        "ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categoria_id = ca.categorias_id;";


                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    bunifuDataGridView1.DataSource = dataTable;
                   
                }
                else if(bandera == 1)
                {
                    string consulta = "SELECT  li.librosSaga_id, li.titulo, li.isbn, li.año_publicacion, li.editorial," +
                        " li.descripcion, li.autor, ge.nombre AS Genero, ca.nombre AS Categoria  FROM librosSaga li JOIN generos ge " +
                        "ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categorias_id = ca.categorias_id;";


                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    bunifuDataGridView1.DataSource = dataTable;
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conexion.Close();
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtTitulo.Text = "";
            TxtEditorial.Text = "";
            Txtisbn.Text = "";
            TextAutor.Text = "";
            TextAnio.Text = "";
            TxtDesc.Text = "";
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            PanelCategorias.Visible = !PanelCategorias.Visible;
        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            conexion.Open();
            if (bandera == 0)
            {
                string criterio = Busueda.Text;
                if (!string.IsNullOrWhiteSpace(criterio))
                {
                    try
                    {

                        string consulta = "SELECT li.libros_id, li.titulo, li.isbn, li.año_publicacion, li.editorial, li.descripcion, li.autor, ge.nombre AS Genero," +
                            " ca.nombre AS Categoria  FROM libros li JOIN generos ge ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categoria_id = ca.categorias_id WHERE " +
                            "li.titulo LIKE @criterio OR li.autor LIKE @criterio OR li.isbn LIKE @criterio OR li.editorial LIKE @criterio";

                        MySqlCommand comando = new MySqlCommand(consulta, conexion);
                        comando.Parameters.AddWithValue("@criterio", "%" + criterio + "%");

                        MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                        DataTable dataTable = new DataTable();

                        adaptador.Fill(dataTable);

                        bunifuDataGridView1.DataSource = dataTable;



                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un criterio de búsqueda.");
                }
            }
            else if(bandera == 1)
            {
                string criterio = Busueda.Text;
                if (!string.IsNullOrWhiteSpace(criterio))
                {
                    try
                    {

                        string consulta = "SELECT li.librosSaga_id, li.titulo, li.isbn, li.año_publicacion, li.editorial, li.descripcion, li.autor, ge.nombre AS Genero," +
                            " ca.nombre AS Categoria  FROM librosSaga li JOIN generos ge ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categorias_id = ca.categorias_id WHERE " +
                            "li.titulo LIKE @criterio OR li.autor LIKE @criterio OR li.isbn LIKE @criterio OR li.editorial LIKE @criterio";

                        MySqlCommand comando = new MySqlCommand(consulta, conexion);
                        comando.Parameters.AddWithValue("@criterio", "%" + criterio + "%");

                        MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                        DataTable dataTable = new DataTable();

                        adaptador.Fill(dataTable);

                        bunifuDataGridView1.DataSource = dataTable;



                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un criterio de búsqueda.");
                }
            }
            conexion.Close();

        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTitulo.Text = bunifuDataGridView1.CurrentRow.Cells[1].Value.ToString();
            TxtEditorial.Text = bunifuDataGridView1.CurrentRow.Cells[4].Value.ToString(); ;
            Txtisbn.Text = bunifuDataGridView1.CurrentRow.Cells[2].Value.ToString(); ;
            TextAutor.Text = bunifuDataGridView1.CurrentRow.Cells[6].Value.ToString(); ;
            TextAnio.Text = bunifuDataGridView1.CurrentRow.Cells[3].Value.ToString(); ;
            TxtDesc.Text = bunifuDataGridView1.CurrentRow.Cells[5].Value.ToString(); ;

            int id = int.Parse(bunifuDataGridView1.CurrentRow.Cells[0].Value.ToString());
            conexion.Open();
            try
            {
                if (bandera == 1)
                {
                
                    string sql = "SELECT imagensag FROM librosSaga WHERE librosSaga_id='" + id + "'";
                    MySqlCommand comando = new MySqlCommand(sql, conexion);
                    MySqlDataReader reader = comando.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        MemoryStream ms = new MemoryStream((byte[])reader["imagensag"]);
                        Bitmap bm = new Bitmap(ms);
                        pbImagenLibro.Visible= true;
                        pbImagenLibro.Image = bm;
                    }
                    else
                    {
                        MessageBox.Show("No se cuenta con imagen");
                    }
                }
                else
                {
                    string sql = "SELECT imagen FROM libros WHERE libros_id='" + id + "'";
                    MySqlCommand comando = new MySqlCommand(sql, conexion);
                    MySqlDataReader reader = comando.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        MemoryStream ms = new MemoryStream((byte[])reader["imagensag"]);
                        Bitmap bm = new Bitmap(ms);
                        pbImagenLibro.Visible = true;
                        pbImagenLibro.Image = bm;
                    }
                    else
                    {
                        MessageBox.Show("No se cuenta con imagen");
                    }
                }


            }
            catch
            {
                MessageBox.Show("No se cuenta con imagene");
                pbImagenLibro.Visible = false;
            }
            conexion.Close();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MostrarLi();
        }

        private void pbImagenLibro_Click(object sender, EventArgs e)
        {

        }

        private void Libros_Load(object sender, EventArgs e)
        {

        }
        public void Busquedas(string categoria)
        {
            conexion.Open();
            if (bandera == 0)
            {
                try
                {

                    string consulta = "SELECT  li.libros_id, li.titulo, li.isbn, li.año_publicacion, li.editorial," +
                        " li.descripcion, li.autor, ge.nombre AS Genero, ca.nombre AS Categoria  FROM libros li JOIN generos ge " +
                        "ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categoria_id = ca.categorias_id WHERE ge.nombre = @categoria;";


                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@categoria", categoria);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    bunifuDataGridView1.DataSource = dataTable;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (bandera == 1)
            {
                try
                {

                    string consulta = "SELECT  li.librosSaga_id, li.titulo, li.isbn, li.año_publicacion, li.editorial," +
                        " li.descripcion, li.autor, ge.nombre AS Genero, ca.nombre AS Categoria  FROM librosSaga  li JOIN generos ge " +
                        "ON li. genero_id = ge.genero_id JOIN categorias ca ON  li.categorias_id = ca.categorias_id WHERE ge.nombre = @categoria;";


                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@categoria", categoria);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                    DataTable dataTable = new DataTable();

                    adaptador.Fill(dataTable);

                    bunifuDataGridView1.DataSource = dataTable;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            conexion.Close();
        }

        private void btnFiccion_Click(object sender, EventArgs e)
        {
            Busquedas("Ficción");
            PanelCategorias.Visible = false;
        }

        private void btnCientifico_Click(object sender, EventArgs e)
        {
            Busquedas("Cientifico");
            PanelCategorias.Visible = false;
        }

        private void btnAcade_Click(object sender, EventArgs e)
        {
            Busquedas("Académico");
            PanelCategorias.Visible = false;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            Busquedas("Filosofía");
            PanelCategorias.Visible = false;
        }

        private void btnNovela_Click(object sender, EventArgs e)
        {
            Busquedas("Novela");
            PanelCategorias.Visible = false;
        }

        private void btnFantasy_Click(object sender, EventArgs e)
        {
            Busquedas("Fantasía");
            PanelCategorias.Visible = false;
        }

        private void btnJuvenil_Click(object sender, EventArgs e)
        {
            Busquedas("Juvenil");
            PanelCategorias.Visible = false;
        }

        private void btnInfantil_Click(object sender, EventArgs e)
        {
            Busquedas("Infantil");
            PanelCategorias.Visible = false;
        }

        private void HistoriaArte_Click(object sender, EventArgs e)
        {
            Busquedas("Historia y Arte");
            PanelCategorias.Visible = false;
        }

        private void BtnRelegión_Click(object sender, EventArgs e)
        {
            Busquedas("Religión");
            PanelCategorias.Visible = false;
        }

        private void BtnCrecimiento_Click(object sender, EventArgs e)
        {
            Busquedas("Crecimiento personal");
            PanelCategorias.Visible = false;
        }

        private void BtnLibros_Click(object sender, EventArgs e)
        {
            bandera = 0;
            MostrarLi();
        }

        private void btnColeccion_Click(object sender, EventArgs e)
        {
            bandera = 1;
            MostrarLi();
        }
    }
}
