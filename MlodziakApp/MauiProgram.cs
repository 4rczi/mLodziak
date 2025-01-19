using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MlodziakApp.ViewModels;
using DataAccess;
using MlodziakApp.Services;
using System.Reflection;
using Auth0.OidcClient;
using MlodziakApp.ApiRequests;
using MlodziakApp.Views;
using CommunityToolkit.Maui;
using System.Data;
using Plugin.LocalNotification;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using MlodziakApp.Logic.Geolocation;
using MlodziakApp.Logic.Authentication;
using MlodziakApp.Logic.Session;
using MlodziakApp.Logic.Token;
using MlodziakApp.Logic.SecureStorage;
using MlodziakApp.Logic.Map;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Crashlytics;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using MlodziakApp.Logic.Notification;
using Refit;
using MlodziakApp.ApiCalls;
using Microsoft.IdentityModel.Tokens;


#if ANDROID
using MlodziakApp.Platforms;
using MlodziakApp.Platforms.Android.Handlers;
using Plugin.Firebase.Bundled.Platforms.Android;
#endif



namespace MlodziakApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .UseLocalNotification()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            builder.ConfigureMauiHandlers(handlers => { handlers.AddHandler(typeof(Map), typeof(CustomMapHandler));});
#endif

#if DEBUG
            var environment = "Development";
#else 
            var environment = "Production";
#endif
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.appsettings.{environment}.json";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");


            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

            builder.Services.AddSingleton<IConfiguration>(configuration);

            builder.Services.AddSingleton(new Auth0Client(new()
            {
                Domain = configuration["Auth0:Domain"],
                ClientId = configuration["Auth0:ClientId"],
                RedirectUri = configuration["Auth0:RedirectUri"],
                PostLogoutRedirectUri = configuration["Auth0:PostLogoutRedirectUri"],
                Scope = configuration["Auth0:Scope"],
            }));

            builder.Services.AddHttpClient("MyAndroidHttpClient", client =>
            {
#if DEBUG
                client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7128" : "https://localhost:5000");
#else
                client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? configuration["Azure:WebApiBaseUrl"] : "https://localhost:5000");
#endif
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
                // This dublication has to be kept, until all api calls are going to be done via refit
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
#if DEBUG
                return InsecureHttpClientFactory.GetInsecureHandler();
#else
                return new HttpClientHandler();
#endif
            });

            // Refit setup
            var apiInterfaces = new[] { typeof(IAccessTokenApiCalls),
                                        typeof(ILocationApiCalls),
                                        typeof(ICategoryApiCalls),
                                        typeof(IInternetConnectionApiCalls),
                                        typeof(INotificationApiCalls),
                                        typeof(IPhysicalLocationApiCalls),
                                        typeof(IUserApiCalls),
                                        typeof(IUserHistoryApiCalls),
                                      };
            foreach (var apiInterface in apiInterfaces)
            {
                builder.Services.AddRefitClient(apiInterface)
                    .ConfigureHttpClient((provider, client) =>
                    {
                        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                        var customClient = httpClientFactory.CreateClient("MyAndroidHttpClient");

                        client.BaseAddress = customClient.BaseAddress;
                        client.DefaultRequestHeaders.Clear();
                        foreach (var header in customClient.DefaultRequestHeaders)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
#if DEBUG
                        return InsecureHttpClientFactory.GetInsecureHandler(); 
#else
            return new HttpClientHandler();
#endif
                    });
            }

            

            // Pages/ViewModels
            builder.Services.AddTransient<ExplorationPage>();
            builder.Services.AddTransient<ExplorationPageViewModel>();

            builder.Services.AddTransient<InvitationPage>();
            builder.Services.AddTransient<InvitationPageViewModel>();

            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<MapPageViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsPageViewModel>();

            // API Requests  
            builder.Services.AddTransient<IAccessTokenRequests, AccessTokenRequests>();
            builder.Services.AddTransient<ILocationRequests, LocationRequests>();
            builder.Services.AddTransient<IUserRequests, UserRequests>();
            builder.Services.AddTransient<ICategoryRequests, CategoryRequests>();
            builder.Services.AddTransient<IPhysicalLocationRequests, PhysicalLocationRequests>();
            builder.Services.AddTransient<IUserHistoryRequests, UserHistoryRequests>();
            builder.Services.AddTransient<IInternetConnectionRequests, InternetConnectionRequests>();
            builder.Services.AddTransient<IApplicationLoggingRequests, ApplicationLoggingRequests>();
            builder.Services.AddTransient<NotificationRequests>();

            // Services      
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton(provider => new Lazy<ISessionService>(() => provider.GetRequiredService<ISessionService>()));
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<ISecureStorageService, SecureStorageService>();
            builder.Services.AddTransient<IMapService, MapService>();
            builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
            builder.Services.AddSingleton<PushNotificationService>();
            builder.Services.AddTransient<IConnectivityService, ConnectivityService>();
            builder.Services.AddTransient<IPopUpService, PopUpService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
            builder.Services.AddTransient<ITitbitService, TitbitService>();
            builder.Services.AddTransient<IPermissionsService, PermissionsService>();
            builder.Services.AddTransient<NavigationService>(); 

            // Logic         
            builder.Services.AddTransient<IGeolocationDataLoader, GeolocationDataLoader>();
            builder.Services.AddTransient<IGeolocationVisitHandler, GeolocationVisitHandler>();
            builder.Services.AddTransient<IGeolocationChangedHandler, GeolocationChangedHandler>();

            builder.Services.AddTransient<IAuth0LoginHandler, Auth0LoginHandler>();
            builder.Services.AddTransient<IAuth0LogoutHandler, Auth0LogoutHandler>();

            builder.Services.AddTransient<ISessionDataHandler, SessionDataHandler>();
            builder.Services.AddTransient<ISessionHandler, SessionHandler>();
            builder.Services.AddTransient<ISessionValidator, SessionValidator>();

            builder.Services.AddTransient<ITokenValidator, TokenValidator>();
            builder.Services.AddTransient<ITokenClaimsValidator, TokenClaimsValidator>();
            builder.Services.AddTransient<ITokenRefresher, TokenRefresher>();

            builder.Services.AddTransient<ISecureStorageWrapper, SecureStorageWrapper>();

            builder.Services.AddTransient<IMapDataLoader, MapDataLoader>();
            builder.Services.AddTransient<IMapHandler, MapHandler>();
            builder.Services.AddTransient<IMapInitializer, MapInitializer>();

            builder.Services.AddTransient<INotificationHandler, FCMPushNotificationHandler>();
            builder.Services.AddTransient<INotificationHandler, LocalPushNotificationHandler>();

#if ANDROID
            builder.Services.AddTransient<AndroidNotificationService>();
            builder.Services.AddSingleton<AndroidForegroundService>();
            builder.Services.AddSingleton<IAndroidForegroundService>(provider => provider.GetRequiredService<AndroidForegroundService>());
#endif


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                    CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
#endif
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true,
            isCloudMessagingEnabled: true, isAnalyticsEnabled: true);
        }
    }
}
