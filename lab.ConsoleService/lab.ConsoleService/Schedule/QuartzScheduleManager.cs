using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Helper;
using lab.ConsoleService.Utility;
using Quartz;
using Quartz.Impl;

namespace lab.ConsoleService.Schedule
{
    public static class QuartzScheduleManager
    {
        #region Global Variable Declaration

        static readonly IScheduler _iScheduler = StdSchedulerFactory.GetDefaultScheduler();

        #endregion

        #region Application Daily Notification

        public static void Daily()
        {
            try
            {
                // Start the scheduler if its in standby
                if (!_iScheduler.IsStarted)
                    _iScheduler.Start();

                bool isScheduleHourWise = SiteConfigurationReader.GetAppSettingsBoolean("IsScheduleHourWise");
                int dailyTimeInHours = SiteConfigurationReader.GetAppSettingsInteger("ScheduleDailyTimeInHours");

                int dailyHour = SiteConfigurationReader.GetAppSettingsInteger("ScheduleDailyHour");
                int dailyMinute = SiteConfigurationReader.GetAppSettingsInteger("ScheduleDailyMinute");

                LoggerHelper.WriteLog(("Schedule QuartzScheduleManager Daily(): IsScheduleHourWise : " + isScheduleHourWise.ToString() + " : ScheduleDailyTimeInHours : " + dailyTimeInHours.ToString() + " : " + DateTime.Now.ToString("F")));

                IJobDetail job = JobBuilder.Create<QuartzScheduleJob>()
                    .WithIdentity("appNotifyJobDaily", "appNotifyGroupDaily")
                    .Build();

                if (isScheduleHourWise)
                {
                    //Every 1 Hours Later
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("appNotifyTriggerDailyInHours", "appNotifyGroupDailyInHours")
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(dailyTimeInHours).RepeatForever()).Build();

                    //ITrigger trigger = TriggerBuilder.Create()
                    //    .WithIdentity("appNotifyTriggerDailyInHours", "appNotifyGroupDailyInHours")
                    //    .StartNow()
                    //    .WithSimpleSchedule(x => x.WithIntervalInHours(dailyTimeInHours).RepeatForever()).Build();

                    //Validate that the job doesn't already exists
                    CheckExitsJob();

                    _iScheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    //Every Day Time
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("appNotifyTriggerDaily", "appNotifyGroupDaily").StartNow().WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(dailyHour, dailyMinute)).Build();

                    //Validate that the job doesn't already exists
                    CheckExitsJob();

                    _iScheduler.ScheduleJob(job, trigger);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Application Weekly Notification

        public static void Weekly()
        {
            try
            {
                _iScheduler.Start();

                IJobDetail job = JobBuilder.Create<QuartzScheduleJob>()
                    .WithIdentity("appNotifyJobWeekly", "appNotifyGroupWeekly")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("appNotifyTriggerWeekly", "appNotifyGroupWeekly").StartNow().WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(5, 30))
                    .Build();

                _iScheduler.ScheduleJob(job, trigger);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Check Exit Job

        private static void CheckExitsJob()
        {
            if (_iScheduler.CheckExists(new JobKey("appNotifyJobDaily", "appNotifyGroupDaily")))
            {
                _iScheduler.DeleteJob(new JobKey("appNotifyJobDaily", "appNotifyGroupDaily"));
            }
            else if (_iScheduler.CheckExists(new TriggerKey("appNotifyTriggerDailyInHours", "appNotifyGroupDailyInHours")))
            {

            }
            else if (_iScheduler.CheckExists(new TriggerKey("appNotifyTriggerDaily", "appNotifyGroupDaily")))
            {

            }
        }

        #endregion

    }
}
