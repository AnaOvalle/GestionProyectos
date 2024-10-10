﻿using System;
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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            DesignCustomize();
        }

        private void DesignCustomize()
        {
            PanelCategorias.Visible = false;
            PanelGestion.Visible = false;
            PanelPrestamos.Visible = false;
            PanelReportes.Visible = false;
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
            if (PanelReportes.Visible == true)
            {
                PanelReportes.Visible = false;
            }
        }

        private void ShowSubmenu(Panel Submenu)
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
            //Inicio home = new Inicio();
            //if (PanelContenido.Contains(home) == false)
            //{
            //    PanelContenido.Controls.Add(home);
            //    home.BringToFront();
            //}
            //else
            //{
            //    home.BringToFront();
            //}
        }

        private void bunifuPanel6_Click(object sender, EventArgs e)
        {

        }

        private void panelmenu_Click(object sender, EventArgs e)
        {

        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            ShowSubmenu(PanelCategorias);
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

        private void btnReportes_Click(object sender, EventArgs e)
        {
            ShowSubmenu(PanelReportes);
        }

        private void btngestion_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelGestion);
        }

        private void btnPrestamos_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelPrestamos);
        }

        private void btnReportes_Click_1(object sender, EventArgs e)
        {
            ShowSubmenu(PanelReportes);
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }

        private void minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void PanelContenido_Click(object sender, EventArgs e)
        {

        }

        private void Menu_Load(object sender, EventArgs e)
        {
              Inicio home = new Inicio();
              home.TopLevel = false; // Establece que no es un formulario de nivel superior
              home.FormBorderStyle = FormBorderStyle.None; // Quita el borde del formulario
              PanelContenido.Controls.Add(home);
              home.Dock = DockStyle.Fill; // Ajusta el tamaño del formulario al panel
              home.Show(); // Muestra el formulario


            //Inicio ini = new Inicio();
            //if (PanelContenido.Contains(ini) == false)
            //{
            //    PanelContenido.Controls.Add(ini);
            //    ini.BringToFront();
            //}
            //else
            //{
            //    ini.BringToFront();
            //}
        }
    }
}
