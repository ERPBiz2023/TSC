using System;
using System.IO;
using System.Linq;


namespace eDSC
{
    public class Logger
    {

        public enum EventType
        {
            None = 0,
            Event = 1,
            Warning = 2,
            Error = 3,
            Exception = 4
        }

        private static int daysHistory;
        private static string logPath;
        private static string logName;
        private static DateTime currentDate;

        private static int symbolRepeat = 80;

        public static void StartLog(int daysToKeepLogs, bool isConsole = false)
        {

            try
            {
                currentDate = DateTime.Today;
                daysHistory = daysToKeepLogs;

                logPath = string.Format(@"{0}\{1}\logs", AssemblyInfo.appPath, AssemblyInfo.appName);

                if (Directory.Exists(logPath) == false)
                    Directory.CreateDirectory(logPath);

                logName = string.Format(@"{0}\{1}.log", logPath, DateTime.Today.ToString("yyyyMMdd"));

                WriteLog(new string('*', symbolRepeat), isConsole);
                WriteLog(string.Format("{0} version {1} has started", AssemblyInfo.appName, AssemblyInfo.appVersion), isConsole);
                
                DeleteOldLogs();
            }
            catch { }
        }

        public static void EndLog(bool isConsole = false)
        {
            try {

                WriteLog(string.Format("{0} version {1} has ended", AssemblyInfo.appName, AssemblyInfo.appVersion), isConsole);
                WriteLog(new string('*', symbolRepeat), isConsole);
            }
            catch
            {
            }
        }

        public static void LogException(Exception e, bool isConsole = false)
        {

            try {
                string dateAndTime = DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH:mm:ss") + " ";

                WriteLog(new string('-', symbolRepeat), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception logging", dateAndTime), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception Type = {1}", dateAndTime, e.GetType().ToString()), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception Message = {1}", dateAndTime, e.Message), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception Source = {1}", dateAndTime, e.Source), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception Target Site = {1}", dateAndTime, e.TargetSite), isConsole, EventType.Exception);
                WriteLog(string.Format("{0}Exception Stacktrace = {1}", dateAndTime, e.StackTrace), isConsole, EventType.Exception);
                WriteLog(new string('-', symbolRepeat), isConsole, EventType.Exception);
            }
            catch
            {
            }
        }

        public static void LogEvent(string message, EventType eventType, bool isConsole = false)
        {
            try
            {
                WriteLog(message, isConsole, eventType);
            }
            catch
            {
            }
        }

        private static void WriteLog(string s, bool isConsole, EventType eventType = EventType.None)
        {
            try {
                if (currentDate != DateTime.Today)
                {
                    currentDate = DateTime.Today;
                    DeleteOldLogs();
                }

                string dateAndTime = "";

                if (!s.StartsWith("-") && !s.StartsWith("*"))
                    dateAndTime = DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH:mm:ss") + " ";

                if (!s.EndsWith(".") && !s.StartsWith("-") && !s.StartsWith("*"))
                    s += ".";

                string type = "";

                using (StreamWriter sw = new StreamWriter(logName, true))
                {
                    if (eventType != EventType.None)
                        type = string.Format("{0} - ", eventType.ToString());

                    string message = string.Format("{0}{1}{2}", dateAndTime, type, s);
                    sw.WriteLine(message);

                    if (isConsole)
                    {
                        switch (eventType)
                        {
                            case EventType.Event:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;

                            case EventType.Warning:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;

                            case EventType.Error:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;

                            case EventType.Exception:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;

                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;

                        }

                        Console.WriteLine(message);
                    }
                    sw.Close();
                }
            }
            catch
            {
            }
        }

        private static void DeleteOldLogs()
        {
            try
            {

                if (Directory.Exists(logPath))
                {

                    string[] listLogs = Directory.GetFiles(logPath, "*.log");

                    foreach (string log in listLogs)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(log);

                        try
                        {

                            int yyyy = int.Parse(fileName.Substring(0, 4));
                            int mm = int.Parse(fileName.Substring(4, 2));
                            int dd = int.Parse(fileName.Substring(6, 2));

                            DateTime checkDate = new DateTime(yyyy, mm, dd);
                            DateTime tempDate = currentDate.AddDays(-1 * daysHistory);

                            if (checkDate <= tempDate)
                                File.Delete(log);

                        }
                        catch { }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}