﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Data.Entity;
using MySql.Data.EntityFramework;
using System.ComponentModel.DataAnnotations;
using WindowsFormsApp1.Model;
namespace WindowsFormsApp1.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BdRvMedicalContext : DbContext
    {
        public BdRvMedicalContext() : base("bdRvMedicalContext") { }

        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Patient> Patients { get; set; }


        public DbSet<Utilisateur> Utilisateurs { get; set; }

        public DbSet<Medecin> Medecins { get; set; }

        public DbSet<Secretaire> Secretaires { get; set; }

        public DbSet<Agenda> Agendas { get; set; }

        public DbSet<RendezVous> AllRendezvous { get; set; }

        public DbSet<GroupeSanguin> GroupeSanguins { get; set; }

        public DbSet<Soin> Soins { get; set; }


    }
}
