using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IIssueRepository
    {
        //View Issue Details
        Result ViewAllIssue();

        //Issue Products
        Result IssueProduct(IssueModel issueModel);

        //Edit Issue Details
        Result EditIssue(IssueModel issueModel, int ID);

        //Get MainArea Dropdown
        Task<IEnumerable> GetMainArea();

        //Get SubArea Dropdown
        Task<IEnumerable> GetSubArea(int id);

        //Get Product Dropdown with Unit And Quantity
        Task<IEnumerable> GetProduct();

       // Task<IEnumerable> GetProductTotalQuantity();
      //  Result total(IssueModel issueModel);
    }
}