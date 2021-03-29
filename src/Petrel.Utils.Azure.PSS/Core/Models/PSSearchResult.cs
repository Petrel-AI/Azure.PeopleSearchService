using System.Collections.Generic;

namespace Petrel.Utils.Azure.PSS.Core.Models
{
    public class PSSearchResult<T>
    {
        public IReadOnlyCollection<T> Data { get; init; } = new List<T>();

        public PSMetadata Metadata { get; init; }
    }
}