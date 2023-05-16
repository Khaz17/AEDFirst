namespace AEDFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Document")]
    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            LogDoc = new HashSet<LogDoc>();
            Suppression = new HashSet<Suppression>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDoc { get; set; }

        [Required]
        [StringLength(255)]
        public string Titre { get; set; }

        [Required]
        [StringLength(255)]
        public string Format { get; set; }

        public int Taille { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUpload { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string NomAuteur { get; set; }

        public int IdUploader { get; set; }

        public int IdSC { get; set; }

        public int IdDoss { get; set; }

        public virtual Dossier Dossier { get; set; }

        public virtual SousCategorie SousCategorie { get; set; }

        public virtual Utilisateur Utilisateur { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogDoc> LogDoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suppression> Suppression { get; set; }
    }
}
