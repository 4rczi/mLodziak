using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MlodziakApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppInfo = Microsoft.Maui.ApplicationModel.AppInfo;

namespace MlodziakApp.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IAuthenticationService _authenticationService;

        [ObservableProperty]
        string? userId;

        [ObservableProperty]
        string? sessionId;

        [ObservableProperty]
        string? appVersion;


        public SettingsPageViewModel(ISecureStorageService secureStorageService, IAuthenticationService authenticationService)
        {
            _secureStorageService = secureStorageService;
            _authenticationService = authenticationService;
        }

        public async Task InitializeAsync()
        {
            var sessionId = await _secureStorageService.GetSessionIdAsync();
            var userId = await _secureStorageService.GetUserIdAsync();

            UserId = userId;
            SessionId = sessionId;
            AppVersion = AppInfo.VersionString;
        }

        [RelayCommand]
        public async Task LogOutAsync()
        {
            await _authenticationService.LogoutAsync();
        }
    }
}
