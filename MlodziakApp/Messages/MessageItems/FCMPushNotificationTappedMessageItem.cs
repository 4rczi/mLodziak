using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages.MessageItems
{
    public class FCMPushNotificationTappedMessageItem
    {
        public string NotificationId { get; set; }
        public string PhysicalLocationId { get; set; }
        public string CreationDate { get; set; }

        public FCMPushNotificationTappedMessageItem(string notificationId, string physicalLocationId, string creationDate)
        {
            NotificationId = notificationId;
            PhysicalLocationId = physicalLocationId;
            CreationDate = creationDate;
        }
    }
}
