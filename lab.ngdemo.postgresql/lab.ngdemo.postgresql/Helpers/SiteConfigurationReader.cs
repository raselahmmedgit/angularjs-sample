using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace lab.ngdemo.postgresql.Helpers
{
    public class SiteConfigurationReader
    {
        public static String SqlConnectionKey
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppDbContext").ToString();
            }
        }

        public static string GetAppSettingsString(string keyName)
        {
            try
            {
                return ConfigurationManager.AppSettings.Get(keyName);
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}