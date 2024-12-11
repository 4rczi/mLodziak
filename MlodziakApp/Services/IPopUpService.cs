using CommunityToolkit.Maui.Core;

namespace MlodziakApp.Services
{
    public interface IPopUpService
    {
        Task ShowPopUpAsync(string message, SnackbarOptions customOptions);
    }
}