using AEDFirst.App_Start;
using AEDFirst.Extensions;
using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class DossierController : Controller
    {
        ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }

        public ActionResult CreateDossier()
        {
            if (CurrentUser.HasRight("CreateDoss"))
            {
                List<CATEGORIESDOSSIERS> CDs = db.CATEGORIESDOSSIERS.ToList();
                return View(CDs);
            } else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }

        [HttpPost]
        public ActionResult CreateDossier (DOSSIERS Dossier)
        {
            if (CurrentUser.HasRight("CreateDoss"))
            {
                db.DOSSIERS.Add(Dossier);
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
        }

        public ActionResult RenommerDossier(int id, string NewName)
        {
            if (CurrentUser.HasRight("UpdateDoss"))
            {
                DOSSIERS Dossier = db.DOSSIERS.Find(id);
                if (Dossier == null)
                {
                    return View();
                }
                Dossier.NomDoss = NewName;
                    
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        // Déplacer un dossier vers un autre
        public ActionResult DeplacerDossier(int id, int ParentId)
        {
            if (CurrentUser.HasRight("UpdateDoss"))
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
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
        }

        public ActionResult DeleteDossierRecursive(int id)
        {
            if (CurrentUser.HasRight("DeleteDoss"))
            {
                DOSSIERS Dossier = db.DOSSIERS.Find(id);

                if (Dossier == null)
                {
                    return HttpNotFound();
                }
                CATEGORIESDOSSIERS DossierCat = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                string folderPath = Server.MapPath($"~/UploadedFiles");

                List<DOCUMENTS> Docs = db.DOCUMENTS.Where(doc => doc.IdDoss == Dossier.IdDoss).ToList();

                foreach (var Doc in Docs)
                {
                    System.IO.File.Delete(Path.Combine(folderPath, Doc.NomDocFile));
                    db.DOCUMENTS.Remove(Doc);
                }

                db.DOSSIERS.Remove(Dossier);
                db.SaveChanges();

                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        public ActionResult DeleteDossier(int id)
        {
            if (CurrentUser.HasRight("DeleteDoss"))
            {
                DOSSIERS Dossier = db.DOSSIERS.Find(id);
                if (Dossier != null)
                {
                    // Remove the Dossier entity from the database
                    db.DOSSIERS.Remove(Dossier);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
        }

    }
}