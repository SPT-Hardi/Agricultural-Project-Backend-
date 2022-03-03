using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model.Common
{
    public class TokenModel
    {
        [Required(ErrorMessage ="Token Is Requred")]
        public string Token { get; set; }
        [Required(ErrorMessage = "RefreshToken Is Requred")]
        public string RefreshToken { get; set; }
    }
}
