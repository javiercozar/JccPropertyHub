using System.Collections.Generic;
using System.Linq;
using JccProperty.Domain.HotelLegsConnector.Dto;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccProperty.Domain.HotelLegsConnector.Mappers {
    public class HotelLegAvailabilitySearchRsMapper : IMapper<HotelLegSearchRs, SearchAvailabilityRs> {
        public SearchAvailabilityRs MapFrom(HotelLegSearchRs source) {
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

        private List<Room> GetRoomList(ILookup<int, Result> resultsGroupedByRoomId) {
            var rooms = new List<Room>();

            foreach (var room in resultsGroupedByRoomId) {
                var rateList = new List<Rate>();

                foreach (var rate in room)
                    rateList.Add(new Rate {
                        RatePlanId = nameof(HotelLegsConnector),
                        MealPlanId = rate.Meal,
                        Price = rate.Price,
                        IsCancellable = rate.CanCancel
                    });

                rooms.Add(new Room {
                    RoomId = room.Key,
                    Rates = rateList
                });
            }

            return rooms;
        }
    }
}