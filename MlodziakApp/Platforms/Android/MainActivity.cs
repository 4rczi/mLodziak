using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.Content;
using Firebase;
using Plugin.Firebase.CloudMessaging;
using Plugin.LocalNotification;

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
        }

        public void OpenNotificationSettings()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionAppNotificationSettings);
            intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, PackageName);
            StartActivity(intent);
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
            //FirebaseCloudMessagingImplementation.SmallIconRef = Resource.Drawable.ic_push_small;
        }
    }

}
