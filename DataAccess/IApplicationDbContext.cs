using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<PhysicalLocation> PhysicalLocations { get; set; }
        DbSet<UserHistory> UserHistory { get; set; }
        DbSet<User> Users { get; set; }
    }
}