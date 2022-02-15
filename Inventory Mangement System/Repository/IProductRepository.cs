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
        Result AddProduct(ProductModel productModel);

        //View Product By Id
        Result ViewProductById(int productID);

        //Edit Product Using Put Method
        Result EditProduct(ProductDetail productDetail, int productID);
        
        //Get Unit
        Task<IEnumerable> GetUnit();

        Result UpdateProduct(JsonPatchDocument productModel, int productID);
    }
}