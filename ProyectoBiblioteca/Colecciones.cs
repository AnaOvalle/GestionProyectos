using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class Colecciones : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");

        public Colecciones()
        {
            InitializeComponent();
            MostrarSa();
        }
        public void MostrarSa()
        {
            try 
            {
                string consulta = "SELECT sagas_id AS Id, nombre AS Nombre, descripcion AS Descripción, editorial AS  Editorial, anio_publicacion AS 'Año de publicación', autor AS Autor FROM sagas;";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                bunifuDataGridView1.DataSource = dataTable;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }
        public void DatosImagen(string titulo)
        {
            conexion.Open();
            try
            {
                string consulta = "SELECT nombre AS Nombre, descripcion AS Descripción, editorial AS  Editorial, anio_publicacion AS 'Año de publicación', autor AS Autor FROM sagas WHERE nombre='"+titulo+"'";

                MySqlCommand comando = new MySqlCommand(consulta, conexion);

                MySqlDataReader reader = comando.ExecuteReader();


                if (reader.Read()) // Si se encuentra el registro
                {
                    // Asigna los datos a los TextBox
                    txtTitulo.Text = reader["Nombre"].ToString();
                    bunifuTextBox5.Text = reader["Descripción"].ToString();
                    bunifuTextBox7.Text = reader["Editorial"].ToString();
                    bunifuTextBox4.Text = reader["Año de publicación"].ToString();
                    bunifuTextBox3.Text = reader["Autor"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontró información para el título.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            conexion.Close();
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTitulo.Text = bunifuDataGridView1.CurrentRow.Cells[1].Value.ToString();
            bunifuTextBox7.Text = bunifuDataGridView1.CurrentRow.Cells[3].Value.ToString();
            bunifuTextBox4.Text = bunifuDataGridView1.CurrentRow.Cells[4].Value.ToString();
            bunifuTextBox5.Text = bunifuDataGridView1.CurrentRow.Cells[2].Value.ToString();
            bunifuTextBox3.Text = bunifuDataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            DatosImagen("Harry Potter");
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            DatosImagen("Señor de los anillos");
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            DatosImagen("Cuidad Blanca");
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            DatosImagen("Dune");
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            DatosImagen("Percy Jackson");
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            DatosImagen("Divergente");
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            DatosImagen("After");
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
            DatosImagen("Maze Runner");
        }
    }
}
