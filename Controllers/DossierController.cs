using AEDFirst.App_Start;
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
            CATEGORIESDOSSIERS FolderCat = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);
            string folderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{Dossier.NomDoss}");
            if (Directory.Exists(folderPath))
            {
                return View();
            } else
            {
                Directory.CreateDirectory(folderPath);
                db.DOSSIERS.Add(Dossier);
            }
            db.SaveChanges();
            return View();
        }

        public ActionResult RenommerDossier(int id, string NewName)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);
            if (Dossier == null)
            {
                return View();
            }
            CATEGORIESDOSSIERS FolderCat = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);
            // MODIFY
            string oldFolderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{Dossier.NomDoss}");
            string newFolderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{NewName}");

            if (Directory.Exists(oldFolderPath))
            {
                Dossier.NomDoss = NewName;
                Directory.Move(oldFolderPath, newFolderPath);
                // Folder renamed successfully
            }
            else
            {
                // Folder already exists
                return RedirectToAction("Index");
            }
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

        public ActionResult DeleteDossierRecursive(int id)
        {
            DOSSIERS Dossier = db.DOSSIERS.Find(id);

            if (Dossier == null)
            {
                return HttpNotFound();
            }
            CATEGORIESDOSSIERS DossierCat = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

            string folderPath = Server.MapPath($"~/UploadedFiles/{DossierCat.NomCatDos}/{Dossier.NomDoss}");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, recursive:true);

                List<DOCUMENTS> Docs = db.DOCUMENTS.Where(doc => doc.IdDoss == Dossier.IdDoss).ToList();

                foreach (var Doc in Docs)
                {
                    //ActionResult result = documentController.DeleteDoc(Doc.IdDoc);
                    db.DOCUMENTS.Remove(Doc);
                }

                db.DOSSIERS.Remove(Dossier);
                db.SaveChanges();
            } else
            {
                ViewBag.Message = "Le dossier n'existe pas";
                return RedirectToAction("Index", "Home");
            }

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