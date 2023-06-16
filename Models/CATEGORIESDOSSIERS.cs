namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CATEGORIESDOSSIERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CATEGORIESDOSSIERS()
        {
            DOSSIERS = new HashSet<DOSSIERS>();
        }

        [Key]
        public int IdCatDos { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Nom")]
        public string NomCatDos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOSSIERS> DOSSIERS { get; set; }
    }
}
