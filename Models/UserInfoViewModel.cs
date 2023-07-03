using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class UserInfoViewModel
    {
        public int IdUtiliz { get; set; }

        public string Login { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? Created_at { get; set; }

        public bool? Active { get; set; }

        [Display(Name = "Créé par")]
        public string Creator { get; set; }

        public string[] Droits { get; set; }
    }
}