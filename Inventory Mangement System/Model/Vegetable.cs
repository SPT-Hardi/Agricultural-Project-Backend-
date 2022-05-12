using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class Vegetable
    {
        public List<VegetableList> vegetables { get; set; } = new List<VegetableList>();
    }
    public class VegetableList 
    {
        [Required(ErrorMessage ="VegetableName is require!")]
        [RegularExpression(@"^[a-z A-Z 0-9]{1,50}$", ErrorMessage = "Alphanumeric,Maximum 50 characters are required!")]
        public string VegetableName { get; set; }
    }

    
}
