using Inventory_Mangement_System.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class IssueModel
    {
        [Required(ErrorMessage = "IssueDate is Required.")]
        public DateTime IssueDate { get; set; }

        public Model.Common.IntegerNullString Area { get; set; } = new Common.IntegerNullString();
        public List<IssueDetail> issueDetails { get; set; } = new List<IssueDetail>();
    }

    public class IssueDetail
    {
        [RegularExpression(@"^.{1,150}$", ErrorMessage = "Maximum 150 characters are allowed!")]
        public string Remark { get; set; }

        [RegularExpression(@"^[0-9.]{1,9}$", ErrorMessage = "Please Enter only float value having max 2 digit after decimal.")]
        [Required(ErrorMessage = "Issue Quantity Required.")]
        public float IssueQuantity { get; set; }
       
        public Model.Common.IntegerNullString Product { get; set; } = new Model.Common.IntegerNullString();
       
    }
    public class UpdateIssue 
    {

        [Required(ErrorMessage = "IssueDate is Required.")]
        public DateTime IssueDate { get; set; }

        public Model.Common.IntegerNullString Area { get; set; } = new Common.IntegerNullString();

        [RegularExpression(@"^[0-9.]{1,9}$", ErrorMessage = "Please Enter only float value having max 2 digit after decimal.")]
        [Required(ErrorMessage = "Issue Quantity Required.")]
        public float IssueQuantity { get; set; }

        public IntegerNullString Product { get; set; } = new IntegerNullString();
    }
    
}
