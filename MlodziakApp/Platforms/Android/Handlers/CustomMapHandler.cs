#if ANDROID
using Android.Gms.Maps;
using Microsoft.Maui.Maps.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Platforms.Android.Handlers
{
    public class CustomMapHandler : MapHandler
    {
        protected override void ConnectHandler(MapView nativeView)
        {
            base.ConnectHandler(nativeView);

            nativeView.GetMapAsync(new MapReadyCallback(map =>
            {
                map.UiSettings.RotateGesturesEnabled = false;

                map.UiSettings.MyLocationButtonEnabled = false;

                map.UiSettings.ScrollGesturesEnabledDuringRotateOrZoom = false;
                map.UiSettings.ScrollGesturesEnabled = false;

                map.UiSettings.ZoomGesturesEnabled = false;
                map.UiSettings.ZoomControlsEnabled = false;

                map.UiSettings.TiltGesturesEnabled = false;

                map.UiSettings.CompassEnabled = false;

                map.UiSettings.IndoorLevelPickerEnabled = false;

                map.UiSettings.MapToolbarEnabled = false;      
            }));
        }
    }

    public class MapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly Action<GoogleMap> _onMapReady;

        public MapReadyCallback(Action<GoogleMap> onMapReady)
        {
            _onMapReady = onMapReady;
        }

        public void OnMapReady(GoogleMap googleMap) => _onMapReady(googleMap);
    }
}
#endif
