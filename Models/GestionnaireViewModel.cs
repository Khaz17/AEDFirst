using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class GestionnaireViewModel
    {
        public CATEGORIESDOSSIERS CategorieDossier { get; set; }

        public List<DossierViewModel> Dossiers { get; set; }
    }
}