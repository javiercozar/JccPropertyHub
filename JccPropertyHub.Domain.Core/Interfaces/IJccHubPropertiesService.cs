using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Dto;

namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface IJccHubPropertiesService {
        Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq search);
    }
}