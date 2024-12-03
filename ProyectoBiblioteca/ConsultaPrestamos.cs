using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProyectoBiblioteca
{
    public partial class ConsultaPrestamos : Form
    {
        // Conexión a la base de datos MySQL
        public MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;");

        public ConsultaPrestamos()
        {
            InitializeComponent();
            CargarPrestamos(); // Cargar los préstamos al inicializar el formulario
        }

        private void CargarPrestamos()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open(); // Abrir la conexión solo si está cerrada

                string query = "SELECT p.prestamos_id, c.nombre AS cliente, p.fecha_prestamo, p.fecha_devolucion, p.estado " +
                               "FROM prestamos p " +
                               "JOIN clientes c ON p.cliente_id = c.cliente_id " +
                               "ORDER BY p.prestamos_id DESC;";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                DataTable prestamosTable = new DataTable();
                adapter.Fill(prestamosTable);

                DGVColecciones.DataSource = prestamosTable; // Asignar el DataTable al DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los préstamos: " + ex.Message);
            }
            finally
            {
                conexion.Close(); // Cerrar la conexión
            }
        }

        private void DGVColecciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Asegúrate de que se haga clic en una fila válida
            {
                DataGridViewRow row = this.DGVColecciones.Rows[e.RowIndex];
                // Aquí puedes manejar lo que ocurre cuando se hace clic en una celda
                MessageBox.Show($"Seleccionaste el préstamo de {row.Cells["cliente"].Value}.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CargarPrestamos();
        }

        private void ConsultaPrestamos_Load(object sender, EventArgs e)
        {
            CargarClientes(); // Cargar los clientes en el ComboBox
            DGVColecciones.DefaultCellStyle.Font = new Font("Arial", 12);

            // Asociar el evento SelectedIndexChanged
            cbCliente.SelectedIndexChanged += cbCliente_SelectedIndexChanged;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cbCliente.Text = "";
            
            txtBuscar.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBuscar.Text;

            if (!string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                // Consulta para buscar préstamos por el nombre del cliente o el estado del préstamo
                string query = "SELECT p.prestamos_id, c.nombre AS cliente, p.fecha_prestamo, p.fecha_devolucion, p.estado " +
                               "FROM prestamos p " +
                               "JOIN clientes c ON p.cliente_id = c.cliente_id " +
                               "WHERE c.nombre LIKE @criterio OR p.estado LIKE @criterio";

                try
                {
                    if (conexion.State == ConnectionState.Closed)
                        conexion.Open();

                    using (MySqlCommand buscar = new MySqlCommand(query, conexion))
                    {
                        buscar.Parameters.AddWithValue("@criterio", "%" + criterioBusqueda + "%");

                        using (MySqlDataReader reader = buscar.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            DGVColecciones.DataSource = dataTable; // Mostrar los resultados en el DataGridView
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar los préstamos: " + ex.Message);
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

    

        private void CargarClientes()
        {
            try
            {
                conexion.Open();
                string query = "SELECT cliente_id, nombre FROM clientes";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable clientesTable = new DataTable();
                adapter.Fill(clientesTable);

                // Agregar una fila en blanco al inicio
                DataRow row = clientesTable.NewRow();
                row["cliente_id"] = DBNull.Value;
                row["nombre"] = "";
                clientesTable.Rows.InsertAt(row, 0);

                cbCliente.DisplayMember = "nombre";
                cbCliente.ValueMember = "cliente_id";
                cbCliente.DataSource = clientesTable;
                cbCliente.SelectedIndex = 0; // Seleccionar el elemento vacío
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void cbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mostrar todos los registros si no se selecciona ningún cliente
            if (cbCliente.SelectedValue == DBNull.Value || cbCliente.SelectedValue == null)
            {
                CargarPrestamos();
            }
            else
            {
                int clienteId = Convert.ToInt32(cbCliente.SelectedValue);

                string query = "SELECT p.prestamos_id, c.nombre AS cliente, p.fecha_prestamo, p.fecha_devolucion, p.estado " +
                               "FROM prestamos p " +
                               "JOIN clientes c ON p.cliente_id = c.cliente_id " +
                               "WHERE c.cliente_id = @clienteId";

                try
                {
                    conexion.Open();
                    using (MySqlCommand buscarPorCliente = new MySqlCommand(query, conexion))
                    {
                        buscarPorCliente.Parameters.AddWithValue("@clienteId", clienteId);

                        using (MySqlDataReader reader = buscarPorCliente.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            DGVColecciones.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar los préstamos por cliente: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

       
    }
}

