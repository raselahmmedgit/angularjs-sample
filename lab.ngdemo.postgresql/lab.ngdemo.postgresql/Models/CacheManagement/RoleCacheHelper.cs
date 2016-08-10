using lab.ngdemo.postgresql.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.postgresql.Models.CacheManagement
{
    public class RoleCacheHelper
    {
        public CacheManager Cache { get; set; }

        public RoleCacheHelper()
        {
            Cache = new CacheManager();
        }
        public List<Role> GetRoles
        {
            get
            {
                List<Role> _roleList = new List<Role>
                            {
                                new Role {RoleName = "Admin"},
                                new Role {RoleName = "User"}
                            };
                string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
                string cacheKey = Constants.CacheKey.RoleList + appConstant;
                if (!CacheManager.ICache.IsSet(cacheKey))
                {
                    CacheManager.ICache.Set(cacheKey, _roleList);
                }
                else
                {
                    _roleList = CacheManager.ICache.Get(cacheKey) as List<Role>;
                }

                return _roleList;
            }
        }

        public Role GetRole(string roleName)
        {
            var role = new Role();
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            string cacheKey = Constants.CacheKey.Role + appConstant;
            if (!CacheManager.ICache.IsSet(cacheKey))
            {
                role = GetRoles.FirstOrDefault(item => item.RoleName == roleName);
                CacheManager.ICache.Set(cacheKey, role);
            }
            else
            {
                role = CacheManager.ICache.Get(cacheKey) as Role;
            }
            return role;
        }

        public void AddRole(Role role)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.RoleAdd + appConstant;

            List<Role> _roleList = new List<Role>();
            _roleList = GetRoles.ToList();
            _roleList.Add(role);

            string cacheKeyList = Constants.CacheKey.RoleList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }

        }

        public void EditRole(Role role)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.RoleEdit + appConstant;

            List<Role> _roleList = new List<Role>();
            _roleList = GetRoles.ToList();
            var editRole = _roleList.FirstOrDefault(item => item.RoleName == role.RoleName);
            editRole = role;

            string cacheKeyList = Constants.CacheKey.RoleList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }

        }

        public void DeleteRole(Role role)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.RoleDelete + appConstant;

            List<Role> _roleList = new List<Role>();
            _roleList = GetRoles.ToList();
            var roleRemove = _roleList.FirstOrDefault(item => item.RoleName == role.RoleName);
            _roleList.Remove(roleRemove);

            string cacheKeyList = Constants.CacheKey.RoleList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _roleList);
            }

        }
    }
}