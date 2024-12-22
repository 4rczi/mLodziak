using CommunityToolkit.Mvvm.Messaging.Messages;
using MlodziakApp.Messages.MessageItems;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    public class FCMPushNotificationTappedMessage : ValueChangedMessage<FCMPushNotificationTappedMessageItem>
    {
        public FCMPushNotificationTappedMessage(FCMPushNotificationTappedMessageItem value) : base(value)
        {
        }
    }
}
