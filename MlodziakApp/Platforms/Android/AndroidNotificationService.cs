using Android.App;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Firebase.CloudMessaging.EventArgs;
using Plugin.Firebase.CloudMessaging;
using Plugin.LocalNotification.EventArgs;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using Android.Content;
using Microsoft.IdentityModel.Tokens;

namespace MlodziakApp.Platforms
{
    public class AndroidNotificationService
    {
        private void OnLocalPushNotificationTapped(NotificationActionEventArgs e)
        {
            var notificationId = e.Request.NotificationId;
            var customData = e.Request.ReturningData.Split(';');

            if (notificationId != 0 && !customData.IsNullOrEmpty())
            {
                WeakReferenceMessenger.Default.Send(new LocalPushNotificationTappedMessage(new LocalPushNotificationTappedMessageItem(notificationId, customData[0], customData[1])));
            }
        }

        private void OnFMCNotificationTapped(object? sender, FCMNotificationTappedEventArgs e)
        {
            var notificationId = e.Notification.Data["notificationId"];
            var physicalLocationid = e.Notification.Data["physicalLocationid"];
            var creationDate = e.Notification.Data["creationDate"];

            if (!notificationId.IsNullOrEmpty() && !physicalLocationid.IsNullOrEmpty() && !creationDate.IsNullOrEmpty())
            {
                WeakReferenceMessenger.Default.Send(new FCMPushNotificationTappedMessage(new FCMPushNotificationTappedMessageItem(notificationId, physicalLocationid, creationDate)));
            }
        }

        public void RegisterEvents()
        {
            CrossFirebaseCloudMessaging.Current.NotificationTapped += OnFMCNotificationTapped;
            LocalNotificationCenter.Current.NotificationActionTapped += OnLocalPushNotificationTapped;
        }

        public void CreateFCMNotificationChannel(Context context)
        {
            var channelId = $"MlodziakApp.general";
            var notificationManager = (NotificationManager)context.GetSystemService("notification");
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
        }

        public void HandleFCMIntent(Context context, Intent intent)
        {
            FirebaseCloudMessagingImplementation.OnNewIntent(intent);
            CreateFCMNotificationChannel(context);
        }
    }
}
