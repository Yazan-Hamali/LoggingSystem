using LoggingSystem.Entites;
using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoggingSystem.EntityFrameworkCore
{
    public class EfCoreLogEntryRepo : EfCoreRepository<LoggingSystemDbContext, LogEntry, Guid>, ILogEntryRepo
    {
        public EfCoreLogEntryRepo(IDbContextProvider<LoggingSystemDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> GetCountAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), service, message, DateMin, DateMax, level);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<LogEntry>> GetListAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), service, message, DateMin, DateMax, level);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? LogEntryConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        protected virtual IQueryable<LogEntry> ApplyFilter(
            IQueryable<LogEntry> query,
            string? service = null,
            string? message = null,
            DateTime? DateMin = null,
            DateTime? DateMax = null,
            LevelEnum? level = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(service), e => e.Service.ToLower().Contains(service.ToLower()!))
                    .WhereIf(!string.IsNullOrWhiteSpace(message), e => e.Message.ToLower().Contains(message.ToLower()!))
                    .WhereIf(level.HasValue, e => e.Level == level.Value)
                    .WhereIf(DateMin.HasValue, e => e.TimeStamp >= DateMin.Value)
                    .WhereIf(DateMax.HasValue, e => e.TimeStamp <= DateMax.Value);
        }
    }
}
