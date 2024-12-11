using MlodziakApp.ApiRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Services
{
    public class ConnectivityService : IConnectivityService
    {
        private readonly IInternetConnectionRequests _internetConnectionRequests;
        private readonly IPopUpService _popUpService;


        public ConnectivityService(IInternetConnectionRequests internetConnectionRequests, IPopUpService popUpService)
        {
            _internetConnectionRequests = internetConnectionRequests;
            _popUpService = popUpService;
        }

        private bool IsConnectedToNetwork()
        {
            return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        public async Task<bool> HasInternetConnectionAsync()
        {
            var isConnectedToNetwork = IsConnectedToNetwork();
            var canAccessInternet = await _internetConnectionRequests.IsInternetAccessibleAsync();

            return isConnectedToNetwork && canAccessInternet;
        }

        public async Task HandleNoInternetConnectionAsync()   
        {
            await _popUpService.ShowPopUpAsync(Constants.AlertMessages.NoInternetMessage, null);
        }
    }
}
