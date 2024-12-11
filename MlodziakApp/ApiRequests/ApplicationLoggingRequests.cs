using Azure.Core;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MlodziakApp.Services;
using MlodziakApp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiRequests
{
    public class ApplicationLoggingRequests : IApplicationLoggingRequests
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISecureStorageService _secureStorageService;

        public ApplicationLoggingRequests(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _secureStorageService = secureStorageService;
        }

        public async Task LogAsync(string level, string message, string logger, string exception, string className, string methodName, string userId, string sessionId, string customMessage, DateTime createdDate, DateTime modifiedDate)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MyAndroidHttpClient");

                var relativePath = $"/api/applicationlogging/";
                var url = UriConstructor.ConstructUrl(httpClient.BaseAddress!, relativePath);

                var logEntry = new ApplicationLogging
                {
                    Level = level,
                    Message = message,
                    Logger = logger,
                    Exception = exception,
                    ClassName = className,
                    MethodName = methodName,
                    UserId = userId,
                    SessionId = sessionId,
                    CustomMessage = customMessage,
                    CreatedDate = createdDate,
                    ModifiedDate = modifiedDate
                };

                var jsonUserData = JsonConvert.SerializeObject(logEntry);

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonUserData, Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode.ToString());
                    return;
                }
            }
            catch (Exception ex)
            {
                // TODO:  Log to file instead? Retry logic?
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}
