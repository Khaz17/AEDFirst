using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class GererDroitsViewModel
    {
        public List<UTILIZ> ListUsers { get; set; }

        public UTILIZ Utiliz { get; set; }

        public List<DROITS> UserDroits { get; set; }

        public List<DROITS> ListDroits { get; set; }

    }
}