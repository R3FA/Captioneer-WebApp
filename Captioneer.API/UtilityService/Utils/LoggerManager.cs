using NLog;
using System.Runtime.CompilerServices;

namespace UtilityService.Utils
{
    public class LoggerManager : ILoggerManager
    {
        private static LoggerManager instance;
        private static Logger logger;

        // Private constructor for singleton
        private LoggerManager()
        {

        }

        public static void LoadConfiguration()
        {
            LogManager.LoadConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        }

        public static LoggerManager GetInstance()
        {
            if (LoggerManager.instance == null)
                LoggerManager.instance = new LoggerManager();

            return LoggerManager.instance;
        }

        private Logger GetLogger()
        {
            if (LoggerManager.logger == null)
                LoggerManager.logger = LogManager.GetCurrentClassLogger();

            return LoggerManager.logger;
        }

        public void LogDebug(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            GetLogger().Debug($"{caller} | {message}");
        }
        public void LogError(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            GetLogger().Error($"{caller} | {message}");
        }
        public void LogInfo(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            GetLogger().Info($"{caller} | {message}");
        }
        public void LogWarning(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            GetLogger().Warn($"{caller} | {message}");
        }
    }
}
