using Json.Protector.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json.Protector
{
    public static class JsonProtectorServiceExtensions
    {
        public static IServiceCollection AddJsonProtector(this IServiceCollection services, Action<JsonProtectorOptions> options)
        {
            if (options != null)
            {
                ConfigOptions(services, options);

            }

            services.AddSingleton<IEncryptionProvider, AesProvider>();


            return services;
        }

        private static void ConfigOptions(IServiceCollection services, Action<JsonProtectorOptions>? options)
        {
            var configOptions = new JsonProtectorOptions();
            options?.Invoke(configOptions);
            services.TryAddSingleton(Options.Create(configOptions));
        }
    }
}
