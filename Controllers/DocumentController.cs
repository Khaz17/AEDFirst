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

            List<DocumentViewModel> Documents = new List<DocumentViewModel>();
            foreach (var Doc in Docs)
            {
                //var Uploader = Users.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
                //var SousCate = SousCats.FirstOrDefault(Sc => Doc.IdSC == Sc.IdSC);
                //var Dossier = Dossiers.FirstOrDefault(Doss => Doc.IdDoss == Doss.IdDoss);

                DocumentViewModel Vm = new DocumentViewModel
                {
                    IdDoc = Doc.IdDoc,
                    Titre = Doc.Titre,
                    Format = Doc.Format,
                    Taille = Doc.Taille,
                    DateUpload = Doc.DateUpload,
                    Image = Doc.Image,
                    Tags = Doc.Tags,
                    NomAuteur = Doc.NomAuteur,
                    //Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                    //SousCategorie = SousCate.NomSC,
                    //Dossier = Dossier.NomDoss
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

            DocumentViewModel Vm = new DocumentViewModel
            {
                IdDoc = Doc.IdDoc,
                Titre = Doc.Titre,
                Format = Doc.Format,
                Taille = Doc.Taille,
                DateUpload = Doc.DateUpload,
                Image = Doc.Image,
                Tags = Doc.Tags,
                NomAuteur = Doc.NomAuteur,
                Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                Dossier = Dossier.NomDoss
            };
            return View();
        }

        // GET: Document/Create
        public ActionResult AddDoc()
        {
            return View();
        }

        // POST: Document/AddDoc
        [HttpPost]
        public ActionResult AddDoc(UploadDocumentViewModel Doc)
        {
            // TODO: Add insert logic here
            DOCUMENTS Document = new DOCUMENTS();

            if (UploadFile(Doc.DocFile, Doc.Titre, Doc.Format.ToLower()))
            {
                Document.Titre = Doc.Titre;
                Document.Format = Doc.Format;
                Document.Taille = Doc.Taille;
                Document.DateUpload = DateTime.UtcNow;
                //UploadFile(Doc.Image);
                Document.Tags = Doc.Tags;
                Document.NomAuteur = Doc.NomAuteur;

                db.DOCUMENTS.Add(Document);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                return View();
            }
            
        }


        public bool UploadFile(HttpPostedFileBase file, string filename, string format)
        {
            string[] ValidExtensions = { "pdf", "txt", "doc", "docx", "xlsx", "xls", "csv", "png", "img", "jpg", "jpeg" };
            if (!ValidExtensions.Contains(format))
            {
                ViewBag.Message = "Extension invalide";
                return false;
            }

            if (file != null && file.ContentLength > 0)
            {
                string SerializedFilename = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}-{filename}.{format}";
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), SerializedFilename);
                file.SaveAs(filePath);

                ViewBag.Message = "File uploaded successfully";
                return true;
            }
            else
            {
                ViewBag.Message = "no file";
                return false;
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
