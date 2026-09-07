using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pace_Note_Generator.Backend.API
{
    public abstract class ApiClient
    {
        protected readonly HttpClient httpClient = new HttpClient();

        protected ApiClient()
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PacenoteGenerator/1.0");
        }

        public async Task<string> SendRequest(string url)
        {
            return await httpClient.GetStringAsync(url);
        }
    }
}
