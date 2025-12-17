using System.Security.Cryptography;
using System.Text;

namespace DA_N6.Services
{
    public static class AsymmetricCryptoService
    {
        public static byte[] Encrypt(string plainText, RSAParameters publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
            }
        }

        public static string Decrypt(byte[] cipherText, RSAParameters privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                var bytes = rsa.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
