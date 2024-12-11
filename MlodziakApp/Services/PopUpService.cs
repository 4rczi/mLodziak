using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class PopUpService : IPopUpService
    {
        private readonly SnackbarOptions _defaultSnackbarOptions;

        public PopUpService()
        {
            _defaultSnackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#75819c"),
                TextColor = Color.FromArgb("FF000000"),
            };
        }

        public async Task ShowPopUpAsync(string message, SnackbarOptions customOptions)
        {

            var options = customOptions ?? _defaultSnackbarOptions;

            var popUp = Snackbar.Make(
                message: message,
                duration: TimeSpan.FromSeconds(7),         
                visualOptions: options
            );

            await popUp.Show();
            await Task.Delay(2000);
        }
    }
}
