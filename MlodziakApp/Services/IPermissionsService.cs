
namespace MlodziakApp.Services
{
    public interface IPermissionsService
    {
        Task<bool> CheckRequiredPermissions();
        Task<PermissionStatus> CheckRequiredPermissions<TPermission>() where TPermission : Permissions.BasePermission, new();
        Task HandleDeniedPermissionsAsync();
    }
}