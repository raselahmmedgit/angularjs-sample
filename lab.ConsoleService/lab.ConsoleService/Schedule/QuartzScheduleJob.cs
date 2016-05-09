using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Helper;
using Quartz;

namespace lab.ConsoleService.Schedule
{
    public class QuartzScheduleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                LoggerHelper.WriteLog(("Quartz Schedule Execute Start: " + DateTime.Now.ToString("F")));

                TestSchedule();

                LoggerHelper.WriteLog(("TestSchedule() - " + DateTime.Now.ToString("F")));

                LoggerHelper.WriteLog(("Quartz Schedule Execute End: " + DateTime.Now.ToString("F")));

            }
            catch (Exception ex)
            {
                ExceptionHelper.Manage(ex, true);
            }

        }

        #region Test

        private void TestSchedule()
        {
            try
            {
                ScheduleHelper scheduleHelper = new ScheduleHelper();
                scheduleHelper.TestSchedule();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
