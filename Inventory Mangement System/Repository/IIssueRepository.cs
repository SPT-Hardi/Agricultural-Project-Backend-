using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IIssueRepository
    {
        //All Issue Details
        Task<IEnumerable> ViewAllIssue();
        
        //Issue Products
        Result IssueProduct(IssueModel issueModel);

        //Issue Detail By Id
        Task<IEnumerable> ViewIssueById(int issueID);
        
        Task<IEnumerable> GetMainArea();
        Task<IEnumerable> GetSubArea(int id);
        Task<IEnumerable> GetProduct();

        Task<IEnumerable> GetProductTotalQuantity();
      //  Result total(IssueModel issueModel);
    }
}