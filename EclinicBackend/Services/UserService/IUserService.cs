using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services
{
    public interface IUserService
    {
        string GetMyName();
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User?> GetByEmail(string email);

        Task Update(UserUpdateDto user);
        Task Delete(int id);
    }
}