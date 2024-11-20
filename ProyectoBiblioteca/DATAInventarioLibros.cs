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
    public partial class DATAInventarioLibros : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");

        public DATAInventarioLibros()
        {
            InitializeComponent();
            Mostrar();
        }
        public void Mostrar()
        {

            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT li.*, stock.cantidad_stock_libros, stock.fecha_entrada  FROM libros li " +
                                 "JOIN stock_libros stock ";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                DataInveLibros.DataSource = dataTable;
                conexion.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private void DataInveLibros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
