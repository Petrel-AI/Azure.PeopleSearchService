using System.Collections.Generic;

namespace Petrel.Utils.Azure.PSS.Core.Models
{
    public class PSPersonExtended
    {
        public PSSubPerson Person { get; init; }

        public List<PSPhone> Phones { get; init; }

        public List<PSEmail> Emails { get; init; }

        public PSPerson Supervisor { get; init; }

        public PSPerson Back2Back { get; init; }
    }
}