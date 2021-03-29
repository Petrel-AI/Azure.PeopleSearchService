using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Petrel.Utils.Azure.PSS.Core;

namespace Petrel.Utils.Azure.PSS
{
    public class BearerToken
    {
        private readonly IPssSettings _settings;
        private readonly IDictionary<string, string> _token = new Dictionary<string, string>();
        private readonly IDictionary<string, DateTime?> _tokenDate = new Dictionary<string, DateTime?>();
        private readonly ILogger<BearerToken> _logger;

        public BearerToken(IPssSettings settings, ILogger<BearerToken> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        private async Task<AuthenticationResult> TokenAsync()
        {
            try
            {
                ClientCredential clientCredential = _settings.ClientCredential;

                var context = new AuthenticationContext(_settings.Authority, false);
                AuthenticationResult result = await context.AcquireTokenAsync(_settings.Resource, clientCredential);

                return result;
            }
            catch (Exception e)
            {
                const string message = "Could not get bearer token for PSS";
                _logger.LogDebug(new EventId(1), e, message);
                throw new InvalidOperationException(message, e);
            }
        }

        public async Task<string> GetAsync()
        {
            string token;

            string url = _settings.Resource;

            if (!_token.ContainsKey(url))
            {
                _token.Add(url, null);
            }

            if (!_tokenDate.ContainsKey(url))
            {
                _tokenDate.Add(url, null);
            }

            var oldToken = _token[url];
            var oldDate = _tokenDate[url];

            if (string.IsNullOrEmpty(oldToken) || oldDate == null ||
                (oldDate - DateTime.Now).Value.Minutes > TimeSpan.FromMinutes(30).Minutes)
            {
                AuthenticationResult result = await TokenAsync();

                token = _token[url] = result.AccessToken;

                _tokenDate[url] = DateTime.Now;
            }
            else
            {
                token = oldToken;
            }

            return token;
        }
    }
}