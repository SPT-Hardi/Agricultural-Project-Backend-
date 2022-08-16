using Inventory_Mangement_System.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ViewPurchaseModel
    {
        public int PurchaseID { get; set; }
        public string ProductName { get; set; } 
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public string VendorName { get; set; }
        public string  PurchaseDate { get; set; }
        public string CreatedBy { get; set; }
        public string  LastUpdated {get;set;}
        public bool? IsEditable { get; set; } 
        public string BillNumber { get; set; } 
        public string PurchaseLocation { get; set; }
        //public List<EditedList> EditedList { get; set; } = new List<EditedList>();
       // public bool HaveEditedList { get; set; }
    }
    public class EditedList 
    {
        public int PurchaseID { get; set; }
        public string ProductName { get; set; } 
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public string VendorName { get; set; }
        public string PurchaseDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdated { get; set; }
        public bool? IsEditable { get; set; }
        public string BillNumber { get; set; }
        public string PurchaseLocation { get; set; }

    }
    public class ViewPurchaseModelNameWithId
    {
        public int PurchaseID { get; set; }
        public IntegerNullString ProductName { get; set; } = new IntegerNullString();
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public string VendorName { get; set; }
        public string PurchaseDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdated { get; set; }
        public bool? IsEditable { get; set; }
        public string BillNumber { get; set; }
        public string PurchaseLocation { get; set; }
        //public List<EditedList> EditedList { get; set; } = new List<EditedList>();
        // public bool HaveEditedList { get; set; }
    }
}
