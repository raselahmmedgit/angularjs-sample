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

    }
}