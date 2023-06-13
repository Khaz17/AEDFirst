using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Models
{
    public class UploadDocumentViewModel
    {
        public int IdDoc { get; set; }

        public HttpPostedFileBase DocFile { get; set; }

        public string Titre { get; set; }

        public string Format { get; set; }

        public int Taille { get; set; }

        public DateTime DateUpload { get; set; }

        public HttpPostedFile Image { get; set; }

        public string Tags { get; set; }

        public string NomAuteur { get; set; }

        public int IdUploader { get; set; }

        public int IdSC { get; set; }

        public int IdDoss { get; set; }
    }
}