using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MG.Utils.AspNetCore.WebResponse;
using MG.Utils.Helpers;
using Microsoft.Extensions.Logging;
using Petrel.Utils.Azure.PSS.Core;
using Petrel.Utils.Azure.PSS.Core.Models;
using Exception = System.Exception;

namespace Petrel.Utils.Azure.PSS
{
    public class PssService : IPssService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly BearerToken _bearerToken;
        private readonly IPssSettings _config;
        private readonly ILogger<PssService> _logger;

        public PssService(
            IHttpClientFactory httpClientFactory,
            IPssSettings config,
            ILogger<PssService> logger,
            BearerToken bearerToken)
        {
            _clientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
            _bearerToken = bearerToken;
        }

        private async Task<HttpRequestMessage> RequestAsync(HttpMethod method, PssEndpoint endpoint, object payload = null)
        {
            HttpRequestMessage message = null;

            if (method == HttpMethod.Get)
            {
                message = new HttpRequestMessage(method, endpoint);
            }

            if (method == HttpMethod.Post)
            {
                message = new HttpRequestMessage(method, endpoint)
                {
                    Content = new JsonSerializedStringContent(payload)
                };
            }

            if (message == null)
            {
                throw new InvalidOperationException($"No request creation way for {method.Method}");
            }

            string token = await _bearerToken.GetAsync();
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return message;
        }

        private async Task<string> SendRequestAsync(
            string endpoint, HttpMethod method, object payload = null)
        {
            HttpRequestMessage request = await RequestAsync(
                method: method,
                endpoint: new PssEndpoint(_config.BaseUrl, endpoint),
                payload: payload);

            return await new WebResponse(_clientFactory, request).AsStringAsync();
        }

        private async Task<WebResponse<T>> SendRequestAsync<T>(
            string endpoint, HttpMethod method, object payload = null)
        {
            try
            {
                string responseString = await SendRequestAsync(endpoint, method, payload);

                return WebResponse<T>.Success(responseString.DeserializeAs<T>());
            }
            catch (HttpRequestException exception)
            {
                return exception.StatusCode switch
                {
                    HttpStatusCode.NotFound => WebResponse<T>.Fail(FailReason.NotFound, exception),
                    HttpStatusCode.RequestTimeout => WebResponse<T>.Fail(FailReason.Timeout, exception),
                    _ => WebResponse<T>.Fail(FailReason.ServerIsNotAvailable, exception)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "The exception during PSS request");
                return WebResponse<T>.Fail(FailReason.UnexpectedBehavior);
            }
        }

        public Task<string> GetPhotoAsync(string id, int w, int h) => SendRequestAsync($"GetPhoto/{id}?w={w}&h={h}", HttpMethod.Get);

        public Task<string> GetSearchableFieldsAsync() => SendRequestAsync($"GetSearchableFields", HttpMethod.Get);

        public Task<string> GetCanonicalIdFromNetworkIdAsync(string networkId) => SendRequestAsync($"GetCanonicalIDFromNetworkID?NetworkID={networkId}", HttpMethod.Get);

        public Task<string> GetCanonicalIdFromBadgeNumberAsync(string badgeNumber) => SendRequestAsync($"GetCanonicalIDFromBadgeNumber?BadgeNumber={badgeNumber}", HttpMethod.Get);

        public Task<string> GetAllPositionsAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllPositions?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllHomeCompaniesAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllHomeCompanies?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllCountriesAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllCountries?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllWorkLocAreasAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllWorkLocAreas?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllWorkLocBuildingsAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllWorkLocBuildings?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllWorkLocFacilitiesAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllWorkLocFacilities?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetPswsEnumerationsAsync(PSWSTypes pSwsType, int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetPSWSEnumerations?searchArgs={pSwsType}&pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetPersonByCanonicalIdAsync(string id) => SendRequestAsync($"GetPersonByCanonicalID/{id}", HttpMethod.Get);

        public Task<string> GetPersonByNetworkIdAsync(string id) => SendRequestAsync($"GetPersonByNetworkID/{id}", HttpMethod.Get);

        public Task<string> GetPersonByBadgeNumberAsync(string id) => SendRequestAsync($"GetPersonByBadgeNumber/{id}", HttpMethod.Get);

        public async Task<WebResponse<PSPersonExtended>> GetPersonExtendedByCanonicalIdAsync(string id)
        {
            return await SendRequestAsync<PSPersonExtended>(
                $"GetPersonExtendedByCanonicalID/{id}", HttpMethod.Get);
        }

        public Task<string> GetPersonExtendedByNetworkIdAsync(string id) => SendRequestAsync($"GetPersonExtendedByNetworkID/{id}", HttpMethod.Get);

        public Task<string> GetPersonExtendedByBadgeNumberAsync(string id) => SendRequestAsync($"GetPersonExtendedByBadgeNumber/{id}", HttpMethod.Get);

        public Task<string> GetApproversAsync(string id) => SendRequestAsync($"GetApprovers/{id}", HttpMethod.Get);

        public Task<string> GetDirectReportsAsync(string id, int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetDirectReports/{id}?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public async Task<WebResponse<PSSearchResult<PSPerson>>> GetPeopleFullTextSearchAsync(
            string searchValue,
            string searchOption = "",
            bool calculateTotalCount = true,
            int pageIndex = 0,
            int pageSize = 10)
        {
            return await SendRequestAsync<PSSearchResult<PSPerson>>(
                $"GetPeopleFullTextSearch?searchValue={searchValue}&pageIndex={pageIndex}&pageSize={pageSize}&calculateTotalCount={calculateTotalCount}",
                HttpMethod.Get);
        }

        public Task<string> PeopleSearchAsync(bool calculateTotalCount = true, int pageIndex = 0, int pageSize = 10, object payload = null) => SendRequestAsync($"PeopleSearch?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Post, payload);

        public Task<string> GetPersonHomeAddressAsync(string id) => SendRequestAsync($"GetPersonHomeAddress/{id}", HttpMethod.Get);

        public Task<string> GetPersonGenderAsync(string id) => SendRequestAsync($"GetPersonGender/{id}", HttpMethod.Get);

        public Task<string> GetPersonDobAsync(string id) => SendRequestAsync($"GetPersonGender/{id}", HttpMethod.Get);

        public Task<string> GetPersonCitizenshipAsync(string id) => SendRequestAsync($"GetPersonGender/{id}", HttpMethod.Get);

        public Task<string> GetCostCenterAsync(string ccCode) => SendRequestAsync($"GetCostCenter?ccCode={ccCode}", HttpMethod.Get);

        public Task<string> GetAllCostCentersAsync(bool openBusinessUnitsOnly, bool empHomeCcOnly, bool includeAfEs, string grpCode, string divCode, string deptCode, int pageIndex = 0, int pageSize = 10)
            => SendRequestAsync($"GetAllCostCenters?OpenBusinessUnitsOnly={openBusinessUnitsOnly}&EmpHomeCCOnly={empHomeCcOnly}&IncludeAFEs={includeAfEs}&grpCode={grpCode}&divCode={divCode}&deptCode={deptCode}&pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetChildCostCentersAsync(string ccCode, bool openBusinessUnitsOnly, bool empHomeCcOnly, bool includeAfEs, int pageIndex = 0, int pageSize = 10)
            => SendRequestAsync($"GetChildCostCenters?ccCode={ccCode}&OpenBusinessUnitsOnly={openBusinessUnitsOnly}&EmpHomeCCOnly={empHomeCcOnly}&IncludeAFEs={includeAfEs}&pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetAllDivisionsAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllDivisions?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetDivisionAsync(string divCode) => SendRequestAsync($"GetDivision?divCode={divCode}", HttpMethod.Get);

        public Task<string> GetAllGroupsAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllGroups?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetGroupAsync(string grpCode) => SendRequestAsync($"GetGroup?grpCode={grpCode}", HttpMethod.Get);

        public Task<string> GetAllDepartmentsAsync(int pageIndex = 0, int pageSize = 10) => SendRequestAsync($"GetAllDepartments?pageIndex={pageIndex}&pageSize={pageSize}", HttpMethod.Get);

        public Task<string> GetDepartmentAsync(string deptCode) => SendRequestAsync($"GetDepartment?deptCode={deptCode}", HttpMethod.Get);
    }
}