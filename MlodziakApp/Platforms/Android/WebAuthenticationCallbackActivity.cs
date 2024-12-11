using Android.App;
using Android.Content.PM;
using Android.OS;


namespace MlodziakApp
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
              Categories = new[] {
                Android.Content.Intent.CategoryDefault,
                Android.Content.Intent.CategoryBrowsable
              },
              DataScheme = CALLBACK_SCHEME)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        const string CALLBACK_SCHEME = "myapp";

        //protected override async void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //
        //    // Check if this activity was launched with the correct intent
        //    if (Intent.Action == Android.Content.Intent.ActionView && Intent.Data != null)
        //    {
        //        var uri = Intent.Data;
        //        if (uri.Scheme.Equals(CALLBACK_SCHEME))
        //        {
        //            // Handle the URI activation here
        //            // This method will be called when the app is activated via a URI, such as the redirect URI after authentication
        //
        //            // Example: Extract token or other data from the URI and proceed with authentication flow
        //            await HandleUriActivated(uri);
        //        }
        //    }
        //
        //    // Finish the activity
        //    Finish();
        //}
        //
        //private async Task HandleUriActivated(Android.Net.Uri uri)
        //{
        //
        //    // Handle the URI activation here
        //    // This method will be called when the app is activated via a URI, such as the redirect URI after authentication
        //
        //    // Example: Navigate to a specific page or perform an action based on the URI
        //    //await MainPage.DisplayAlert("URI Activated", $"URI: {uri}", "OK");
        //}
    }
}
