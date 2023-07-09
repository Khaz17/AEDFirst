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

        //private readonly DocumentController _documentController;
        private ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }


        //public CorbeilleController()
        //{
        //    _documentController = new DocumentController();
        //}

        // GET: Corbeille
        public ActionResult Index()
        {
            if (CurrentUser.HasRight("DisplayTrash"))
            {
                List<DOCUMENTSSUPPRIMES> DeletedDocs = db.DOCUMENTSSUPPRIMES.ToList();

                List<CorbeilleViewModel> Corbeille = new List<CorbeilleViewModel>();
                foreach (var Doc in DeletedDocs)
                {
                    UTILIZ Suppresseur = db.UTILIZ.Find(Doc.SupprimePar);
                    CorbeilleViewModel Vm = new CorbeilleViewModel
                    {
                        IdDoc = Doc.IdDoc,
                        Titre = Doc.Titre,
                        Format = Doc.Format,
                        DateSuppression = Doc.DateSuppression,
                        SupprimePar = $"{Suppresseur.Prenom} {Suppresseur.Nom}",
                        EmplacementOriginel = Doc.EmplacementOriginel,
                        Taille = Doc.Taille
                    };
                    Corbeille.Add(Vm);
                }
                return View(Corbeille);
            } else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit." });
            }
           
        }

        public ActionResult ViderCorbeille()
        {
            if (CurrentUser.HasRight("EmptyTrash"))
            {
                List<DOCUMENTSSUPPRIMES> DeletedDocs = db.DOCUMENTSSUPPRIMES.ToList();
                foreach (var Doc in DeletedDocs)
                {

                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);

                    string filePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    Debug.WriteLine("filePath : " + filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        db.DOCUMENTSSUPPRIMES.Remove(Doc);
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
                DOCUMENTSSUPPRIMES Doc = db.DOCUMENTSSUPPRIMES.Where(ds => ds.IdDoc == id).FirstOrDefault();
                Debug.WriteLine(id);
                Debug.WriteLine(Doc.Titre);
                if (Doc != null)
                {
                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    string filePath = Server.MapPath($"~/UploadedFiles/{Doc.NomDocFile}");


                    if (System.IO.File.Exists(corbeillePath))
                    {
                        DOCUMENTS RestoredDoc = new DOCUMENTS
                        {
                            IdDoc = Doc.IdDoc,
                            Titre = Doc.Titre,
                            Format = Doc.Format,
                            Taille = Doc.Taille,
                            DateUpload = Doc.DateUpload,
                            NomDocFile = Doc.NomDocFile,
                            IdUploader = Doc.IdUploader,
                            DateModifRecente = Doc.DateModifRecente,
                            IdDoss = Doc.IdDoss,
                            Tags = Doc.Tags
                        };
                        db.DOCUMENTS.Add(RestoredDoc);
                        db.DOCUMENTSSUPPRIMES.Remove(Doc);
                        db.SaveChanges();
                        System.IO.File.Move(corbeillePath, filePath);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        public ActionResult Supprimer(int id)
        {
            if (CurrentUser.HasRight("DeleteDoc"))
            {
                DOCUMENTSSUPPRIMES Doc = db.DOCUMENTSSUPPRIMES.Where(ds => ds.IdDoc == id).FirstOrDefault();
                if (Doc != null)
                {
                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    if (System.IO.File.Exists(corbeillePath))
                    {
                        System.IO.File.Delete(corbeillePath);
                        db.DOCUMENTSSUPPRIMES.Remove(Doc);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
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
                List<DOCUMENTSSUPPRIMES> DeletedDocs = db.DOCUMENTSSUPPRIMES.ToList();
                foreach (var Doc in DeletedDocs)
                {
                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");
                    string filePath = Server.MapPath($"~/UploadedFiles/{Doc.NomDocFile}");

                    if (System.IO.File.Exists(corbeillePath))
                    {
                        System.IO.File.Move(corbeillePath, filePath);
                        DOCUMENTS RestoredDoc = new DOCUMENTS
                        {
                            IdDoc = Doc.IdDoc,
                            Titre = Doc.Titre,
                            Format = Doc.Format,
                            Taille = Doc.Taille,
                            DateUpload = Doc.DateUpload,
                            NomDocFile = Doc.NomDocFile,
                            IdUploader = Doc.IdUploader,
                            DateModifRecente = Doc.DateModifRecente,
                            IdDoss = Doc.IdDoss,
                            Tags = Doc.Tags
                        };
                        db.DOCUMENTS.Add(RestoredDoc);
                        db.DOCUMENTSSUPPRIMES.Remove(Doc);
                        db.SaveChanges();
                    } 

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