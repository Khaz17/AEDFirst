using AEDFirst.Extensions;
using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEDFirst.Controllers
{
    public class LogDocController : Controller
    {

        private ModelAED db = new ModelAED();

        private UTILIZ CurrentUser
        {
            get
            {
                return (UTILIZ) Session["CurrentUser"];
            }
        }

        // GET: LogDoc
        public ActionResult Index()
        {
            if (CurrentUser.HasRight("ListLogDoc"))
            {
                List<LOGDOCS> LogsDocs = db.LOGDOCS.ToList();
                List<LogDocViewModel> Logs = new List<LogDocViewModel>();
                foreach (var log in LogsDocs)
                {
                    Logs.Add(new LogDocViewModel
                    {
                        IdLog = log.IdLog,
                        Description = log.Description,
                        DateLog = log.DateLog,
                        LogType = log.LogType,
                        ConcernedDoc = db.DOCUMENTS.Find(log.IdDoc),
                        ConcernedUser = db.UTILIZ.Find(log.IdUtiliz)
                    });
                }
                return View(Logs);
            } else
            {
                return RedirectToAction("Index", "Home", new { message = "L'accès à cette ressource vous est interdit." });
            }
            
        }
    }
}