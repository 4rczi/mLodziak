using Azure;
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
    public class PhysicalLocationRequests : IPhysicalLocationRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly IPhysicalLocationApiCalls _physicalLocationApiCalls;


        public PhysicalLocationRequests(IApplicationLoggingRequests applicationLogger, IPhysicalLocationApiCalls physicalLocationApiCalls)
        {
            _applicationLoggingRequests = applicationLogger;
            _physicalLocationApiCalls = physicalLocationApiCalls;
        }

        public async Task<List<PhysicalLocationModel>> GetPhysicalLocationModelsAsync(string accessToken, string userId, int categoryId, int locationId, string sessionId)
        {
            try
            {
                var response = await _physicalLocationApiCalls.GetPhysicalLocationsAsync(userId, categoryId, locationId, $"Bearer {accessToken}");
                return response;
            }

            catch (ApiException apiEx)
            {  
                await _applicationLoggingRequests.LogAsync("Warning",
                    "Unsuccessful code returned",
                    "",
                    "",
                    this.GetType().Name,
                    nameof(GetPhysicalLocationModelsAsync),
                    userId,
                    sessionId,
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);  
                
                return [];
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(GetPhysicalLocationModelsAsync),
                    userId,
                    sessionId,
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return [];
            }
        }

        public async Task<List<PhysicalLocationModel>> GetVisitablePhysicalLocationModelsAsync(string accessToken, string userId, string sessionId)
        {
            try
            {
                var response = await _physicalLocationApiCalls.GetVisitablePhysicalLocationsAsync(userId, $"Bearer {accessToken}");
                return response;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning",
                    "Unsuccessful code returned",
                    "",
                    "",
                    this.GetType().Name,
                    nameof(GetVisitablePhysicalLocationModelsAsync),
                    userId,
                    sessionId,
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return [];
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error", "Exception caught", "", ex.Message,
                    this.GetType().Name, nameof(GetVisitablePhysicalLocationModelsAsync), userId, sessionId,
                    "", DateTime.UtcNow, DateTime.UtcNow);

                return [];
            }
        }
    }


}
