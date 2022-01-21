using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class ProductRepository : IProductRepository
    {

        public Result AddProduct(ProductModel productModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            Product product = new Product();

            var pname = context.Products.Where(name => name.ProductName == productModel.ProductName).SingleOrDefault();
            if (pname != null)
            {
                throw new ArgumentException("Alredy Exits");
            }
            product.ProductName = productModel.ProductName;
            product.Variety = productModel.Variety;
            product.Company = productModel.Company;
            product.Description = productModel.Description;
            product.Unit = (string)productModel.type.Text;
            product.CategoryID = (int)productModel.categorytype.Id;
            context.Products.InsertOnSubmit(product);
            context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            return new Result()
            {
                Message = string.Format($"{productModel.ProductName} Added successfully!"),
                Status = Result.ResultStatus.warning,
                Data= productModel.ProductName,
            };
        }

        public async Task<IEnumerable> GetUnit()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.ProductUnits 
                        select new IntegerNullString()
                        {
                            Text = x.Type ,
                            Id = x.UnitID 
                        }).ToList();
            }
        }
    }
}
