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
            if (cbPrestamoSaga.SelectedValue != null)
            {
                int sagaId = Convert.ToInt32(cbPrestamoSaga.SelectedValue); // Obtener el ID de la saga seleccionada
                int cantidadASagaDevolver = (int)NumCantidadSaga.Value;

                if (cantidadASagaDevolver <= 0)
                {
                    MessageBox.Show("La cantidad a devolver debe ser mayor a cero.");
                    return;
                }

                string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction(); // Iniciar una transacción

                    try
                    {
                        // Obtener la cantidad prestada de la saga
                        MySqlCommand cmdCantidadPrestadaSaga = new MySqlCommand(
                            "SELECT SUM(ps.cantidad_prestamo) AS total_prestado " +
                            "FROM prestamos_saga ps " +
                            "JOIN prestamos p ON ps.prestamos_id = p.prestamos_id " +
                            "WHERE p.cliente_id = @clienteId AND ps.sagas_id = @sagaId", conn, transaction);
                        cmdCantidadPrestadaSaga.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                        cmdCantidadPrestadaSaga.Parameters.AddWithValue("@sagaId", sagaId);

                        object resultSaga = cmdCantidadPrestadaSaga.ExecuteScalar();
                        int cantidadPrestadaSaga = resultSaga != DBNull.Value ? Convert.ToInt32(resultSaga) : 0;

                        if (cantidadPrestadaSaga == 0)
                        {
                            MessageBox.Show("El cliente no tiene préstamos de esta saga.");
                            return;
                        }

                        if (cantidadASagaDevolver > cantidadPrestadaSaga)
                        {
                            MessageBox.Show($"El cliente solo tiene {cantidadPrestadaSaga} saga(s) en préstamo.");
                            return;
                        }

                        // Actualizar el stock de la saga
                        MySqlCommand cmdActualizarStockSaga = new MySqlCommand(
                            "UPDATE stock_sagas SET cantidad_stock_sagas = cantidad_stock_sagas + @cantidadASagaDevolver WHERE sagas_id = @sagaId", conn, transaction);
                        cmdActualizarStockSaga.Parameters.AddWithValue("@cantidadASagaDevolver", cantidadASagaDevolver);
                        cmdActualizarStockSaga.Parameters.AddWithValue("@sagaId", sagaId);
                        int rowsUpdatedSaga = cmdActualizarStockSaga.ExecuteNonQuery();

                        if (rowsUpdatedSaga == 0)
                        {
                            throw new Exception("No se pudo actualizar el stock de la saga. Verifique los datos.");
                        }

                        // Actualizar la cantidad prestada o eliminar el préstamo si se devuelve toda la saga
                        if (cantidadASagaDevolver < cantidadPrestadaSaga)
                        {
                            // Reducir la cantidad prestada
                            MySqlCommand cmdActualizarPrestamoSaga = new MySqlCommand(
                                "UPDATE prestamos_saga ps " +
                                "JOIN prestamos p ON ps.prestamos_id = p.prestamos_id " +
                                "SET ps.cantidad_prestamo = ps.cantidad_prestamo - @cantidadASagaDevolver " +
                                "WHERE p.cliente_id = @clienteId AND ps.sagas_id = @sagaId", conn, transaction);
                            cmdActualizarPrestamoSaga.Parameters.AddWithValue("@cantidadASagaDevolver", cantidadASagaDevolver);
                            cmdActualizarPrestamoSaga.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                            cmdActualizarPrestamoSaga.Parameters.AddWithValue("@sagaId", sagaId);
                            cmdActualizarPrestamoSaga.ExecuteNonQuery();
                        }
                        else
                        {
                            // Eliminar el préstamo si se devuelve toda la saga
                            MySqlCommand cmdEliminarPrestamoSaga = new MySqlCommand(
                                "DELETE ps " +
                                "FROM prestamos_saga ps " +
                                "JOIN prestamos p ON ps.prestamos_id = p.prestamos_id " +
                                "WHERE p.cliente_id = @clienteId AND ps.sagas_id = @sagaId", conn, transaction);
                            cmdEliminarPrestamoSaga.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                            cmdEliminarPrestamoSaga.Parameters.AddWithValue("@sagaId", sagaId);
                            cmdEliminarPrestamoSaga.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("La saga ha sido devuelta correctamente.");
                        CargarPrestamosCliente(); // Actualizar los datos en pantalla
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al devolver la saga: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una saga para devolver.");
            }
        }



        private void btnDevolverLibro_Click(object sender, EventArgs e)
        {
            if (cbLibro.SelectedValue != null)
            {
                int libroId = Convert.ToInt32(cbLibro.SelectedValue); // Obtener el ID del libro seleccionado
                int cantidadADevolver = (int)NumCantidadLibros.Value;

                if (cantidadADevolver <= 0)
                {
                    MessageBox.Show("La cantidad a devolver debe ser mayor a cero.");
                    return;
                }

                string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction(); // Iniciar una transacción

                    try
                    {
                        // Obtener la cantidad prestada del libro
                        MySqlCommand cmdCantidadPrestada = new MySqlCommand(
                            "SELECT SUM(pl.cantidad_prestada) AS total_prestada " +
                            "FROM prestamos_libros pl " +
                            "JOIN prestamos p ON pl.prestamos_id = p.prestamos_id " +
                            "WHERE p.cliente_id = @clienteId AND pl.libros_id = @libroId", conn, transaction);
                        cmdCantidadPrestada.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                        cmdCantidadPrestada.Parameters.AddWithValue("@libroId", libroId);

                        object result = cmdCantidadPrestada.ExecuteScalar();
                        int cantidadPrestada = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                        if (cantidadPrestada == 0)
                        {
                            MessageBox.Show("El cliente no tiene préstamos de este libro.");
                            return;
                        }

                        if (cantidadADevolver > cantidadPrestada)
                        {
                            MessageBox.Show($"El cliente solo tiene {cantidadPrestada} libro(s) en préstamo.");
                            return;
                        }

                        // Actualizar el stock
                        MySqlCommand cmdActualizarStock = new MySqlCommand(
                            "UPDATE stock_libros SET cantidad_stock_libros = cantidad_stock_libros + @cantidadADevolver WHERE libros_id = @libroId", conn, transaction);
                        cmdActualizarStock.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                        cmdActualizarStock.Parameters.AddWithValue("@libroId", libroId);
                        int rowsUpdated = cmdActualizarStock.ExecuteNonQuery();

                        if (rowsUpdated == 0)
                        {
                            throw new Exception("No se pudo actualizar el stock. Verifique los datos.");
                        }

                        // Actualizar la cantidad prestada o eliminar el préstamo si se devuelve todo
                        if (cantidadADevolver < cantidadPrestada)
                        {
                            // Reducir la cantidad prestada
                            MySqlCommand cmdActualizarPrestamo = new MySqlCommand(
                                "UPDATE prestamos_libros pl " +
                                "JOIN prestamos p ON pl.prestamos_id = p.prestamos_id " +
                                "SET pl.cantidad_prestada = pl.cantidad_prestada - @cantidadADevolver " +
                                "WHERE p.cliente_id = @clienteId AND pl.libros_id = @libroId", conn, transaction);
                            cmdActualizarPrestamo.Parameters.AddWithValue("@cantidadADevolver", cantidadADevolver);
                            cmdActualizarPrestamo.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                            cmdActualizarPrestamo.Parameters.AddWithValue("@libroId", libroId);
                            cmdActualizarPrestamo.ExecuteNonQuery();
                        }
                        else
                        {
                            // Eliminar el préstamo si se devuelve toda la cantidad
                            MySqlCommand cmdEliminarPrestamo = new MySqlCommand(
                                "DELETE pl " +
                                "FROM prestamos_libros pl " +
                                "JOIN prestamos p ON pl.prestamos_id = p.prestamos_id " +
                                "WHERE p.cliente_id = @clienteId AND pl.libros_id = @libroId", conn, transaction);
                            cmdEliminarPrestamo.Parameters.AddWithValue("@clienteId", cbTipoDevolucion.SelectedValue);
                            cmdEliminarPrestamo.Parameters.AddWithValue("@libroId", libroId);
                            cmdEliminarPrestamo.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("El libro ha sido devuelto correctamente.");
                        CargarPrestamosCliente(); // Actualizar los datos en pantalla
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
                MessageBox.Show("Seleccione un libro para devolver.");
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
            string connectionString = "Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;";
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

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;"))
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
                    "SELECT sa.nombre, SUM(ps.cantidad_prestamo) AS total_prestada " +
                    "FROM prestamos_saga ps " +
                    "JOIN sagas sa ON ps.sagas_id = sa.sagas_id " +
                    "WHERE ps.cliente_id = @clienteId " +
                    "GROUP BY sa.nombre", conn);
                cmdSagas.Parameters.AddWithValue("@clienteId", clienteId);

                MySqlDataReader readerSagas = cmdSagas.ExecuteReader();
                ListSagadevol.Items.Clear();
                while (readerSagas.Read())
                {
                    ListViewItem item = new ListViewItem(readerSagas["nombre"].ToString());
                    item.SubItems.Add(readerSagas["total_prestada"].ToString());
                    ListSagadevol.Items.Add(item);
                }
                readerSagas.Close();
            }
        }

        private void LlenarComboBoxLibros()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;"))
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
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=Biblio6;Uid=root;Pwd=hola123;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT sagas_id, nombre FROM sagas", conn);
                DataTable dtSagas = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dtSagas);

                cbPrestamoSaga.DisplayMember = "nombre";
                cbPrestamoSaga.ValueMember = "sagas_id";
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
            Inventario inve = new Inventario();
            inve.Show();
        }
    }
}