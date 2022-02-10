using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable> GetunitByid(int id);
        Result AddPurchaseDetails(PurchaseModel purchaseModel);
    }
}