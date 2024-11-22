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
    public partial class Prestamo : Form
    {
        public Prestamo()
        {
            InitializeComponent();
        }


        // ------------------- Préstamo de Libros ------------------- //
        private void btnAgregarLibro_Click(object sender, EventArgs e)
        {
            if (cbLibro.SelectedItem != null)
            {
                int libroId = Convert.ToInt32(cbLibro.SelectedValue);
                string titulo = cbLibro.Text;

                ListViewItem item = new ListViewItem(titulo);
                item.SubItems.Add(libroId.ToString());
                ListLibros.Items.Add(item);

                cbLibro.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un libro.");
            }
        }

        private void btnGuardarLibro_Click(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtFechaLibro.Text, out DateTime fechaPrestamo))
            {
                int clienteId = Convert.ToInt32(cbCliente.SelectedValue);
                string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        MySqlCommand cmdPrestamo = new MySqlCommand("INSERT INTO prestamos (cliente_id, fecha_prestamo) VALUES (@clienteId, @fechaPrestamo); SELECT LAST_INSERT_ID();", conn);
                        cmdPrestamo.Parameters.AddWithValue("@clienteId", clienteId);
                        cmdPrestamo.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                        cmdPrestamo.Transaction = transaction;

                        int prestamosId = Convert.ToInt32(cmdPrestamo.ExecuteScalar());

                        foreach (ListViewItem item in ListLibros.Items)
                        {
                            int libroId = Convert.ToInt32(item.SubItems[1].Text);
                            int cantidadPrestada = (int)NumCantidadLibros.Value;

                            MySqlCommand cmdVerificarStock = new MySqlCommand("SELECT cantidad_stock_libros FROM stock_libros WHERE libros_id = @librosId", conn);
                            cmdVerificarStock.Parameters.AddWithValue("@librosId", libroId);
                            cmdVerificarStock.Transaction = transaction;
                            int cantidadDisponible = Convert.ToInt32(cmdVerificarStock.ExecuteScalar());

                            if (cantidadDisponible >= cantidadPrestada)
                            {
                                MySqlCommand cmdPrestamosLibros = new MySqlCommand("INSERT INTO prestamos_libros (prestamos_id, libros_id, cantidad_prestada) VALUES (@prestamosId, @librosId, @cantidadPrestada)", conn);
                                cmdPrestamosLibros.Parameters.AddWithValue("@prestamosId", prestamosId);
                                cmdPrestamosLibros.Parameters.AddWithValue("@librosId", libroId);
                                cmdPrestamosLibros.Parameters.AddWithValue("@cantidadPrestada", cantidadPrestada);
                                cmdPrestamosLibros.Transaction = transaction;
                                cmdPrestamosLibros.ExecuteNonQuery();
                            }
                            else
                            {
                                MessageBox.Show($"No hay suficiente stock para el libro: {item.Text}. Disponibles: {cantidadDisponible}, solicitados: {cantidadPrestada}");
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Todos los préstamos de libros han sido registrados exitosamente.");
                        ListLibros.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al registrar el préstamo de libros: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Fecha de préstamo inválida.");
            }
        }

        // ------------------- Préstamo de Sagas ------------------- //
        private void btnAgregarSaga_Click(object sender, EventArgs e)
        {
            if (cbSaga.SelectedItem != null)
            {
                int sagaId = Convert.ToInt32(cbSaga.SelectedValue);
                string nombreSaga = cbSaga.Text;

                ListViewItem item = new ListViewItem(nombreSaga);
                item.SubItems.Add(sagaId.ToString());
                ListSagas.Items.Add(item);

                cbSaga.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una saga.");
            }
        }
        private void btnGuardarSaga_Click(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtFechaSaga.Text, out DateTime fechaPrestamo))
            {
                int clienteId = Convert.ToInt32(cbClienSaga.SelectedValue);
                string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        foreach (ListViewItem item in ListSagas.Items)
                        {
                            int sagaId = Convert.ToInt32(item.SubItems[1].Text);
                            int cantidadPrestada = (int)NumCantidadSagas.Value;

                            MySqlCommand cmdVerificarStockSaga = new MySqlCommand("SELECT cantidad_stock FROM stock_libros_saga WHERE librosSaga_id = @sagaId", conn);
                            cmdVerificarStockSaga.Parameters.AddWithValue("@sagaId", sagaId);
                            cmdVerificarStockSaga.Transaction = transaction;
                            int cantidadDisponible = Convert.ToInt32(cmdVerificarStockSaga.ExecuteScalar());

                            if (cantidadDisponible >= cantidadPrestada)
                            {
                                // Insertar directamente en la tabla `prestamos_saga` la cantidad prestada
                                MySqlCommand cmdRegistrarPrestamoSaga = new MySqlCommand("INSERT INTO prestamos_saga (cliente_id, sagas_id, fecha_prestamo, cantidad_prestamo) VALUES (@clienteId, @sagaId, @fechaPrestamo, @cantidadPrestada)", conn);
                                cmdRegistrarPrestamoSaga.Parameters.AddWithValue("@clienteId", clienteId);
                                cmdRegistrarPrestamoSaga.Parameters.AddWithValue("@sagaId", sagaId);
                                cmdRegistrarPrestamoSaga.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                                cmdRegistrarPrestamoSaga.Parameters.AddWithValue("@cantidadPrestada", cantidadPrestada);
                                cmdRegistrarPrestamoSaga.Transaction = transaction;
                                cmdRegistrarPrestamoSaga.ExecuteNonQuery();
                            }
                            else
                            {
                                MessageBox.Show($"No hay suficiente stock para la saga: {item.Text}. Disponibles: {cantidadDisponible}, solicitados: {cantidadPrestada}");
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Todos los préstamos de sagas han sido registrados exitosamente.");
                        ListSagas.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al registrar el préstamo de sagas: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Fecha de préstamo inválida.");
            }
        }






        // Métodos de carga de datos para los ComboBoxes y ListView
        private void CargarDatosComboBox()
        {
            string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Clientes para libros y sagas
                MySqlCommand cmdClientes = new MySqlCommand("SELECT cliente_id, nombre FROM clientes", conn);
                DataTable dtClientes = new DataTable();
                new MySqlDataAdapter(cmdClientes).Fill(dtClientes);
                cbCliente.DisplayMember = "nombre";
                cbCliente.ValueMember = "cliente_id";
                cbCliente.DataSource = dtClientes;

                cbClienSaga.DisplayMember = "nombre";
                cbClienSaga.ValueMember = "cliente_id";
                cbClienSaga.DataSource = dtClientes.Copy();

                // Libros
                MySqlCommand cmdLibros = new MySqlCommand("SELECT libros_id, titulo FROM libros", conn);
                DataTable dtLibros = new DataTable();
                new MySqlDataAdapter(cmdLibros).Fill(dtLibros);
                cbLibro.DisplayMember = "titulo";
                cbLibro.ValueMember = "libros_id";
                cbLibro.DataSource = dtLibros;

                // Sagas
                MySqlCommand cmdSagas = new MySqlCommand("SELECT MIN(sagas_id) AS sagas_id, nombre FROM sagas GROUP BY nombre", conn);
                DataTable dtSagas = new DataTable();
                new MySqlDataAdapter(cmdSagas).Fill(dtSagas);
                cbSaga.DisplayMember = "nombre";
                cbSaga.ValueMember = "sagas_id";
                cbSaga.DataSource = dtSagas;

            }
        }

        private void ConfigurarListView()
        {
            ListLibros.View = View.Details;
            ListLibros.Columns.Add("Título", 150);
            ListLibros.Columns.Add("ID Libro", 100);

            ListSagas.View = View.Details;
            ListSagas.Columns.Add("Saga", 150);
            ListSagas.Columns.Add("ID Saga", 100);
        }

        private void btnLimpiarSaga_Click(object sender, EventArgs e)
        {

        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        private void btnRefreshSagas_Click(object sender, EventArgs e)
        {
            Inventario inve = new Inventario();
            inve.Show();
        }

        private void Prestamo_Load(object sender, EventArgs e)
        {
            CargarDatosComboBox();
            ConfigurarListView();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiarSaga_Click_1(object sender, EventArgs e)
        {

        }
    }

}
