using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class UserInfoViewModel
    {
        public UTILIZ User { get; set; }

        public List<DROITS> DroitsUser { get; set; }
    }
}