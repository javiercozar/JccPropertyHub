using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JccProperty.Domain.HotelLegsConnector.Dto;
using JccProperty.Domain.HotelLegsConnector.Mappers;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;

namespace JccProperty.Domain.HotelLegsConnector {
    public  class HotelLegsConnector : ISupplierConnector {
        private readonly HotelLegAvailabilitySearchRqMapper availabilitySearchRqMapper;
        private readonly HotelLegAvailabilitySearchRsMapper availabilitySearchRsMapper;
        private readonly ConnectorConfiguration connectorConfiguration;
        private readonly HttpClient httpClient;

        public HotelLegsConnector(ConnectorConfiguration connectorConfiguration) {
            httpClient = new HttpClient();
            availabilitySearchRqMapper = new HotelLegAvailabilitySearchRqMapper();
            availabilitySearchRsMapper = new HotelLegAvailabilitySearchRsMapper();
            this.connectorConfiguration = connectorConfiguration;
        }

        public string SupplierConnectorName => nameof(HotelLegsConnector);

        public Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq request) {
            var supplierRq = availabilitySearchRqMapper.MapFrom(request);
            var supplierRs = MockSupplierResponse();

            var availabilityRs = availabilitySearchRsMapper.MapFrom(supplierRs);

            return Task.FromResult(availabilityRs);
        }

        private HotelLegSearchRs MockSupplierResponse() {
            return new HotelLegSearchRs {
                Results = new List<Result> {
                    new Result {
                        Room = 1,
                        CanCancel = false,
                        Meal = 1,
                        Price = 123.08
                    },
                    new Result {
                        Room = 1,
                        CanCancel = true,
                        Meal = 1,
                        Price = 150.00
                    },
                    new Result {
                        Room = 2,
                        CanCancel = false,
                        Meal = 1,
                        Price = 148.08
                    },
                    new Result {
                        Room = 2,
                        CanCancel = true,
                        Meal = 2,
                        Price = 150.08
                    }
                }
            };
        }
    }
}