using System;
using System.Security.Cryptography;
using System.Text;

namespace OAuth2Example.DataServices.Crypto
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password, ref string salt)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] saltBytes;

                if (string.IsNullOrEmpty(salt))
                {
                    saltBytes = new byte[16];
                    rng.GetBytes(saltBytes);
                    salt = BitConverter.ToString(saltBytes).Replace("-", "");
                }
                else
                {
                    saltBytes = new byte[16];
                    for (int i = 0; i < 16; i++)
                    {
                        saltBytes[i] = Convert.ToByte(salt.Substring(i * 2, 2), 16);
                    }
                }

                byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

                using (var pbkdf2 = new Rfc2898DeriveBytes(pwdBytes, saltBytes, 10000))
                {
                    byte[] result = pbkdf2.GetBytes(64);
                    return BitConverter.ToString(result).Replace("-", "");
                }
            }
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            return hash == HashPassword(password, ref salt);
        }
    }
}
