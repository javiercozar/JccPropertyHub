using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Dto;

namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface ISupplierConnector {
        public string SupplierConnectorName { get; }
        public Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq request);
    }
}