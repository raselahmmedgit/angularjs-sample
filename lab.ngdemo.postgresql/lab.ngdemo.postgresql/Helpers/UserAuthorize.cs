using lab.ngdemo.postgresql.Models;
using lab.ngdemo.postgresql.Models.CacheManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab.ngdemo.postgresql.Helpers
{
    public class UserAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    User loggedInUser = SessionHelper.Content.LoggedInUser;
                    if (loggedInUser == null)
                    {
                        filterContext.Result = new RedirectResult("/LogOn");
                    }
                    if (loggedInUser != null)
                    {
                        filterContext.Result = CreateResult(filterContext);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/LogOn");
                    }
                }
                base.OnAuthorization(filterContext);
            }
        }
        
        protected ActionResult CreateResult(AuthorizationContext filterContext)
        {
            var viewName = "~/Views/Shared/AccessDenied.cshtml";
            return new PartialViewResult
            {
                ViewName = viewName
            };
        }
    }
}