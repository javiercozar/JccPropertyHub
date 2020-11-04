using System;

namespace JccProperty.Domain.HotelLegsConnector.Dto {
    public class HotelLegSearchRq {
        public int Hotel { get; set; }
        public DateTime CheckInDate { get; set; }
        public int NumberOfNights { get; set; }
        public int Guests { get; set; }
        public int Rooms { get; set; }
        public string Currency { get; set; }
    }
}