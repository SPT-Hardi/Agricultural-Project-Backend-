using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Middleware
{
    public class PasswordHasher
    {
        public static string Key = "Agricultural@Project";
        public string EncryptPassword(string Password)
        {
            Password = Password + Key;
            var PasswordBytes = Encoding.UTF8.GetBytes(Password);
            return Convert.ToBase64String(PasswordBytes);

            /*byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(Encryptval);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);*/
        }
        public string DecryptPassword(string Password)
        {
            var PasswordBytes = Convert.FromBase64String(Password);
            var result = Encoding.UTF8.GetString(PasswordBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result; 
        }
    }
}