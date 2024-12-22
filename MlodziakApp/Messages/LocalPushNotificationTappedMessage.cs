using CommunityToolkit.Mvvm.Messaging.Messages;
using MlodziakApp.Messages.MessageItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    public class LocalPushNotificationTappedMessage : ValueChangedMessage<LocalPushNotificationTappedMessageItem>
    {
        public LocalPushNotificationTappedMessage(LocalPushNotificationTappedMessageItem value) : base(value)
        {
        }
    }
}
