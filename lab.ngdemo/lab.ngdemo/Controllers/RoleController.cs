using lab.ngdemo.Helpers;
using lab.ngdemo.Models;
using lab.ngdemo.Models.CacheManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab.ngdemo.Controllers
{
    //[UserAuthorize]
    [Authorize]
    public class RoleController : Controller//BaseController
    {
        private RoleCacheHelper _roleCacheHelper = new RoleCacheHelper();

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListAjax()
        {
            var list = _roleCacheHelper.GetRoles;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByIdAjax(string roleName)
        {
            var role = _roleCacheHelper.GetRole(roleName);
            return Json(role, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAjax(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isExistRole = _roleCacheHelper.GetRole(role.RoleName);
                    if (isExistRole != null)
                    {
                        isExistRole.RoleName = role.RoleName;
                        _roleCacheHelper.EditRole(isExistRole);
                        role.IsSuccess = true;
                        role.SuccessMessage = Constants.Messages.UpdateSuccess;
                    }
                    else
                    {
                        _roleCacheHelper.AddRole(role);
                        role.IsSuccess = true;
                        role.SuccessMessage = Constants.Messages.SaveSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                role.IsSuccess = false;
                role.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }
            
            return Json(role, JsonRequestBehavior.DenyGet);

        }

        [HttpPost]
        public ActionResult DeleteAjax(string roleName)
        {
            var role = new Role();
            try
            {
                role = _roleCacheHelper.GetRole(roleName);
                if (role != null)
                {
                    _roleCacheHelper.DeleteRole(role);
                    role.IsSuccess = true;
                    role.SuccessMessage = Constants.Messages.UpdateSuccess;
                }
                else
                {
                    role.IsSuccess = false;
                    role.SuccessMessage = Constants.Messages.NotFound;
                }
            }
            catch (Exception ex)
            {
                role.IsSuccess = false;
                role.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }

            return Json(role, JsonRequestBehavior.DenyGet);

        }
    }
}