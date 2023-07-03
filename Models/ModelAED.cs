using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AEDFirst.Models
{
    public partial class ModelAED : DbContext
    {
        public ModelAED()
            : base("name=ModelAED")
        {
        }

        public virtual DbSet<CATEGORIESDOSSIERS> CATEGORIESDOSSIERS { get; set; }
        public virtual DbSet<DOCUMENTS> DOCUMENTS { get; set; }
        public virtual DbSet<DOSSIERS> DOSSIERS { get; set; }
        public virtual DbSet<DROITS> DROITS { get; set; }
        public virtual DbSet<LOGDOCS> LOGDOCS { get; set; }
        public virtual DbSet<UTILIZ> UTILIZ { get; set; }
        public virtual DbSet<UTILIZDROITS> UTILIZDROITS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CATEGORIESDOSSIERS>()
                .Property(e => e.NomCatDos)
                .IsUnicode(false);

            modelBuilder.Entity<CATEGORIESDOSSIERS>()
                .HasMany(e => e.DOSSIERS)
                .WithRequired(e => e.CATEGORIESDOSSIERS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DOCUMENTS>()
                .Property(e => e.Titre)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENTS>()
                .Property(e => e.Format)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENTS>()
                .Property(e => e.NomDocFile)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENTS>()
                .Property(e => e.Tags)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENTS>()
                .HasMany(e => e.LOGDOCS)
                .WithRequired(e => e.DOCUMENTS)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<DOCUMENTS>()
            //    .Property(e => e.DateModifRecente)
            //    .H

            modelBuilder.Entity<DOSSIERS>()
                .Property(e => e.NomDoss)
                .IsUnicode(false);

            //Incorrect
            //modelBuilder.Entity<DOSSIERS>()
            //    .HasMany(e => e.DOSSIERSENF)
            //    .WithOptional(e => e.DOSSIERSENF);

            modelBuilder.Entity<DOSSIERS>()
                .HasMany(d => d.DOSSIERSENF)  // Collection navigation property
                .WithOptional()  // Optional relationship
                .HasForeignKey(d => d.IdParent)
                .WillCascadeOnDelete(false);  // Set cascade delete behavior if needed

            modelBuilder.Entity<DOSSIERS>()
                .HasOptional(d => d.Parent)  // Optional relationship
                .WithMany()
                .HasForeignKey(d => d.IdParent)
                .WillCascadeOnDelete(false);  // Set cascade delete behavior if needed

            modelBuilder.Entity<DOSSIERS>()
                .HasMany(e => e.DOCUMENTS)
                .WithRequired(e => e.DOSSIERS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DROITS>()
                .Property(e => e.LibelleDrt)
                .IsUnicode(false);

            modelBuilder.Entity<DROITS>()
                .Property(e => e.CodeDrt)
                .IsUnicode(false);

            modelBuilder.Entity<DROITS>()
                .HasMany(e => e.UTILIZDROITS)
                .WithRequired(e => e.DROITS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LOGDOCS>()
                .Property(e => e.LogType)
                .IsUnicode(false);

            modelBuilder.Entity<LOGDOCS>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .Property(e => e.Nom)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .Property(e => e.Prenom)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UTILIZ>()
                .HasMany(e => e.DOCUMENTS)
                .WithRequired(e => e.UTILIZ)
                .HasForeignKey(e => e.IdUploader)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UTILIZ>()
                .HasMany(e => e.LOGDOCS)
                .WithRequired(e => e.UTILIZ)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UTILIZ>()
                .HasMany(e => e.UTILIZ1)
                .WithOptional(e => e.UTILIZ2)
                .HasForeignKey(e => e.IdCreator);

            modelBuilder.Entity<UTILIZ>()
                .HasMany(e => e.UTILIZDROITS)
                .WithRequired(e => e.UTILIZ)
                .WillCascadeOnDelete(false);

        }
    }
}
