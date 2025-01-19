using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using MlodziakApp.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Platforms
{
    [Service(Enabled = true, Exported = true, ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeLocation)]
    public class AndroidForegroundService : Service, IAndroidForegroundService
    {
        private const int SERVICE_RUNNING_NOTIFICATION_ID = 10001;
        private const string FOREIGN_CHANNEL_ID = "9001";
        private Context _context = global::Android.App.Application.Context;
        private CancellationTokenSource _cts;


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public void SetContext(Context context)
        {
            _context = context;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Notification notification = GetServiceStartedNotification();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

            Task.Run(() => PerformBackgroundWorkAsync(_cts.Token));


            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }

            base.OnDestroy();
        }

        private async Task PerformBackgroundWorkAsync(CancellationToken cancellationToken)
        {
            try
            {
                var rand = new Random();

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Still active: " + rand.Next());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in background work: {ex.Message}");
            }
        }


        public Notification GetServiceStartedNotification()
        {
            var intent = new Intent(_context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(_context, 0, intent, PendingIntentFlags.Immutable);

            var notificationBuilder = new NotificationCompat.Builder(_context, FOREIGN_CHANNEL_ID)
                .SetContentTitle("mŁodziak - Twoja lokalizacja jest w użyciu")
                .SetSmallIcon(Resource.Drawable.application_logo_01)
                .SetOngoing(true)
                .SetAutoCancel(false)
                .SetContentIntent(pendingIntent);

            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(FOREIGN_CHANNEL_ID, "Title", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300 });

                var notificationManager = _context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notificationManager != null)
                {
                    notificationBuilder.SetChannelId(FOREIGN_CHANNEL_ID);
                    notificationBuilder.SetOngoing(true);
                    notificationManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notificationBuilder.Build();
        }

        public bool IsServiceRunning()
        {
            var activityManager = (ActivityManager)_context.GetSystemService(Context.ActivityService);

            foreach (var service in activityManager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName == Java.Lang.Class.FromType(typeof(AndroidForegroundService)).Name)
                {
                    return true;
                }
            }

            return false;
        }


        public void Start()
        {
            var serviceIntent = new Intent(_context, typeof(AndroidForegroundService));
            serviceIntent.PutExtra("inputExtra", "Foreground Service");

            if (!IsServiceRunning())
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    _context.StartForegroundService(serviceIntent);
                }
                else
                {
                    _context.StartService(serviceIntent);
                }
            }
        }

        public void Stop()
        {
            if (IsServiceRunning())
            {
                var serviceIntent = new Intent(_context, typeof(AndroidForegroundService));
                StopService(serviceIntent);
            }

        }

    }
}
