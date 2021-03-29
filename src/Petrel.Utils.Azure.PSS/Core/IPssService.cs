using System.Threading.Tasks;
using MG.Utils.AspNetCore.WebResponse;
using Petrel.Utils.Azure.PSS.Core.Models;

namespace Petrel.Utils.Azure.PSS.Core
{
    public interface IPssService
    {
        Task<WebResponse<PSSearchResult<PSPerson>>> GetPeopleFullTextSearchAsync(
            string searchValue,
            string searchOption = "",
            bool calculateTotalCount = true,
            int pageIndex = 0,
            int pageSize = 10);

        Task<WebResponse<PSPersonExtended>> GetPersonExtendedByCanonicalIdAsync(string id);
    }
}