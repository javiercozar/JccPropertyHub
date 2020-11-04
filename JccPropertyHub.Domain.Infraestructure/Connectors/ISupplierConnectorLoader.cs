using System.Collections.Generic;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;

namespace JccPropertyHub.Domain.Infraestructure.Connectors {
    public interface ISupplierConnectorLoader {
        IEnumerable<ISupplierConnector> GetInstances(ConnectorsConfiguration configuration);
    }
}