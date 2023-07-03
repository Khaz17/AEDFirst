using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class GrantRightsViewModel
    {
        public UTILIZ User { get; set; }

        public string[] DroitsDispo { get; set; }
        
        public string[] DroitsUser { get; set; }
    }
}