using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Infraestructure.Cache;
using JccPropertyHub.Domain.Infraestructure.Connectors;
using JccPropertyHub.Domain.Infraestructure.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JccPropertyHub.Domain.Infraestructure {
    public static class DependencyInyector {
        public static IServiceCollection AddInfraestructureManagement(this IServiceCollection service, IConfiguration configuration) {
            service.AddTransient<ISupplierConnectorManager, SupplierConnectorManager>();
            service.AddTransient<ISupplierConnectorLoader, SupplierConnectorLoader>();
            service.AddTransient<AssemblyLoader>();
            service.AddTransient<ILogStorage<Log>, LogStorage>();
            service.AddTransient((p) => new LogDBConfiguration {
                ConnectionString = configuration.GetSection("ConnectionStrings:LogsDB").Value,
                DatabaseName = configuration.GetSection("ConnectionStrings:DataBaseName").Value,
                CollectionName = configuration.GetSection("ConnectionStrings:CollectionName").Value
            });

            service.AddSingleton<ISupplierConnectionsCache, SupplierConnectorsCache>();

            return service;
        }
    }
}