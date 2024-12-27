using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages.MessageItems
{
    public class FCMPushNotificationPendingMessageItem
    {
        public FCMPushNotificationTappedMessageItem _FCMPushNotificationTappedMessageItem { get; set; }

        public FCMPushNotificationPendingMessageItem(FCMPushNotificationTappedMessageItem FCMPushNotificationTappedMessageItem)
        {
            _FCMPushNotificationTappedMessageItem = FCMPushNotificationTappedMessageItem;
        }
    }
}
