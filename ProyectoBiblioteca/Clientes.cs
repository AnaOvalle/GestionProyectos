using Bunifu.UI.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Asegúrate de tener este using

namespace ProyectoBiblioteca
{
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }

        MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=Biblio3;Uid=root;Pwd=hola123;");

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            MySqlCommand altas = new MySqlCommand("INSERT INTO clientes (nombre, apellidos, email, direccion, telefono, fecha_registro) VALUES (@nombre, @apellidos, @email, @direccion, @telefono, @fecha_registro)", conexion);

            altas.Parameters.AddWithValue("@nombre", txtNombreClnt.Text);
            altas.Parameters.AddWithValue("@apellidos", txtApellido.Text);
            altas.Parameters.AddWithValue("@email", txtEmail.Text);
            altas.Parameters.AddWithValue("@direccion", txtDireccion.Text);
            altas.Parameters.AddWithValue("@telefono", txtTel.Text);

            DateTime fechaRegistro;
            if (DateTime.TryParse(txtFecha.Text, out fechaRegistro))
            {
                altas.Parameters.AddWithValue("@fecha_registro", fechaRegistro.ToString("yyyy-MM-dd"));
            }
            else
            {
                MessageBox.Show("Formato de fecha no válido.");
                return;
            }

            try
            {
                conexion.Open();
                altas.ExecuteNonQuery();
                MessageBox.Show("Los datos se almacenaron correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }

            txtNombreClnt.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            txtTel.Clear();
            txtFecha.Clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int clienteId;

            if (int.TryParse(Id_User.Text, out clienteId))
            {
                MySqlCommand eliminar = new MySqlCommand("DELETE FROM clientes WHERE cliente_id = @cliente_id", conexion);
                eliminar.Parameters.AddWithValue("@cliente_id", clienteId);

                try
                {
                    conexion.Open();
                    int filasAfectadas = eliminar.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("El cliente fue eliminado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un cliente con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int clienteId;

            if (int.TryParse(Id_User.Text, out clienteId))
            {
                MySqlCommand actualizar = new MySqlCommand(
                    "UPDATE clientes SET nombre = @nombre, apellidos = @apellidos, email = @email, direccion = @direccion, telefono = @telefono, fecha_registro = @fecha_registro WHERE cliente_id = @cliente_id",
                    conexion
                );

                actualizar.Parameters.AddWithValue("@nombre", txtNombreClnt.Text);
                actualizar.Parameters.AddWithValue("@apellidos", txtApellido.Text);
                actualizar.Parameters.AddWithValue("@email", txtEmail.Text);
                actualizar.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                actualizar.Parameters.AddWithValue("@telefono", txtTel.Text);

                DateTime fechaRegistro;
                if (DateTime.TryParse(txtFecha.Text, out fechaRegistro))
                {
                    actualizar.Parameters.AddWithValue("@fecha_registro", fechaRegistro.ToString("yyyy-MM-dd"));
                }
                else
                {
                    MessageBox.Show("Formato de fecha no válido.");
                    return;
                }

                actualizar.Parameters.AddWithValue("@cliente_id", clienteId);
                try
                {
                    conexion.Open();
                    int filasAfectadas = actualizar.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Los datos del cliente fueron actualizados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un cliente con ese ID.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string query = "SELECT * FROM clientes";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        DGVClientes.DataSource = dataTable;
                        DGVClientes.BackgroundColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = txtBuscar.Text;

            if (!string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                string query = "SELECT * FROM clientes WHERE nombre LIKE @criterio OR apellidos LIKE @criterio OR email LIKE @criterio";

                try
                {
                    conexion.Open();
                    using (MySqlCommand buscar = new MySqlCommand(query, conexion))
                    {
                        buscar.Parameters.AddWithValue("@criterio", "%" + criterioBusqueda + "%");

                        using (MySqlDataReader reader = buscar.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            DGVClientes.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar los datos: " + ex.Message);
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

        private void DGVClientes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVClientes.Rows[e.RowIndex];

                Id_User.Text = row.Cells["cliente_id"].Value.ToString();
                txtNombreClnt.Text = row.Cells["nombre"].Value.ToString();
                txtApellido.Text = row.Cells["apellidos"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtDireccion.Text = row.Cells["direccion"].Value.ToString();
                txtTel.Text = row.Cells["telefono"].Value.ToString();
                txtFecha.Text = Convert.ToDateTime(row.Cells["fecha_registro"].Value).ToString("yyyy-MM-dd");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Id_User.Clear();
            txtNombreClnt.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            txtTel.Clear();
            txtFecha.Clear();

        }
    }
}
