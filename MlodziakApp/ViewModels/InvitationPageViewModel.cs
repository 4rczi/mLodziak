﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp.Constants;
using MlodziakApp.Services;
using MlodziakApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Plugin.Firebase.CloudMessaging;
using CommunityToolkit.Mvvm.Messaging;
using MlodziakApp.Messages;
using MlodziakApp.Messages.MessageItems;


namespace MlodziakApp.ViewModels
{
    public partial class InvitationPageViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITitbitService _titbitService;
        private readonly IPopUpService _popUpService;
        private readonly NavigationService _navigationService;


        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string titbit;

        [ObservableProperty]
        bool didYouKnowLabelVisibility;

        [ObservableProperty]
        bool invitationGridVisibility;

        [ObservableProperty]
        bool continueOverlayGridVisibility;

        [ObservableProperty]
        bool tapToContinueLabelVisibility;

        [ObservableProperty]
        double tapToContinueLabelOpacity;

        private FCMPushNotificationPendingMessage? _pendingRedirectionMessage;


        public InvitationPageViewModel(ISessionService sessionService,
                                       IAuthenticationService authenticationService,
                                       ITitbitService titbitService,
                                       IPopUpService popUpService,
                                       NavigationService navigationService)
        {
            _sessionService = sessionService;
            _authenticationService = authenticationService;
            _titbitService = titbitService;
            _popUpService = popUpService;
            _navigationService = navigationService;

            WeakReferenceMessenger.Default.Register<FCMPushNotificationPendingMessage>(this, OnFCMPushNotificationPendingMessage);
            
        }

        private async void OnFCMPushNotificationPendingMessage(object recipient, FCMPushNotificationPendingMessage message)
        {
            _pendingRedirectionMessage = message;
        }

        [RelayCommand]
        public async Task LogInAsync()
        {
            try
            {
                IsBusy = true;
                HideInvitationPage();

                var (isSessionValid, accessToken, refreshToken, sessionId, userId) = await _sessionService.ValidateSessionAsync(autoLoginCheck: true);
                if (isSessionValid)
                {
                    await Shell.Current.GoToAsync($"//{nameof(ExplorationPage)}");
                    return;
                }
                    
                await _sessionService.HandleInvalidSessionAsync(isLoggedIn: false);

                var isLoginSuccess = await _authenticationService.LoginAsync();
                if (!isLoginSuccess)
                {
                    await ShowInvitationPageAsync();
                    return;
                }

                if(_pendingRedirectionMessage != null)
                {
                    await _navigationService.NavigateToPhysicalLocationAsync(_pendingRedirectionMessage);
                    _pendingRedirectionMessage = null;
                    return;
                }
           
                await Shell.Current.GoToAsync($"//{nameof(ExplorationPage)}");             
            }

            catch (Exception ex)
            {
                await _popUpService.ShowPopUpAsync(AlertMessages.LoginFailedMessage, null);
                await ShowInvitationPageAsync();
            }

            finally 
            {
                IsBusy = false;
            }
        }

        public async Task ShowInvitationPageAsync()
        {          
            PrepareInvitationPage();
            _ = StartFlashingTapToContinueLabelAsync();

            await Task.Delay(500);
            Titbit = _titbitService.GetRandomTitbit();
            await Task.Delay(2000);
            TapToContinueLabelVisibility = true;
            ContinueOverlayGridVisibility = true;
        }

        private void HideInvitationPage()
        {
            InvitationGridVisibility = false;
        }

        private void PrepareInvitationPage()
        {
            InvitationGridVisibility = true;
            TapToContinueLabelVisibility = false;
            ContinueOverlayGridVisibility = false;
            Titbit = "";
            DidYouKnowLabelVisibility = true;

        }

        public async Task StartFlashingTapToContinueLabelAsync()
        {
            while (true)
            {
                TapToContinueLabelOpacity = 0;
                await Task.Delay(250); 

                TapToContinueLabelOpacity = 0.25;
                await Task.Delay(250); 

                TapToContinueLabelOpacity = 0.5;
                await Task.Delay(250); 

                TapToContinueLabelOpacity = 0.75;
                await Task.Delay(250); 

                TapToContinueLabelOpacity = 1;
                await Task.Delay(250); 

                TapToContinueLabelOpacity = 0.75;
                await Task.Delay(250);

                TapToContinueLabelOpacity = 0.5;
                await Task.Delay(250);

                TapToContinueLabelOpacity = 0.25;
                await Task.Delay(250);
            }
        }
    }
    

}
