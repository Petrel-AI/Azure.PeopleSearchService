using System;
using System.Collections.Concurrent;
using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Petrel.Utils.Azure.PSS.Core
{
    public interface IPssSettings
    {
        NonNullableString BaseUrl { get; }

        NonNullableString Authority { get; }

        NonNullableString Resource { get; }

        ClientCredential ClientCredential { get; }

        ConcurrentDictionary<string, string> Tokens { get; }

        ConcurrentDictionary<string, DateTime?> TokensDatetime { get; }
    }
}