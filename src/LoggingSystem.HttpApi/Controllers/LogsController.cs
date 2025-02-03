using LoggingSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LoggingSystem.Controllers
{
    [RemoteService(Name = "LogEntry")]
    [Area("LogEntry")]
    [ControllerName("LogEntry")]
    [Route("api/app/log-entrys")]
    public class LogsController : AbpController
    {
        private readonly ILogEntryAppService _logEntryAppService;

        public LogsController(ILogEntryAppService logEntryAppService)
        {
            _logEntryAppService = logEntryAppService;
        }

        [HttpGet]
        //[SwaggerOperation(summary: "Get List of Items with paging and filters", description: "Get List of Items with paging and filters")]
        public virtual Task<PagedResultDto<LogEntrySharedDto>> GetListAsync(GetLogEntryInput input)
        {

            return _logEntryAppService.GetListAsync(input);
        }
        [HttpPost]
        public virtual Task<LogEntrySharedDto> CreateAsync(LogEntryCreateDto input)
        {
            return _logEntryAppService.CreateAsync(input);
        }
    }
}
