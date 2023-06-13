using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class DroitController : Controller
    {
        ModelAED db = new ModelAED();

        // GET: Droit
        public ActionResult Index()
        {
            return View();
        }

        public DROITS GetRightByLibelle(string Libelle)
        {
            DROITS Drt = db.DROITS.Where(d => d.LibelleDrt == Libelle).FirstOrDefault();
            return Drt;
        }

        public ActionResult GererDroits()
        {
            List<DROITS> droitsnow = (List<DROITS>)Session["listdroits"];
            UTILIZ user = (UTILIZ) Session["user"];

            List<DROITS> ListDroits = db.DROITS.ToList();
            List<UTILIZ> ListUsers = db.UTILIZ.ToList();

            GererDroitsViewModel Vm = new GererDroitsViewModel();

            Vm.ListDroits = ListDroits;
            Vm.ListUsers = ListUsers;
            if (droitsnow != null)
            {
                Vm.UserDroits = droitsnow;
                Vm.Utiliz = user;
            }
            Session.Remove("listdroits");
            Session.Remove("user");

            return View(Vm);
        }
    }
}