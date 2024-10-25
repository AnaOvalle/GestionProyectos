using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ProyectoBiblioteca
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            DesignCustomize();
            originalWidth = sedebar.Width;
            originalHeight = sedebar.Height;
        }

        private void DesignCustomize()
        {
            PanelCategorias.Visible = false;
            PanelGestion.Visible = false;
            PanelPrestamos.Visible = false;
                 
        }
        private void HideSubmenu()
        {
            if (PanelCategorias.Visible == true)
            {
                PanelCategorias.Visible = false;
            }
            if (PanelPrestamos.Visible == true)
            {
                PanelPrestamos.Visible = false;
            }
            if (PanelGestion.Visible == true)
            {
                PanelGestion.Visible = false;
            }
        }

        private void ShowSubmenu(BunifuPanel Submenu)
        {
            if (Submenu.Visible == false)
            {
                HideSubmenu();
                Submenu.Visible = true;
            }
            else
            {
                Submenu.Visible = false;
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void bunifuPanel6_Click(object sender, EventArgs e)
        {

        }

        private void panelmenu_Click(object sender, EventArgs e)
        {

        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            
        }

        private void btnFiccion_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnNoFiccion_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnInfantil_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnJuvenil_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnAcade_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnCientifico_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnFantasy_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void Arte_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }


        private void btnConsultarP_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnrealPrestamos_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btndevolucion_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }


        private void btngestionLibros_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnusuarios_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btnclientes_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }

        private void btninventario_Click(object sender, EventArgs e)
        {
            // codigo

            HideSubmenu();
        }


        private void btngestion_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelGestion);
        }

        private void btnPrestamos_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelPrestamos);
        }

     

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void PanelContenido_Click(object sender, EventArgs e)
        {

        }

        private void Menu_Load(object sender, EventArgs e)
        {       
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            PanelUsuario.Visible = !PanelUsuario.Visible;
      
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        bool siderbarExpand = true;
        private void sidebarTransistor_Tick(object sender, EventArgs e)
        {
            if (siderbarExpand)
            {
                sedebar.Width -= 25;
                if (sedebar.Width <= 60)
                {
                    siderbarExpand = false;
                    sidebarTransistor.Stop();
                }

            }
            else
            {
                sedebar.Width += 25;
                if (sedebar.Width >= 225)
                {
                    siderbarExpand = true;
                    sidebarTransistor.Stop();
                }

            }
        }
        private int originalWidth;
        private int originalHeight;
        private bool isResized = false;
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (isResized)
            {
                // Si el panel está en tamaño modificado, lo restauramos al tamaño original
                sedebar.Width = originalWidth;
                sedebar.Height = originalHeight;
                isResized = false; // Cambiamos el estado a no modificado
            }
            else
            {
                // Si el panel está en su tamaño original, lo redimensionamos
                sedebar.Width = 232;  // Nuevo ancho
                sedebar.Height = 200; // Nueva altura
                isResized = true; // Cambiamos el estado a modificado
            }
        }

        private void btnLibros_Click(object sender, EventArgs e)
        {
            sedebar.Width = 75;
            Libros book = new Libros();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario

        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            RegistroUsuarios Ru = new RegistroUsuarios();
            Ru.Show();
            this.Hide();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            
            FrmAltaUsuarios alta =new FrmAltaUsuarios();
            alta.Show();
            this.Hide();
  
        }

        private void BtnCategorias_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelCategorias);
        }

        private void btngestion_Click(object sender, EventArgs e)
        {
            ShowSubmenu(PanelGestion);
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            ShowSubmenu(PanelPrestamos);
        }

        private void btnReportes_Click_2(object sender, EventArgs e)
        {
            Reportes book = new Reportes();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario
        }

        private void btnusuarios_Click_1(object sender, EventArgs e)
        {
          
            RegistroUsuarios Ru = new RegistroUsuarios();
            Ru.TopLevel = false; // Establece que no es un formulario de nivel superior
            Ru.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            Ru.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(Ru);
            bunifuPictureBox1.Hide();
            Ru.Show(); // Muestra el formulario
        

        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            Inicio home = new Inicio();
            home.TopLevel = false; // Establece que no es un formulario de nivel superior
            home.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            home.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(home);
            bunifuPictureBox1.Hide();
            home.Show(); // Muestra el formulario
        }

        private void btninventario_Click_1(object sender, EventArgs e)
        {
            Inventario stock = new Inventario();
            stock.TopLevel = false; // Establece que no es un formulario de nivel superior
            stock.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            stock.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(stock);
            bunifuPictureBox1.Hide();
            stock.Show(); // Muestra el formulario
        }

        private void lblHora_Click(object sender, EventArgs e)
        {

        }

        private void Hora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnsagas_Click(object sender, EventArgs e)
        {
            RegistroColeccion book = new RegistroColeccion();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario
        }

        private void btngestionLibros_Click_1(object sender, EventArgs e)
        {
            RegistroLibros book = new RegistroLibros();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario
        }

        private void btnclientes_Click_1(object sender, EventArgs e)
        {
            Clientes cli = new Clientes();
            cli.TopLevel = false; // Establece que no es un formulario de nivel superior
            cli.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            cli.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(cli);
            bunifuPictureBox1.Hide();
            cli.Show(); // Muestra el formulario
        }

        private void btnRealizarP_Click(object sender, EventArgs e)
        {
            Prestamo pres = new Prestamo();
            pres.TopLevel = false; // Establece que no es un formulario de nivel superior
            pres.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            pres.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(pres);
            bunifuPictureBox1.Hide();
            pres.Show(); // Muestra el formulario
        }

        private void btnConsulPrestamos_Click(object sender, EventArgs e)
        {
            ConsultaPrestamos book = new ConsultaPrestamos();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario
        }

        private void btndevolucion_Click_1(object sender, EventArgs e)
        {
            Devolucion book = new Devolucion();
            book.TopLevel = false; // Establece que no es un formulario de nivel superior
            book.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
            book.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel

            PanelContenido.Controls.Clear(); // Limpia los controles anteriores en el panel
            PanelContenido.Controls.Add(book);
            bunifuPictureBox1.Hide();
            book.Show(); // Muestra el formulario
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
