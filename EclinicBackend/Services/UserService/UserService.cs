using System.Security.Claims;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Helpers;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public UserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {

                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            }
            return result ?? string.Empty;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Update(UserUpdateDto model)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == model.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User {model.UserId} not found");

            // validate
            bool emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            bool emailExists = await _context.Users.AnyAsync(x => x.Email == model.Email && x.UserID != model.UserId);
            if (emailChanged && emailExists)
                throw new Exception("User with the email '" + model.Email + "' already exists");


            if (!string.IsNullOrEmpty(model.Password))
            {
                PasswordHashingHelper.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            if (model.PractitionerId.HasValue) user.PractitionerId = model.PractitionerId.Value;
            user.Role = model.Role;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = GetUserId();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id) ?? throw new KeyNotFoundException($"User {id} not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

        }
    }
}


