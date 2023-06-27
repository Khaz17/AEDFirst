using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AEDFirst.App_Start;
using AEDFirst.Models;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class CategorieDossierController : Controller
    {
        private ModelAED db = new ModelAED();

        public ActionResult Index()
        {
            List<CatDossierViewModel> ListCategoriesDossiers = new List<CatDossierViewModel>();
            var CDs = db.CATEGORIESDOSSIERS.ToList();

            foreach(CATEGORIESDOSSIERS CD in CDs)
            {
                var Nbre = db.DOSSIERS.Where(d => d.IdCatDos == CD.IdCatDos).Count();
                CatDossierViewModel Vm = new CatDossierViewModel
                {
                    IdCatDos = CD.IdCatDos,
                    NomCatDos = CD.NomCatDos,
                    NbreDossiers = Nbre,
                };
                ListCategoriesDossiers.Add(Vm);
            }

            return View(ListCategoriesDossiers);
        }

        public ActionResult DetailsCatDossier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
            if (CategorieDossier == null)
            {
                return HttpNotFound();
            }
            return View(CategorieDossier);
        }

        public ActionResult CreateCatDossier()
        {
            return PartialView("_CreateCatDossier", new CATEGORIESDOSSIERS());
        }

        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCatDossier([Bind(Include = "IdCatDos,NomCatDos")] CATEGORIESDOSSIERS CategorieDossier)
        {
            if (ModelState.IsValid)
            {
                string folderPath = $"~/UploadedFiles/{CategorieDossier.NomCatDos}";

                string physicalPath = Server.MapPath(folderPath);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                    db.CATEGORIESDOSSIERS.Add(CategorieDossier);
                    // Folder created successfully
                }
                else
                {
                    // Folder already exists
                    return RedirectToAction("Index");
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CategorieDossier);
        }

        public JsonResult GetFolders(int idcateg)
        {
            var Dossiers = db.DOSSIERS.Where(d => d.IdCatDos == idcateg).ToList();
            var jsonData = Dossiers.Select(f => new { IdDoss = f.IdDoss, NomDoss = f.NomDoss });

            // Return the JSON response
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCatDossier(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
            if (CategorieDossier == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditCatDossier", CategorieDossier);
        }

        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCatDossier([Bind(Include = "IdCatDos,NomCatDos")] CATEGORIESDOSSIERS CategorieDossier)
        {
            CATEGORIESDOSSIERS CD = db.CATEGORIESDOSSIERS.Find(CategorieDossier.IdCatDos);
            if (ModelState.IsValid && CD != null)
            {
                string oldFolderPath = Server.MapPath($"~/UploadedFiles/{CD.NomCatDos}");
                string newFolderPath = Server.MapPath($"~/UploadedFiles/{CategorieDossier.NomCatDos}");

                if (Directory.Exists(oldFolderPath))
                {
                    CD.NomCatDos = CategorieDossier.NomCatDos;
                    Directory.Move(oldFolderPath, newFolderPath);
                    // Folder renamed successfully
                }
                else
                {
                    // The folder to rename is not found
                    return RedirectToAction("Index");
                }
                
                //db.Entry(CategorieDossier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(CategorieDossier);
        }

        //public ActionResult DeleteCatDossier(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
        //    if (CategorieDossier == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(CategorieDossier);
        //}


        // LOGIQUE INCOMPLETE
        [HttpPost]
        public ActionResult DeleteCatDossier(int id)
        {
            CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);

            db.CATEGORIESDOSSIERS.Remove(CategorieDossier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCatDossierRecursive(int id)
        {
            CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);


            string folderPath = Server.MapPath($"~/UploadedFiles/{CategorieDossier.NomCatDos}");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, recursive: true);
                int[] RelatedFolders = db.DOSSIERS.Where(d => d.IdCatDos == id).Select(doss => doss.IdDoss).ToArray();
                var Dossiers = db.DOSSIERS.Where(d => RelatedFolders.Contains(d.IdDoss));
                var Documents = db.DOCUMENTS.Where(doc => RelatedFolders.Contains(doc.IdDoss.Value));


                foreach (var doc in Documents)
                {
                    db.DOCUMENTS.Remove(doc);
                }

                foreach (var doss in Dossiers)
                {
                    db.DOSSIERS.Remove(doss);
                }
                db.SaveChanges();
            } else
            {
                ViewBag.Message = "Le dossier correspondant à la catégorie est introuvable";
                
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
