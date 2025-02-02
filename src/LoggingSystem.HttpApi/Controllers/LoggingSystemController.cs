using LoggingSystem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LoggingSystem.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class LoggingSystemController : AbpControllerBase
{
    protected LoggingSystemController()
    {
        LocalizationResource = typeof(LoggingSystemResource);
    }
}
