using System;
using Microsoft.Extensions.Logging;

namespace ForumProjectWebAPI.Logs
{
    public static class LogInfo
    {
        public static void LogInfoMethod(Exception exception, ILogger logger)
        {
            logger.LogInformation("Message: " + exception.Message);
            logger.LogInformation("In method: " + exception.TargetSite);
        }
    }
}