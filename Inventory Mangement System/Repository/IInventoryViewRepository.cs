using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IInventoryViewRepository
    {
        Task<IEnumerable> GetInventoryView();
    }
}