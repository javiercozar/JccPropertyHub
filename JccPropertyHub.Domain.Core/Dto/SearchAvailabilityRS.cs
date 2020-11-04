using System.Collections.Generic;

namespace JccPropertyHub.Domain.Core.Dto {
    public class SearchAvailabilityRs : Response {
        public IEnumerable<Room> Rooms { get; set; }
    }
}