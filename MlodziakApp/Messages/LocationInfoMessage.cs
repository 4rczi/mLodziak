using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using MlodziakApp.Messages.MessageItems;

    public class LocationInfoMessage : ValueChangedMessage<LocationInfoMessageItem>
    {
        public LocationInfoMessage(LocationInfoMessageItem value) : base(value)
        { 
        }
    }
}
