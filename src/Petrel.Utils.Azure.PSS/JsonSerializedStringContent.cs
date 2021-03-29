using System.Net.Http;
using System.Text;
using MG.Utils.Helpers;

namespace Petrel.Utils.Azure.PSS
{
    internal class JsonSerializedStringContent : StringContent
    {
        private const string MediaType = "application/json";

        public JsonSerializedStringContent(object payload)
            : base(payload.AsJson(), Encoding.UTF8, MediaType)
        {
        }
    }
}