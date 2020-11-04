using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;

namespace JccPropertyHub.Domain.Infraestructure.Connectors {
    public class SupplierConnectorManager : ISupplierConnectorManager {
        private readonly ISupplierConnectionsCache supplierConnectionsCache;
        private readonly ISupplierConnectorLoader supplierConnectorLoader;

        public SupplierConnectorManager(
            ISupplierConnectorLoader supplierConnectorLoader,
            ISupplierConnectionsCache supplierConnectionsCache) {
            this.supplierConnectorLoader = supplierConnectorLoader;
            this.supplierConnectionsCache = supplierConnectionsCache;
        }

        public Task<IEnumerable<ISupplierConnector>> GetSupplierConnectors(
            ConnectorsConfiguration connectorsConfiguration) {
            if (!supplierConnectionsCache.IsCacheExpired)
                return supplierConnectionsCache.GetSupplierConnections();

            var suppliers = supplierConnectorLoader.GetInstances(connectorsConfiguration);
            var supplierConnectors = suppliers.ToArray();
            supplierConnectionsCache.AddSupplierConnectors(supplierConnectors, connectorsConfiguration.ExpirationTime);

            return Task.FromResult(supplierConnectors.AsEnumerable());
        }
    }
}