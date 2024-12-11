using CommunityToolkit.Mvvm.Messaging.Messages;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    public class VisitedPhysicalLocationMessage : ValueChangedMessage<PhysicalLocationModel>
    {
        public VisitedPhysicalLocationMessage(PhysicalLocationModel value) : base(value)
        {
        }
    }
}
