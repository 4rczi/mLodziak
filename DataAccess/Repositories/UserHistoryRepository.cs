using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserHistory>> GetUserHistoryAsync(string userId)
        {
            return await _context.UserHistory.Where(ulh => ulh.UserId == userId).ToListAsync();
        }

        public async Task CreateUserHistoryAsync(UserHistory userHistoryEntity)
        {
            await _context.UserHistory.AddAsync(userHistoryEntity);
            await _context.SaveChangesAsync();
        }
    }
}
