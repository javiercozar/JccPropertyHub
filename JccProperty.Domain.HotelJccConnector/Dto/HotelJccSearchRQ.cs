using System;

namespace JccProperty.Domain.HotelJccConnector.Dto {
    public class HotelJccSearchRq {
        public int Hotel { get; set; }
        public DateTime CheckInDate { get; set; }
        public int NumberOfNights { get; set; }
        public int Guests { get; set; }
        public int Rooms { get; set; }
        public string Currency { get; set; }
    }
}