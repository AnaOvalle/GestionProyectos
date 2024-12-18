﻿using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Packaging;

namespace ProyectoBiblioteca
{
    public partial class FrmAltaUsuarios : Form
    {
        public FrmAltaUsuarios()
        {
            InitializeComponent();
            CargarUsuariosId();
            mostrar();
        }
        

        MySqlConnection conexion = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;");

        private void FrmAltaUsuarios_Load(object sender, EventArgs e)

        {
            CargarUsuariosId(); // Cargar los IDs en el ComboBox
             // Asociar el evento
        } // Llama al método para cargar los IDs en el ComboBox al iniciar el formulario


        public void CargarUsuariosId()
        {
            try
            {
                conexion.Open();
                string query = "SELECT usuarios_id, nombre FROM usuarios;";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adaptador.Fill(dataTable);
                cbIDUsuario.DataSource = null;

                cbIDUsuario.DataSource = dataTable;
                cbIDUsuario.DisplayMember = "nombre";
                cbIDUsuario.ValueMember = "usuarios_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los IDs de usuarios: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            btnCerrarSesion.Visible = !btnCerrarSesion.Visible;
        }

        private void btmagregar_Click(object sender, EventArgs e)
        {
            string usuario = txtusuario.Text;
            string contrasena = txtContra.Text;
            int acceso = cmbTusuario.SelectedIndex + 1;

            int usuariosId = (int)cbIDUsuario.SelectedValue;

            if (usuariosId > 0)
            {
                MySqlCommand insertarLogin = new MySqlCommand("INSERT INTO login (usuarios_id, acceso, usuario, contra) VALUES (@usuarios_id, @acceso, @usuario, @contra)", conexion);

                insertarLogin.Parameters.AddWithValue("@usuarios_id", usuariosId);
                insertarLogin.Parameters.AddWithValue("@acceso", acceso);
                insertarLogin.Parameters.AddWithValue("@usuario", usuario);
                insertarLogin.Parameters.AddWithValue("@contra", contrasena);

                try
                {
                    conexion.Open();
                    insertarLogin.ExecuteNonQuery();
                    MessageBox.Show("Login creado correctamente.");
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
                MessageBox.Show("No se encontró un usuario para asociar con el login.");
            }
        }

        private int ObtenerUltimoUsuarioId()
        {
            int usuariosId = -1;
            string query = "SELECT MAX(usuarios_id) FROM usuarios";

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    usuariosId = Convert.ToInt32(command.ExecuteScalar() ?? -1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar el último usuario: {ex.Message}");
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return usuariosId;
        }

        private void btmlimpiar_Click(object sender, EventArgs e)
        {
            txtusuario.Clear();
            txtContra.Clear();
            // Restablecer la selección de ComboBox
            cbIDUsuario.SelectedIndex = -1;  // Deseleccionar cualquier opción en cmbUsuariosId
            cmbTusuario.SelectedIndex = -1;
        }

        private void btmeliminar_Click(object sender, EventArgs e)
        {

            int usuariosId = (int)cbIDUsuario.SelectedValue;

            if (usuariosId > 0)
            {
                MySqlCommand insertarLogin = new MySqlCommand(" DELETE FROM login WHERE usuarios_id = @usuarios_id", conexion);

                insertarLogin.Parameters.AddWithValue("@usuarios_id", usuariosId);
                
                try
                {
                    conexion.Open();
                    insertarLogin.ExecuteNonQuery();
                    MessageBox.Show("Loin eliminado");
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
                MessageBox.Show("No se encontró un usuario para asociar con el login.");
            }
        }

            private void btmmodificar_Click(object sender, EventArgs e)
        {
            if (cbIDUsuario.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un ID de usuario para modificar.");
                return;
            }
         
            int usuariosId = (int)cbIDUsuario.SelectedValue;
            string usuario = txtusuario.Text;
            string contrasena = txtContra.Text;
            int acceso = cmbTusuario.SelectedIndex + 1;

            MySqlCommand modificarLogin = new MySqlCommand("UPDATE login SET usuario = @usuario, contra = @contra, acceso = @acceso WHERE usuarios_id = @usuarios_id", conexion);
            modificarLogin.Parameters.AddWithValue("@usuarios_id", usuariosId);
            modificarLogin.Parameters.AddWithValue("@usuario", usuario);
            modificarLogin.Parameters.AddWithValue("@contra", contrasena);
            modificarLogin.Parameters.AddWithValue("@acceso", acceso);

            try
            {
                conexion.Open();
                int filasAfectadas = modificarLogin.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Login modificado correctamente.");
                }
                else
                {
                    MessageBox.Show("No se encontró un login con el ID especificado.");
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
        private void mostrar()
        {
            try
            {
                conexion.Open();
                string query = "SELECT * FROM login";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.BackgroundColor = Color.White;
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
            
            mostrar();
        }

        private void cbIDUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIDUsuario.SelectedItem != null)
            {
                int usuariosId = (int)cbIDUsuario.SelectedItem;

                // Consulta para obtener el nombre y la contraseña del usuario seleccionado
                string query = "SELECT usuario, contra FROM login WHERE usuarios_id = @usuarios_id";

                try
                {
                    conexion.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuarios_id", usuariosId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtusuario.Text = reader["usuario"].ToString();
                                txtContra.Text = reader["contra"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron datos para el usuario seleccionado.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos del usuario: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtusuario.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtContra.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            string acceso = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            if(acceso == "1")
            {
                cmbTusuario.Text = "Administrador.";
            }
            else if(acceso == "2") { cmbTusuario.Text = "Trabajador."; }

            string user = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            cbIDUsuario.SelectedValue = int.Parse(user);


        }
    }
}
