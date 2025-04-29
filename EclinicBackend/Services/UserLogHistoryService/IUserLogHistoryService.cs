using System.Collections.Generic;
using System.Threading.Tasks;
using EclinicBackend.Models;
using EclinicBackend.Data;

namespace EclinicBackend.Services.UserLogHistoryService
{
    public interface IUserLogHistoryService
    {
        Task<UserLogHistory> CreateLoginRecord(int userId, string userName, string ipAddress);
        Task UpdateLogoutTime(int userId);
        Task<IEnumerable<UserLogHistory>> GetUserLogHistory(int? userId = null);
    }
}
