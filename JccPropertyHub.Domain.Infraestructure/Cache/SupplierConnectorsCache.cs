using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Interfaces;

namespace JccPropertyHub.Domain.Infraestructure.Cache {
    public class SupplierConnectorsCache : ISupplierConnectionsCache {
        private const string supplierConnectionKey = "supplierConnectionKey";
        private readonly MemoryCache memoryCache;

        public bool IsCacheExpired => memoryCache.Get(supplierConnectionKey) == null;
        
        public SupplierConnectorsCache() {
            memoryCache = new MemoryCache(new MemoryCacheOptions {
                
            });
        }
        public Task<IEnumerable<ISupplierConnector>> GetSupplierConnections() {
            var list = memoryCache.Get(supplierConnectionKey) as IEnumerable<ISupplierConnector>;
            return Task.FromResult(list);
        }

        public Task<bool> AddSupplierConnectors(
            IEnumerable<ISupplierConnector> supplierConnectors,
            int secondsToExpireCache) {
            var cacheExpirationOptions = new MemoryCacheEntryOptions {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(secondsToExpireCache),
                Priority = CacheItemPriority.Normal
            };
            
            memoryCache.Set(
                supplierConnectionKey, 
                supplierConnectors,
                cacheExpirationOptions);
            
            return Task.FromResult(true);
        }
    }
}