using MlodziakApp.Services;
using MlodziakApp.ViewModels;

namespace MlodziakApp.Views;

public partial class InvitationPage : ContentPage
{
	private readonly InvitationPageViewModel _vm;


	public InvitationPage(InvitationPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);

        _ = InitializeAsync();     
    }

	private async Task InitializeAsync()
	{
        await _vm.ShowInvitationPageAsync();
    }
}