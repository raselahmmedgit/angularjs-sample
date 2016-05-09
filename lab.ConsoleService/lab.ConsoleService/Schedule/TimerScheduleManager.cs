using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using lab.ConsoleService.Helper;
using lab.ConsoleService.Utility;

namespace lab.ConsoleService.Schedule
{
    public class TimerScheduleManager
    {
        #region Global Variable Declaration

        private System.Timers.Timer _timer = null;

        #endregion

        #region Application Daily Notification

        public void Daily()
        {
            try
            {
                bool isScheduleHourWise = SiteConfigurationReader.GetAppSettingsBoolean("IsScheduleHourWise");
                int dailyTimeInHours = SiteConfigurationReader.GetAppSettingsInteger("ScheduleDailyTimeInHours");
                double timerInterval = ConvertHoursToMilliseconds(dailyTimeInHours);

                LoggerHelper.WriteLog(("Schedule TimerScheduleManager Daily(): IsScheduleHourWise : " + isScheduleHourWise.ToString() + " : ScheduleDailyTimeInHours : " + timerInterval.ToString() + " : " + DateTime.Now.ToString("F")));

                if (isScheduleHourWise)
                {
                    _timer = new Timer();
                    this._timer.Interval = timerInterval;
                    this._timer.Elapsed += new System.Timers.ElapsedEventHandler(this._timer_Tick);
                    this._timer.Enabled = true;
                    this._timer.AutoReset = true;
                }
                else
                {

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Methods

        private void _timer_Tick(object sender, ElapsedEventArgs e)
        {
            TimerScheduleJob appNotifyMyJob = new TimerScheduleJob();
            appNotifyMyJob.Execute();


        }

        public double ConvertSecondsToMilliseconds(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
        }

        public double ConvertMinutesToMilliseconds(double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        }

        private double ConvertHoursToMilliseconds(double hours)
        {
            return TimeSpan.FromHours(hours).TotalMilliseconds;
        }

        #endregion

    }
}
