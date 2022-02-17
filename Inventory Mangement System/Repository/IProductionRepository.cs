using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductionRepository
    {
        Result AddProductionDetails(ProductionModel value);
        Result ViewAllProductionDetails();
        Task<IEnumerable> ViewProductionById(int id);
        Result Editproduction(ProductionModel productionModel, int id);
    }
}