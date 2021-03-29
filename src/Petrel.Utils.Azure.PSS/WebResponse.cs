using System;
using System.Net.Http;
using System.Threading.Tasks;
using MG.Utils.Abstract;

namespace Petrel.Utils.Azure.PSS
{
    internal class WebResponse
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpRequestMessage _request;

        public WebResponse(IHttpClientFactory clientFactory, HttpRequestMessage request)
        {
            _clientFactory = clientFactory.ThrowIfNull(nameof(clientFactory));
            _request = request.ThrowIfNull(nameof(request));
        }

        public async Task<string> AsStringAsync()
        {
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(2);

            HttpResponseMessage response = await client.SendAsync(_request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}