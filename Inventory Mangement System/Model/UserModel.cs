using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class UserModel
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "UserName required")]
        public string  UserName { get; set; }

        [RegularExpression(@"[a-zA-Z0-9.-_]{1,}@[a-zA-Z.-]{2,}[.]{1}[a-zA-Z]{2,}$", ErrorMessage = "Please enter proper EmailAddress")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"[A-Za-z][a-z0-9@#_]{6,}[a-z0-9]$", ErrorMessage = "Minimum 8 character," +
            "Start first letter with uppercase or lower case,one number and one special symbol")]
        public string  Password { get; set; }
    }
}
