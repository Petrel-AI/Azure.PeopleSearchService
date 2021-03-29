using System;
using Microsoft.Extensions.DependencyInjection;
using Petrel.Utils.Azure.PSS.Core;

namespace Petrel.Utils.Azure.PSS
{
    public static class AzurePssServiceExtensions
    {
        public static IServiceCollection RegisterPssServices<T>(this IServiceCollection services)
            where T : IPssSettings
        {
            services.AddTransient(typeof(IPssSettings), typeof(T));
            services.AddTransient<IPssService, PssService>();
            services.AddTransient<BearerToken>();

            return services;
        }
    }
}