using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IApplicationLoggingRepository
    {
        Task CreateApplicationLoggingAsync(ApplicationLogging logEntry);
    }
}