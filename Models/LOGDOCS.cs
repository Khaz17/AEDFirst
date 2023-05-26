namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LOGDOCS
    {
        [Key]
        public int IdLog { get; set; }

        public int IdUtiliz { get; set; }

        public int IdDoc { get; set; }

        [Required]
        [StringLength(255)]
        public string LogType { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateLog { get; set; }

        public virtual DOCUMENTS DOCUMENTS { get; set; }

        public virtual UTILIZ UTILIZ { get; set; }
    }
}
