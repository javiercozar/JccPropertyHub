using System.Collections.Generic;
using System.Linq;
using JccProperty.Domain.HotelJccConnector.Dto;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccProperty.Domain.HotelJccConnector.Mappers {
    public class HotelLegAvailabilitySearchRsMapper : IMapper<HotelJccSearchRs, SearchAvailabilityRs> {
        public SearchAvailabilityRs MapFrom(HotelJccSearchRs source) {
            if (source == null || !source.Results.Any())
                return new SearchAvailabilityRs {
                    Success = false,
                    Rooms = Enumerable.Empty<Room>()
                };

            var resultsGroupedByRoomId = source.Results.ToLookup(p => p.Room);
            var rooms = GetRoomList(resultsGroupedByRoomId);

            return new SearchAvailabilityRs {
                Success = true,
                Rooms = rooms.AsEnumerable(),
                Errors = Enumerable.Empty<Error>()
            };
        }

        private IEnumerable<Room> GetRoomList(ILookup<int, Result> resultsGroupedByRoomId) {
            return (from room in resultsGroupedByRoomId 
                let rateList = room
                    .Select(rate => new Rate {RatePlanId = nameof(HotelJccConnector), MealPlanId = rate.Meal, Price = rate.Price, IsCancellable = rate.CanCancel})
                    .ToList() 
                select new Room {RoomId = room.Key, Rates = rateList})
                .ToList();
        }
    }
}