using System.Linq;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;
using JccPropertyHub.Domain.Core.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JccPropertyHub.Domain.Core {
    public static class DependencyInyector {
        public static IServiceCollection AddCoreValidadators(this IServiceCollection service) {
            service.AddTransient<IValidator<SearchAvailabilityRq, ResponseValidator>, SearchAvailabilityRqValidator>();

            return service;
        }

        public static IServiceCollection AddHubService(this IServiceCollection service, IConfiguration configuration) {
            service.AddTransient<IJccHubPropertiesService, JccHubPropertiesService>();

            service.AddTransient(p => {
                var pathConnectors = configuration.GetSection("ConnectorsConfiguration:PathConnectors").Value;
                var expirationTime =
                    int.Parse(configuration.GetSection("ConnectorsConfiguration:ExpirationTime").Value);
                var connectors = configuration
                    .GetSection("ConnectorsConfiguration:Connector")
                    .GetChildren()
                    .Select(config => new ConnectorConfiguration {
                        ConnectorName = config.GetSection("ConnectorName").Value,
                        Url = config.GetSection("Url").Value,
                        User = config.GetSection("User").Value,
                        Password = config.GetSection("Password").Value
                    });

                return new ConnectorsConfiguration {
                    PathConnectors = pathConnectors,
                    ExpirationTime = expirationTime,
                    Connectors = connectors
                };
            });

            return service;
        }
    }
}