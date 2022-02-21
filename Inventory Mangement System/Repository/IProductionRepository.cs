using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductionRepository
    {
        //View All Production Details 
        Result AddProductionDetails(ProductionModel value);

        //Add Production Details
        Result ViewAllProductionDetails();

        //Edit Production
        Result Editproduction(ProductionModel productionModel, int id);
    }
}