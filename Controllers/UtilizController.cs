using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class UtilizController : Controller
    {
        ModelAED db = new ModelAED();

        // GET: User
        public ActionResult Index()
        {
            var Users = db.UTILIZ.ToList();
            return View(Users);
        }

        public ActionResult GetData()
        {
            var Users = db.UTILIZ.ToList();
            return Json(Users, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsUser(int Id)
        {
            UTILIZ User = db.UTILIZ.Find(Id);
            var query = from d in db.DROITS
                        join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
                        join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
                        where u.IdUtiliz == Id
                        select d.LibelleDrt;

            string[] rights = query.ToArray();

            UserRightsViewModel Vm = new UserRightsViewModel();
            Vm.User = User;
            Vm.DroitsDispo = rights;
            return View(Vm);
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(UTILIZ User)
        {
            User.Created_at = DateTime.UtcNow;
            UTILIZ Us = db.UTILIZ.Add(User);
            db.SaveChanges();
            return RedirectToAction("DetailsUser", "Utiliz", new { Id = Us.IdUtiliz });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            UTILIZ User = db.UTILIZ.Where(u => u.Login == Login && u.Password == Password && u.Active == true).FirstOrDefault();
            if(User == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "Utiliz");
        }

        public List<DROITS> GetUserRights(int Id)
        {
            var query = from d in db.DROITS
                        join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
                        join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
                        where u.IdUtiliz == Id
                        select d;

            List<DROITS> rights = query.ToList();
            return rights;
        }


        public bool IsUserAuthorized(int Id, string NomDroit)
        {
            DROITS Dt = db.DROITS.Where(d => d.LibelleDrt == NomDroit).FirstOrDefault();
            var query = from d in db.DROITS
                        join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
                        join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
                        where u.IdUtiliz == Id & u.Active == true
                        select d;
            List<DROITS> rights = query.ToList();
            return rights.Contains(Dt);
        }

        public ActionResult GrantRights(int Id)
        {
            UTILIZ User = db.UTILIZ.Find(Id);
            string[] DroitsDispo = db.DROITS.Select(dd => dd.LibelleDrt).ToArray();
            var query = from u in db.UTILIZ
                        join ud in db.UTILIZDROITS on u.IdUtiliz equals ud.IdUtiliz
                        join d in db.DROITS on ud.IdDrt equals d.IdDrt
                        where u.IdUtiliz == Id
                        select d.LibelleDrt;

            string[] DroitsUser = query.ToArray();

            if (User == null && DroitsDispo.Length == 0)
            {
                return HttpNotFound();
            }

            UserRightsViewModel Vm = new UserRightsViewModel();
            Vm.User = User;
            Vm.DroitsDispo = DroitsDispo;
            Vm.DroitsUser = DroitsUser;

            return View(Vm);
        }

        [HttpPost]
        public ActionResult GrantRights(int IdUtiliz, string[] Rights)
        {
            //var query = from u in db.UTILIZ
            //                join ud in db.UTILIZDROITS on u.IdUtiliz equals ud.IdUtiliz
            //                join d in db.DROITS on ud.IdDrt equals d.IdDrt
            //                where u.IdUtiliz == IdUtiliz && Rights.Contains(d.LibelleDrt)
            //                select ud;

            List<UTILIZDROITS> oldUDs = db.UTILIZDROITS.Where(ud => ud.IdUtiliz == IdUtiliz).ToList();

            foreach (UTILIZDROITS oldUD in oldUDs)
            {
                db.UTILIZDROITS.Remove(oldUD);
            }

            if (Rights != null)
            {
                foreach (string rt in Rights)
                {
                    var droit = db.DROITS.FirstOrDefault(d => d.LibelleDrt == rt);

                    if (droit != null)
                    {
                        UTILIZDROITS newUD = new UTILIZDROITS();
                        newUD.IdUtiliz = IdUtiliz;
                        newUD.IdDrt = droit.IdDrt;
                        newUD.DateUD = DateTime.UtcNow;
                        db.UTILIZDROITS.Add(newUD);
                   
                    }
                }
            }
            
            db.SaveChanges();
            //foreach (string rt in Rights)
            //{
            //    var query = from u in db.UTILIZ
            //                join ud in db.UTILIZDROITS on u.IdUtiliz equals ud.IdUtiliz
            //                join d in db.DROITS on ud.IdDrt equals d.IdDrt
            //                where u.IdUtiliz == IdUtiliz && d.LibelleDrt == rt
            //                select ud;

            //    UTILIZDROITS OldUD = query.FirstOrDefault();

            //    db.UTILIZDROITS.Remove(OldUD);

            //    UTILIZDROITS UD = new UTILIZDROITS();
            //    UD.IdUtiliz = IdUtiliz;
            //    UD.IdDrt = db.DROITS.Where(d => d.LibelleDrt == rt).FirstOrDefault().IdDrt;
            //    UD.DateUD = DateTime.UtcNow;
            //    db.UTILIZDROITS.Add(UD);
            //    db.SaveChanges();
            //}
            //db.SaveChanges();
            return RedirectToAction("DetailsUser", "Utiliz", new { id = IdUtiliz });
        }

        public ActionResult EditUser(int Id)
        {
            UTILIZ User = db.UTILIZ.Find(Id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        [HttpPost]
        public ActionResult EditUser(UTILIZ User)
        {
            UTILIZ Us = db.UTILIZ.Find(User.IdUtiliz);
            if(Us != null)
            {
                Us.Login = User.Login;
                Us.Nom = User.Nom;
                Us.Prenom = User.Prenom;
                Us.Email = User.Email;
                Us.Photo = User.Photo;
                Us.TypeCompte = User.TypeCompte;
                Us.Active = User.Active;

                db.SaveChanges();
                return RedirectToAction("DetailsUser", "Utiliz", new { Id = Us.IdUtiliz } );
            } else
            {
                return HttpNotFound();
            }
        }

        public ActionResult DeleteUser(int Id)
        {
            UTILIZ User = db.UTILIZ.Find(Id);
            if(User != null)
            {
                db.UTILIZ.Remove(User);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Utiliz");
        }

        public ActionResult Crm()
        {
            return View();
        }
    }
}