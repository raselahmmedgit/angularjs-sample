using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using lab.ngdemo.Helpers;
using lab.ngdemo.Models.CacheManagement;
using lab.ngdemo.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using lab.ngdemo.Models;

namespace lab.ngdemo.Controllers
{
    //[UserAuthorize]
    public class AccountController : Controller
    {
        private AppDbContext _db = new AppDbContext();
        private UserCacheHelper _userCacheHelper = new UserCacheHelper();
        private RoleCacheHelper _roleCacheHelper = new RoleCacheHelper();
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                lab.ngdemo.Models.User user = _userCacheHelper.GetUser(model.UserName);

                if (user != null)
                {

                    if (IsValidateUser(model.UserName, model.Password, user))
                    {
                        SessionHelper.Content.LoggedInUser = user;
                        
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The user not register, please register first.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool IsValidateUser(string userName, string password, User user)
        {
            //byte[] bytePassword = Encoding.ASCII.GetBytes(password);
            string strPassword = ASCIIEncoding.ASCII.GetString(user.PasswordHash);

            if (user.UserName == userName && password == strPassword)
            {
                return true; 
            }
            else
            {
                return false;    
            }
            
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            var registerViewModel = new RegisterViewModel { };

            return View(registerViewModel);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    byte[] password = Encoding.ASCII.GetBytes(model.Password);

                    var user = new User
                    {
                        UserName = model.UserName,
                        PasswordHash = password,
                        PasswordSalt = password,
                        Email = model.Email,
                        Roles = new List<Role>() { _roleCacheHelper.GetRole("User") }
                    };

                    _userCacheHelper.AddUser(user);

                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [UserAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [UserAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                lab.ngdemo.Models.User user = _userCacheHelper.GetUser(User.Identity.Name);

                bool changePasswordSucceeded = false;
                try
                {
                    
                    if (user != null)
                    {

                        if (IsValidateUser(user.UserName, model.OldPassword, user))
                        {
                            byte[] password = Encoding.ASCII.GetBytes(model.NewPassword);

                            user.PasswordHash = password;
                            user.PasswordSalt = password;
                            _userCacheHelper.EditUser(user);

                            changePasswordSucceeded = true;
                        }
                        else
                        {
                            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "The user not register, please register first.");
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

}