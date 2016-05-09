using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Utility;

namespace lab.ConsoleService.Helper
{
    public class LoggerHelper
    {
        public static void WriteErrorLog(string message)
        {
            string logPath = SiteConfigurationReader.GetAppSettingsString("ErrorLogPath");
            using (StreamWriter streamWriter = new StreamWriter(logPath + @"\error.txt", true))
            {
                streamWriter.WriteLine(message);
                streamWriter.Flush();
            }

            //streamWriter.Close();
        }

        public static void WriteLog(string message)
        {
            string logPath = SiteConfigurationReader.GetAppSettingsString("LogPath");
            using (StreamWriter streamWriter = new StreamWriter(logPath + @"\log.txt", true))
            {
                streamWriter.WriteLine(DateTime.Now.ToString("F") + " : " + message);
                streamWriter.Flush();
            }

            //streamWriter.Close();
        }
    }
}
