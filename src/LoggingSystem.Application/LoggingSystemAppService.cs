using System;
using System.Collections.Generic;
using System.Text;
using LoggingSystem.Localization;
using Volo.Abp.Application.Services;

namespace LoggingSystem;

/* Inherit your application services from this class.
 */
public abstract class LoggingSystemAppService : ApplicationService
{
    protected LoggingSystemAppService()
    {
        LocalizationResource = typeof(LoggingSystemResource);
    }
}
