using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class IssueModel
    {
        public Model.Common.IntegerNullString MainArea { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString SubArea { get; set; } = new Model.Common.IntegerNullString();
        public DateTime Date { get; set; }
        public List<IssueDetail> issueDetails { get; set; } = new List<IssueDetail>();
        
    }

    public class IssueDetail
    {
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "IssueQuantity Required.")]
        public float IssueQuantity { get; set; }

        [Required(ErrorMessage = "Date Is Required.")]
       
        public string Remark { get; set; }
        public Model.Common.IntegerNullString Product { get; set; } = new Model.Common.IntegerNullString();
       
    }

}
