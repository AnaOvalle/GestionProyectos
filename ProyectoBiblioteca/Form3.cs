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
    public partial class FrmAltaUsuarios : Form
    {
        public FrmAltaUsuarios()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Menu menu = new Menu();
            menu.Show();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Close();
            Menu menu = new Menu();
            menu.Show();
        }



        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bottonusuario_Click(object sender, EventArgs e)
        {
            panelmenu.Visible = ! panelmenu.Visible;
        }
    }
}
