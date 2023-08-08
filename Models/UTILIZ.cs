namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UTILIZ")]
    public partial class UTILIZ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UTILIZ()
        {
            DOCUMENTS = new HashSet<DOCUMENTS>();
            LOGDOCS = new HashSet<LOGDOCS>();
            UTILIZ1 = new HashSet<UTILIZ>();
            UTILIZDROITS = new HashSet<UTILIZDROITS>();
        }

        [Key]
        public int IdUtiliz { get; set; }

        [Required]
        [StringLength(255)]
        public string Login { get; set; }

        [Required]
        [StringLength(255)]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        [Required]
        [StringLength(255)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [Display(Name = "Créé le")]
        [Column(TypeName = "date")]
        public DateTime? Created_at { get; set; }

        [Display(Name = "État")]
        public bool? Active { get; set; }

        //[Display(Name = "Compte administrateur")]
        //public bool? Admin { get; set; }

        [Display(Name = "Créateur")]
        public int? IdCreator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTS> DOCUMENTS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOGDOCS> LOGDOCS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UTILIZ> UTILIZ1 { get; set; }

        public virtual UTILIZ UTILIZ2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UTILIZDROITS> UTILIZDROITS { get; set; }
    }
}
