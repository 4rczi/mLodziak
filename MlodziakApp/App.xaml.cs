using Microsoft.Maui;
using MlodziakApp.Views;
using Microsoft.Extensions.Logging;
using DataAccess.Entities;
using MlodziakApp.Services;
using MlodziakApp.ApiRequests;


namespace MlodziakApp
{
    public partial class App : Application
    {
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly ITokenService _accessTokenService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGeolocationService _geolocationService;


        public App(IApplicationLoggingRequests applicationLogger, ITokenService accessTokenService, ISecureStorageService secureStorageService, IServiceProvider serviceProvider, IGeolocationService geolocationService)
        {
            _applicationLogger = applicationLogger;
            _accessTokenService = accessTokenService;
            _secureStorageService = secureStorageService;
            _serviceProvider = serviceProvider;
            _geolocationService = geolocationService;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            InitializeComponent();
            InitializeServices();
            MainPage = new AppShell();
            
        }

        private void InitializeServices()
        {
            var pushNotificationService = _serviceProvider.GetService<PushNotificationService>();   
        }

        // Handle UI thread exceptions
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            HandleExceptionAsync(exception);
        }

        // Handle background thread exceptions
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved(); // Mark the exception as observed
            HandleExceptionAsync(e.Exception);
        }

        private async void HandleExceptionAsync(Exception exception)
        {
            var accessToken = await _secureStorageService.GetAccessTokenAsync();
            await _applicationLogger.LogAsync("Error", "Caught unhandled error", "", exception.Message, this.GetType().Name, nameof(HandleExceptionAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
        }


    }
}
