using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pace_Note_Generator.Backend.API
{
    public abstract class ApiClient
    {
        protected readonly HttpClient httpClient = new HttpClient();

        public async Task<string> SendRequest(string url)
        {
            return await httpClient.GetStringAsync(url);
        }
    }
}
