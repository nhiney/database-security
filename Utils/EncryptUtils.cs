using System;
using System.Text;

namespace DA_N6.Utils
{
    internal class EncryptUtils
    {
        public const int Key = 7;
        public const int Modulus = 75;

        public static int GetEffectiveKey()
        {
            return (Key * Key) % Modulus;
        }
        // Bảng ký tự
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&*()-_=+.";

        /// Mã hoá nhân (Multiplicative Cipher)
        public static string EncryptMultiplicative(string input, int key)
        {
            StringBuilder result = new StringBuilder();
            int m = Alphabet.Length;

            foreach (char c in input)
            {
                int index = Alphabet.IndexOf(c);
                if (index >= 0)
                {
                    int newIndex = (index * key) % m;
                    result.Append(Alphabet[newIndex]);
                }
                else
                {
                    // Giữ nguyên nếu không có trong bảng
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// Giải mã nhân (Multiplicative Cipher)
        public static string DecryptMultiplicative(string input, int key)
        {
            StringBuilder result = new StringBuilder();
            int m = Alphabet.Length;

            // Tìm nghịch đảo của key theo modulo m
            int inverseKey = ModInverse(key, m);
            if (inverseKey == -1)
                throw new Exception($"Không có nghịch đảo của {key} mod {m}");

            foreach (char c in input)
            {
                int index = Alphabet.IndexOf(c);
                if (index >= 0)
                {
                    int newIndex = (index * inverseKey) % m;
                    result.Append(Alphabet[newIndex]);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        
        }

        /// Tìm nghịch đảo modulo
        public static int ModInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
                if ((a * x) % m == 1)
                    return x;
            return -1; // Không có nghịch đảo
        }
    }
}
