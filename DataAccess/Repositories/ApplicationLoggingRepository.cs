using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ApplicationLoggingRepository : IApplicationLoggingRepository
    {
        private readonly ApplicationDbContext _context;


        public ApplicationLoggingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateApplicationLoggingAsync(ApplicationLogging logEntry)
        {
            await _context.ApplicationLogging.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
