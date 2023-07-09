using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class CorbeilleViewModel
    {
        public int IdDoc { get; set; }
         
        public string Titre { get; set; }

        [Display(Name = "Emplacement d'origine")]
        public string EmplacementOriginel { get; set; }

        [Display(Name = "Date de suppression")]
        public DateTime DateSuppression { get; set; }

        [Display(Name = "Supprimé par")]
        public string SupprimePar { get; set; }

        public int Taille { get; set; }

        public string Format { get; set; }
    }
}