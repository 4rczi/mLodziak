using Azure.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Maps;
using MlodziakApp.ApiRequests;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using MlodziakApp.Services;
using MlodziakApp.Utilities;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plugin.LocalNotification.NotificationRequestGeofence;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.ViewModels
{
    public partial class MapPageViewModel : ObservableObject
    {
        private readonly IMapService _mapService;

        
        [ObservableProperty]
        bool isPhysicalLocationViewVisible;

        [ObservableProperty]
        ObservableCollection<PhysicalLocationModel> selectedPhysicalLocationModel = new ObservableCollection<PhysicalLocationModel>();

        [ObservableProperty]
        public Map map;

        private MapSpan originalMapLocation;

        public MapPageViewModel(IMapService mapService)
        {
            _mapService = mapService;

            WeakReferenceMessenger.Default.Register<LocationInfoMessage>(this, OnLocationInfoMessageReceived);
            WeakReferenceMessenger.Default.Register<UserGeolocationMessage>(this, OnUserGeolocationChangedMessageReceived);
        }

        private async void OnLocationInfoMessageReceived(object recipient, LocationInfoMessage message)
        {
            Map = await _mapService.InitalizeMapAsync(this, message.Value);
            Map.MapClicked += OnMapClicked;

            await Task.Delay(500); // Giving time for VisibleRegion to initalize 
            if (Map.VisibleRegion != null)
            {

                originalMapLocation = new MapSpan(Map.VisibleRegion.Center,
                    Map.VisibleRegion.LatitudeDegrees,
                    Map.VisibleRegion.LongitudeDegrees);
            }
        }

        private void OnUserGeolocationChangedMessageReceived(object recipient, UserGeolocationMessage message)
        {
            var userLocation = message.Value;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateOrCreateUserPin(userLocation.Latitude, userLocation.Longitude);
            });
        }

        private void UpdateOrCreateUserPin(double latitude, double longitude)
        {
            if (Map == null)
            {
                return;
            }

            var currentUserPin = Map.Pins.FirstOrDefault(pin => pin.Label == "User");

            if (currentUserPin != null)
            {
                Map.Pins.Remove(currentUserPin);
            }

            var newPin = new Pin
            {
                Label = "User",
                Location = new Location(latitude, longitude)
            };
            newPin.MarkerClicked += PinClickedHandler;

            Map.Pins.Add(newPin);

            return;
        }

        private async void PinClickedHandler(object? sender, PinClickedEventArgs e)
        {  
            // After pin is clicked map automatically repositions itself
            // Thus we need immediately retain original location, by using dispatcher
            map.Dispatcher.Dispatch(() => map.MoveToRegion(originalMapLocation));
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {      
            var physicalLocationModelClicked = _mapService.HandleMapClicked(e.Location);

            if (physicalLocationModelClicked != null)
            {
                SelectedPhysicalLocationModel.Clear();
                SelectedPhysicalLocationModel.Add(physicalLocationModelClicked);
                IsPhysicalLocationViewVisible = true;         
                return;
            }

            SelectedPhysicalLocationModel.Clear();
            IsPhysicalLocationViewVisible = false;                  
        }
    }
}
