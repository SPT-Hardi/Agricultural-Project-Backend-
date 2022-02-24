using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IPurchaseRepository
    {
        //View Purchase Details
        Result AddPurchaseDetails(PurchaseModel purchaseModel, int LoginId);

        //Add Purchase Details
        Result EditPurchaseProduct(PurchaseModel purchaseModel, int ID, int LoginId);

        //Edit Purchase Details
        Result GetPurchaseDetails();
    }
}