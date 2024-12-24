using MlodziakApp.ApiCalls;
using MlodziakApp.Services;
using MlodziakApp.Utilities;
using Newtonsoft.Json;
using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiRequests
{
    public class CategoryRequests : ICategoryRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly ICategoryApiCalls _categoryApiCalls;

        public CategoryRequests(IApplicationLoggingRequests applicationLogger, ICategoryApiCalls categoryApiCalls)
        {
            _applicationLoggingRequests = applicationLogger;
            _categoryApiCalls = categoryApiCalls;
        }

        public async Task<List<CategoryModel>> GetCategoryModelsAsync(string accessToken, string userId, string sessionId)
        {
            try
            {
                return await _categoryApiCalls.GetCategoriesAsync($"Bearer {accessToken}", userId);
            }
            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync(
                    "Warning",
                    "Unsuccessful code returned",
                    "",
                    apiEx.Message,
                    this.GetType().Name,
                    nameof(GetCategoryModelsAsync),
                    userId,
                    sessionId,
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return [];
            }
            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync(
                    "Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(GetCategoryModelsAsync),
                    userId,
                    sessionId,
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return [];
            }
        }
    }
}
