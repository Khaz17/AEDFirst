using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class DossierViewModel
    {
        public int IdDoss { get; set; }

        public string NomDoss { get; set; }

        public int? Taille { get; set; }

        public int? NbreDocs { get; set; }

        public int? IdParent { get; set; }
    }
}