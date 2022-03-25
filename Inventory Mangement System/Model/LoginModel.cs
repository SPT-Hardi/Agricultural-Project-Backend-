using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class LoginModel
    {
        public string EmailAddress { get; set; }

       
        public string Password { get; set; }
        
    }
}
