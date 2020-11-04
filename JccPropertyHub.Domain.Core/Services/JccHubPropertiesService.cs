using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Validators;
using Newtonsoft.Json;

namespace JccPropertyHub.Domain.Core.Services {
    public class JccHubPropertiesService : IJccHubPropertiesService {
        private const int DefaultTimeout = 5000;
        private readonly ConnectorsConfiguration configuration;
        private readonly ILogStorage<Log> logStorage;
        private readonly IValidator<SearchAvailabilityRq, ResponseValidator> searchAvailabilityValidator;
        private readonly ISupplierConnectorManager supplierConnectorManager;

        public JccHubPropertiesService(
            IValidator<SearchAvailabilityRq, ResponseValidator> searchAvailabilityValidator,
            ISupplierConnectorManager supplierConnectorManager,
            ConnectorsConfiguration configuration,
            ILogStorage<Log> logStorage) {
            this.searchAvailabilityValidator = searchAvailabilityValidator;
            this.supplierConnectorManager = supplierConnectorManager;
            this.configuration = configuration;
            this.logStorage = logStorage;
        }

        /// <summary>
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq search) {
            var requestValidated = searchAvailabilityValidator.Validate(search);

            if (!requestValidated.IsValid) 
                return await InvalidRequest(requestValidated);

            var rooms = await GetRoomsFromSuppliers(search);
            var response = new SearchAvailabilityRs {
                Success = true,
                Rooms = rooms,
                Errors = Enumerable.Empty<Error>()
            };

            var save = await logStorage.Save(new Log {
                Request =  JsonConvert.SerializeObject(search),
                Response =  JsonConvert.SerializeObject(response),
                CreatetionDate = DateTime.UtcNow
            });

            return response;
        }

        private async Task<IEnumerable<Room>> GetRoomsFromSuppliers(SearchAvailabilityRq search) {
            var supplierConnectors = await supplierConnectorManager.GetSupplierConnectors(configuration);
            var roomsBag = new ConcurrentBag<SearchAvailabilityRs>();
            var tokenSource = new CancellationTokenSource(DefaultTimeout);

            await Task.WhenAll(supplierConnectors.Select(supplier => Task.Run(async () => {
                var roomList = await supplier.SearchAvailability(search);
                roomsBag.Add(roomList);
            }, tokenSource.Token))
                .ToArray());

            var rooms = roomsBag
                .Where(p => p.Success)
                .SelectMany(p => p.Rooms);

            return rooms;
        }

        private static Task<SearchAvailabilityRs> InvalidRequest(ResponseValidator requestValidated) {
            return Task.FromResult(new SearchAvailabilityRs {
                Success = requestValidated.IsValid,
                Errors = requestValidated.Errors,
                Rooms = Enumerable.Empty<Room>()
            });
        }
    }
}