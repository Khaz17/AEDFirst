namespace AEDFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UTILIZDROITS
    {
        [Key]
        public int IdUD { get; set; }

        public int IdDrt { get; set; }

        public int IdUtiliz { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUD { get; set; }

        public virtual DROITS DROITS { get; set; }

        public virtual UTILIZ UTILIZ { get; set; }
    }
}
