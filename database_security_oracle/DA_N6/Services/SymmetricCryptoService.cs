using System.Security.Cryptography;
using System.Text;

namespace DA_N6.Services
{
    public static class SymmetricCryptoService
    {
        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var encryptor = aes.CreateEncryptor())
                {
                    var bytes = Encoding.UTF8.GetBytes(plainText);
                    return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                }
            }
        }

        public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor())
                {
                    var bytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }
    }
}
