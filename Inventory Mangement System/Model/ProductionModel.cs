using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductionModel
    {
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Vegetable Name Required.")]
        public string vegetablenm { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Quantity Required.")]
        public float Quantity { get; set; }
        public Model.Common.IntegerNullString mainAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString subAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        //public Model.IntegerNullString vegetableDetails { get; set; } = new Model.IntegerNullString();

    }
}
