using LoggingSystem.Dtos;
using LoggingSystem.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LoggingSystem.Entites
{
    public class LogEntryManager : DomainService
    {
        private readonly ILogEntryRepo _LogEntryRepository;
        private readonly IConfiguration _configuration;
        private readonly DataBaseManager _DBManager;
        private readonly LocalFilesManager _LocalFilesManager;

        public LogEntryManager(ILogEntryRepo logEntryRepository, IConfiguration configuration, DataBaseManager dBManager, LocalFilesManager localFilesManager)
        {
            _LogEntryRepository = logEntryRepository;
            _configuration = configuration;
            _DBManager = dBManager;
            _LocalFilesManager = localFilesManager;
        }

        public async Task<LogEntrySharedDto> CreateAsync(
            string service,
            string message,
            DateTime time,
            LevelEnum level
            )
        {
            Check.NotNullOrWhiteSpace(service, nameof(service));
            Check.NotNullOrWhiteSpace(message, nameof(message));
            Check.NotNull(time, nameof(time));
            Check.NotNull(level, nameof(level));
            Check.Length(service, nameof(service), LogEntryConsts.ServiceMaxLength, LogEntryConsts.ServiceMinLength);
            Check.Length(message, nameof(message), LogEntryConsts.MessageMaxLength, LogEntryConsts.MessageMinLength);
            switch(_configuration["Storage"])
            {
                case "DB":
                    return await _DBManager.CreateAsync(service, message, time, level);
                case "LocalFiles":
                    return await _LocalFilesManager.CreateAsync(service, message, time, level);

                default:
                    throw new BusinessException("Please setup storage provider");
            }
        }

        
        public async Task<List<LogEntrySharedDto>> GetListAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            switch (_configuration["Storage"])
            {
                case "DB":
                    return await _DBManager.GetListAsync(
                    service: service,
                    message: message,
                    DateMin: DateMin,
                    DateMax: DateMax,
                    level: level,
                    sorting: sorting,
                    maxResultCount: maxResultCount,
                    skipCount: skipCount
                    );
                case "LocalFiles":
                    return  _LocalFilesManager.GetListAsync(
                    service: service,
                    message: message,
                    DateMin: DateMin,
                    DateMax: DateMax,
                    level: level,
                    sorting: sorting,
                    maxResultCount: maxResultCount,
                    skipCount: skipCount
                    );

                default:
                    throw new BusinessException("Please setup storage provider");
            }
        }
        public async Task<long> GetCountAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, CancellationToken cancellationToken = default)
        {
            switch (_configuration["Storage"])
            {
                case "DB":
                    return await _DBManager.GetCountAsync(
                    service: service,
                    message: message,
                    DateMin: DateMin,
                    DateMax: DateMax,
                    level: level
                    );
                case "LocalFiles":
                    return _LocalFilesManager.GetCountAsync(
                    service: service,
                    message: message,
                    DateMin: DateMin,
                    DateMax: DateMax,
                    level: level
                    );

                default:
                    throw new BusinessException("Please setup storage provider");
            }
        }

    }
}
