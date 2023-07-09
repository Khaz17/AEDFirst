using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class DOCUMENTSSUPPRIMES
    {
        [Key]
        public int Id { get; set; }

        public int IdDoc { get; set; }

        [Required]
        [StringLength(255)]
        public string Titre { get; set; }

        [Required]
        [StringLength(255)]
        public string EmplacementOriginel { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateSuppression { get; set; }

        public int SupprimePar { get; set; }

        public int Taille { get; set; }

        [Required]
        [StringLength(255)]
        public string Format { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateUpload { get; set; }

        [StringLength(255)]
        public string NomDocFile { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public DateTime DateModifRecente { get; set; }

        public int? IdUploader { get; set; }

        public int? IdDoss { get; set; }
    }
}