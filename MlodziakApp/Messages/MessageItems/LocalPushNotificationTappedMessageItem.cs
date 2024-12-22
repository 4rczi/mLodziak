using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages.MessageItems
{
    public class LocalPushNotificationTappedMessageItem
    {
        public int NotificationId { get; set; }
        public string PhysicalLocationId { get; set; }
        public string CreationDate { get; set; }

        public LocalPushNotificationTappedMessageItem(int notificationId, string physicalLocationId, string creationDate)
        {
            NotificationId = notificationId;
            PhysicalLocationId = physicalLocationId;
            CreationDate = creationDate;
        }
    }
}
