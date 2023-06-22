using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class DocumentController : Controller
    {
        ModelAED db = new ModelAED();
        // GET: Document
        public ActionResult Index()
        {
            List<DOCUMENTS> Docs = db.DOCUMENTS.ToList();
            List<UTILIZ> Users = db.UTILIZ.ToList();
            List<DOSSIERS> Dossiers = db.DOSSIERS.ToList();
            List<CATEGORIESDOSSIERS> Categories = db.CATEGORIESDOSSIERS.ToList();

            List<DocumentViewModel> Documents = new List<DocumentViewModel>();
            foreach (var Doc in Docs)
            {
                //var Uploader = Users.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
                //var SousCate = SousCats.FirstOrDefault(Sc => Doc.IdSC == Sc.IdSC);
                //var Dossier = Dossiers.FirstOrDefault(Doss => Doc.IdDoss == Doss.IdDoss);
                //var CateDos = Categories.Where(CD => Dossier.IdCatDos == CD.IdCatDos).FirstOrDefault();

                DocumentViewModel Vm = new DocumentViewModel
                {
                    IdDoc = Doc.IdDoc,
                    Titre = Doc.Titre,
                    Format = Doc.Format,
                    Taille = Doc.Taille,
                    DateUpload = Doc.DateUpload,
                    Tags = Doc.Tags,
                    //Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                    //SousCategorie = SousCate.NomSC,
                    //Dossier = Dossier != null ? Dossier.NomDoss : "",
                    //CateDos = CateDos != null ? CateDos.NomCatDos : "",
                };

                Documents.Add(Vm);
            }
            return View(Documents);
        }
        
        // GET: Document/Details/5
        public ActionResult DetailsDoc(int id)
        {
            DOCUMENTS Doc = db.DOCUMENTS.Find(id);  
            var Uploader = db.UTILIZ.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
            var Dossier = db.DOSSIERS.FirstOrDefault(Doss => Doc.IdDoss == Doss.IdDoss);
            var CateDos = db.CATEGORIESDOSSIERS.Where(CD => Dossier.IdCatDos == CD.IdCatDos).FirstOrDefault();

            DocumentViewModel Vm = new DocumentViewModel
            {
                IdDoc = Doc.IdDoc,
                Titre = Doc.Titre,
                Format = Doc.Format,
                Taille = Doc.Taille,
                DateUpload = Doc.DateUpload,
                Tags = Doc.Tags,
                Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                Dossier = Dossier.NomDoss,
                CateDos = CateDos.NomCatDos
            };
            return View();
        }

        // GET: Document/Create
        public ActionResult AddDoc()
        {
            UploadDocumentViewModel Vm = new UploadDocumentViewModel
            {
                Dossiers = db.DOSSIERS.ToList(),
                Categories = db.CATEGORIESDOSSIERS.ToList()
            };
            return View(Vm);
        }

        // POST: Document/AddDoc
        [HttpPost]
        public ActionResult AddDoc(UploadDocumentViewModel Doc)
        {
            // TODO: Add insert logic here
            // Logic for Doc.Titre unicity
            DOCUMENTS Document = new DOCUMENTS();
            DOSSIERS folder = db.DOSSIERS.Find(Doc.IdDoss);
            CATEGORIESDOSSIERS foldercat = db.CATEGORIESDOSSIERS.Find(Doc.IdCateDos);

            if (folder == null || foldercat == null)
            {
                ViewBag.Message = "Dossier ou catégorie introuvable";
                return null;
            }

            var result = UploadFile(Doc.DocFile, Doc.Titre, Doc.Format.ToLower(), folder, foldercat);

            if (result != null)
            {
                Document.Titre = Doc.Titre;
                Document.Format = Doc.Format;
                Document.Taille = Doc.Taille;
                Document.DateUpload = (DateTime)result;
                Document.Tags = Doc.Tags;
                Document.IdDoss = Doc.IdDoss;
                db.DOCUMENTS.Add(Document);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                return View();
            }
            
        }


        public DateTime? UploadFile(HttpPostedFileBase file, string filename, string format, DOSSIERS dossier, CATEGORIESDOSSIERS catedossier)
        {
            string fn = dossier.NomDoss;
            string fcn = catedossier.NomCatDos;

            string[] ValidExtensions = { "pdf", "txt", "doc", "docx", "xlsx", "xls", "csv", "png", "img", "jpg", "jpeg" };
            if (!ValidExtensions.Contains(format))
            {
                ViewBag.Message = "Extension invalide";
                return null;
            }

            if (file != null && file.ContentLength > 0)
            {
                var DateUpload = DateTime.UtcNow;
                string SerializedFilename = $"{DateUpload:yyyyMMdd-HHmm}-{filename}.{format}";
                string folderPath = Server.MapPath($"~/UploadedFiles/{fcn}/{fn}");
                Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, SerializedFilename);
                file.SaveAs(filePath); 

                ViewBag.Message = "File uploaded successfully";
                return DateUpload;
            }
            else
            {
                ViewBag.Message = "no file";
                return null;
            }

        }

        public ActionResult EditDocData(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditDocData(EditDocDataViewModel Doc)
        {
            return RedirectToAction("Index");
        }

        public ActionResult ReplaceDocFile(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReplaceDocFile(HttpPostedFileBase file, string filename, string format)
        {
            return RedirectToAction("Index");
        }

        // GET: Document/Edit/5
        public ActionResult EditDoc(int id)
        {
            return View();
        }

        // POST: Document/Edit/5
        [HttpPost]
        public ActionResult EditDoc(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Document/Delete/5
        public ActionResult DeleteDoc(int id)
        {
            DOCUMENTS Doc = db.DOCUMENTS.Find(id);
            if (Doc != null)
            {
                db.DOCUMENTS.Remove(Doc);
                db.SaveChanges();
            }
            return View();
        }

        // POST: Document/Delete/5
        [HttpPost]
        public ActionResult DeleteDoc(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
