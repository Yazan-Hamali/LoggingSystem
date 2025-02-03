using LoggingSystem.Data;
using LoggingSystem.Dtos;
using LoggingSystem.Entites;
using LoggingSystem.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace LoggingSystem
{
    public class LogEntryAppService : LoggingSystemAppService, ILogEntryAppService
    {
        private readonly IConfiguration _configuration;
        private readonly DataBaseManager _DBManager;
        private readonly LocalFilesManager _LocalFilesManager;
        private readonly S3BucketManager _S3BucketManager;

        public LogEntryAppService(IConfiguration configuration, DataBaseManager dBManager, LocalFilesManager localFilesManager, S3BucketManager s3BucketManager)
        {
            _configuration = configuration;
            _DBManager = dBManager;
            _LocalFilesManager = localFilesManager;
            _S3BucketManager = s3BucketManager;
        }
        [Authorize(LoggingSystemPermissions.LogEntry.Create)]
        public async Task<LogEntrySharedDto> CreateAsync(LogEntryCreateDto input)
        {
            try
            {
                Check.NotNullOrWhiteSpace(input.Service, nameof(input.Service));
                Check.NotNullOrWhiteSpace(input.Message, nameof(input.Message));
                Check.NotNull(input.TimeStamp, nameof(input.TimeStamp));
                Check.NotNull(input.Level, nameof(input.Level));
                Check.Length(input.Service, nameof(input.Service), LogEntryConsts.ServiceMaxLength, LogEntryConsts.ServiceMinLength);
                Check.Length(input.Message, nameof(input.Message), LogEntryConsts.MessageMaxLength, LogEntryConsts.MessageMinLength);
                switch (_configuration["Storage"])
                {
                    case "DB":
                        return await _DBManager.CreateAsync(input.Service, input.Message, input.TimeStamp.Value, input.Level);
                    case "LocalFiles":
                        return await _LocalFilesManager.CreateAsync(input.Service, input.Message, input.TimeStamp.Value, input.Level);
                    case "S3Bucket":
                        return await _S3BucketManager.CreateAsync(input.Service, input.Message, input.TimeStamp.Value, input.Level);

                    default:
                        throw new BusinessException("Please setup storage provider");
                }

            }
            catch (BusinessException e)
            {
                throw new UserFriendlyException(e.Code);
            }
        }
        [Authorize(LoggingSystemPermissions.LogEntry.Default)]
        public async Task<PagedResultDto<LogEntrySharedDto>> GetListAsync(GetLogEntryInput input)
        {
            try
            {
                switch (_configuration["Storage"])
                {
                    case "DB":
                        var items1= await _DBManager.GetListAsync(
                            service: input.Service,
                            message: input.Message,
                            DateMin: input.StartTime,
                            DateMax: input.EndTime,
                            level: input.Level,
                            sorting: input.Sorting,
                            maxResultCount: input.MaxResultCount,
                            skipCount: input.SkipCount
                            );
                        var total1 = await _DBManager.GetCountAsync(
                            service: input.Service,
                            message: input.Message,
                            DateMin: input.StartTime,
                            DateMax: input.EndTime,
                            level: input.Level
                            );
                        return new PagedResultDto<LogEntrySharedDto>
                        {
                            TotalCount = total1,
                            Items = items1
                        };
                    case "LocalFiles":
                        var res= _LocalFilesManager.GetListAsync(
                            service: input.Service,
                            message: input.Message,
                            DateMin: input.StartTime,
                            DateMax: input.EndTime,
                            level: input.Level,
                            sorting: input.Sorting,
                            maxResultCount: input.MaxResultCount,
                            skipCount: input.SkipCount
                            );
                        return new PagedResultDto<LogEntrySharedDto>
                        {
                            TotalCount = res.Count,
                            Items = res.Items
                        };

                    default:
                        throw new BusinessException("Please setup storage provider");
                }

            }
            catch (BusinessException e)
            {
                throw new UserFriendlyException(e.Code);
            }

        }
    }
}
