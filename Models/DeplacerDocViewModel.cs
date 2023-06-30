using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class DeplacerDocViewModel
    {
        public int IdDoc { get; set; }

        public int? OldFolderId { get; set; }

        public int? OldFolderCatId { get; set; }

        public int NewFolderId { get; set; }

        public int? NewFolderCatId { get; set; }
    }
}