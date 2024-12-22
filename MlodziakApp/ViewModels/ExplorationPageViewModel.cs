using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = DataAccess.Entities.Location;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MlodziakApp.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;
using DataAccess.Entities;
using System.Windows.Input;
using Microsoft.Maui.Devices.Sensors;

namespace MlodziakApp.ViewModels
{
    public partial class ExplorationPageViewModel : ObservableObject
    {

        private readonly ISessionService _sessionService;
        private readonly ILocationRequests _locationRequests;
        private readonly ICategoryRequests _categoryRequests;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectivityService _connectivityService;
        private readonly IPermissionsService _permissionsService;

        private Dictionary<int, List<LocationModel>> AllLocationModels { get; set; } = [];

        [ObservableProperty]
        bool isBusy = false;

        [ObservableProperty]
        bool backButtonVisibility = false;

        [ObservableProperty]
        CategoryModel selectedCategory;

        [ObservableProperty]
        ObservableCollection<LocationModel> displayedLocationModels = new ObservableCollection<LocationModel>();

        [ObservableProperty]
        ObservableCollection<CategoryModel> categoryModels = new ObservableCollection<CategoryModel>();

        [ObservableProperty]
        bool isCategoryViewVisible = true;

        [ObservableProperty]
        bool isLocationViewVisible = false;


        public ExplorationPageViewModel(
            ISessionService sessionService,
            ILocationRequests locationRequests,
            ICategoryRequests categoryRequests,
            IServiceProvider serviceProvider,
            IConnectivityService connectivityService,
            IPermissionsService permissionsService)
        {
            _sessionService = sessionService;
            _locationRequests = locationRequests;
            _categoryRequests = categoryRequests;
            _serviceProvider = serviceProvider;
            _connectivityService = connectivityService;
            _permissionsService = permissionsService;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync();             
                if (!isSessionValid)
                {
                    await _sessionService.HandleInvalidSessionAsync(isLoggedIn: true, notifyUser: true);
                    return;
                }

                if (!await _connectivityService.HasInternetConnectionAsync())
                {
                    await _connectivityService.HandleNoInternetConnectionAsync();
                    return;
                }

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (!await _permissionsService.CheckRequiredPermissionsAsync())
                    {
                        await _permissionsService.HandleDeniedPermissionsAsync();
                    }
                });

                ClearData();

                IsBusy = true;
                await LoadCategoryAndLocationModelsAsync(accessToken!, userId!, sessionId!);
                IsBusy = false;               
            }

            catch (Exception ex)
            {
                // Maybe Display some error message?
                throw;
            }

            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public void SelectedCategoryChanged(CategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                return;
            }

            DisplayedLocationModels.Clear();
            DisplayedLocationModels = new ObservableCollection<LocationModel>(AllLocationModels[categoryModel.Id]);

            IsCategoryViewVisible = false;
            IsLocationViewVisible = true;

            BackButtonVisibility = true;
        }

        [RelayCommand]
        private void Back()
        {
            IsCategoryViewVisible = true;
            IsLocationViewVisible = false;
            BackButtonVisibility = false;
        }

        [RelayCommand]
        public async Task OpenMapAsync(object multiBoundParameter)
        {
            var boundInfo = multiBoundParameter as object[];
            var locationId = (int)boundInfo[0];
            var categoryId = (int)boundInfo[1];
            var latitude = (double)boundInfo[2];
            var longitude = (double)boundInfo[3];
            var zoomLevel = (float)boundInfo[4];

            var mapPage = _serviceProvider.GetRequiredService<MapPage>();
            if (mapPage != null)
            {            
                await App.Current?.MainPage?.Navigation.PushModalAsync(mapPage);
                WeakReferenceMessenger.Default.Send(new LocationInfoMessage(new LocationInfoMessageItem(locationId, categoryId, latitude, longitude, zoomLevel, null)));
            }
        }

        private void ClearData()
        {
            CategoryModels.Clear();
            AllLocationModels.Clear();
        }

        private async Task LoadCategoryAndLocationModelsAsync(string accessToken, string userId, string sessionId)
        {
            var getCategoryModelsTask = _categoryRequests.GetCategoryModelsAsync(accessToken, userId, sessionId);
            var getAllLocationModelsTask = _locationRequests.GetAllLocationModelsAsync(accessToken, userId, sessionId);

            await Task.WhenAll(getCategoryModelsTask, getAllLocationModelsTask);

            CategoryModels = new ObservableCollection<CategoryModel>(await getCategoryModelsTask);
            AllLocationModels = await getAllLocationModelsTask;
        }
    }
}
