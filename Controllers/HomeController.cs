using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using AEDFirst.App_Start;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class HomeController : Controller
    {
        ModelAED db = new ModelAED();
        public ActionResult Index()
        {
            // PREMIERE PARTIE: RETOURNER LE VM CONTENANT LES CATEGORIES DOSSIERS ET LEURS DOSSIERS

            List<CATEGORIESDOSSIERS> CateDossiers = db.CATEGORIESDOSSIERS.ToList();
            List<DOSSIERS> Dossiers = db.DOSSIERS.ToList();
            
            List<GestionnaireViewModel> ListVms = new List<GestionnaireViewModel>();

            foreach (var CD in CateDossiers)
            {
                GestionnaireViewModel Vm = new GestionnaireViewModel();
                Vm.CategorieDossier = CD;
                List<DossierViewModel> DossiersContenus = new List<DossierViewModel>();
                foreach (var item in Dossiers)
                {
                    if (item.IdCatDos == CD.IdCatDos) {
                        DossierViewModel Dos = new DossierViewModel {
                            IdDoss = item.IdDoss,
                            NomDoss = item.NomDoss,
                            IdParent = item.IdParent,
                            //NbreDossiers = ,
                            //Taille = 
                        };
                        DossiersContenus.Add(Dos);
                    }
                }
                Vm.Dossiers = DossiersContenus;
                ListVms.Add(Vm);
                
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                //ContractResolver = new DefaultContractResolver
                //{
                //    NamingStrategy = new CamelCaseNamingStrategy()
                //}
            };

            //var jsonData = JsonConvert.SerializeObject(ListVms, settings);

            //SECONDE PARTIE: GÉNÉRER LE FICHIER JS0N CONTENANT LES INFOS DE TOUS LES DOCUMENTS
            List<DocumentViewModel> docData = new List<DocumentViewModel>();
            List<DOCUMENTS> Docs = db.DOCUMENTS.ToList();

            foreach (var Doc in Docs)
            {
                //var Uploader = Users.FirstOrDefault(U => Doc.IdUploader == U.IdUtiliz);
                var Dossier = Dossiers.Where(Doss => Doc.IdDoss == Doss.IdDoss).FirstOrDefault();
                var CateDos = CateDossiers.Where(CD => Dossier.IdCatDos == CD.IdCatDos).FirstOrDefault();



                DocumentViewModel Dvm = new DocumentViewModel
                {
                    IdDoc = Doc.IdDoc,
                    Titre = Doc.Titre,
                    Format = Doc.Format,
                    Taille = Doc.Taille,
                    DateUpload = Doc.DateUpload,
                    NomDocFile = Doc.NomDocFile,
                    Tags = Doc.Tags,
                    //Uploader = $"{Uploader.Prenom} {Uploader.Nom}",
                    Dossier = Dossier != null ? Dossier.NomDoss : "",
                    CateDos = CateDos != null ? CateDos.NomCatDos : "",
                };

                docData.Add(Dvm);
            }

            var jsonDocData = JsonConvert.SerializeObject(docData, settings);

            var filePath = Server.MapPath("~/Data/documents.json");

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.Write(jsonDocData);
            }
            return View(ListVms);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}