using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AEDFirst.App_Start;
using AEDFirst.Extensions;
using AEDFirst.Models;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class CategorieDossierController : Controller
    {
        private ModelAED db = new ModelAED();
        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }

        public ActionResult Index()
        {
            if (CurrentUser.HasRight("ListCat"))
            {
                List<CatDossierViewModel> ListCategoriesDossiers = new List<CatDossierViewModel>();
                var CDs = db.CATEGORIESDOSSIERS.ToList();

                foreach (CATEGORIESDOSSIERS CD in CDs)
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
            } else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult CreateCatDossier()
        {
            if (CurrentUser.HasRight("CreateCat"))
            {
                return PartialView("_CreateCatDossier", new CATEGORIESDOSSIERS());
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

        }

        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCatDossier([Bind(Include = "IdCatDos,NomCatDos")] CATEGORIESDOSSIERS CategorieDossier)
        {
            if (CurrentUser.HasRight("CreateCat"))
            {
                if (ModelState.IsValid)
                {
                    db.CATEGORIESDOSSIERS.Add(CategorieDossier);
                    // Folder created successfully
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(CategorieDossier);
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

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
            if (CurrentUser.HasRight("UpdateCat"))
            {
                CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
                if (CategorieDossier == null)
                {
                    return HttpNotFound();
                }
                return PartialView("_EditCatDossier", CategorieDossier);
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCatDossier([Bind(Include = "IdCatDos,NomCatDos")] CATEGORIESDOSSIERS CategorieDossier)
        {
            if (CurrentUser.HasRight("UpdateCat"))
            {
                CATEGORIESDOSSIERS CD = db.CATEGORIESDOSSIERS.Find(CategorieDossier.IdCatDos);
                if (ModelState.IsValid && CD != null)
                {
                    CD.NomCatDos = CategorieDossier.NomCatDos;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(CategorieDossier);
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult DeleteCatDossier(int id)
        {
            if (CurrentUser.HasRight("DeleteDoss"))
            {
                CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
                if (CategorieDossier != null)
                {
                    db.CATEGORIESDOSSIERS.Remove(CategorieDossier);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

        }

        // Modifier type de suppression des documents ?
        public ActionResult DeleteCatDossierRecursive(int id)
        {
            if (CurrentUser.HasRight("DeleteDoss"))
            {
                CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);

                string folderPath = Server.MapPath($"~/UploadedFiles");

                if (Directory.Exists(folderPath))
                {
                    int[] RelatedFolders = db.DOSSIERS.Where(d => d.IdCatDos == id).Select(doss => doss.IdDoss).ToArray();
                    var Dossiers = db.DOSSIERS.Where(d => RelatedFolders.Contains(d.IdDoss));
                    var Documents = db.DOCUMENTS.Where(doc => RelatedFolders.Contains(doc.IdDoss.Value));

                    foreach (var Doc in Documents)
                    {
                        // Suppression définitive
                        System.IO.File.Delete(Path.Combine(folderPath, Doc.NomDocFile));
                        db.DOCUMENTS.Remove(Doc);
                    }

                    foreach (var doss in Dossiers)
                    {
                        db.DOSSIERS.Remove(doss);
                    }
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Le dossier correspondant à la catégorie est introuvable";

                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

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
