using JccProperty.Domain.HotelJccConnector.Dto;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccProperty.Domain.HotelJccConnector.Mappers {
    public class HotelJccAvailabilitySearchRqMapper : IMapper<SearchAvailabilityRq, HotelJccSearchRq> {
        public HotelJccSearchRq MapFrom(SearchAvailabilityRq source) {
            return new HotelJccSearchRq {
                Hotel = source.HotelId,
                CheckInDate = source.CheckIn,
                NumberOfNights = (source.CheckOut - source.CheckIn).Days - 1,
                Guests = source.NumberOfGuests,
                Currency = source.Currency,
                Rooms = source.NumberOfRooms
            };
        }
    }
}