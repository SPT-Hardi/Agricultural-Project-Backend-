using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductModel
    {
        public List<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }

    public class ProductDetail
    {
        [RegularExpression(@"^[a-z A-Z 0-9]+$", ErrorMessage = "Please Enter Character or Letter.")]
        [Required(ErrorMessage = "Product Name is Required")]
        public string ProductName { get; set; }
        public string Variety { get; set; }

        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Company Name is Required")]
        public string Company { get; set; }
        public string Description { get; set; }
        public Model.Common.IntegerNullString categorytype { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString type { get; set; } = new Model.Common.IntegerNullString();
    }
}
