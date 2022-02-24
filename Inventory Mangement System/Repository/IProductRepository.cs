using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductRepository
    {
        //View All Product
        Result ViewAllProduct();

        //Add Product
        Result AddProduct(ProductModel productModel,int LoginId);

        //Edit Product 
        Result EditProduct(ProductDetail productDetail, int productID,int LoginId);
        
        //Get Unit
        Task<IEnumerable> GetUnit();

    }
}