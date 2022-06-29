using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface ICategoryRepository
    {
        //View Category
        Result ViewCategory(int? Id);

        //Add New Category
        Result AddCategory(CategoryModel categoryModel,int LoginId);
        
        //Edit Category
        Result EditCategory(CategoryModel categoryModel, int id,int LoginId);

        //DropDown For Category
        Task<IEnumerable> GetCategory();

        //View Category With Paging
        Result ViewCategorys(Paging value);

    }
}