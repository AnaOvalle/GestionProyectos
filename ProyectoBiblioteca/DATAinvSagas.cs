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
    public partial class DATAinvSagas : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");
        public DATAinvSagas()
        {
            InitializeComponent();
            Mostrar();

        }
        public void Mostrar()
        {

            try
            {
                conexion.Open();
                string consulta = "SELECT DISTINCT sa.*, stock.cantidad_stock, stock.fecha_entrada  FROM sagas sa " +
                                 "JOIN stock_libros_saga stock ";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);

                DatainvSaga.DataSource = dataTable;
                conexion.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
