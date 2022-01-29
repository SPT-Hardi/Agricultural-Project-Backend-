using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class CategoryModel
    {
        
        [Required(ErrorMessage = "Category required")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

    }
}
