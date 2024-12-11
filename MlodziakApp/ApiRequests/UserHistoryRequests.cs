using Azure;
using DataAccess.Entities;
using MlodziakApp.ApiCalls;
using MlodziakApp.Services;
using MlodziakApp.Utilities;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiRequests
{
    public class UserHistoryRequests : IUserHistoryRequests
    {
        private readonly IApplicationLoggingRequests _applicationLoggingRequests;
        private readonly IUserHistoryApiCalls _userHistoryApiCalls;

        public UserHistoryRequests(IApplicationLoggingRequests applicationLogger, IUserHistoryApiCalls userHistoryApiCalls)
        {
            _applicationLoggingRequests = applicationLogger;
            _userHistoryApiCalls = userHistoryApiCalls;
        }

        public async Task<bool> CreateUserHistoryAsync(string userId, int physicalLocationId, string accessToken, string sessionId)
        {
            try
            {
                var userHistoryData = new UserHistory
                {
                    UserId = userId,
                    PhysicalLocationId = physicalLocationId
                };

                var response = await _userHistoryApiCalls.CreateUserHistoryAsync(userHistoryData, $"Bearer {accessToken}");
                return response;
            }

            catch (ApiException apiEx)
            {
                await _applicationLoggingRequests.LogAsync("Warning",
                    "Unsuccessful code returned",
                    "",
                    "",
                    this.GetType().Name,
                    nameof(CreateUserHistoryAsync),
                    userId,
                    sessionId,
                    apiEx.StatusCode.ToString(),
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return false;
            }

            catch (Exception ex)
            {
                await _applicationLoggingRequests.LogAsync("Error",
                    "Exception caught",
                    "",
                    ex.Message,
                    this.GetType().Name,
                    nameof(CreateUserHistoryAsync),
                    userId,
                    sessionId,
                    "",
                    DateTime.UtcNow,
                    DateTime.UtcNow);

                return false;
            }
        }
    }
}
