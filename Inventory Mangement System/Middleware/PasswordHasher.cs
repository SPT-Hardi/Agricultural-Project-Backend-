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