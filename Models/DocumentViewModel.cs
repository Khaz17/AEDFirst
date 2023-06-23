using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class DocumentViewModel
    {
        public int IdDoc { get; set; }

        public string Titre { get; set; }

        public string Format { get; set; }

        public int Taille { get; set; }

        [Display(Name = "Date d'ajout")]
        public DateTime DateUpload { get; set; }

        public string NomDocFile { get; set; }

        public string Tags { get; set; }

        [Display(Name = "Ajouté par")]
        public string Uploader { get; set; }

        //[Display(Name = "Sous Catégorie")]
        //public string SousCategorie { get; set; }

        public string Dossier { get; set; }

        public string CateDos { get; set; }
    }
}