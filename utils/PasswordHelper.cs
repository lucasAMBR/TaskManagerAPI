using Microsoft.AspNetCore.Identity;

namespace Utils{
    public class PasswordHelper{
        public static string HashPassword<T>(T user, string password) where T : class{
            var passwordHasher = new PasswordHasher<T>();
            return passwordHasher.HashPassword(user, password); 
        }

        public static bool VerifyPassword<T>(T user, string hashedPassword, string rawPassword) where T : class{
            var passwordHasher = new PasswordHasher<T>();
            var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, rawPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}