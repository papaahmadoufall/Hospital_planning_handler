﻿using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;
using WindowsFormsApp1.Model;

using System.Windows.Forms;
using WindowsFormsApp1.View;
namespace WindowsFormsApp1
{
    public partial class frmMDI : Form
    {
        public frmMDI()
        {
            InitializeComponent();
            this.IsMdiContainer = true; // Ajoute cette ligne
        }

        private void frmMDI_Load(object sender, EventArgs e)
        {
            Computer myComputer = new Computer();
            this.Width = myComputer.Screen.Bounds.Width;
            this.Height = myComputer.Screen.Bounds.Height;
            this.Location = new Point(0, 0);
        }

        // Fonction générique pour ouvrir un formulaire MDI
        private void OpenMdiChildForm(Form form)
        {
            // Vérifier si l'instance du formulaire est déjà ouverte
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == form.GetType()) // Vérifie si le formulaire est déjà ouvert
                {
                    frm.BringToFront(); // Si le formulaire est déjà ouvert, on le met au premier plan
                    return;
                }
            }

            // Crée une nouvelle instance du formulaire et définit-le comme enfant MDI
            form.MdiParent = this; // frmMDI est le parent
            form.WindowState = FormWindowState.Maximized; // Ouvre le formulaire en mode maximisé
            form.Show(); // Affiche le formulaire
        }


        private void fermer(Type formType)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm.GetType() == formType)
                {
                    childForm.Close();
                    return; // On ferme uniquement le premier trouvé et on sort
                }
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {

        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {

        }

        private void btn_view_Click(object sender, EventArgs e)
        {

        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void docToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void patientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChildForm(new frmPatients()); // Ouvre frmPatients

        }

        private void toolStripButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void rendezToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMdiChildForm(new frmRendezVous()); // Ouvre frmPatients

        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void docteursToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click_2(object sender, EventArgs e)
        {

        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
