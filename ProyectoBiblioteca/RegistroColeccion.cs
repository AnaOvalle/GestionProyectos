using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion; Uid=DELL; Pwd=1423; Port = 3306;");

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

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = ("insert into librosSaga(titulo,isbn,año_publicacion,editorial,descripcion,genero_id,categorias_id) " +
                    "values('"+ ti+"','"+isbn+"',"+anio+",'"+edi+"','"+des+"',"+gen+","+cat+");");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Libro de coleccion agregada");
                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            conexion.Close();
        }
        public void MostrarLi()
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.titulo, li.isbn, li.año_publicacion,li.editorial, li.descripcion, gen. nombre, ca.nombre  FROM librosSaga li " +
                                  "JOIN generos gen  JOIN categorias ca";

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
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.titulo, li.isbn, li.año_publicacion,li.editorial, li.descripcion, gen. nombre, ca.nombre  FROM librosSaga li " +
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

                MessageBox.Show("Libro de coleccion actualizado");

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            conexion.Close();

        }
    }

}
