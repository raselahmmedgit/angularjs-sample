using System;
using System.Data.Entity;
using System.IO;
using lab.ngdemo.Helpers;

namespace lab.ngdemo
{
    public class BootStrapper
    {
        public static void Run()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Web.config")));
            }
            catch (Exception ex)
            {
                ExceptionHelper.Manage(ex, true);
            }

        }

    }
}