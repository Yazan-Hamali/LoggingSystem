using AutoMapper;
using LoggingSystem.Dtos;
using LoggingSystem.Entites;
using Volo.Abp.AutoMapper;

namespace LoggingSystem;

public class LoggingSystemApplicationAutoMapperProfile : Profile
{
    public LoggingSystemApplicationAutoMapperProfile()
    {
        CreateMap<LogEntry, LogEntrySharedDto>().Ignore(x => x.StorageType);
    }
}
