using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AEDFirst.Models;

namespace AEDFirst.Controllers
{
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
                db.CATEGORIESDOSSIERS.Add(CategorieDossier);
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
            if (ModelState.IsValid)
            {
                db.Entry(CategorieDossier).State = EntityState.Modified;
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

        [HttpPost]
        public ActionResult DeleteCatDossier(int id)
        {
            CATEGORIESDOSSIERS CategorieDossier = db.CATEGORIESDOSSIERS.Find(id);
            db.CATEGORIESDOSSIERS.Remove(CategorieDossier);
            db.SaveChanges();
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
