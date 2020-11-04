using System.Collections.Generic;

namespace JccPropertyHub.Domain.Core.Services {
    public class ConnectorsConfiguration {
        public IEnumerable<ConnectorConfiguration> Connectors { get; set; }
        public string PathConnectors { get; set; }
        public int ExpirationTime { get; set; }
    }
}