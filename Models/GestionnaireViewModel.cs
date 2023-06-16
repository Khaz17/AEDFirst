using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class GestionnaireViewModel
    {
        public List<CATEGORIESDOSSIERS> CategoriesDossiers { get; set; }

        public List<DOSSIERS> Dossiers { get; set; }

        public List<DOCUMENTS> Documents { get; set; }
    }
}