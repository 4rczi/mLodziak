using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace MlodziakApp.Logic.Notification
{
    public static class NotificationRequestMapper
    {
        public static NotificationRequest ToLocalNotification(this NotificationRequestModel notificationRequest)
        {


            return new NotificationRequest
            {
                Title = notificationRequest.Title,
                CategoryType = NotificationCategoryType.Event,
                NotificationId = int.Parse(notificationRequest.NotificationId),
                ReturningData = $"physicalLocationId={notificationRequest.PhysicalLocationId};creationDate={notificationRequest.CreationDate}"


            };
        }
    }
}
