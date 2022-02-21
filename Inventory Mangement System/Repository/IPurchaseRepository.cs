using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IPurchaseRepository
    {
        //View Purchase Details
        Result AddPurchaseDetails(PurchaseModel purchaseModel);

        //Add Purchase Details
        Result EditPurchaseProduct(PurchaseModel purchaseModel, int ID);

        //Edit Purchase Details
        Result GetPurchaseDetails();
    }
}