
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuTextbox;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class RegistroUsuarios : Form
    {
        public RegistroUsuarios()
        {
            InitializeComponent();
            mostraruse();
        }

        MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion3; Uid=DELL; Pwd=1423; Port = 3306;");


        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Menu menu = new Menu();
            menu.Show();
        }

        private void btnAltasCuenta_Click(object sender, EventArgs e)
        {
           
            FrmAltaUsuarios login = new FrmAltaUsuarios();
            login.Show();
        }

        private void Registrar_Click(object sender, EventArgs e)
        {
            MySqlCommand altas = new MySqlCommand("INSERT INTO usuarios (nombre, apellido, email, direccion, telefono, fecha_registro) VALUES (@nombre, @apellido, @email, @direccion, @telefono, @fecha_registro)", conexion);

            // Altas 
            altas.Parameters.AddWithValue("@nombre", NombreU.Text);
            altas.Parameters.AddWithValue("@apellido", ApellidoPU.Text);
            altas.Parameters.AddWithValue("@email", bunifuTextBox1.Text);
            altas.Parameters.AddWithValue("@direccion", bunifuTextBox4.Text);
            altas.Parameters.AddWithValue("@telefono", NumeroContactoU.Text);

            DateTime fechaRegistro;
            if (DateTime.TryParse(bunifuTextBox2.Text, out fechaRegistro))
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

            NombreU.Clear();
            ApellidoPU.Clear();
            bunifuTextBox1.Clear();
            bunifuTextBox4.Clear();
            NumeroContactoU.Clear();
            bunifuTextBox2.Clear();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            int idUsuario;

            if (int.TryParse(Id_User.Text, out idUsuario))
            {
                MySqlCommand eliminar = new MySqlCommand("DELETE FROM usuarios WHERE usuarios_id = @usuarios_id", conexion);
                eliminar.Parameters.AddWithValue("@usuarios_id", idUsuario);

                try
                {
                    conexion.Open();
                    int filasAfectadas = eliminar.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("El usuario fue eliminado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un usuario con ese ID.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error de base de datos: " + ex.Message);
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

        private void Actualizar_Click(object sender, EventArgs e)
        {
            int idUsuario;

            if (int.TryParse(Id_User.Text, out idUsuario))
            {
                MySqlCommand actualizar = new MySqlCommand(
                    "UPDATE usuarios SET nombre = @nombre, apellido = @apellido, email = @email, direccion = @direccion, telefono = @telefono, fecha_registro = @fecha_registro WHERE usuarios_id = @usuarios_id",
                    conexion
                );

                actualizar.Parameters.AddWithValue("@nombre", NombreU.Text);
                actualizar.Parameters.AddWithValue("@apellido", ApellidoPU.Text);
                actualizar.Parameters.AddWithValue("@email", bunifuTextBox1.Text);
                actualizar.Parameters.AddWithValue("@direccion", bunifuTextBox4.Text);
                actualizar.Parameters.AddWithValue("@telefono", NumeroContactoU.Text);

                DateTime fechaRegistro;
                if (DateTime.TryParse(bunifuTextBox2.Text, out fechaRegistro))
                {
                    actualizar.Parameters.AddWithValue("@fecha_registro", fechaRegistro.ToString("yyyy-MM-dd"));
                }
                else
                {
                    MessageBox.Show("Formato de fecha no válido.");
                    return;
                }

                actualizar.Parameters.AddWithValue("@usuarios_id", idUsuario);
                try
                {
                    conexion.Open();
                    int filasAfectadas = actualizar.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Los datos del usuario fueron actualizados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un usuario con ese ID.");
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
        private void mostraruse()
        {
            try
            {
                conexion.Open();
                string query = "SELECT * FROM usuarios";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        bunifuDataGridView1.DataSource = dataTable;
                        bunifuDataGridView1.BackgroundColor = Color.White;
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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
           mostraruse();
        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = bunifuTextBox3.Text;

            if (!string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                string query = "SELECT * FROM usuarios WHERE nombre LIKE @criterio OR apellido LIKE @criterio OR email LIKE @criterio";

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
                            bunifuDataGridView1.DataSource = dataTable;
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

        private void bunifuDataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = bunifuDataGridView1.Rows[e.RowIndex];
                Id_User.Text = row.Cells["usuarios_id"].Value.ToString();
                NombreU.Text = row.Cells["nombre"].Value.ToString();
                ApellidoPU.Text = row.Cells["apellido"].Value.ToString();
                bunifuTextBox1.Text = row.Cells["email"].Value.ToString();
                bunifuTextBox4.Text = row.Cells["direccion"].Value.ToString();
                NumeroContactoU.Text = row.Cells["telefono"].Value.ToString();
                bunifuTextBox2.Text = Convert.ToDateTime(row.Cells["fecha_registro"].Value).ToString("yyyy-MM-dd");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            NombreU.Clear();
            ApellidoPU.Clear();
            bunifuTextBox1.Clear();
            bunifuTextBox4.Clear();
            NumeroContactoU.Clear();
            bunifuTextBox2.Clear();

        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Id_User.Text = bunifuDataGridView1.CurrentRow.Cells[0].Value.ToString();
            NombreU.Text = bunifuDataGridView1.CurrentRow.Cells[1].Value.ToString();
            ApellidoPU.Text = bunifuDataGridView1.CurrentRow.Cells[2].Value.ToString();
            bunifuTextBox1.Text = bunifuDataGridView1.CurrentRow.Cells[3].Value.ToString();
            bunifuTextBox4.Text = bunifuDataGridView1.CurrentRow.Cells[4].Value.ToString();
            NumeroContactoU.Text = bunifuDataGridView1.CurrentRow.Cells[5].Value.ToString();
            bunifuTextBox2.Text = bunifuDataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            Id_User.Text = "";
            NombreU.Text = "";
            ApellidoPU.Text = "";
            bunifuTextBox1.Text = "";
            bunifuTextBox4.Text = "";
            NumeroContactoU.Text = "";
            bunifuTextBox2.Text = "";
        }
    }
}

