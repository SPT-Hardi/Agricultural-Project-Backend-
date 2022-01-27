using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAreaRepository
    {
        //New Main Area Add
        Result AddMainAreaAsync(MainAreaModel mainAreaModel);
       
    }
}