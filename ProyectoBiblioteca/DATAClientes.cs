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
    public partial class DATAClientes : Form
    {
        public MySqlConnection conexion = new MySqlConnection("Server=localhost; Database=BibliotecaGestion; Uid=root; Pwd=SB1299; Port = 3306;");

        private void DATAClientes_Load(object sender, EventArgs e)
        {
            ConexionClientes();
        }

        public DATAClientes()
        {
            InitializeComponent();
        }

        public void ConexionClientes()
        {
            conexion.Open();

            string consulta = "SELECT * FROM clientes";

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            DataTable dataTable = new DataTable();

            adaptador.Fill(dataTable);

            DATAcliente.DataSource = dataTable;
            conexion.Close();
        }


    }
}
