using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EclinicBackend.Models;
using EclinicBackend.Data;

namespace EclinicBackend.Services.UserLogHistoryService
{
    public class UserLogHistoryService : IUserLogHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLogHistoryService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserLogHistory> CreateLoginRecord(int userId, string userName, string ipAddress)
        {
            var logEntry = new UserLogHistory
            {
                UserId = userId,
                UserName = userName,
                IpAddress = ipAddress,
                LoginTime = DateTime.UtcNow
            };

            _context.UserLogHistories.Add(logEntry);
            await _context.SaveChangesAsync();
            return logEntry;
        }

        public async Task UpdateLogoutTime(int userId)
        {
            var lastLogin = await _context.UserLogHistories
                .Where(x => x.UserId == userId && x.LogoutTime == null)
                .OrderByDescending(x => x.LoginTime)
                .FirstOrDefaultAsync();

            if (lastLogin != null)
            {
                lastLogin.LogoutTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserLogHistory>> GetUserLogHistory(int? userId = null)
        {
            var query = _context.UserLogHistories.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return await query
                .OrderByDescending(x => x.LoginTime)
                .ToListAsync();
        }
    }
}
