using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiCalls
{
    public interface INotificationApiCalls
    {
        [Post("/api/notification/")]
        Task<string> SendNotificationAsync([Body] NotificationRequestModel notificationRequest, [Header("Authorization")] string accessToken);
    }
}
