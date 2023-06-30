using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AEDFirst.App_Start;
using System.Diagnostics;
using AEDFirst.Extensions;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class DocumentController : Controller
    {
        ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }

        // GET: Document
        public ActionResult Index()
        {
            if (CurrentUser.HasRight("ListDoc"))
            {
                List<DOCUMENTS> Docs = db.DOCUMENTS.ToList();
                List<UTILIZ> Users = db.UTILIZ.ToList();
                List<DOSSIERS> Dossiers = db.DOSSIERS.ToList();
                List<CATEGORIESDOSSIERS> Categories = db.CATEGORIESDOSSIERS.ToList();

                List<DocumentViewModel> Documents = new List<DocumentViewModel>();
                foreach (var Doc in Docs)
                {
                    var Uploader = Users.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
                    var Dossier = Dossiers.FirstOrDefault(Doss => Doc.IdDoss == Doss.IdDoss);
                    var CateDos = Categories.Where(CD => Dossier.IdCatDos == CD.IdCatDos).FirstOrDefault();

                    DocumentViewModel Vm = new DocumentViewModel
                    {
                        IdDoc = Doc.IdDoc,
                        Titre = Doc.Titre,
                        Format = Doc.Format,
                        Taille = Doc.Taille,
                        DateUpload = Doc.DateUpload,
                        NomDocFile = Doc.NomDocFile,
                        Tags = Doc.Tags,
                        Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                        Dossier = Dossier != null ? Dossier.NomDoss : "",
                        CateDos = CateDos != null ? CateDos.NomCatDos : ""
                    };

                    Documents.Add(Vm);
                }
                return View(Documents);
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }
        
        // GET: Document/Details/5
        public ActionResult DetailsDoc(int id)
        {
            if (CurrentUser.HasRight("ReadDoc"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(id);
                var Uploader = db.UTILIZ.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
                var Dossier = db.DOSSIERS.FirstOrDefault(Doss => Doc.IdDoss == Doss.IdDoss);
                var CateDos = db.CATEGORIESDOSSIERS.Where(CD => Dossier.IdCatDos == CD.IdCatDos).FirstOrDefault();

                DocumentViewModel Vm = new DocumentViewModel
                {
                    IdDoc = Doc.IdDoc,
                    Titre = Doc.Titre,
                    Format = Doc.Format,
                    Taille = Doc.Taille,
                    DateUpload = Doc.DateUpload,
                    NomDocFile = Doc.NomDocFile,
                    Tags = Doc.Tags,
                    Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                    Dossier = Dossier.NomDoss,
                    CateDos = CateDos.NomCatDos
                };
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }

        // GET: Document/Create
        public ActionResult AddDoc()
        {
            if (CurrentUser.HasRight("UploadDoc"))
            {
                UploadDocumentViewModel Vm = new UploadDocumentViewModel
                {
                    Dossiers = db.DOSSIERS.ToList(),
                    Categories = db.CATEGORIESDOSSIERS.ToList()
                };
                return View(Vm);
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        // POST: Document/AddDoc
        [HttpPost]
        public ActionResult AddDoc(UploadDocumentViewModel Doc)
        {
            if (CurrentUser.HasRight("UploadDoc"))
            {
                // Logic for Doc.Titre unicity
                var Documents = db.DOCUMENTS.Select(doc => doc.Titre).ToArray();

                if (Documents.Contains(Doc.Titre))
                {
                    ViewBag.Message = "Un document porte déjà ce nom.";
                    return View();
                }

                DOCUMENTS Document = new DOCUMENTS();
                DOSSIERS folder = db.DOSSIERS.Find(Doc.IdDoss);
                CATEGORIESDOSSIERS foldercat = db.CATEGORIESDOSSIERS.Find(Doc.IdCateDos);
                UTILIZ User = (UTILIZ)Session["CurrentUser"];

                if (folder == null || foldercat == null)
                {
                    ViewBag.Message = "Dossier ou catégorie introuvable";
                    return null;
                }

                var result = UploadFile(Doc.DocFile, Doc.Titre, Doc.Format.ToLower(), folder, foldercat);

                if (result != null)
                {
                    Document.Titre = Doc.Titre;
                    Document.Format = Doc.Format;
                    Document.Taille = Doc.Taille;
                    Document.DateUpload = Document.DateModifRecente = (DateTime)result[0];
                    Document.NomDocFile = (string)result[1];
                    Document.Tags = Doc.Tags;
                    Document.IdDoss = Doc.IdDoss;
                    Document.IdUploader = User.IdUtiliz;
                    db.DOCUMENTS.Add(Document);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }


        public object[] UploadFile(HttpPostedFileBase file, string filename, string format, DOSSIERS dossier, CATEGORIESDOSSIERS catedossier)
        {
            string fn = dossier.NomDoss;
            string fcn = catedossier.NomCatDos;

            string[] ValidExtensions = { "pdf", "txt", "doc", "docx", "xlsx", "xls", "csv", "png", "img", "jpg", "jpeg" };
            if (!ValidExtensions.Contains(format))
            {
                ViewBag.Message = "Extension invalide";
                return null;
            }

            if (file != null && file.ContentLength > 0)
            {
                var DateUpload = DateTime.UtcNow;
                string SerializedFilename = $"{DateUpload:yyyyMMdd-HHmm}-{filename}.{format}";
                string folderPath = Server.MapPath($"~/UploadedFiles/{fcn}/{fn}");
                Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, SerializedFilename);
                file.SaveAs(filePath); 

                ViewBag.Message = "File uploaded successfully";
                return new object[] { DateUpload, SerializedFilename };
            }
            else
            {
                ViewBag.Message = "no file";
                return null;
            }

        }

        //public ActionResult EditDocData(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult EditDocData(EditDocDataViewModel Doc)
        //{
        //    //DOCUMENTS Document = db.DOCUMENTS.Find(Doc.);
        //    return RedirectToAction("Index");
        //}

        public ActionResult RenameFile(int id, string newFileName)
        {
            if (CurrentUser.HasRight("UpdateDoc"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(id);

                string newName = $"{Doc.DateUpload:yyyyMMdd-HHmm}-{newFileName}.{Doc.Format}";

                string oldFilePath = Server.MapPath($"~/UploadedFiles/{Doc.NomDocFile}");
                string newFilePath = Server.MapPath($"~/UploadedFiles/{newName}");

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Move(oldFilePath, newFilePath);
                    Doc.Titre = newFileName;
                    Doc.NomDocFile = newName;
                    Doc.DateModifRecente = DateTime.UtcNow;
                    // File renamed successfully
                    return RedirectToAction("Index");
                }
                else
                {
                    // File does not exist
                    return RedirectToAction("Index", new { message = "Fichier introuvable" });
                }
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }

        public ActionResult ReplaceDocFile(int id)
        {
            if (CurrentUser.HasRight("UpdateDoc"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }


        // MISSING LOGIC
        [HttpPost]
        public ActionResult ReplaceDocFile(int id, HttpPostedFileBase file, string filename, string format, int taille)
        {
            if (CurrentUser.HasRight("UpdateDoc"))
            {
                DOCUMENTS ConcernedDoc = db.DOCUMENTS.Find(id);

                if (ConcernedDoc == null)
                {
                    return View();
                }

                var Folder = db.DOSSIERS.Find(ConcernedDoc.IdDoss);
                var FolderCat = db.CATEGORIESDOSSIERS.Find(Folder.IdCatDos);

                if (Folder == null || FolderCat == null)
                {
                    ViewBag.Message = "Dossier ou catégorie dossier inconnus";
                    return View();
                }

                string[] ValidExtensions = { "pdf", "txt", "doc", "docx", "xlsx", "xls", "csv", "png", "img", "jpg", "jpeg" };
                if (!ValidExtensions.Contains(format))
                {
                    ViewBag.Message = "Extension invalide";
                    return null;
                }

                if (file != null && file.ContentLength > 0)
                {
                    string oldFilePath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{Folder.NomDoss}/{ConcernedDoc.NomDocFile}");
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                        // File deleted successfully
                    }
                    else
                    {
                        return View();
                    }

                    var DateUpload = DateTime.UtcNow;
                    string SerializedFilename = $"{DateUpload:yyyyMMdd-HHmm}-{filename}.{format}";
                    string folderPath = Server.MapPath($"~/UploadedFiles/{FolderCat.NomCatDos}/{Folder.NomDoss}");
                    //Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, SerializedFilename);
                    file.SaveAs(filePath);

                    ConcernedDoc.DateUpload = DateUpload;
                    ConcernedDoc.NomDocFile = SerializedFilename;
                    ConcernedDoc.Format = format;
                    ConcernedDoc.Taille = taille;
                    ConcernedDoc.Titre = filename;
                    ConcernedDoc.DateModifRecente = DateTime.UtcNow;

                    ViewBag.Message = "File uploaded successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "no file";
                    return null;
                }
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
        }

        // GET: Document/Edit/5
        //public ActionResult EditDoc(int id)
        //{
        //    return View();
        //}

        public ActionResult DownloadDoc(int id)
        {
            if (CurrentUser.HasRight("DownloadDoc"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(id);

                if (Doc != null)
                {
                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        return File(fileBytes, MimeMapping.GetMimeMapping(Doc.NomDocFile), Doc.NomDocFile);
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }

        }

        // INCOMPLETE
        public ActionResult DeplacerDoc(DeplacerDocViewModel Vm)
        {
            if (CurrentUser.HasRight("UpdateDoc"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(Vm.IdDoc);
                DOSSIERS OldFolder = Vm.OldFolderId != null ? db.DOSSIERS.Find(Vm.OldFolderId) : null;
                DOSSIERS NewFolder = db.DOSSIERS.Find(Vm.NewFolderId);

                if (Doc != null && NewFolder != null)
                {
                    CATEGORIESDOSSIERS NewFolderCat = db.CATEGORIESDOSSIERS.Find(NewFolder.IdCatDos);
                    if (NewFolderCat != null)
                    {

                    }
                    else
                    {

                    }
                }
                return RedirectToAction("Home", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            
        }

        public ActionResult SendToTrash(int id)
        {
            if (CurrentUser.HasRight("SendToTrash"))
            {
                DOCUMENTS Doc = db.DOCUMENTS.Find(id);
                if (Doc != null)
                {
                    DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
                    CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

                    string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");
                    string corbeillePath = Server.MapPath($"~/Corbeille/{Doc.NomDocFile}");

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Move(filePath, corbeillePath);
                    }

                    Doc.ToDelete = true;
                    Doc.DateSuppression = DateTime.UtcNow;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
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

        // GET: Document/DeleteDoc/5
        public ActionResult DeleteDoc(int id)
        {
            if (CurrentUser.HasRight("DeleteDoc"))
            {

            }
            else
            {
                return RedirectToAction("Index", new { message = "L'accès à cette ressource vous est interdit !" });
            }
            DOCUMENTS Doc = db.DOCUMENTS.Find(id);

            if (Doc == null)
            {
                return View();
            }

            DOSSIERS Dossier = db.DOSSIERS.Find(Doc.IdDoss);
            CATEGORIESDOSSIERS CateDos = db.CATEGORIESDOSSIERS.Find(Dossier.IdCatDos);

            string filePath = Server.MapPath($"~/UploadedFiles/{CateDos.NomCatDos}/{Dossier.NomDoss}/{Doc.NomDocFile}");
            Debug.WriteLine("filePath : " + filePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                db.DOCUMENTS.Remove(Doc);
                // File deleted successfully
            } else
            {
                return HttpNotFound();
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}
