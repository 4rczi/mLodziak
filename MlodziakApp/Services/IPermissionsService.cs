
namespace MlodziakApp.Services
{
    public interface IPermissionsService
    {
        Task<bool> CheckRequiredPermissionsAsync();
        Task<PermissionStatus> CheckRequiredPermissionsAsync<TPermission>() where TPermission : Permissions.BasePermission, new();
        Task HandleDeniedPermissionsAsync();
    }
}