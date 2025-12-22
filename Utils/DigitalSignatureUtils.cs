using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace DA_N6.Utils
{
    public static class DigitalSignatureHelper
    {
        // 1. Tạo cặp khóa (Chỉ chạy 1 lần để admin lấy Key, sau đó lưu lại dùng dần)
        public static void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                privateKey = rsa.ToXmlString(true);  // Chứa cả Private (để Ký)
                publicKey = rsa.ToXmlString(false);  // Chỉ chứa Public (để Khách hàng kiểm tra)
            }
        }

        // 2. Hàm Ký số (Dùng Private Key)
        public static string SignData(string dataContent, string privateKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyXml);
                byte[] dataBytes = Encoding.UTF8.GetBytes(dataContent);
                // Hash dữ liệu bằng SHA256 rồi Ký
                byte[] signatureBytes = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"));
                return Convert.ToBase64String(signatureBytes);
            }
        }

        // 3. Hàm Kiểm tra chữ ký (Dùng Public Key)
        public static bool VerifyData(string dataContent, string signatureBase64, string publicKeyXml)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKeyXml);
                    byte[] dataBytes = Encoding.UTF8.GetBytes(dataContent);
                    byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

                    return rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA256"), signatureBytes);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}