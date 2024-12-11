using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MlodziakApp.ViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MlodziakApp.Views;

public partial class MapPage : ContentPage
{
	private readonly MapPageViewModel _vm;


	public MapPage(MapPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
    }



}