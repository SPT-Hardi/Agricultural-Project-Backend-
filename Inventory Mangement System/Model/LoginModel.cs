using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage ="EmailAddress is required!")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage ="Password is required!")]
        public string Password { get; set; }
        
    }
}
