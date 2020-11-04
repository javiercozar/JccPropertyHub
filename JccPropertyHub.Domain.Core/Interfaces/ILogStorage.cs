using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JccPropertyHub.Domain.Core.Interfaces {
    public interface ILogStorage<TLog> {
        Task<IEnumerable<TLog>> GetLogsFromRangeDate(DateTime startDate, DateTime endDate);
        Task<bool> Save(TLog log);
    }
}