using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class HomeController : Controller
    {
        ModelAED db = new ModelAED();
        public ActionResult Index()
        {
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