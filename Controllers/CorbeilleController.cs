using AEDFirst.App_Start;
using AEDFirst.Extensions;
using AEDFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class CorbeilleController : Controller
    {

        private readonly DocumentController _documentController;
        private ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }


        public CorbeilleController()
        {
            _documentController = new DocumentController();
        }

        // GET: Corbeille
        public ActionResult Index()
        {
            if (CurrentUser.HasRight("DisplayTrash"))
            {
                List<DOCUMENTS> DeletedDocs = db.DOCUMENTS.Where(d => d.ToDelete == true).ToList();
                return View(DeletedDocs);
            } else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit." });
            }
           
        }

        public ActionResult ViderCorbeille()
        {
            if (CurrentUser.HasRight("EmptyTrash"))
            {
                List<DOCUMENTS> DeletedDocs = db.DOCUMENTS.Where(d => d.ToDelete == true).ToList();
                foreach (var Doc in DeletedDocs)
                {

                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");
                    Debug.WriteLine("filePath : " + filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        db.DOCUMENTS.Remove(Doc);
                        // File deleted successfully
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit." });
            }
        }


        public ActionResult Restaurer(int id)
        {
            if (CurrentUser.HasRight("RestoreFromTrash"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(id);
                if (Doc != null)
                {
                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");


                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Move(corbeillePath, filePath);
                    }

                    Doc.ToDelete = false;
                    Doc.DateSuppression = null;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        public ActionResult RestoreAll()
        {
            if (CurrentUser.HasRight("RestoreFromTrash"))
            {
                List<DOCUMENTS> DeletedDocs = db.DOCUMENTS.Where(d => d.ToDelete == true).ToList();
                foreach (var Doc in DeletedDocs)
                { 
                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");


                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Move(corbeillePath, filePath);
                    }

                    Doc.ToDelete = false;
                    Doc.DateSuppression = null;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit." });
            }
        }
    }
}