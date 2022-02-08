using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class PurchaseModel
    {
        public List<PurchaseList> purchaseList { get; set; } = new List<PurchaseList>();
    }

    public class PurchaseList
    {
        [Required(ErrorMessage = "Date Is Required.")]
        public DateTime Purchasedate { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Total Quantity Required.")]
        public float totalquantity { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Cost Required.")]
        public float totalcost { get; set; }
        public string remarks { get; set; }
        public string vendorname { get; set; }
        public Model.Common.IntegerNullString productname { get; set; } = new Model.Common.IntegerNullString();
    }
}
