namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DOCUMENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENTS()
        {
            LOGDOCS = new HashSet<LOGDOCS>();
        }

        [Key]
        public int IdDoc { get; set; }

        [Required]
        [StringLength(255)]
        public string Titre { get; set; }

        [Required]
        [StringLength(255)]
        public string Format { get; set; }

        public int Taille { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateUpload { get; set; }

        [StringLength(255)]
        public string NomDocFile { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public DateTime DateModifRecente { get; set; }

        public int? IdUploader { get; set; }

        public int? IdDoss { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOGDOCS> LOGDOCS { get; set; }

        public virtual DOSSIERS DOSSIERS { get; set; }

        public virtual UTILIZ UTILIZ { get; set; }
    }
}
