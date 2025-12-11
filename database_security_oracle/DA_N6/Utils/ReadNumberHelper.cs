using System;
using System.Linq;

namespace DA_N6.Utils
{
    public static class ReadNumberHelper
    {
        private static readonly string[] ChuSo = new string[] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        private static readonly string[] Tien = new string[] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        // Hàm chính để gọi từ bên ngoài
        public static string ReadNumber(decimal total)
        {
            try
            {
                long number = Convert.ToInt64(total);
                if (number == 0) return "Không đồng";

                string strNumber = number.ToString();
                string result = "";
                int ones, tens, hundreds;
                int positionDigit = strNumber.Length; // Vị trí của chữ số đang xét

                // Nếu số quá lớn
                if (positionDigit > 89) return "Số quá lớn";

                // Nhóm 3 chữ số (hàng trăm, chục, đơn vị)
                int group = 0;
                int i = 0;

                // Chạy vòng lặp từ phải sang trái
                while (positionDigit > 0)
                {
                    // Lấy 3 số cuối
                    ones = int.Parse(strNumber.Substring(Math.Max(0, positionDigit - 1), 1));
                    positionDigit--;
                    tens = positionDigit > 0 ? int.Parse(strNumber.Substring(Math.Max(0, positionDigit - 1), 1)) : -1;
                    positionDigit--;
                    hundreds = positionDigit > 0 ? int.Parse(strNumber.Substring(Math.Max(0, positionDigit - 1), 1)) : -1;
                    positionDigit--;

                    // Đọc 3 số này
                    string groupStr = ReadTriple(hundreds, tens, ones, positionDigit < 0); // positionDigit < 0 nghĩa là đã hết số

                    // Nếu nhóm này có giá trị (không phải 000), thì cộng thêm tên nhóm (nghìn, triệu...)
                    if (!string.IsNullOrEmpty(groupStr))
                    {
                        result = groupStr + Tien[group] + result;
                    }

                    group++;
                }

                // Xử lý chuỗi kết quả
                result = result.Trim();
                // Viết hoa chữ cái đầu
                if (result.Length > 0)
                {
                    result = result.Substring(0, 1).ToUpper() + result.Substring(1);
                }

                return result + " đồng chẵn";
            }
            catch
            {
                return "Lỗi đọc số";
            }
        }

        // Hàm đọc bộ 3 số
        private static string ReadTriple(int tram, int chuc, int donvi, bool isLastGroup)
        {
            string ketqua = "";

            // 1. Đọc hàng TRĂM
            if (tram != -1)
            {
                ketqua += ChuSo[tram] + " trăm";
                // Nếu trăm = 0, chục = 0, đơn vị = 0 thì không đọc gì cả (trừ khi là nhóm cuối cùng)
                if (tram == 0 && chuc == 0 && donvi == 0) return "";
            }

            // 2. Đọc hàng CHỤC
            if (chuc != -1)
            {
                if (chuc == 0)
                {
                    if (donvi != 0) ketqua += " linh";
                }
                else if (chuc == 1)
                {
                    ketqua += " mười";
                }
                else
                {
                    ketqua += ChuSo[chuc] + " mươi";
                }
            }

            // 3. Đọc hàng ĐƠN VỊ
            if (donvi != -1)
            {
                if (chuc != -1 && donvi == 0)
                {
                    // Chẵn chục (ví dụ 20: hai mươi) -> không đọc đơn vị
                }
                else if (donvi == 1)
                {
                    if (chuc > 1) ketqua += " mốt";
                    else ketqua += " một";
                }
                else if (donvi == 5)
                {
                    if (chuc > 0) ketqua += " lăm";
                    else ketqua += " năm";
                }
                else
                {
                    ketqua += ChuSo[donvi];
                }
            }

            return ketqua;
        }
    }
}