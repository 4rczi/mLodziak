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

namespace MlodziakApp.Platforms.Android
{
    [Activity(Theme = "@style/CustomSplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        private readonly AndroidNotificationService _androidNotificationService;
        private readonly AndroidForegroundService _androidForegroundService;


        public MainActivity()
        {
            _androidNotificationService = IPlatformApplication.Current.Services.GetService<AndroidNotificationService>()
                ?? throw new ArgumentNullException(nameof(AndroidNotificationService), "AndroidNotificationService is not registered in the service provider.");

            _androidNotificationService.RegisterEvents();
            _androidForegroundService = IPlatformApplication.Current.Services.GetService<AndroidForegroundService>()
                ?? throw new ArgumentNullException(nameof(AndroidForegroundService), "AndroidForegroundService is not registered in the service provider.");
        }

        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _androidNotificationService.HandleFCMIntent(this, Intent);

        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            string inputExtra = intent.GetStringExtra("inputExtra");

            _androidNotificationService.HandleFCMIntent(this, intent);
        }
    }

}
