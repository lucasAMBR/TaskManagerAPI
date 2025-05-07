using System.Threading.Tasks;
using Data;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Services{

    public class AuthService : IAuthService{

        private readonly AppDbContext _context;

        public AuthService(AppDbContext context){
            _context = context;
        }
        public async Task<LoginResultDTO?> LoginAsync(string email, string password){
            
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.Email == email);

            if(manager != null && PasswordHelper.VerifyPassword(manager, manager.Password, password)){
                var token = TokenService.GenerateToken(manager.Id);

                return new LoginResultDTO{
                    Token = token,
                    Name = manager.Name,
                    Role = "MNG"
                };
            }

            var dev = await _context.Devs.FirstOrDefaultAsync(d => d.Email == email);

            if(dev != null && PasswordHelper.VerifyPassword(dev, dev.Password, password)){
                var token = TokenService.GenerateToken(dev.Id);

                return new LoginResultDTO{
                    Token = token,
                    Name = dev.Name,
                    Role = "DEV"
                };
            }

            return null;
        }

    }

}