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
       /* [Required(ErrorMessage = "Date Is Required.")]
        public DateTime Purchasedate { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Total Quantity Required.")]
        public float totalquantity { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Cost Required.")]
        public float totalcost { get; set; }
        public string remarks { get; set; }
        public string vendorname { get; set; }
        public Model.Common.IntegerNullString productname { get; set; } = new Model.Common.IntegerNullString();*/
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
       
        public string PurchaseLocation { get; set; }
        [Required(ErrorMessage = "BillNumber is required!")]
        [RegularExpression(@"^[a-zA-Z0-9'@-#.\s]{1,50}$", ErrorMessage = "Maximum 50 character allowed!")]
        public string BillNumber { get; set; }
        public Model.Common.IntegerNullString productname { get; set; } = new Model.Common.IntegerNullString();
    }
    
}
