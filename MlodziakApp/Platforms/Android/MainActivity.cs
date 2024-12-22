using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Entities;
using Firebase;
using Firebase.Messaging;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.CloudMessaging.EventArgs;
using Plugin.Firebase.CloudMessaging.Platforms.Android.Extensions;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace MlodziakApp
{
    [Activity(Theme = "@style/CustomSplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HandleFCMIntent(Intent);
            CreateNotificationChannelIfNeeded();
            CrossFirebaseCloudMessaging.Current.NotificationTapped += FCM_NotificationTapped;
            LocalNotificationCenter.Current.NotificationActionTapped += OnLocalPushNotificationTapped;
        }

        private void OnLocalPushNotificationTapped(NotificationActionEventArgs e)
        {
            var notificationId = e.Request.NotificationId;
            var customData = e.Request.ReturningData.Split(';');

            if (notificationId != 0 && !customData.IsNullOrEmpty())
            {
                WeakReferenceMessenger.Default.Send(new LocalPushNotificationTappedMessage(new LocalPushNotificationTappedMessageItem(notificationId, customData[0], customData[1])));
            }

        }

        private void FCM_NotificationTapped(object? sender, FCMNotificationTappedEventArgs e)
        {
            var notificationId = e.Notification.Data["notificationId"];
            var physicalLocationid = e.Notification.Data["physicalLocationid"];
            var creationDate = e.Notification.Data["creationDate"];

            if (!notificationId.IsNullOrEmpty() && !physicalLocationid.IsNullOrEmpty() && !creationDate.IsNullOrEmpty())
            {
                WeakReferenceMessenger.Default.Send(new FCMPushNotificationTappedMessage(new FCMPushNotificationTappedMessageItem(notificationId, physicalLocationid, creationDate)));
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleFCMIntent(intent);
        }

        private static void HandleFCMIntent(Intent intent)
        {
            FirebaseCloudMessagingImplementation.OnNewIntent(intent);
        }

        private void CreateNotificationChannelIfNeeded()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }
        }

        private void CreateNotificationChannel()
        {
            var channelId = $"{PackageName}.general";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
        }
    }

}
