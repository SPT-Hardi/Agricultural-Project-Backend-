
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class CategoryModel
    {
        [RegularExpression(@"^[a-z A-Z 0-9]+$", ErrorMessage = "Please Enter Character or Letter.")]
        [Required(ErrorMessage = "Category Required.")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

    }
}
