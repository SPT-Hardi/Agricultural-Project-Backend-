using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IIssueRepository
    {
        Task<IEnumerable> GetMainArea();
        Task<IEnumerable> GetSubArea(int id);
        Task<IEnumerable> GetProduct();
        Result IssueProduct(IssueModel issueModel);
        Result total(IssueModel issueModel);
    }
}