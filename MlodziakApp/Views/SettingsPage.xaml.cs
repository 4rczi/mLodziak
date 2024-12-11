namespace MlodziakApp.Views;

using MlodziakApp.ViewModels;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsPageViewModel _vm;

	public SettingsPage(SettingsPageViewModel vm)
	{
		InitializeComponent();       
        _vm = vm;
        BindingContext = _vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();     
        await _vm.InitializeAsync();
    }
}