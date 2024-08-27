using NLog;

namespace AccountManagementPortal.Repositories;
public static class LogRepository
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void LogInfo(string Message)
    {
        Logger.Info(Message);
    }

    public static void LogError(Exception ex)
    {
        Logger.Error(ex, "An error occurred.");
    }
}
