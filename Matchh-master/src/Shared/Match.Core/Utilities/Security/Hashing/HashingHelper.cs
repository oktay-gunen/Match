using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Match.Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            var passwordHashB64 = Convert.FromBase64String(passwordHash);
            var passwordSaltB64= Convert.FromBase64String(passwordSalt);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSaltB64))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHashB64[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

