namespace AEDFirst.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DOSSIERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOSSIERS()
        {
            DOCUMENTS = new HashSet<DOCUMENTS>();
        }

        [Key]
        public int IdDoss { get; set; }

        [Required]
        [StringLength(255)]
        public string NomDoss { get; set; }

        public int IdCatDos { get; set; }

        public int? IdParent { get; set; }

        public virtual CATEGORIESDOSSIERS CATEGORIESDOSSIERS { get; set; }

        public virtual DOSSIERS Parent { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTS> DOCUMENTS { get; set; }

        [JsonIgnore]
        public virtual ICollection<DOSSIERS> DOSSIERSENF { get; set; }
    }
}
