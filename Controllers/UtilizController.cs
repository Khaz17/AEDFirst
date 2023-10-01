using AEDFirst.Extensions;
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

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ)Session["CurrentUser"];
            }
        }

        // GET: User
        public ActionResult Index()
        {
            if (CurrentUser.HasRight("ListUser"))
            {
                List<UserInfoViewModel> UsersData = new List<UserInfoViewModel>();
                var Users = db.UTILIZ.ToList();
                foreach (var User in Users)
                {
                    var Creator = db.UTILIZ.Find(User.IdCreator);
                    UserInfoViewModel Vm = new UserInfoViewModel();
                    Vm.IdUtiliz = User.IdUtiliz;
                    Vm.Login = User.Login;
                    Vm.Nom = User.Nom;
                    Vm.Prenom = User.Prenom;
                    Vm.Password = User.Password;
                    Vm.Email = User.Email;
                    Vm.Active = User.Active;
                    Vm.Creator = Creator != null ? $"{Creator.Prenom} {Creator.Nom}" : null;
                    Vm.Created_at = User.Created_at;
                    UsersData.Add(Vm);
                }
                return View(UsersData);
            } 
            ViewBag.Message = "L'accès à cette ressource vous est interdit !";
            return RedirectToAction("Index");
        }

        public ActionResult GetData()
        {
            var Users = db.UTILIZ.ToList();
            return Json(Users, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsUser(int Id)
        {
            if (CurrentUser.HasRight("ListUser"))
            {
                UTILIZ User = db.UTILIZ.Find(Id);
                var query = from d in db.DROITS
                            join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
                            join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
                            where u.IdUtiliz == Id
                            select d.LibelleDrt;

                string[] rights = query.ToArray();
                var Creator = db.UTILIZ.Find(User.IdCreator);

                UserInfoViewModel Vm = new UserInfoViewModel();
                Vm.IdUtiliz = User.IdUtiliz;
                Vm.Login = User.Login;
                Vm.Nom = User.Nom;
                Vm.Prenom = User.Prenom;
                Vm.Password = User.Password;
                Vm.Email = User.Email;
                Vm.Active = User.Active;
                if (Creator != null)
                {
                    Vm.Creator = $"{Creator.Prenom} {Creator.Nom}";
                }
                Vm.Created_at = User.Created_at;
                Vm.Droits = rights;
                return View(Vm);
            }
            ViewBag.Message = "L'accès à cette ressource vous est interdit !";
            return RedirectToAction("Index");
        }

        public ActionResult AddUser()
        {
            if (CurrentUser.HasRight("CreateUser"))
            {
                return View();
            } else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult AddUser(UTILIZ User)
        {
            if (CurrentUser.HasRight("CreateUser"))
            {
                User.Created_at = DateTime.UtcNow;
                User.IdCreator = CurrentUser.IdUtiliz;
                User.Password = BCrypt.Net.BCrypt.HashPassword(User.Password);
                UTILIZ Us = db.UTILIZ.Add(User);
                // Add rights according to the type of account chosen, after creating the type attribute in the user class
                db.SaveChanges();
                return RedirectToAction("DetailsUser", "Utiliz", new { Id = Us.IdUtiliz });
            } else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            UTILIZ User = db.UTILIZ.Where(u => u.Login == Login && u.Active == true).FirstOrDefault();
            string hashedPasswordFromDatabase = User.Password;

            if (!BCrypt.Net.BCrypt.Verify(Password, hashedPasswordFromDatabase))
            {
                ViewBag.Message = "Nom d'utilisateur ou mot de passe incorrect";
                ViewBag.MessageClass = "alert-danger";
                return View();
            } else {
                if (User.Active == false)
                {
                    ViewBag.Message = "Compte inactif ! Connexion impossible.";
                    ViewBag.MessageClass = "alert-danger";
                    return View();
                }
                Session["CurrentUser"] = User;

                var query = from u in db.UTILIZ
                            join ud in db.UTILIZDROITS on u.IdUtiliz equals ud.IdUtiliz
                            join d in db.DROITS on ud.IdDrt equals d.IdDrt
                            where u.IdUtiliz == User.IdUtiliz
                            select d;

                List<DROITS> Droits = query.ToList();

                if (Droits == null)
                {
                    ViewBag.Message = "Aucun droit ne vous a été accordé !";
                    ViewBag.MessageClass = "alert-danger";
                    return View();
                }

                Session["Droits"] = Droits;
                
                ViewBag.Message = $"Utilisateur {User.Prenom} {User.Nom} connecté";
                ViewBag.MessageClass = "alert-success";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Utiliz");
        }

        public ActionResult GetUserRights(int Id)
        {
            if (CurrentUser.HasRight("GrantRights"))
            {
                var query = from u in db.UTILIZ
                            join ud in db.UTILIZDROITS on u.IdUtiliz equals ud.IdUtiliz
                            join d in db.DROITS on ud.IdDrt equals d.IdDrt
                            where u.IdUtiliz == Id
                            select d;

                List<DROITS> rights = query.ToList();
                Session["listdroits"] = rights;
                Session["user"] = db.UTILIZ.Find(Id);
                return RedirectToAction("GererDroits", "Droit");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }


        //public bool IsUserAuthorized(int Id, string NomDroit)
        //{
        //    DROITS Dt = db.DROITS.Where(d => d.LibelleDrt == NomDroit).FirstOrDefault();
        //    var query = from d in db.DROITS
        //                join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
        //                join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
        //                where u.IdUtiliz == Id & u.Active == true
        //                select d;
        //    List<DROITS> rights = query.ToList();
        //    return rights.Contains(Dt);
        //}

        public ActionResult GrantRights(int Id)
        {
            if (CurrentUser.HasRight("GrantRights"))
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

                GrantRightsViewModel Vm = new GrantRightsViewModel();
                Vm.User = User;
                Vm.DroitsDispo = DroitsDispo;
                Vm.DroitsUser = DroitsUser;

                return View(Vm);
            } else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult GrantRights(int IdUtiliz, string[] Rights)
        {
            if (CurrentUser.HasRight("GrantRights"))
            {
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

                return RedirectToAction("GererDroits", "Droit");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditUser(int Id)
        {
            if (CurrentUser.HasRight("UpdateUser"))
            {
                UTILIZ User = db.UTILIZ.Find(Id);
                if (User == null)
                {
                    return HttpNotFound();
                }
                return View(User);
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EditUser(UTILIZ User)
        {
            if (CurrentUser.HasRight("UpdateUser"))
            {
                UTILIZ Us = db.UTILIZ.Find(User.IdUtiliz);
                if (Us != null)
                {
                    Us.Login = User.Login;
                    Us.Nom = User.Nom;
                    Us.Prenom = User.Prenom;
                    Us.Email = User.Email;
                    Us.Active = User.Active;
                    Us.Password = BCrypt.Net.BCrypt.HashPassword(User.Password);


                    db.SaveChanges();
                    return RedirectToAction("DetailsUser", "Utiliz", new { Id = Us.IdUtiliz });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }
        }

        public ActionResult ToggleActivation(int id)
        {
            if (CurrentUser.HasRight("UpdateUser"))
            {
                UTILIZ User = db.UTILIZ.Find(id);
                if (User != null)
                {
                    if (User.Active == true)
                    {
                        User.Active = false;
                        ViewBag.Message = "Utilisateur désactivé";
                    }
                    else if (User.Active == false)
                    {
                        User.Active = true;
                        ViewBag.Message = "Utilisateur activé";
                    }

                    db.SaveChanges(); // Save the changes to the database
                }
                else
                {
                    /* ViewBag.Message = "Utilisateur introuvable"*/
                    ;
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

        }


        public ActionResult DeleteUser(int Id)
        {
            if (CurrentUser.HasRight("DeleteUser"))
            {
                UTILIZ User = db.UTILIZ.Find(Id);
                if (User != null)
                {
                    db.UTILIZ.Remove(User);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Utiliz");
            }
            else
            {
                ViewBag.Message = "L'accès à cette ressource vous est interdit !";
                return RedirectToAction("Index");
            }

        }

        public ActionResult Crm()
        {
            return View();
        }
    }
}