using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace ProyectoBiblioteca
{
    public partial class RegistroColeccion : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");

        public RegistroColeccion()
        {
            InitializeComponent();
            MostrarLi();
            agregarCat();
            agregaGe();

        }


        private void cbGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }
        public void agregaGe()
        {
            try
            {
                conexion.Open();


                string consulta = "SELECT genero_id, nombre FROM generos";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                cbGenero.DataSource = dataTable;
                cbGenero.DisplayMember = "nombre";
                cbGenero.ValueMember = "genero_id";
                conexion.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void agregarCat()
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT categorias_id, nombre FROM categorias";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                cbCategoria.DataSource = dataTable;
                cbCategoria.DisplayMember = "nombre";
                cbCategoria.ValueMember = "categorias_id";
                conexion.Close();
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }

        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
            conexion.Open();
            try
            {
                
                string ti = txtTitulo.Text;
                string isbn = txtIBSN.Text;
                string edi = txtEditorial.Text;
                int anio = int.Parse(txtAño.Text);
                string des = txtDescrip.Text;
                int cat = (int)cbCategoria.SelectedValue;
                int gen = (int)cbGenero.SelectedValue;
                string aut = txtAutor.Text;
                //convierte la imagen a byte
                MemoryStream ms = new MemoryStream();
                Imagen.Image.Save(ms, ImageFormat.Jpeg);
                byte[] data = ms.ToArray();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = ("insert into librosSaga(titulo,isbn,año_publicacion,editorial,descripcion,genero_id,categorias_id, autor, imagensag) " +
                    "values('"+ ti+"','"+isbn+"',"+anio+",'"+edi+"','"+des+"',"+gen+","+cat+",'"+aut+ "', @imagensag);");
                cmd.Parameters.AddWithValue("imagensag", data);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Libro de coleccion agregada");
                Imagen.Image = null;
                
            } 
            catch(MySqlException ex) { MessageBox.Show("Error al guar imagen " + ex.Message); }
            conexion.Close();
        }
        public void MostrarLi()
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.titulo, li.isbn, li.año_publicacion, li.editorial, li.descripcion, li.autor, gen.nombre AS genero, ca.nombre AS categoria " +
                                  "FROM librosSaga li " +
                                  "JOIN generos gen ON li.genero_id = gen.genero_id " +
                                  "JOIN categorias ca ON li.categorias_id = ca.categorias_id";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                DGVColecciones.DataSource = dataTable;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtTitulo.Text = "";
            txtIBSN.Text = "";
            txtEditorial.Text = "";
            txtAño.Text="";
            txtDescrip.Text= "";
            txtAutor.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.titulo, li.isbn, li.año_publicacion,li.editorial, li.descripcion,li.autor, gen.nombre, ca.nombre  FROM librosSaga li " +
                                  "JOIN generos gen  JOIN categorias ca where li.titulo = '"+ txtBuscar.Text+ "'";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                DGVColecciones.DataSource = dataTable;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MostrarLi();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            conexion.Open();
            try
            {

                string ti = txtTitulo.Text;
                string isbn = txtIBSN.Text;
                string edi = txtEditorial.Text;
                int anio = int.Parse(txtAño.Text);
                string des = txtDescrip.Text;
                int cat = (int)cbCategoria.SelectedValue;
                int gen = (int)cbGenero.SelectedValue;
                string aut = txtAutor.Text;

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = ("UPDATE librosSaga SET " +
                      "titulo = '" + ti + "', " +
                      "isbn = '" + isbn + "', " +
                      "año_publicacion = " + anio + ", " +
                      "editorial = '" + edi + "', " +
                      "descripcion = '" + des + "', " +
                      "genero_id = " + gen + ", " +
                      "categorias_id = " + cat + " " +
                      "autor = " + aut + " " +
                      "WHERE titulo = '"+ti+"';");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Libro de coleccion actualizado");

            }   
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            conexion.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            conexion.Open();
            try
            {

                string ti = txtTitulo.Text;
               

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = ("DELETE FROM librosSaga WHERE titulo = '"+ti+"';");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Libro de coleccion eliminado");

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            conexion.Close();



        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagenes|*.jpg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Title = "Seleccionar imagen";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Imagen.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void Imagen_Click(object sender, EventArgs e)
        {

        }

        private void txtDescrip_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DGVColecciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTitulo.Text = DGVColecciones.CurrentRow.Cells[1].Value.ToString();
            txtAutor.Text = DGVColecciones.CurrentRow.Cells[6].Value.ToString();
            txtIBSN.Text = DGVColecciones.CurrentRow.Cells[2].Value.ToString();
            txtEditorial.Text = DGVColecciones.CurrentRow.Cells[3].Value.ToString();
            txtAño.Text = DGVColecciones.CurrentRow.Cells[4].Value.ToString();
            cbGenero.Text = DGVColecciones.CurrentRow.Cells[7].Value.ToString();
            cbCategoria.Text = DGVColecciones.CurrentRow.Cells[8].Value.ToString();
            txtDescrip.Text = DGVColecciones.CurrentRow.Cells[5].Value.ToString();

            int id = int.Parse(DGVColecciones.CurrentRow.Cells[0].Value.ToString());
            conexion.Open();

            string sql = "SELECT imagensag FROM librosSaga WHERE librosSaga_id='" + id + "'";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            MySqlDataReader reader = comando.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    MemoryStream ms = new MemoryStream((byte[])reader["imagensag"]);
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
    }

}
