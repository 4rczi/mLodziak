using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    public class RefreshExplorationPageUIMessage : ValueChangedMessage<bool>
    {
        public RefreshExplorationPageUIMessage(bool value) : base(value)
        {
        }
    }
}
