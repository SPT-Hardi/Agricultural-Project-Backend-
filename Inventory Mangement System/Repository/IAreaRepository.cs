using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAreaRepository
    {
        //View All Main And Sub Area
        Result ViewAllArea(int? Id);
        //New Main Area Add
        Result AddMainAreaAsync(AreaModel AreaModel,int LoginId);
        //Edit Main And Sub Area
        Result EditArea(UpdateAreaModel value, int Id,int LoginId);
        
    }
}