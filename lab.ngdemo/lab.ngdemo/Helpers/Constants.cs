using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using lab.ngdemo.Helpers;
using lab.ngdemo.Models;

namespace lab.ngdemo
{
    public static class Constants
    {
        public static class CacheKey
        {
            public const string AppConstant = "AppConstant";
            public const string DefaultCacheLifeTimeInMinute = "DefaultCacheLifeTimeInMinute";
            public const string UserList = "UserList";
            public const string User = "User";
            public const string UserAdd = "UserAdd";
            public const string UserEdit = "UserEdit";
            public const string UserDelete = "UserDelete";

            public const string RoleList = "RoleList";
            public const string Role = "Role";
            public const string RoleAdd = "RoleAdd";
            public const string RoleEdit = "RoleEdit";
            public const string RoleDelete = "RoleDelete";

            public const string StudentList = "StudentList";
            public const string Student = "Student";
            public const string StudentAdd = "StudentAdd";
            public const string StudentEdit = "StudentEdit";
            public const string StudentDelete = "StudentDelete";

            public const string EmployeeList = "EmployeeList";
            public const string Employee = "Employee";
            public const string EmployeeAdd = "EmployeeAdd";
            public const string EmployeeEdit = "EmployeeEdit";
            public const string EmployeeDelete = "EmployeeDelete";
        }

        public static bool IsAuthenticated()
        {
            if (SessionHelper.Content.LoggedInUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsUserInRole(string roleName)
        {
            if (SessionHelper.Content.LoggedInUser == null)
            {
                var user = (User)SessionHelper.Content.LoggedInUser;

                var role = user.Roles.Where(item => item.RoleName == roleName);

                if (role != null)
                {
                    return true;
                }
                else
                {
                    return false; 
                }
                
            }
            else
            {
                return true;
            }
        }

        public static string GetUserName()
        {
            if (SessionHelper.Content.LoggedInUser != null)
            {
                var user = (User)SessionHelper.Content.LoggedInUser;

                if (user != null)
                {
                    return user.UserName;
                }
                else
                {
                    return string.Empty; 
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static class Messages
        {
            public const string UnhandelledError = "We are facing some problem while processing the current request. Please try again later.";
            public const string NotFound = "Requested object not found.";
            public const string SaveSuccess = "Save successfully.";
            public const string UpdateSuccess = "Update successfully.";
            public const string DeleteSuccess = "Delete successfully.";

            public static string ExceptionError(Exception exception)
            {
                return "We are facing some problem while processing the current request. Please try again later. " + exception.Message;
            }
        }
    }
}