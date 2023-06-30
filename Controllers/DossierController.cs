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
                CATEGORIESDOSSIERS FolderCat = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);
                // MODIFY
                string oldFolderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{Dossier.NomDoss}");
                string newFolderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{NewName}");
                bool isEmpty = !Directory.EnumerateFileSystemEntries(oldFolderPath).Any();

                if (isEmpty && Directory.Exists(oldFolderPath))
                {
                    Dossier.NomDoss = NewName;
                    Directory.Move(oldFolderPath, newFolderPath);
                    // Folder renamed successfully
                }
                else
                {
                    // Folder already exists or Folder not empty
                    return RedirectToAction("Index");
                }
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
            if (CurrentUser.HasRight("UpdateDoss"))
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
                    Directory.Delete(folderPath, recursive: true);

                    List<DOCUMENTS> Docs = db.DOCUMENTS.Where(doc => doc.IdDoss == Dossier.IdDoss).ToList();

                    foreach (var Doc in Docs)
                    {
                        //ActionResult result = documentController.DeleteDoc(Doc.IdDoc);
                        db.DOCUMENTS.Remove(Doc);
                    }

                    db.DOSSIERS.Remove(Dossier);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Le dossier n'existe pas";
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        public ActionResult DeleteDossier(int id)
        {
            if (CurrentUser.HasRight("UpdateDoss"))
            {
                DOSSIERS Dossier = db.DOSSIERS.Find(id);
                if (Dossier != null)
                {
                    List<DOCUMENTS> Documents = db.DOCUMENTS.Where(d => d.IdDoss == Dossier.IdDoss).ToList();
                    if (Documents.Count > 0)
                    {
                        string CatDos = db.CATEGORIESDOSSIERS.Where(cd => cd.IdCatDos == Dossier.IdCatDos).Select(cd => cd.NomCatDos).FirstOrDefault();
                        string folderPath = Server.MapPath("~/UploadedFiles/" + CatDos + "/" + Dossier.NomDoss);

                        // Move the documents to the general UploadedFiles folder
                        foreach (var Doc in Documents)
                        {
                            string oldPath = Server.MapPath("~/UploadedFiles/" + CatDos + "/" + Dossier.NomDoss + "/" + Doc.NomDocFile);
                            string newPath = Server.MapPath("~/UploadedFiles/" + Doc.NomDocFile);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Move(oldPath, newPath);
                            }
                        }

                        // Delete the folder and its contents
                        if (Directory.Exists(folderPath))
                        {
                            Directory.Delete(folderPath, true);
                        }
                    }

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