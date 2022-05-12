using Inventory_Mangement_System.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductionModel
    {
       /* [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Vegetable Name Required.")]
        public string Vegetablenm { get; set; }*/

        [RegularExpression(@"^[0-9.]{1,9}$", ErrorMessage = "Please Enter only float value having max 2 digit after decimal.")]
        [Required(ErrorMessage = "Quantity Required.")]
        public float Quantity { get; set; }
        public IntegerNullString Vegetable { get; set; } = new IntegerNullString();
        [RegularExpression(@"^.{1,150}$",ErrorMessage ="Maximum 150 characters are allowed!")]
        public string Remark { get; set; }
        public Model.Common.IntegerNullString MainAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString SubAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public DateTime ProductionDate { get; set; }
        //public List<ProductionList> ProductionLists { get; set; } = new List<ProductionList>();
    }
    /*public class ProductionList
    {
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Vegetable Name Required.")]
        public string Vegetablenm { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Quantity Required.")]
        public float Quantity { get; set; }

        public string Remark { get; set; }
        public Model.Common.IntegerNullString MainAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString SubAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        //public Model.IntegerNullString vegetableDetails { get; set; } = new Model.IntegerNullString();
    }*/
}
