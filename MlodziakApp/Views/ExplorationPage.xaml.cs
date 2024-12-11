using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using MlodziakApp.ViewModels;
using System.Diagnostics;

namespace MlodziakApp.Views;

public partial class ExplorationPage : ContentPage
{
	private readonly ExplorationPageViewModel _vm;

	public ExplorationPage(ExplorationPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
	
	    _ = InitializeAsync();	
	}

	private async Task InitializeAsync()
	{
		await _vm.LoadDataAsync();
    }
}