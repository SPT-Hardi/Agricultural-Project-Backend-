using Inventory_Mangement_System.Model;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IProductionRepository
    {
        Result AddProductionDetails(ProductionModel value);
    }
}