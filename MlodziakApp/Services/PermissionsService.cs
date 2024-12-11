using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MlodziakApp.Services
{
    public class PermissionsService
    {
        public async Task<bool> CheckPushNotificationPermissions()
        {
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (permissionResult == PermissionStatus.Granted)
            {
                return true;
            }

            return false;
        }

        public async Task AskForPushNotificationPermissions()
        {
            await Permissions.RequestAsync<Permissions.PostNotifications>();
        }
    }
}
