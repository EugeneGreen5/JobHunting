using System.Security.Cryptography;
using System.Text;

namespace JobHunting.Services.Password;

public class SHA512EncryptionPassword : IPassword
{
    public string Encryption(string password)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha512.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
