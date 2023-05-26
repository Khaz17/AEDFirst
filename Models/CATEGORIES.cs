namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CATEGORIES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CATEGORIES()
        {
            SOUSCATEGORIES = new HashSet<SOUSCATEGORIES>();
        }

        [Key]
        public int IdCat { get; set; }

        [Required]
        [StringLength(255)]
        public string NomCat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOUSCATEGORIES> SOUSCATEGORIES { get; set; }
    }
}
