namespace MlodziakApp.ApiRequests
{
    public interface IApplicationLoggingRequests
    {
        Task LogAsync(string level, string message, string logger, string exception, string className, string methodName, string userId, string sessionId, string customMessage, DateTime createdDate, DateTime modifiedDate);
    }
}