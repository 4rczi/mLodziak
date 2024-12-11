using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MlodziakApp.Services;
using Microsoft.Maui.Storage;

namespace MlodziakApp.Logic.SecureStorage
{
    public class SecureStorageWrapper : ISecureStorageWrapper
    {
        public async Task<string?> GetAsync(string key)
        {
            return await Microsoft.Maui.Storage.SecureStorage.GetAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(key, value);
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Run(() => Microsoft.Maui.Storage.SecureStorage.Remove(key));
        }
    }
}
