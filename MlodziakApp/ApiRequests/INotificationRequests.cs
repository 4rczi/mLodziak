using SharedModels;

namespace MlodziakApp.ApiRequests
{
    public interface INotificationRequests
    {
        Task<bool> SendFCMNotificationMessageRequestAsync(string? accessToken, NotificationRequestModel notificationRequest);
    }
}