using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Messages
{
    public class SessionExpiredMessage : ValueChangedMessage<bool>
    {
        public TaskCompletionSource<bool> CompletionSource { get; }

        public SessionExpiredMessage(bool value) : base(value)
        {
            CompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
