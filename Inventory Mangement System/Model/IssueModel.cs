using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class IssueModel
    {
        public float PurchaseQuantity { get; set; }
        public DateTime Date { get; set; }
        public Model.Common.IntegerNullString Product { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString MainArea { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString SubArea { get; set; } = new Model.Common.IntegerNullString();

    }
}
