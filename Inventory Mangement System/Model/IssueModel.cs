using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class IssueModel
    {
        [Required(ErrorMessage = "Date Is Required.")]
        public DateTime Date { get; set; }
        public Model.Common.IntegerNullString MainArea { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString SubArea { get; set; } = new Model.Common.IntegerNullString();
        public List<IssueDetail> issueDetails { get; set; } = new List<IssueDetail>();
    }

    public class IssueDetail
    {
        public string Remark { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Issue Quantity Required.")]
        public float IssueQuantity { get; set; }

        public Model.Common.IntegerNullString Product { get; set; } = new Model.Common.IntegerNullString();
       
    }
    
}
