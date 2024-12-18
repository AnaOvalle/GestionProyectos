﻿using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProyectoBiblioteca
{
    public partial class Devolucion : Form
    {
        public Devolucion()
        {
            InitializeComponent();
        }

        private void btnSagaDevol_Click(object sender, EventArgs e)
        {
            if (cbPrestamoSaga.SelectedValue == null || cbTipoDevolucion.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una saga y un cliente para devolver.");
                return;
            }

            int sagaId = Convert.ToInt32(cbPrestamoSaga.SelectedValue);
            int clienteId = Convert.ToInt32(cbTipoDevolucion.SelectedValue);
            int cantidadADevolver = (int)NumCantidadSaga.Value;

            if (cantidadADevolver <= 0)
            {
                MessageBox.Show("La cantidad a devolver debe ser mayor a cero.");
                return;
            }

            string connectionString = "Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Obtener el ID del préstamo asociado
                        MySqlCommand cmdObtenerPrestamoId = new MySqlCommand(
                            @"SELECT ps.prestamos_id, ps.cantidad_prestada 
                  FROM prestamos_sagass ps 
                  JOIN prestamos p ON ps.prestamos_id = p.prestamos_id 
                  WHERE p.cliente_id = @clienteId AND ps.librosSaga_id = @sagaId", conn, transaction);
                        cmdObtenerPrestamoId.Parameters.AddWithValue("@clienteId", clienteId);
                        cmdObtenerPrestamoId.Parameters.AddWithValue("@sagaId", sagaId);

                        using (MySqlDataReader reader = cmdObtenerPrestamoId.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("El cliente no tiene préstamos de esta saga.");
                                return;
                            }

                            int prestamoId = reader.GetInt32(0);
                            int cantidadPrestada = reader.GetInt32(1);

                            reader.Close();

                            if (cantidadADevolver > cantidadPrestada)
                            {
                                MessageBox.Show($"El cliente solo tiene {cantidadPrestada} saga(s) en préstamo.");
                                return;
                            }

                            // Actualizar stock
                            MySqlCommand cmdActualizarStock = new MySqlCommand(
                                @"UPDATE stock_libros_saga 
                      SET cantidad_stock = cantidad_stock + @cantidadADevolver 
                      WHERE librosSaga_id = @sagaId", conn, transaction);
                            cmdActualizarStock.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                            cmdActualizarStock.Parameters.AddWithValue("@sagaId", sagaId);
                            cmdActualizarStock.ExecuteNonQuery();

                            // Actualizar préstamo
                            if (cantidadADevolver < cantidadPrestada)
                            {
                                MySqlCommand cmdActualizarPrestamo = new MySqlCommand(
                                    @"UPDATE prestamos_sagass 
                          SET cantidad_prestada = cantidad_prestada - @cantidadADevolver 
                          WHERE prestamos_id = @prestamoId AND librosSaga_id = @sagaId", conn, transaction);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@sagaId", sagaId);
                                cmdActualizarPrestamo.ExecuteNonQuery();
                            }
                            else
                            {
                                MySqlCommand cmdEliminarPrestamo = new MySqlCommand(
                                    @"DELETE FROM prestamos_sagass 
                          WHERE prestamos_id = @prestamoId AND librosSaga_id = @sagaId", conn, transaction);
                                cmdEliminarPrestamo.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdEliminarPrestamo.Parameters.AddWithValue("@sagaId", sagaId);
                                cmdEliminarPrestamo.ExecuteNonQuery();
                            }

                            // Verificar si el préstamo específico ya no tiene más sagas y actualizar su estado
                            MySqlCommand cmdVerificarEstado = new MySqlCommand(
                                @"SELECT COUNT(*) 
                      FROM prestamos_sagass 
                      WHERE prestamos_id = @prestamoId", conn, transaction);
                            cmdVerificarEstado.Parameters.AddWithValue("@prestamoId", prestamoId);

                            int prestamosActivos = Convert.ToInt32(cmdVerificarEstado.ExecuteScalar() ?? 0);

                            if (prestamosActivos == 0)
                            {
                                MySqlCommand cmdActualizarEstado = new MySqlCommand(
                                    @"UPDATE prestamos 
                          SET estado = 'devuelto', fecha_devuelto = CURDATE() 
                          WHERE prestamos_id = @prestamoId", conn, transaction);
                                cmdActualizarEstado.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdActualizarEstado.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("La saga ha sido devuelta correctamente.");
                        CargarPrestamosCliente();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al devolver la saga: " + ex.Message);
                    }
                }
            }


        }



        private void btnDevolverLibro_Click(object sender, EventArgs e)
        {
            if (cbLibro.SelectedValue != null && cbTipoDevolucion.SelectedValue != null)
            {
                int libroId = Convert.ToInt32(cbLibro.SelectedValue);
                int clienteId = Convert.ToInt32(cbTipoDevolucion.SelectedValue);
                int cantidadADevolver = (int)NumCantidadLibros.Value;

                if (cantidadADevolver <= 0)
                {
                    MessageBox.Show("La cantidad a devolver debe ser mayor a cero.");
                    return;
                }

                string connectionString = "Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Obtener la cantidad prestada y el ID del préstamo
                        MySqlCommand cmdCantidadPrestada = new MySqlCommand(
                            @"SELECT pl.prestamos_id, pl.cantidad_prestada 
                  FROM prestamos_libros pl 
                  JOIN prestamos p ON pl.prestamos_id = p.prestamos_id 
                  WHERE p.cliente_id = @clienteId AND pl.libros_id = @libroId", conn, transaction);
                        cmdCantidadPrestada.Parameters.AddWithValue("@clienteId", clienteId);
                        cmdCantidadPrestada.Parameters.AddWithValue("@libroId", libroId);

                        using (MySqlDataReader reader = cmdCantidadPrestada.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("El cliente no tiene préstamos de este libro.");
                                return;
                            }

                            int prestamoId = reader.GetInt32(0);
                            int cantidadPrestada = reader.GetInt32(1);

                            if (cantidadADevolver > cantidadPrestada)
                            {
                                MessageBox.Show($"El cliente solo tiene {cantidadPrestada} libro(s) en préstamo.");
                                return;
                            }

                            reader.Close();

                            // Actualizar el stock
                            MySqlCommand cmdActualizarStock = new MySqlCommand(
                                "UPDATE stock_libros SET cantidad_stock_libros = cantidad_stock_libros + @cantidadADevolver WHERE libros_id = @libroId", conn, transaction);
                            cmdActualizarStock.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                            cmdActualizarStock.Parameters.AddWithValue("@libroId", libroId);
                            cmdActualizarStock.ExecuteNonQuery();

                            // Actualizar o eliminar préstamo
                            if (cantidadADevolver < cantidadPrestada)
                            {
                                MySqlCommand cmdActualizarPrestamo = new MySqlCommand(
                                    @"UPDATE prestamos_libros 
                          SET cantidad_prestada = cantidad_prestada - @cantidadADevolver 
                          WHERE prestamos_id = @prestamoId AND libros_id = @libroId", conn, transaction);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdActualizarPrestamo.Parameters.AddWithValue("@libroId", libroId);
                                cmdActualizarPrestamo.ExecuteNonQuery();
                            }
                            else
                            {
                                MySqlCommand cmdEliminarPrestamo = new MySqlCommand(
                                    "DELETE FROM prestamos_libros WHERE prestamos_id = @prestamoId AND libros_id = @libroId", conn, transaction);
                                cmdEliminarPrestamo.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdEliminarPrestamo.Parameters.AddWithValue("@libroId", libroId);
                                cmdEliminarPrestamo.ExecuteNonQuery();
                            }

                            // Verificar si no hay más préstamos activos para este préstamo
                            MySqlCommand cmdVerificarEstado = new MySqlCommand(
                                "SELECT COUNT(*) FROM prestamos_libros WHERE prestamos_id = @prestamoId", conn, transaction);
                            cmdVerificarEstado.Parameters.AddWithValue("@prestamoId", prestamoId);

                            int prestamosRestantes = Convert.ToInt32(cmdVerificarEstado.ExecuteScalar());
                            if (prestamosRestantes == 0)
                            {
                                MySqlCommand cmdActualizarEstado = new MySqlCommand(
                                    @"UPDATE prestamos 
                          SET estado = 'devuelto', fecha_devuelto = CURDATE() 
                          WHERE prestamos_id = @prestamoId", conn, transaction);
                                cmdActualizarEstado.Parameters.AddWithValue("@prestamoId", prestamoId);
                                cmdActualizarEstado.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("El libro ha sido devuelto correctamente.");
                            CargarPrestamosCliente();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al devolver el libro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un libro y un cliente para devolver.");
            }



        }


        private void ConfigurarListView()
        {
            // Configurar ListLibrosDevol
            ListLibrosDevol.View = View.Details;
            ListLibrosDevol.Columns.Clear();
            ListLibrosDevol.Columns.Add("Título", 200); // Columna para el título
            ListLibrosDevol.Columns.Add("Cantidad", 100); // Columna para la cantidad

            // Configurar ListSagadevol
            ListSagadevol.View = View.Details;
            ListSagadevol.Columns.Clear();
            ListSagadevol.Columns.Add("Saga", 200); // Columna para el nombre de la saga
            ListSagadevol.Columns.Add("Cantidad", 100); // Columna para la cantidad

        }

        private void CargarDatosCliente()
        {
            string connectionString = "Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Cargar clientes en el ComboBox
                MySqlCommand cmdClientes = new MySqlCommand("SELECT cliente_id, nombre FROM clientes", conn);
                DataTable dtClientes = new DataTable();
                new MySqlDataAdapter(cmdClientes).Fill(dtClientes);
                cbTipoDevolucion.DisplayMember = "nombre";
                cbTipoDevolucion.ValueMember = "cliente_id";
                cbTipoDevolucion.DataSource = dtClientes;

                // Llenar los ListView con los préstamos de libros y sagas cuando se seleccione un cliente
                cbTipoDevolucion.SelectedIndexChanged += (s, e) =>
                {
                    CargarPrestamosCliente();
                };
            }
        }

        private void CargarPrestamosCliente()
        {
            int clienteId = Convert.ToInt32(cbTipoDevolucion.SelectedValue);

            using (MySqlConnection conn = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;"))
            {
                conn.Open();

                // Agrupar libros por título y sumar la cantidad prestada
                MySqlCommand cmdLibros = new MySqlCommand(
                    "SELECT lb.titulo, SUM(pl.cantidad_prestada) AS total_prestada " +
                    "FROM prestamos_libros pl " +
                    "JOIN libros lb ON pl.libros_id = lb.libros_id " +
                    "JOIN prestamos p ON pl.prestamos_id = p.prestamos_id " +
                    "WHERE p.cliente_id = @clienteId " +
                    "GROUP BY lb.titulo", conn);
                cmdLibros.Parameters.AddWithValue("@clienteId", clienteId);

                MySqlDataReader readerLibros = cmdLibros.ExecuteReader();
                ListLibrosDevol.Items.Clear();
                while (readerLibros.Read())
                {
                    ListViewItem item = new ListViewItem(readerLibros["titulo"].ToString());
                    item.SubItems.Add(readerLibros["total_prestada"].ToString());
                    ListLibrosDevol.Items.Add(item);
                }
                readerLibros.Close();

                // Agrupar sagas por nombre y sumar la cantidad prestada
                MySqlCommand cmdSagas = new MySqlCommand(
                    "SELECT sa.titulo, SUM(ps.cantidad_prestada) AS total_prestada " +
                    "FROM prestamos_sagass ps " +
                    "JOIN sagas sa ON ps.librosSaga_id = sa.librosSaga_id " +
                    "JOIN prestamos p ON ps.prestamos_id = p.prestamos_id " +
                    "WHERE p.cliente_id = @clienteId " +
                    "GROUP BY sa.titulo", conn);
                cmdSagas.Parameters.AddWithValue("@clienteId", clienteId);

                MySqlDataReader readerSagas = cmdSagas.ExecuteReader();
                ListSagadevol.Items.Clear();
                while (readerSagas.Read())
                {
                    ListViewItem item = new ListViewItem(readerSagas["titulo"].ToString());
                    item.SubItems.Add(readerSagas["total_prestada"].ToString());
                    ListSagadevol.Items.Add(item);
                }
                readerSagas.Close();
            }
        }

        private void LlenarComboBoxLibros()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT libros_id, titulo FROM libros", conn);
                DataTable dtLibros = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dtLibros);

                cbLibro.DisplayMember = "titulo";
                cbLibro.ValueMember = "libros_id";
                cbLibro.DataSource = dtLibros;
            }
        }

        private void LlenarComboBoxSagas()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT librosSaga_id, titulo FROM sagas", conn);
                DataTable dtSagas = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dtSagas);

                cbPrestamoSaga.DisplayMember = "titulo";
                cbPrestamoSaga.ValueMember = "librosSaga_id";
                cbPrestamoSaga.DataSource = dtSagas;
            }
        }

        private void Devolucion_Load(object sender, EventArgs e)
        {
            ConfigurarListView();
            CargarDatosCliente();
            CargarPrestamosCliente();
            LlenarComboBoxLibros();
            LlenarComboBoxSagas();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CargarPrestamosCliente();
        }

        private void btnRefreshSagas_Click(object sender, EventArgs e)
        {
            CargarPrestamosCliente();
        }

        private void ListLibrosDevol_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
            
        }
    }
}
