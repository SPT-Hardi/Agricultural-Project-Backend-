using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductRepository
    {
        Result AddProduct(ProductModel productModel);
        Task<IEnumerable> GetUnit();
        Task<IEnumerable> ViewAllProduct();
        Task<IEnumerable> ViewProductById(int productID);
        Result EditProduct(ProductDetail productDetail, int id);
    }
}