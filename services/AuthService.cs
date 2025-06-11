using System.Threading.Tasks;
using Data;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Utils;

namespace Services{

    public class AuthService : IAuthService{

        private readonly AppDbContext _context;

        public AuthService(AppDbContext context){
            _context = context;
        }
        public async Task<LoginResultDTO?> LoginAsync(string email, string password){
            
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.Email == email);

            if(manager != null && PasswordHelper.VerifyPassword(manager.Password, password)){
                var token = TokenService.GenerateToken(manager.Id);

                return new LoginResultDTO{
                    Token = token,
                    Id = manager.Id,
                    Name = manager.Name,
                    Role = "MNG"
                };
            }
            var dev = await _context.Devs.FirstOrDefaultAsync(d => d.Email == email);

            if (dev != null && PasswordHelper.VerifyPassword(dev.Password, password))
            {
                var token = TokenService.GenerateToken(dev.Id);

                return new LoginResultDTO
                {
                    Token = token,
                    Id = dev.Id,
                    Name = dev.Name,
                    Role = "DEV"
                };
            }

            return null;
        }

    }

}