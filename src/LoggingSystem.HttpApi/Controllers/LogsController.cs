using LoggingSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LoggingSystem.Controllers
{
    [RemoteService(Name = "LogsService")]
    [Area("LogsService")]
    [ControllerName("Logs")]
    [Route("api/logs")]
    public class LogsController : LoggingSystemController
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
    }
}
