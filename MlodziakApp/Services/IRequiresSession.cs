namespace MlodziakApp.Services
{
    public interface IRequiresSession
    {
        Task HandleSessionExpiredAsync();
        Task HandleSessionInitializedAsync();
    }
}