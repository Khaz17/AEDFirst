using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class LogDocViewModel
    {
        public int IdLog { get; set; }

        [Display(Name = "Utilisateur")]
        public UTILIZ ConcernedUser { get; set; }

        [Display(Name = "Document")]
        public DOCUMENTS ConcernedDoc { get; set; }

        [Display(Name = "Type de log")]
        public string LogType { get; set; }

        public string Description { get; set; }

        [Display(Name = "Date et heure")]
        public DateTime DateLog { get; set; }
    }
}