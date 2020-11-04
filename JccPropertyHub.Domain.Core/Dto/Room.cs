using System.Collections.Generic;

namespace JccPropertyHub.Domain.Core.Dto {
    public class Room {
        public int RoomId { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}