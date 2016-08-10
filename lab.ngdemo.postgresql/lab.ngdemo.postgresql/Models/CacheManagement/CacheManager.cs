using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using lab.ngdemo.postgresql.Helpers;

namespace lab.ngdemo.postgresql.Models.CacheManagement
{
    public class CacheManager : ICacheManager
    {
        static CacheManager _iCache;
        private ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public static CacheManager ICache
        {
            get
            {
                return (_iCache == null ? (_iCache = new CacheManager()) : _iCache);
            }
        }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        public void Set(string key, object data)
        {
            if (data == null)
            {
                return;
            }
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(Convert.ToDouble(SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute)));
            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }

    public interface ICacheManager
    {
        T Get<T>(string key);
        object Get(string key);
        void Set(string key, object data, int cacheTime);
        void Set(string key, object data);
        bool IsSet(string key);
        void Remove(string key);
        void Clear();
    }
}