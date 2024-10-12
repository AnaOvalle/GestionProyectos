using Bunifu.UI.WinForms;
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
            bunifuCircleProgress1.Visible = true;  // Asegúrate de que tienes este control en tu formulario
            bunifuCircleProgress1.Value = 0;       // Reiniciar el progreso
            bunifuCircleProgress1.Animated = true; // Activar la animación

            // Desactivar el botón de login mientras se realiza la validación
            bunifuButton1.Enabled = false;

            // **Corrección:** Actualizar UI desde el hilo principal usando Invoke
            for (int i = 0; i <= 100; i++)
            {
                this.Invoke((MethodInvoker)delegate {
                    bunifuCircleProgress1.Value = i; // Actualizar el valor del progreso en el hilo UI
                });

                await Task.Delay(30); // Controlar la velocidad de la animación
            }

            // Validar usuario y contraseña (ejemplo simple)
            string username = bunifuTextBox1.Text;  // Campo de texto para 
            string password = bunifuTextBox2.Text;  // Campo de texto para contraseña

            if (username == "admin" && password == "12345")
            {
                // Login exitoso
                Menu principal = new Menu();
                principal.Show();
                this.Hide();
            }
            else
            {
                // Mostrar mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Mostrar el panel de nuevo si los datos son incorrectos
                bunifuPanel1.Visible = true;
            }

            // Ocultar la animación de carga
            bunifuCircleProgress1.Visible = false;

            // Reactivar el botón de login
            bunifuButton1.Enabled = true;
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            bunifuPanel1.Visible =false;
            bunifuGradientPanel2.Visible = true;
            bunifuPictureBox2.Visible = true;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void bunifuButton1_Click_2(object sender, EventArgs e)
        {
            LoginProcessAsync();
        }
    }
}
