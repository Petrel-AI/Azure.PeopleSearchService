using MG.Utils.Abstract;
using MG.Utils.Abstract.NonNullableObjects;

namespace Petrel.Utils.Azure.PSS
{
    public class PssEndpoint
    {
        private readonly string _fullEndpoint;

        public PssEndpoint(NonNullableString baseUrlArg, string endpointArg)
        {
            var baseUrl = AddSlashToEnd(baseUrlArg.ThrowIfNull(nameof(baseUrlArg)).ToString());
            var endpoint = RemoveFirstSlashIfExists(endpointArg.ThrowIfNullOrEmpty(nameof(endpointArg)));

            _fullEndpoint = baseUrl + endpoint;
        }

        public override string ToString() => _fullEndpoint;

        public static implicit operator string(PssEndpoint endpoint) => endpoint!.ToString();

        private static string RemoveFirstSlashIfExists(string value)
        {
            return value.StartsWith("/") ? value.TrimStart('/') : value;
        }

        private static string AddSlashToEnd(string value)
        {
            return value.EndsWith("/") ? value : value + "/";
        }
    }
}