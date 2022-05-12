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

        [RegularExpression(@"^[0-9.]{1,9}$", ErrorMessage = "Please Enter only float value having max 2 digit after decimal.")]
        [Required(ErrorMessage = "Total Quantity Required.")]
        public float totalquantity { get; set; }

        [RegularExpression(@"^[0-9.]{1,9}$", ErrorMessage = "Please Enter only float value having max 2 digit after decimal.")]
        [Required(ErrorMessage = "Cost Required.")]
        public float totalcost { get; set; }
        [RegularExpression(@"^.{1,150}$", ErrorMessage ="Maximum 150 charcters are allowed!")]
        public string remarks { get; set; }
        [RegularExpression(@"^.{1,50}$", ErrorMessage = "Maximum 50 charcters are allowed!")]
        public string vendorname { get; set; }
        [RegularExpression(@"^.{1,150}$", ErrorMessage = "Maximum 150 charcters are allowed!")]
        public string PurchaseLocation { get; set; }
        [Required(ErrorMessage = "BillNumber is required!")]
        [RegularExpression(@"^[a-z A-Z 0-9 -'@#.]{1,50}$", ErrorMessage = "Maximum 50 character allowed!")]
        public string BillNumber { get; set; }
        public Model.Common.IntegerNullString productname { get; set; } = new Model.Common.IntegerNullString();
    }
    
}
