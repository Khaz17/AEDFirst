using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AEDFirst
{
    public partial class ModelDocumentManager : DbContext
    {
        public ModelDocumentManager()
            : base("name=ModelDocumentManager")
        {
        }

        public virtual DbSet<Categorie> Categorie { get; set; }
        public virtual DbSet<CategorieDossier> CategorieDossier { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Dossier> Dossier { get; set; }
        public virtual DbSet<LogDoc> LogDoc { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<SousCategorie> SousCategorie { get; set; }
        public virtual DbSet<Suppression> Suppression { get; set; }
        public virtual DbSet<TypeUtilisateur> TypeUtilisateur { get; set; }
        public virtual DbSet<Utilisateur> Utilisateur { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>()
                .Property(e => e.NomCat)
                .IsUnicode(false);

            modelBuilder.Entity<CategorieDossier>()
                .Property(e => e.NomCatD)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Titre)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Format)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Tags)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.NomAuteur)
                .IsUnicode(false);

            modelBuilder.Entity<Dossier>()
                .Property(e => e.NomDoss)
                .IsUnicode(false);

            modelBuilder.Entity<LogDoc>()
                .Property(e => e.LogType)
                .IsUnicode(false);

            modelBuilder.Entity<LogDoc>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.LibellePrm)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Utilisateur)
                .WithMany(e => e.Permission)
                .Map(m => m.ToTable("PermsToUsers").MapLeftKey("IdPrm").MapRightKey("IdUser"));

            modelBuilder.Entity<SousCategorie>()
                .Property(e => e.NomSC)
                .IsUnicode(false);

            modelBuilder.Entity<TypeUtilisateur>()
                .Property(e => e.NomTU)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.NomUser)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.PrenomUser)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<Utilisateur>()
                .HasMany(e => e.Document)
                .WithRequired(e => e.Utilisateur)
                .HasForeignKey(e => e.IdUploader);

            modelBuilder.Entity<Utilisateur>()
                .HasMany(e => e.Utilisateur1)
                .WithOptional(e => e.Utilisateur2)
                .HasForeignKey(e => e.IdCreator);
        }
    }
}
