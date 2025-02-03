using LoggingSystem.Dtos;
using LoggingSystem.Entites;
using LoggingSystem.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LoggingSystem.Data
{
    public class S3FilesManager : DomainService
    {
        private readonly string _logDirectory;
        private readonly IConfiguration _configuration;

        public S3FilesManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LogEntrySharedDto> CreateAsync(
            string service,
            string message,
            DateTime time,
            LevelEnum level
            )
        {

            var Item = new LogEntry(
                Guid.NewGuid(),
                service,
                message,
                time,
                level
                );

            string datePath = Item.TimeStamp.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string logFileName = $"{Item.TimeStamp:yyyyMMdd}.log";
            string logFilePath = Path.Combine(_logDirectory, logFileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            var newList = new List<LogEntry>();
            newList.Add(Item);
            if (Path.Exists(logFilePath))
            {
                var oldJson = GetLogsFromFile(logFilePath);
                newList.AddRange(oldJson);

                // Create a log entry as a JSON string
                string logJson = JsonConvert.SerializeObject(newList);
                // Update log entry to the file
                await File.WriteAllTextAsync(logFilePath, logJson + Environment.NewLine);
            }
            else
            {
                // Create a log entry as a JSON string
                string logJson = JsonConvert.SerializeObject(newList);
                // Write log entry to the file
                await File.AppendAllTextAsync(logFilePath, logJson + Environment.NewLine);
            }


            return new LogEntrySharedDto
            {
                Id = Item.Id,
                Level = Item.Level,
                Message = Item.Message,
                Service = Item.Service,
                TimeStamp = Item.TimeStamp,
                StorageType = "LocalFile"
            };
        }
        public LogEntryListDto GetListAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var logs = new List<LogEntry>();

            // Retrieve logs from all files in the log directory
            foreach (var filePath in Directory.GetFiles(_logDirectory, "*.log"))
            {
                var fileLogs = GetLogsFromFile(filePath);
                logs.AddRange(fileLogs);
            }

            var query = ApplyFilter(logs, service, message, DateMin, DateMax, level);
            long total = query.LongCount();
            var res = query.Skip(skipCount).Take(maxResultCount).ToList();

            return new LogEntryListDto
            {
                Count = total,
                Items = res.Select(x => new LogEntrySharedDto
                {
                    Id = x.Id,
                    Level = x.Level,
                    Message = x.Message,
                    Service = x.Service,
                    TimeStamp = x.TimeStamp,
                    StorageType = "LocalFiles",

                }).ToList()
            };
            //query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? LogEntryConsts.GetDefaultSorting(false) : sorting);
            //return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);

        }
        private IEnumerable<LogEntry> ApplyFilter(
            IEnumerable<LogEntry> logs,
            string service = null,
            string message = null,
            DateTime? DateMin = null,
            DateTime? DateMax = null,
            LevelEnum? level = null)
        {
            return logs.WhereIf(!string.IsNullOrWhiteSpace(service), e => e.Service.ToLower().Contains(service.ToLower()!))
                    .WhereIf(!string.IsNullOrWhiteSpace(message), e => e.Message.ToLower().Contains(message.ToLower()!))
                    .WhereIf(level.HasValue, e => e.Level == level.Value)
                    .WhereIf(DateMin.HasValue, e => e.TimeStamp >= DateMin.Value)
                    .WhereIf(DateMax.HasValue, e => e.TimeStamp <= DateMax.Value);
        }
        public List<LogEntry> GetLogs(string level = null, string service = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            var logs = new List<LogEntry>();

            // Retrieve logs from all files in the log directory
            foreach (var filePath in Directory.GetFiles(_logDirectory, "*.log"))
            {
                var fileLogs = GetLogsFromFile(filePath);
                logs.AddRange(fileLogs);
            }

            // Filter logs based on provided query parameters
            if (!string.IsNullOrEmpty(level))
            {
                logs = logs.Where(log => log.Level.ToString().Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(service))
            {
                logs = logs.Where(log => log.Service.Equals(service, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startTime.HasValue)
            {
                logs = logs.Where(log => log.TimeStamp >= startTime.Value).ToList();
            }

            if (endTime.HasValue)
            {
                logs = logs.Where(log => log.TimeStamp <= endTime.Value).ToList();
            }

            return logs;
        }

        // Helper method to deserialize logs from a file
        private List<LogEntry> GetLogsFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            catch
            {
                return new List<LogEntry>(); // In case the file is empty or unreadable
            }
        }







    }
}
