using Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Utils{
    public class PasswordHelper {
        public static string HashPassword(string password){
            var passwordHasher = new PasswordHasher<object>();
            #pragma warning disable CS8625 // Estou silenciando o Null warning, não vai dar B.O
            return passwordHasher.HashPassword(null, password);
            #pragma warning restore CS8625 
        }

        public static bool VerifyPassword(string hashedPassword, string rawPassword){
            var passwordHasher = new PasswordHasher<object>();
            #pragma warning disable CS8625
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, rawPassword);
            #pragma warning restore CS8625

            return result == PasswordVerificationResult.Success;
        }
    }

}