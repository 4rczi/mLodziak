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
using MlodziakApp.Platforms;
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
        private readonly AndroidNotificationService _androidNotificationService;

        public MainActivity()
        {
            _androidNotificationService = IPlatformApplication.Current.Services.GetService<AndroidNotificationService>();
            _androidNotificationService.RegisterEvents();
        }

        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _androidNotificationService.HandleFCMIntent(this, Intent);         
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            _androidNotificationService.HandleFCMIntent(this, intent);
        }
    }

}
