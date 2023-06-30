using AEDFirst.App_Start;
using AEDFirst.Extensions;
using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    [AuthenticationFilter]
    public class DroitController : Controller
    {
        ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }

        public ActionResult GererDroits()
        {
            if (CurrentUser.HasRight("GrantRights"))
            {
                List<DROITS> droitsnow = (List<DROITS>)Session["listdroits"];
                UTILIZ user = (UTILIZ)Session["user"];

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
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }
    }
}