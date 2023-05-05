using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Analyzing_and_profiling_tools
{
    public class FirstTask
    {
        public static string GeneratePasswordHashUsingSaltOldImplementation(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public static string GeneratePasswordHashUsingSaltNewImplementation(string passwordText, byte[] salt)
        {
            const int iterate = 10000;
            const int bytesCount = 20;
            const int magicNumber = 16;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            var hash = pbkdf2.GetBytes(bytesCount);
            var hashBytes = new byte[magicNumber + bytesCount];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, magicNumber);
            Buffer.BlockCopy(hash, 0, hashBytes, magicNumber, bytesCount);

            return Convert.ToBase64String(hashBytes);
        }

    }
}
