using MlodziakApp.ApiCalls;
using MlodziakApp.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.ApiRequests
{
    public class InternetConnectionRequests : IInternetConnectionRequests
    {
        public async Task<bool> IsInternetAccessibleAsync()
        {
            try
            {
                using (var client = CreateHttpClient())
                {
                    var response = await client.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

#if DEBUG
            handler = InsecureHttpClientFactory.GetInsecureHandler();
#endif

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(3)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("https://www.google.com");

            return client;
        }
    }
}
