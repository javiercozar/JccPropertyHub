using System.Collections.Generic;
using System.Threading.Tasks;

namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface ISupplierConnectionsCache {
        bool IsCacheExpired { get; }
        Task<IEnumerable<ISupplierConnector>> GetSupplierConnections();
        Task<bool> AddSupplierConnectors(IEnumerable<ISupplierConnector> supplierConnectors, int secondsToExpireCache);
    }
}