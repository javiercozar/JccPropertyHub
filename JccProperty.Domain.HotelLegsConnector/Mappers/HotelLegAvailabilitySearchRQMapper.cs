using JccProperty.Domain.HotelLegsConnector.Dto;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccProperty.Domain.HotelLegsConnector.Mappers {
    public class HotelLegAvailabilitySearchRqMapper : IMapper<SearchAvailabilityRq, HotelLegSearchRq> {
        public HotelLegSearchRq MapFrom(SearchAvailabilityRq source) {
            return new HotelLegSearchRq {
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