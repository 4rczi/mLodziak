
namespace MlodziakApp.Services
{
    public interface ISecureStorageWrapper
    {
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
        Task SetAsync(string key, string value);
    }
}