using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class DossierController : Controller
    {
        ModelAED db = new ModelAED();
        // GET: Dossier
        //public ActionResult Index()
        //{
        //    return View(db);
        //}

        public ActionResult CreateDossier()
        {
            List<CATEGORIESDOSSIERS> CDs = db.CATEGORIESDOSSIERS.ToList();
            return View(CDs);
        }

        [HttpPost]
        public ActionResult CreateDossier (DOSSIERS Dossier)
        {
            db.DOSSIERS.Add(Dossier);
            db.SaveChanges();
            return View();
        }

        public ActionResult RenommerDossier(int id, string NewName)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);
            if (Dossier == null)
            {
                return HttpNotFound();
            }
            Dossier.NomDoss = NewName;
            db.SaveChanges();
            return View();
        }

        // Déplacer un dossier vers un autre
        public ActionResult DeplacerDossier(int id, int ParentId)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);
            DOSSIERS Parent = db.DOSSIERS.Find(ParentId);
            if (Dossier == null || Parent == null)
            {
                return HttpNotFound();
            }
            Dossier.IdParent = ParentId;
            db.SaveChanges();
            return View();
        }

        public ActionResult ViderDossier(int id)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);

            if (Dossier == null)
            {
                return HttpNotFound();
            }

            // Logic

            return View();
        }

        public ActionResult DeleteDossier(int id)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);
            if (Dossier == null)
            {
                return HttpNotFound();
            }
            db.DOSSIERS.Remove(Dossier);
            return View();
        }
    }
}