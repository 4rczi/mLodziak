using Azure;
using Azure.Core;
using DataAccess.Entities;
using IdentityModel.OidcClient;
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
    public class LocationRequests : ILocationRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly ILocationApiCalls _locationApiCalls;

        public LocationRequests(IApplicationLoggingRequests applicationLoggingRequests, ILocationApiCalls locationApiCalls)
        {
            _applicationLoggingRequests = applicationLoggingRequests;
            _locationApiCalls = locationApiCalls;
        }

        public async Task<List<LocationModel>> GetLocationModelsAsync(string accessToken, int categoryId, string userId, string sessionId)
        {
            try
            {
                var response = await _locationApiCalls.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId);
                return response;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning", "Unsuccessful code returned", "", "", this.GetType().Name, nameof(GetLocationModelsAsync), userId, sessionId, apiEx.StatusCode.ToString(), DateTime.UtcNow, DateTime.UtcNow);
                return [];
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error", "Exception caught", "", ex.Message, this.GetType().Name, nameof(GetLocationModelsAsync), userId, sessionId, "", DateTime.UtcNow, DateTime.UtcNow);
                return [];
            }
        }

        public async Task<Dictionary<int, List<LocationModel>>> GetAllLocationModelsAsync(string accessToken, string userId, string sessionId)
        {
            try
            {
                var response = await _locationApiCalls.GetAllLocationModelsAsync($"Bearer {accessToken}", userId);
                return response;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning", "Unsuccessful code returned", "", "", this.GetType().Name, nameof(GetAllLocationModelsAsync), userId, sessionId, apiEx.StatusCode.ToString(), DateTime.UtcNow, DateTime.UtcNow);
                return [];
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error", "Exception caught", "", ex.Message, this.GetType().Name, nameof(GetAllLocationModelsAsync), userId, sessionId, "", DateTime.UtcNow, DateTime.UtcNow);
                return [];
            }
        }

        public async Task<LocationModel> GetSingleLocationModelAsync(string accessToken, int physicalLocationId, string userId, string sessionId)
        {
            try
            {
                var response = await _locationApiCalls.GetSingleLocationModelAsync($"Bearer {accessToken}", userId, physicalLocationId);
                return response;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning", "Unsuccessful code returned", "", "", this.GetType().Name, nameof(GetSingleLocationModelAsync), userId, sessionId, apiEx.StatusCode.ToString(), DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error", "Exception caught", "", ex.Message, this.GetType().Name, nameof(GetSingleLocationModelAsync), userId, sessionId, "", DateTime.UtcNow, DateTime.UtcNow);
                return null;
            }
        }
    }
}
