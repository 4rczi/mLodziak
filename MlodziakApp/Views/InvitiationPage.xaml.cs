using MlodziakApp.Services;
using MlodziakApp.ViewModels;

namespace MlodziakApp.Views;

public partial class InvitationPage : ContentPage
{
	private readonly InvitationPageViewModel _vm;
    private readonly IPermissionsService _permissionsService;


    public InvitationPage(InvitationPageViewModel vm, IPermissionsService permissionsService)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
        _permissionsService = permissionsService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);

        _ = InitializeAsync();
    }

	private async Task InitializeAsync()
	{
        if (!await _permissionsService.CheckRequiredPermissions())
        {
            await _permissionsService.HandleDeniedPermissionsAsync();
        }

        await _vm.ShowInvitationPageAsync();   
    }
}