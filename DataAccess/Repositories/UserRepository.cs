using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task CreateUserAsync(User userEntity)
        {           
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();                    
        }
    }
}
