namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SOUSCATEGORIES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOUSCATEGORIES()
        {
            DOCUMENTS = new HashSet<DOCUMENTS>();
        }

        [Key]
        public int IdSC { get; set; }

        [Required]
        [StringLength(255)]
        public string NomSC { get; set; }

        public int IdCat { get; set; }

        public virtual CATEGORIES CATEGORIES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTS> DOCUMENTS { get; set; }
    }
}
