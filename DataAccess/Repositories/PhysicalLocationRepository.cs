using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PhysicalLocationRepository : IPhysicalLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public PhysicalLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PhysicalLocation?> GetPhysicalLocationAsync(int physicalLocationId)
        {
            return await _context.PhysicalLocations.FirstOrDefaultAsync(pl => pl.Id == physicalLocationId);
        }

        public async Task<List<PhysicalLocation>?> GetPhysicalLocationsAsync()
        {
            return await _context.PhysicalLocations.ToListAsync();
        }
    }
}
