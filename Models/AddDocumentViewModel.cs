using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class AddDocumentViewModel
    {
        public List<DOSSIERS> Dossiers { get; set; }

        public List<CATEGORIESDOSSIERS> CateDossiers { get; set; }
    }
}