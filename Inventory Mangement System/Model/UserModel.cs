using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class UserModel
    {
        [RegularExpression(@"^[a-z A-Z 0-9]+$", ErrorMessage = "Please Enter Character or Letter") ]
        [Required(ErrorMessage = "UserName Required")]
        public string  UserName { get; set; }
        //^([\w-\.]+@(?!gmail.com)(?!yahoo.com)(?!hotmail.com)([\w- ]+\.)+[\w-]{2,4})?$

        [RegularExpression(@"[a-zA-Z0-9.-_]{1,}@[a-zA-Z.-]{2,}[.]{1}[a-zA-Z]{2,}", ErrorMessage = "Please Enter Valid Emial Address")]
        [Required(ErrorMessage = "EmailAddress Required")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"[A-Za-z][a-z0-9@#_]{6,}[a-z0-9]$", ErrorMessage = "Minimum 8 character," +
            "Start first letter uppercase,atleast one lower case,one number and one special symbol")]
        [Required(ErrorMessage = "Password Required")]
        public string  Password { get; set; }
    }
}
