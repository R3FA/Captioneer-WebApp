using System.Runtime.CompilerServices;

namespace UtilityService.Utils
{ 
    interface ILoggerManager
    {
        public void LogDebug(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");
        public void LogError(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");
        public void LogInfo(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");
        public void LogWarning(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "");

    }
}
