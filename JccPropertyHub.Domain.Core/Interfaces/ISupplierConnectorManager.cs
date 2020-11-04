using System.Collections.Generic;
using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Services;

namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface ISupplierConnectorManager {
        Task<IEnumerable<ISupplierConnector>> GetSupplierConnectors(ConnectorsConfiguration connectorsConfiguration);
    }
}