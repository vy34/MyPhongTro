using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace tmLib
{
    public static class ViCom
    {

        private static readonly string eKey = "b14ca5898a4e4133bbce2ea2315a1916";

        public static bool IsNgay(string sNgay)
        {
            _ = new DateTime();
            if (DateTime.TryParse(sNgay, out DateTime myDate))
            {
                if (myDate == DateTime.MinValue) { return false; } else { return true; }
            }
            else { return false; }
        }
        public static DateTime GetNgay(string sNgay)
        {
            if (DateTime.TryParse(sNgay, out DateTime myDate)) { return myDate; } else { return DateTime.MinValue; }
        }
        public static DateTime GetNgayTrongthang(bool Dauthang)
        {
            if (Dauthang)
            {
                DateTime fTungay = new(DateTime.Now.Year, DateTime.Now.Month, 1);//DateTime.Now
                _ = DateTime.TryParse(fTungay.ToShortDateString() + " 00:00:01", out fTungay);
                return fTungay;

            }
            else
            {
                _ = DateTime.TryParse(DateTime.Now.ToShortDateString() + " 23:59:59", out DateTime fDenngay);
                return fDenngay;

            }
        }

        public static DateTime GetNgayGio(DateTime ngay, bool Daungay)
        {
            if (Daungay)
            {
                _ = DateTime.TryParse(ngay.ToShortDateString() + " 00:00:01", out DateTime fTungay);
                return fTungay;

            }
            else
            {
                _ = DateTime.TryParse(ngay.ToShortDateString() + " 23:59:59", out DateTime fDenngay);
                return fDenngay;

            }
        }

        public static string GetTenkhongdau(string Chuoi)
        {
            if (string.IsNullOrEmpty(Chuoi))
                return "";

            string[] dau1 = ["à", "á", "ạ", "ả", "ã", "À", "Á", "Ạ", "Ả", "Ã"];
            string[] dau2 = ["ă", "ằ", "ắ", "ặ", "ẳ", "ẵ", "Ă", "Ằ", "Ắ", "Ặ", "Ẳ", "Ẵ"];
            string[] dau3 = ["â", "ầ", "ấ", "ậ", "ẩ", "ẫ", "Â", "Ầ", "Ấ", "Ậ", "Ẩ", "Ẫ"];
            string[] dau4 = ["è", "é", "ẹ", "ẻ", "ẽ", "È", "É", "Ẹ", "Ẻ", "Ẽ"];
            string[] dau5 = ["ê", "ề", "ế", "ệ", "ể", "ễ", "Ê", "Ề", "Ế", "Ệ", "Ể", "Ễ"];
            string[] dau6 = ["ì", "í", "ị", "ỉ", "ĩ", "Ì", "Í", "Ị", "Ỉ", "Ĩ"];
            string[] dau7 = ["ò", "ó", "ọ", "ỏ", "õ", "Ò", "Ó", "Ọ", "Ỏ", "Õ"];
            string[] dau8 = ["ô", "ồ", "ố", "ộ", "ổ", "ỗ", "Ô", "Ồ", "Ố", "Ộ", "Ổ", "Ỗ"];
            string[] dau9 = ["ơ", "ờ", "ớ", "ợ", "ở", "ỡ", "Ơ", "Ờ", "Ớ", "Ợ", "Ở", "Ỡ"];
            string[] dau10 = ["ù", "ú", "ụ", "ủ", "ũ", "Ù", "Ú", "Ụ", "Ủ", "Ũ"];
            string[] dau11 = ["ư", "ừ", "ứ", "ự", "ử", "ữ", "Ư", "Ừ", "Ứ", "Ự", "Ử", "Ữ"];
            string[] dau12 = ["ỳ", "ý", "ỵ", "ỷ", "ỹ", "Ỳ", "Ý", "Ỵ", "Ỷ", "Ỹ"];
            string tempten = Chuoi;
            for (int i = 0; i < dau1.Length; i++)
            {
                tempten = tempten.Replace(dau1[i], "a");
                tempten = tempten.Replace(dau4[i], "e");
                tempten = tempten.Replace(dau6[i], "i");
                tempten = tempten.Replace(dau7[i], "o");
                tempten = tempten.Replace(dau10[i], "u");
                tempten = tempten.Replace(dau12[i], "y");
            }
            for (int i = 0; i < dau2.Length; i++)
            {
                tempten = tempten.Replace(dau2[i], "a");
                tempten = tempten.Replace(dau3[i], "a");
                tempten = tempten.Replace(dau5[i], "e");
                tempten = tempten.Replace(dau8[i], "o");
                tempten = tempten.Replace(dau9[i], "o");
                tempten = tempten.Replace(dau11[i], "u");
            }
            tempten = tempten.Replace("đ", "d");
            tempten = tempten.Replace("Đ", "d");
            tempten = tempten.ToLower();
            return tempten;
        }
        public static string GetChuoikhongdau(string Chuoi)
        {
            if (string.IsNullOrEmpty(Chuoi))
                return "";

            string[] dau1 = ["à", "á", "ạ", "ả", "ã", "ă", "ằ", "ắ", "ặ", "ẳ", "ẵ", "â", "ầ", "ấ", "ậ", "ẩ", "ẫ"];
            string[] dau2 = ["À", "Á", "Ạ", "Ả", "Ã", "Ă", "Ằ", "Ắ", "Ặ", "Ẳ", "Ẵ", "Â", "Ầ", "Ấ", "Ậ", "Ẩ", "Ẫ"];
            string[] dau7 = ["ò", "ó", "ọ", "ỏ", "õ", "ô", "ồ", "ố", "ộ", "ổ", "ỗ", "ơ", "ờ", "ớ", "ợ", "ở", "ỡ"];
            string[] dau8 = ["Ò", "Ó", "Ọ", "Ỏ", "Õ", "Ô", "Ồ", "Ố", "Ộ", "Ổ", "Ỗ", "Ơ", "Ờ", "Ớ", "Ợ", "Ở", "Ỡ"];

            string[] dau3 = ["è", "é", "ẹ", "ẻ", "ẽ", "ê", "ề", "ế", "ệ", "ể", "ễ"];
            string[] dau4 = ["È", "É", "Ẹ", "Ẻ", "Ẽ", "Ê", "Ề", "Ế", "Ệ", "Ể", "Ễ"];
            string[] dau9 = ["ù", "ú", "ụ", "ủ", "ũ", "ư", "ừ", "ứ", "ự", "ử", "ữ"];
            string[] dau10 = ["Ù", "Ú", "Ụ", "Ủ", "Ũ", "Ư", "Ừ", "Ứ", "Ự", "Ử", "Ữ"];

            string[] dau5 = ["ì", "í", "ị", "ỉ", "ĩ"];
            string[] dau6 = ["Ì", "Í", "Ị", "Ỉ", "Ĩ"];
            string[] dau11 = ["ỳ", "ý", "ỵ", "ỷ", "ỹ"];
            string[] dau12 = ["Ỳ", "Ý", "Ỵ", "Ỷ", "Ỹ"];

            string tempten = Chuoi;
            for (int i = 0; i < dau1.Length; i++)
            {
                tempten = tempten.Replace(dau1[i], "a");
                tempten = tempten.Replace(dau2[i], "A");
                tempten = tempten.Replace(dau7[i], "o");
                tempten = tempten.Replace(dau8[i], "O");
            }
            for (int i = 0; i < dau3.Length; i++)
            {
                tempten = tempten.Replace(dau3[i], "e");
                tempten = tempten.Replace(dau4[i], "E");
                tempten = tempten.Replace(dau9[i], "u");
                tempten = tempten.Replace(dau10[i], "U");
            }
            for (int i = 0; i < dau5.Length; i++)
            {
                tempten = tempten.Replace(dau5[i], "i");
                tempten = tempten.Replace(dau6[i], "I");
                tempten = tempten.Replace(dau11[i], "y");
                tempten = tempten.Replace(dau12[i], "Y");
            }
            tempten = tempten.Replace("đ", "d");
            tempten = tempten.Replace("Đ", "D");
            return tempten;
        }

        public static string ChuanhoaChuoi(string chuoi)
        {
            if (chuoi.Length > 0)
            {
                string[] aTen = chuoi.Split(null);
                string Ten = "";
                for (int j = 0; j < aTen.Length; j++)
                {
                    if (aTen[j].Length > 0)
                        Ten += aTen[j] + " ";
                }
                Ten = Ten.Trim();
                return Ten;
            }
            else
                return "";
        }
        public static string ConvertDate(DateTime ngay)
        {
            //string sRet = ngay.Year + "-" + ngay.Month + "-" + ngay.Day + " " + ngay.ToString("HH:mm:ss");
            string sRet = ngay.ToString("yyyy-MM-dd HH:mm:ss");
            return sRet;
        }
        public static string TranslateText(string input)
        {
            try
            {
                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                 "vi", "en", Uri.EscapeDataString(input));
                HttpClient httpClient = new();
                string result = httpClient.GetStringAsync(url).Result;
                var jsonData = JsonSerializer.Deserialize<List<dynamic>>(result);
                var translationItems = jsonData[0];
                var test = translationItems[0];
                string translation = ((JsonElement)test[0]).ToString();
                return translation;
            }
            catch (Exception)
            {
                return "";
                //throw new ArgumentException(ex.Message);
            }
        }
        public static string GetMyStringValue(string sKey)
        {
            IValueManager<string> valueManager = ValueManager.GetValueManager<string>(sKey);
            if (valueManager.CanManageValue)
                return valueManager.Value;
            else return null;
        }

        public static void SetMyStringValue(string sKey, string value)
        {
            IValueManager<string> valueManager = ValueManager.GetValueManager<string>(sKey);
            if (valueManager.CanManageValue)
                valueManager.Value = value;
        }



        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new(decoded_char);
            return result;
        }
        public static double CDouble(string expression)
        {
            if (expression == null)
                return 0;
            string chuoi = expression;
            chuoi = chuoi.Replace(" ", "");

            //try the entire string, then progressively smaller substrings to replicate the behavior of VB's 'Val', which ignores trailing characters after a recognizable value:
            for (int size = chuoi.Length; size > 0; size--)
            {
                if (double.TryParse(chuoi[..size], NumberStyles.Any, CultureInfo.InvariantCulture, out double testDouble))
                    return testDouble;
            }

            //no value is recognized, so return 0:
            return 0;
        }

        public static double CDouble(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return 0;
            string chuoi = expression.ToString();
            chuoi = chuoi.Replace(" ", "");
            if (double.TryParse(chuoi, NumberStyles.Any, CultureInfo.InvariantCulture, out double testDouble))
                return testDouble;
            return 0;
        }

        public static double CDoubleInv(string expression)
        {
            try
            {
                if (expression == null)
                    return 0;
                string chuoi = expression;
                chuoi = chuoi.Replace(" ", "");
                double testDouble = double.Parse(chuoi, System.Globalization.CultureInfo.InvariantCulture);

                //no value is recognized, so return 0:
                return testDouble;

            }
            catch
            {
                return 0;
            }
        }

        public static decimal CDecimal(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return 0;
            string chuoi = expression.ToString();
            chuoi = chuoi.Replace(" ", "");
            if (decimal.TryParse(chuoi, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal testDecimal))
                return testDecimal;
            return 0;
        }
        public static int CInt(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return 0;
            string chuoi = expression.ToString();
            chuoi = chuoi.Replace(" ", "");
            if (int.TryParse(chuoi, NumberStyles.Any, CultureInfo.InvariantCulture, out int testInt))
                return testInt;
            return 0;
        }
        public static bool CBool(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return false;
            if (bool.TryParse(expression.ToString(), out bool testBool))
                return testBool;
            return false;
        }
        public static DateTime CDate(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return DateTime.MinValue;
            if (DateTime.TryParse(expression.ToString(), out DateTime testDate))
                return testDate;
            return DateTime.MinValue;
        }
        public static string CString(object expression)
        {
            string chuoi = "";
            if (expression != null && expression != DBNull.Value)
            {
                chuoi = expression.ToString().Trim();
            }
            return chuoi;
        }
        public static Guid CGuid(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return Guid.Empty;
            if (Guid.TryParse(expression.ToString(), out Guid testDouble))
                return testDouble;
            return Guid.Empty;
        }
        public static string Left(string somestring, int nCount)
        {
            if (somestring.Length > nCount)
                return somestring[..nCount];
            else
                return somestring;
        }
        public static string Right(string somestring, int nCount)
        {
            if (somestring.Length > nCount)
                return somestring.Substring(somestring.Length - nCount, nCount);
            else
                return somestring;
        }
        public static string ChuyenSo(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string[] strTachPhanSauDauPhay;
            if (number.Contains('.') || number.Contains(','))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSo(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSo(strTachPhanSauDauPhay[1]));
            }

            string[] dv = ["", "mươi", "trăm", "nghìn", "triệu", "tỉ"];
            string[] cs = ["không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"];
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc + " đồng";
        }
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(eKey);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new();
                    using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }

                return Convert.ToBase64String(array);
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(eKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new((Stream)cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }
        public static string GetSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            { return ConfigurationManager.AppSettings[key]; }
            else { return null; }
        }

        public static void SetSetting(string key, string value)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string tm = GetSetting(key);
            if (tm == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string GetConnectionByMode(string modeStr)
        {
            //Mode: 0 - Local Network, 1 - Public Network, 2 - LocalDB, 3 - Developer Mode-- >
            string myString;
            string Tmp;
            if (modeStr == "0")//public sql server
            {
                Tmp = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                myString = Decrypt(Tmp);
            }
            else if (modeStr == "1")//local sql server
            {
                Tmp = ConfigurationManager.ConnectionStrings["LocalNet"].ConnectionString;
                myString = Decrypt(Tmp);
            }
            else if (modeStr == "2")//local db
            {
                myString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;
                //myString = Path.GetFileNameWithoutExtension(sFile);
                //myString = MSSqlConnectionProvider.GetConnectionStringWithAttachForLocalDB(
                //"(LocalDB)\\MSSQLLocalDB", dbName, sFile);
            }
            else
            {
                myString = ConfigurationManager.ConnectionStrings["DevMode"].ConnectionString;
            }
            return myString;
        }
        public static string GetConnectionString(string constrName, bool bDecrypt)
        {
            string myString;
            string Tmp = ConfigurationManager.ConnectionStrings[constrName].ConnectionString;
            if (bDecrypt)
                myString = Decrypt(Tmp);
            else
                myString = Tmp;
            return myString;
        }

        public static string CustomConnectionString()
        {

            string snet = GetSetting("UseNet");
            string myString;
            string Tmp;
            if (snet == "0")//public sql server
            {
                Tmp = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                myString = Decrypt(Tmp);
            }
            else if (snet == "1")//local network sql server
            {
                Tmp = ConfigurationManager.ConnectionStrings["LocalNet"].ConnectionString;
                myString = Decrypt(Tmp);
            }
            else if (snet == "2")//local db
            {
                string sFile = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;
                string dbName = Path.GetFileNameWithoutExtension(sFile);
                myString = MSSqlConnectionProvider.GetConnectionStringWithAttachForLocalDB(
                "(LocalDB)\\MSSQLLocalDB", dbName, sFile);
            }
            else
            {
                myString = ConfigurationManager.ConnectionStrings["DevMode"].ConnectionString;
            }
            return myString;
        }
        public static bool IsNumeric(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return false;
            bool isOK = int.TryParse(expression.ToString(), out _);
            return isOK;
        }
        public static int GetMyIntValue(string sKey)
        {
            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(sKey);
            if (valueManager.CanManageValue)
                return valueManager.Value;
            else return 0;
        }

        public static void SetMyIntValue(string sKey, int value)
        {
            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(sKey);
            if (valueManager.CanManageValue)
                valueManager.Value = value;
        }
        public static string MaEAN13(string mahang)
        {
            int iSum = 0;

            // Calculate the checksum digit here.
            for (int i = mahang.Length; i >= 1; i--)
            {
                int iDigit = Convert.ToInt32(mahang.Substring(i - 1, 1));
                if (i % 2 == 0)
                {   // odd
                    iSum += iDigit * 3;
                }
                else
                {   // even
                    iSum += iDigit * 1;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            return mahang + iCheckSum.ToString();
        }
        public static string GetSQLDateTime(DateTime Ngay)
        {
            string sRet = "CONVERT(DATETIME, '" + Ngay.Year + "-" + Ngay.Month + "-" + Ngay.Day + " " + Ngay.Hour + ":" + Ngay.Minute + ":" + Ngay.Second + "', 102)";
            return sRet;
        }

        public static string GetVietIntelPass()
        {
            DateTime ngay = DateTime.Today;
            string pass = "";
            int num = 0;
            string Temp = ngay.Day.ToString();
            for (int i = 0; i < Temp.Length; i++)
            {
                num += Convert.ToInt32(Temp.Substring(i, 1));
            }
            pass += num.ToString();
            num = 0;
            Temp = ngay.Month.ToString();
            for (int i = 0; i < Temp.Length; i++)
            {
                num += Convert.ToInt32(Temp.Substring(i, 1));
            }
            pass += num.ToString();
            num = 0;
            Temp = ngay.Year.ToString();
            for (int i = 0; i < Temp.Length; i++)
            {
                num += Convert.ToInt32(Temp.Substring(i, 1));
            }
            pass += num.ToString();
            return pass;
        }

    }
}
