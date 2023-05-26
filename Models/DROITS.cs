namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DROITS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DROITS()
        {
            UTILIZDROITS = new HashSet<UTILIZDROITS>();
        }

        [Key]
        public int IdDrt { get; set; }

        [Required]
        [StringLength(255)]
        public string LibelleDrt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UTILIZDROITS> UTILIZDROITS { get; set; }
    }
}
