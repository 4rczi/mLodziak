using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Notification
{
    public interface INotificationHandler
    {
        Task<bool> CanSendNotificationAsync();
        Task<bool> SendNotificationAsync(NotificationRequestModel notificationRequest);
        Task<NotificationRequestModel> CreateNotificationRequestAsync(PhysicalLocationModel physicalLocationModel);
    }
}
