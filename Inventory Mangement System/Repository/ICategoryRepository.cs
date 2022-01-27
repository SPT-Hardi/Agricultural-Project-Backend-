using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface ICategoryRepository
    {
        Result AddCategory(CategoryModel categoryModel, int Uid);
        Task<IEnumerable> GetCategory();

    }
}