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
    }
}