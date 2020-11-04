using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using MongoDB.Driver;

namespace JccPropertyHub.Domain.Infraestructure.Logs {
    public class LogStorage : ILogStorage<Log> {
        private readonly LogDBConfiguration logDbConfiguration;
        private IMongoCollection<Log> logs;

        public LogStorage(LogDBConfiguration logDbConfiguration) {
            this.logDbConfiguration = logDbConfiguration;
            var client = new MongoClient(logDbConfiguration.ConnectionString);
            var database = client.GetDatabase(logDbConfiguration.DatabaseName);
            logs = database.GetCollection<Log>(logDbConfiguration.CollectionName);
        }
        
        public async Task<IEnumerable<Log>> GetLogsFromRangeDate(DateTime startDate, DateTime endDate) {
            try {
                return await logs
                    .Find(p => startDate <= p.CreatetionDate && p.CreatetionDate <= endDate)
                    .ToListAsync();;
            }
            catch (Exception e) {
                return Enumerable.Empty<Log>();
            }
        }

        public async Task<bool> Save(Log log) {
            try {
                await logs.InsertOneAsync(log);
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
    }
}