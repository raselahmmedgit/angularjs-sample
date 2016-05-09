using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.ConsoleService.Utility
{
    public static class SiteConfigurationReader
    {
        private static string databaseConnectionString = string.Empty;
        public static string DatabaseConnectionString
        {
            get
            {
                if (databaseConnectionString == null)
                {
                    databaseConnectionString = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;
                }
                return databaseConnectionString;
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

        public static Boolean GetAppSettingsBoolean(string keyName)
        {
            try
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get(keyName));
            }
            catch
            {
                return false;
            }
        }

        public static Int32 GetAppSettingsInteger(string keyName)
        {
            try
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get(keyName));
            }
            catch
            {
                return 0;
            }
        }

    }
}
