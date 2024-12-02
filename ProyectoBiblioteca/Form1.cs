using Bunifu.UI.WinForms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private async Task LoginProcessAsync()
        {
            // Ocultar el panel cuando comienza el progreso
            bunifuPanel1.Visible = false;

            // Mostrar la animación de carga
            bunifuCircleProgress1.Visible = true;
            bunifuCircleProgress1.Value = 0;
            bunifuCircleProgress1.Animated = true;

            // Desactivar el botón de login mientras se realiza la validación
            bunifuButton1.Enabled = false;

            // Animación de carga
            for (int i = 0; i <= 100; i++)
            {
                this.Invoke((MethodInvoker)delegate {
                    bunifuCircleProgress1.Value = i;
                });
                await Task.Delay(30);
            }

            // Obtener datos del formulario
            string username = bunifuTextBox1.Text;
            string password = bunifuTextBox2.Text;

            // Conexión a la base de datos MySQL
            using (MySqlConnection conn = new MySqlConnection("Server=BilliJo; Database=BibliotecaGestion5; Uid=DELL; Pwd=1423; Port = 3306;"))
            {
                try
                {
                    await conn.OpenAsync();
                    string query = "SELECT acceso FROM login WHERE usuario = @usuario AND contra = @contra";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@usuario", username);
                    cmd.Parameters.AddWithValue("@contra", password);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                    {
                        int acceso = Convert.ToInt32(result);
                        if (acceso == 1) // Admin
                        {
                            Menu principal = new Menu();
                            principal.Show();
                            principal.name(username);
                            this.Hide();
                        }
                        else if (acceso == 2) // Usuario
                        {
                            Menu principal = new Menu();
                            principal.trab();
                            principal.name(username);
                            principal.Show();
                            
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bunifuPanel1.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error de conexión: " + ex.Message);
                }
            }

            // Ocultar la animación de carga
            bunifuCircleProgress1.Visible = false;

            // Reactivar el botón de login
            bunifuButton1.Enabled = true;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Configuración inicial del formulario
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            bunifuPanel1.Visible = false;
            bunifuGradientPanel2.Visible = true;
            bunifuPictureBox2.Visible = true;
        }

        private async void bunifuButton1_Click_2(object sender, EventArgs e)
        {
            await LoginProcessAsync();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

