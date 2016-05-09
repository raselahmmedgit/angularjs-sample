using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.ConsoleService.Helper
{
    public static class ExceptionHelper
    {
        public static string Manage(Exception ex, bool log = false)
        {
            string message = "Error: " + ex.Message;

            if (ex.InnerException != null)
            {
                Exception inner = ex.InnerException;
                if (inner is System.Data.Common.DbException)
                    message = "Database is currently experiencing problems. " + inner.Message;
                else if (inner is NullReferenceException)
                    message = "There are one or more required fields that are missing.";
                else if (inner is ArgumentException)
                {
                    string paramName = ((ArgumentException)inner).ParamName;
                    message = string.Concat("The ", paramName, " value is illegal.");
                }
                else if (inner is ApplicationException)
                    message = "Exception in application" + inner.Message;
                else
                    message = inner.Message;

            }

            if (log)
            {
                Logger(ex);
            }

            return message;
        }

        public static void Logger(Exception exception)
        {
            LoggerHelper.WriteErrorLog(exception.Message);
        }

        public static string Format(object oSource, string nCode, string sMessage, string messageToUser, string systemDefinedMessage, Exception oInnerException)
        {
            StringBuilder sNewMessage = new StringBuilder();
            string sErrorStack = null;
            sErrorStack = BuildErrorStack(oInnerException);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append(DateTime.Now.ToString("F"));
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Exception Summary :");
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Error Message :");
            sNewMessage.Append(sMessage);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Message To User :");
            sNewMessage.Append(messageToUser);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("System Defined Message :");
            sNewMessage.Append(systemDefinedMessage);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Machine Name :");
            sNewMessage.Append(Environment.MachineName);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Domain Name :");
            sNewMessage.Append(System.AppDomain.CurrentDomain.FriendlyName.ToString());
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("Host Name :");
            sNewMessage.Append(System.Net.Dns.GetHostName().ToString());
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append("OS Version :");
            sNewMessage.Append(Environment.OSVersion);
            sNewMessage.Append(Environment.NewLine);
            sNewMessage.Append(sErrorStack);
            return sNewMessage.ToString().Trim();
        }

        public static string BuildErrorStack(Exception oChainedException)
        {
            string sErrorStack = null;
            StringBuilder sbErrorStack = new StringBuilder();
            int nErrStackNum = 1;
            System.Exception oInnerException = null;
            if (oChainedException != null)
            {
                sbErrorStack.Append("Error Stack ");
                //.Append("------------------------\n\n");
                oInnerException = oChainedException;
                while (oInnerException != null)
                {
                    sbErrorStack.Append(nErrStackNum)
                    .AppendLine(")\n ");
                    sbErrorStack.Append(oInnerException.Message)
                    .AppendLine("\n");
                    oInnerException =
                    oInnerException.InnerException;
                    nErrStackNum++;
                }
                sbErrorStack.Append("\n");
                sbErrorStack.AppendLine("Call Stack\n");
                sbErrorStack.Append(oChainedException.StackTrace);
                sErrorStack = sbErrorStack.ToString();
            }
            else
            {
                sErrorStack = "exception was not chained";
            }

            return sErrorStack;
        }

    }
}
