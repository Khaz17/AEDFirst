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
    public class CategorieController : Controller
    {
        private ModelAED db = new ModelAED();

        // GET: Categorie
        public ActionResult Index()
        {
            return View(db.CATEGORIES.ToList());
        }

        // GET: Categorie/Details/5
        public ActionResult DetailsCat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIES Categorie = db.CATEGORIES.Find(id);
            if (Categorie == null)
            {
                return HttpNotFound();
            }
            return View(Categorie);
        }

        // GET: Categorie/Create
        public ActionResult CreateCat()
        {
            return View();
        }

        // POST: Categorie/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCat([Bind(Include = "IdCat,NomCat")] CATEGORIES Categorie)
        {
            if (ModelState.IsValid)
            {
                db.CATEGORIES.Add(Categorie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Categorie);
        }

        // GET: Categorie/Edit/5
        public ActionResult EditCat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIES cATEGORIES = db.CATEGORIES.Find(id);
            if (cATEGORIES == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORIES);
        }

        // POST: Categorie/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCat([Bind(Include = "IdCat,NomCat")] CATEGORIES Categorie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Categorie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Categorie);
        }

        // GET: Categorie/Delete/5
        public ActionResult DeleteCat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIES Categorie = db.CATEGORIES.Find(id);
            if (Categorie == null)
            {
                return HttpNotFound();
            }
            return View(Categorie);
        }

        // POST: Categorie/Delete/5
        [HttpPost, ActionName("DeleteCat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCatConfirmed(int id)
        {
            CATEGORIES Categorie = db.CATEGORIES.Find(id);
            db.CATEGORIES.Remove(Categorie);
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
