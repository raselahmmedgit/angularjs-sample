using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Helper;
using lab.ConsoleService.Schedule;
using lab.ConsoleService.Utility;

namespace lab.ConsoleService
{
    public static class BootStrapper
    {
        public static void Run()
        {
            try
            {
                AppConnection.ConnectionString = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;

                Console.WriteLine("Schedule BootStrapper Run() start: " + DateTime.Now.ToString("F"));

                ExecuteSchedule();

                Console.WriteLine("Schedule BootStrapper Run() end: " + DateTime.Now.ToString("F"));

            }
            catch (Exception ex)
            {
                ExceptionHelper.Manage(ex, true);
            }

        }
        
        private static void ExecuteSchedule()
        {
            try
            {
                bool isQuartzSchedule = SiteConfigurationReader.GetAppSettingsBoolean("IsQuartzSchedule");

                LoggerHelper.WriteLog(("Schedule BootStrapper ExecuteSchedule(): IsQuartzSchedule : " + isQuartzSchedule.ToString() + " : " + DateTime.Now.ToString("F")));

                if (isQuartzSchedule)
                {
                    LoggerHelper.WriteLog(("Schedule BootStrapper ExecuteSchedule(): IsQuartzSchedule : if " + isQuartzSchedule.ToString() + " : " + DateTime.Now.ToString("F")));

                    ExecuteQuartzSchedule();
                }
                else
                {
                    LoggerHelper.WriteLog(("Schedule BootStrapper ExecuteSchedule(): IsQuartzSchedule : else " + isQuartzSchedule.ToString() + " : " + DateTime.Now.ToString("F")));

                    ExecuteTimerSchedule();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ExecuteQuartzSchedule()
        {
            try
            {
                QuartzScheduleManager.Daily();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ExecuteTimerSchedule()
        {
            try
            {
                TimerScheduleManager timerScheduleManager = new TimerScheduleManager();
                timerScheduleManager.Daily();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
