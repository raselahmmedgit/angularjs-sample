using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using lab.ngdemo.Helpers;

namespace lab.ngdemo
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Get Current Action, Controller, Area
            string currentActionName = string.Empty;
            string currentControllerName = string.Empty;
            string currentAreaName = string.Empty;

            object objCurrentControllerName;
            this.RouteData.Values.TryGetValue("controller", out objCurrentControllerName);

            object objCurrentActionName;
            this.RouteData.Values.TryGetValue("action", out objCurrentActionName);

            if (this.RouteData.DataTokens.ContainsKey("area"))
            {
                currentAreaName = this.RouteData.DataTokens["area"].ToString();
            }
            if (objCurrentActionName != null)
            {
                currentActionName = objCurrentActionName.ToString();
            }
            if (objCurrentControllerName != null)
            {
                currentControllerName = objCurrentControllerName.ToString();
            }
            #endregion

            if (filterContext != null)
            {
                HttpSessionStateBase httpSessionStateBase = filterContext.HttpContext.Session;
                if (httpSessionStateBase != null)
                {
                    if (!CheckIfUserIsAuthenticated(filterContext))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.HttpContext.Response.StatusCode = 403;
                            filterContext.Result = new JsonResult
                            {
                                Data = new
                                {
                                    // put whatever data you want which will be sent
                                    // to the client
                                    message = MessageResourceHelper.UnAuthenticated
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                        else
                        {
                            if (filterContext.HttpContext.Request.Url != null)
                            {
                                string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);

                                filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + redirectUrl, true);
                            }
                        }
                    }
                    else
                    {

                        if (!CheckIfUserAccessRight(currentActionName, currentControllerName, currentAreaName))
                        {
                            if (filterContext.HttpContext.Request.IsAjaxRequest())
                            {
                                filterContext.HttpContext.Response.StatusCode = 403;
                                filterContext.Result = new JsonResult
                                {
                                    Data = new
                                    {
                                        // put whatever data you want which will be sent
                                        // to the client
                                        message = MessageResourceHelper.UnAuthenticated
                                    },
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                            else
                            {
                                if (filterContext.HttpContext.Request.Url != null)
                                {
                                    string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);

                                    filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + redirectUrl, true);
                                }
                            }
                        }
                        else
                        {
                            base.OnActionExecuting(filterContext);
                        }
                    }
                }
            }
        }

        #region Check User Authenticated
        private bool CheckIfUserIsAuthenticated(ActionExecutingContext filterContext)
        {
            // If Result is null, we’re OK: the user is authenticated and authorized. 
            if (filterContext.Result == null)
            {
                return true;
            }

            // If here, you’re getting an HTTP 401 status code. In particular,
            // filterContext.Result is of HttpUnauthorizedResult type. Check Ajax here. 
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfUserAccessRight(string actionName, string controllerName, string areaName)
        {
            return true;
        }
        #endregion

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

    }
}