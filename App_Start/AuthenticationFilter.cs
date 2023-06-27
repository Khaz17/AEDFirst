using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace AEDFirst.App_Start
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        filterContext.Result = new RedirectResult("~/Utiliz/Login");
        //    }
        //}
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["CurrentUser"])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller  
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Utiliz" },
                     { "action", "Login" }
                });
            }
        }
    }
}