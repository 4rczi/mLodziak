using DataAccess.Entities;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MlodziakApp.ApiRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MlodziakApp.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPopUpService _popUpService;


        public PermissionsService(IPopUpService popUpService)
        {
            _popUpService = popUpService;
        }

        public async Task<bool> CheckRequiredPermissionsAsync()
        {          
            // If upon LocationAlways request user chooses "Allow only while using app" returned status is restricted
            var locationAlwaysStatus = await CheckRequiredPermissionsAsync<Permissions.LocationAlways>();
            if (locationAlwaysStatus == PermissionStatus.Denied)
            {
                return false;
            }

            var networkStatus = await CheckRequiredPermissionsAsync<Permissions.NetworkState>();
            if (networkStatus == PermissionStatus.Denied)
            {
                return false;
            }

            var pushNotificationStatus = await CheckRequiredPermissionsAsync<Permissions.PostNotifications>();
            if (pushNotificationStatus == PermissionStatus.Denied)
            {
                return false;
            }

            return true;
        }

        public async Task<PermissionStatus> CheckRequiredPermissionsAsync<TPermission>() where TPermission : Permissions.BasePermission, new()
        {
            
            var currentPermissionStatus = await Permissions.CheckStatusAsync<TPermission>();

            if (currentPermissionStatus == PermissionStatus.Granted || currentPermissionStatus == PermissionStatus.Restricted)
            {
                return PermissionStatus.Granted;
            }

            // If user chooses "Only this time" returned status is Restricted, but is also might open appsettings, 
            var requestedPermissionStatus = await Permissions.RequestAsync<TPermission>();
            if (requestedPermissionStatus == PermissionStatus.Granted || requestedPermissionStatus == PermissionStatus.Restricted)
            {
                return PermissionStatus.Granted;
            }

            await _popUpService.ShowPopUpAsync($"Brak wymaganych uprawnień - {typeof(TPermission).Name}", null);
            await Task.Delay(3000);

            // If user previously denied permission we might have one more chance to request for it
            if (Permissions.ShouldShowRationale<TPermission>())
            {
                var rationaledPermissionStatus = await Permissions.RequestAsync<TPermission>();
                if (rationaledPermissionStatus == PermissionStatus.Granted || rationaledPermissionStatus == PermissionStatus.Restricted)
                {
                    return PermissionStatus.Granted;
                }
            }

            // As a last resort we navigate user to our app settings so he can enable permissions manually
            // After settings are changed android will most likely restart our app
            await Task.Run(() => AppInfo.Current.ShowSettingsUI());

            return PermissionStatus.Denied;         
        }

        private static bool IsGranted(PermissionStatus status)
        {
            return status == PermissionStatus.Granted || status == PermissionStatus.Limited;
        }

        public async Task HandleDeniedPermissionsAsync()
        {           
            await _popUpService.ShowPopUpAsync("Brak wymaganych uprawnień. Aplikacja zostanie zamknięta.", null);
            await Task.Delay(3000);
            System.Environment.Exit(0);
        }
    }
}
