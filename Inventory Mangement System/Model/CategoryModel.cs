
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class CategoryModel
    {
        [RegularExpression(@"^[a-z A-Z 0-9]{1,30}$", ErrorMessage = "Alphanumeric,Maximum 30 character are allowed!")]
        [Required(ErrorMessage = "Category Required.")]
        public string CategoryName { get; set; }
        [RegularExpression(@"^.{1,300}$", ErrorMessage = "Alphanumeric,Maximum 300 character are allowed!")]
        public string Description { get; set; }

    }
}
