using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public static class logRepository
    {

        //const string LOG_FILE_NAME = "TestLog.txt";

        const string LOG_FOLDER_NAME = "Logs";
        static string enableLog = ConfigurationManager.AppSettings["EnableErrorLog"].ToString();

        public static void ErrorLog(Exception exception, string module, string action, string message, string innerException, bool createSystemLog, EventLogEntryType? LogType, string currentUserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(enableLog) && enableLog == "1")
                {
                    string logTypeString = LogType != null ? LogType.ToString() : "Error";
                    message = message == null ? string.Empty : message;
                    innerException = innerException == null ? string.Empty : innerException;
                    string FullMessage = logTypeString + " Message : " + message + " | Inner Exception : " + innerException;
                    //Logs Base Directory
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + LOG_FOLDER_NAME;

                    if (!Directory.Exists(baseDirectory))
                    {
                        Directory.CreateDirectory(baseDirectory);
                    }

                    //New Log File by Date
                    string path = baseDirectory + "\\" + DateTime.Today.ToString("MM-dd-yy") + ".txt"; //"~/" + LOG_FOLDER_NAME

                    //Build New Log
                    StringBuilder sb = new StringBuilder();
                    sb.Append(string.Format("{0} | Module : {1} | Action : {2}", DateTime.Now.ToString(CultureInfo.InvariantCulture), module, action));
                    sb.Append(Environment.NewLine);
                    sb.Append(FullMessage);
                    sb.Append(Environment.NewLine);

                    sb.Append("ExceptionDetail : " + exceptionDetail(exception, currentUserID, module + "/" + action));

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("_____________________________________________________");
                    sb.Append(Environment.NewLine);
                    //Append Log In Log File
                    File.AppendAllText(path, sb.ToString());

                    //Create System Event Log
                    if (createSystemLog && LogType != null)
                    {
                        WriteToEventLog(FullMessage, "Error", (EventLogEntryType)LogType);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            // File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_FOLDER_NAME + "\\" + LOG_FILE_NAME, sb.ToString());

        }


        public static void WriteToEventLog(string message, string msgType, EventLogEntryType logEntryType)
        {
            try
            {
                string cs = "TCS External Verification";
                string LogName = "TCSExternalVerificationV1";
                EventLog elog = new EventLog();
                if (!EventLog.SourceExists(cs))
                {
                    EventLog.CreateEventSource(cs, LogName);
                }
                elog.Source = cs;
                elog.Log = LogName;

                elog.EnableRaisingEvents = true;
                elog.WriteEntry(message, logEntryType);

            }
            catch (Exception ex)
            {

            }
        }



        private static string exceptionDetail(Exception ex, string LoginID, string FilePath)
        {
            StackTrace ST = new StackTrace(ex, true);
            StackFrame Frame = ST.GetFrame(0);
            string errorInfo = string.Empty;

            if (Frame != null && ex.TargetSite != null)
            {
                string MemberName = ex.TargetSite.ToString();
                string MethodBase = ex.TargetSite.Name;
                string MemberType = ex.TargetSite.MemberType.ToString();
                string ClassName = ex.TargetSite.DeclaringType.ToString();
                string ExceptionMsg = ex.Message;
                string Source = ex.Source;
                string Trace = ex.StackTrace;
                int ErrorLineNumber = GetLineNumber(ex);
                int ThrowLineNumber = Frame.GetFileLineNumber();
                string ColumnNumber = Frame.GetFileColumnNumber().ToString();
                string HelpLink = ex.HelpLink;

                errorInfo =
                Environment.NewLine + "  [Login ID]: " + LoginID.ToString() +
                Environment.NewLine + "  [Member Name]: " + MemberName +
                Environment.NewLine + "  [Method Base]: " + MethodBase +
                Environment.NewLine + "  [Member Type]: " + MemberType +
                Environment.NewLine + "  [File Path]: " + FilePath +
                Environment.NewLine + "  [Class Name]: " + ClassName +
                Environment.NewLine + "  [Exception Msg]: " + ExceptionMsg +
                Environment.NewLine + "  [Source]: " + Source +
                Environment.NewLine + "  [Trace]: " + Trace +
                Environment.NewLine + "  [Error Line Number]: " + ErrorLineNumber +
                Environment.NewLine + "  [Throw Line Number]: " + ThrowLineNumber +
                Environment.NewLine + "  [Column Number]: " + ColumnNumber +
                Environment.NewLine + "  [Help Link]: " + HelpLink;
            }
            else
            {
                errorInfo = Environment.NewLine + "  [Login ID]: " + LoginID.ToString();
            }
            return errorInfo;
        }


        private static int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                string[] numbers = Regex.Split(lineNumberText, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        lineNumber = int.Parse(value);
                    }
                }
            }
            return lineNumber;
        }

      


    }
}
