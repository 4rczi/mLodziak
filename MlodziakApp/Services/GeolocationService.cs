using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Devices.Sensors;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Geolocation;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MlodziakApp.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApplicationLoggingRequests _applicationLogger;
        private readonly IGeolocationChangedHandler _userGeolocationChangedHandler;
        
        private bool _isRunning;
        private CancellationTokenSource _cts;

        public GeolocationService(ISecureStorageService secureStorageService,
                                  IApplicationLoggingRequests applicationLogger,
                                  IGeolocationChangedHandler userGeolocationChangedHandler)
        {
            _secureStorageService = secureStorageService;
            _applicationLogger = applicationLogger;
            _userGeolocationChangedHandler = userGeolocationChangedHandler;

            WeakReferenceMessenger.Default.Register<SessionExpiredMessage>(this, OnSessionExpiredMessageReceived);
            WeakReferenceMessenger.Default.Register<SessionInitializedMessage>(this, OnSessionInitializedMessageReceived);
        }

        private async void OnSessionInitializedMessageReceived(object recipient, SessionInitializedMessage message)
        {
            await HandleSessionInitializedAsync();
        }

        private async void OnSessionExpiredMessageReceived(object recipient, SessionExpiredMessage message)
        {
            await StopTrackingUserLocationAsync();
            message.CompletionSource.TrySetResult(true);
        }

        public async Task<Location?> GetUserGeolocationAsync(GeolocationAccuracy accuracy, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var request = new GeolocationRequest(accuracy, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.Default.GetLocationAsync(request);

                    return location;
                }

                return null;
            }

            catch (Exception ex)
            {
                await _applicationLogger.LogAsync("Warning", "Exception caught", "", ex.Message, this.GetType().Name, nameof(GetUserGeolocationAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }
        }

        private async IAsyncEnumerable<Location?> YieldUserLocationAsync(GeolocationAccuracy accuracy, int requestIntervalInSeconds, [EnumeratorCancellation] CancellationToken trackingCancellationToken)
        {
            while (!trackingCancellationToken.IsCancellationRequested)
            {
                // TODO:
                //if user is too far away from any location, call it with low accuracy
                var userGeolocation = await GetUserGeolocationAsync(accuracy, trackingCancellationToken);

                if (trackingCancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return userGeolocation;

                await Task.Delay(requestIntervalInSeconds * 1000, CancellationToken.None);
            }
        }

        public async Task StartTrackingUserLocationAsync(GeolocationAccuracy accuracy, int requestIntervalInSeconds)
        {
            if (!_isRunning)
            {
                await _applicationLogger.LogAsync("Information", "Starting tracking user geolocation", "", "", this.GetType().Name, nameof(StartTrackingUserLocationAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);

                _isRunning = true;
                CreateCancellationTokenSource();

                await ProcessUserGeolocationUpdatesAsync(accuracy, requestIntervalInSeconds);
            }
        }

        private async Task ProcessUserGeolocationUpdatesAsync(GeolocationAccuracy accuracy, int requestIntervalInSeconds)
        {
            while (!_cts.IsCancellationRequested)
            {
                await foreach (var userGeolocation in YieldUserLocationAsync(accuracy, requestIntervalInSeconds, _cts.Token))
                {
                    if (userGeolocation != null)
                    {
                        WeakReferenceMessenger.Default.Send(new UserGeolocationMessage(new UserGeolocationMessageItem(userGeolocation.Latitude, userGeolocation.Longitude)));
                    }

                    var visitedPhysicalLocationModel = await _userGeolocationChangedHandler.HandleUserGeolocationChangeAsync(userGeolocation);
                    if (visitedPhysicalLocationModel != null)
                    {
                        WeakReferenceMessenger.Default.Send(new VisitedPhysicalLocationMessage(visitedPhysicalLocationModel));
                    }
                }
            }
        }

        public async Task StopTrackingUserLocationAsync()
        {
            await _applicationLogger.LogAsync("Information", "Stopping tracking user geolocation", "", "", this.GetType().Name, nameof(StopTrackingUserLocationAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);

            this._cts?.Cancel();
            this._cts?.Dispose();

            _isRunning = false;          
        }

        private void CreateCancellationTokenSource()
        {
            _cts = new CancellationTokenSource();
        }

        public async Task HandleSessionExpiredAsync()
        {
            await StopTrackingUserLocationAsync();
        }

        public async Task HandleSessionInitializedAsync()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await StartTrackingUserLocationAsync(GeolocationSettings.GeolocationAccuracy, GeolocationSettings.GeolocationRequestIntervalInSeconds);
                }

                catch (Exception ex)
                {
                    await _applicationLogger.LogAsync("Error", "Exception caught in infinite loop task", "", ex.Message, this.GetType().Name, nameof(HandleSessionInitializedAsync), await _secureStorageService.GetUserIdAsync() ?? "Unknown", await _secureStorageService.GetSessionIdAsync() ?? "Unknown", "", DateTime.UtcNow, DateTime.UtcNow);
                }
            });
        }
    }


}
