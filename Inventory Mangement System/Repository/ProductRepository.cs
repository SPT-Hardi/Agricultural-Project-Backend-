using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.JsonPatch;
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
        //View All Product 
        public Result ViewAllProduct()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from x in context.Products
                            orderby x.ProductID descending
                            select new
                            {
                                ProductID = x.ProductID,
                                ProductName = x.ProductName,
                                Variety = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Type = new Model.Common.IntegerNullString() { Id=x.ProductUnit.UnitID,Text=x.ProductUnit.Type},
                                CategoryType = new Model.Common.IntegerNullString() { Id = x.Category.CategoryID, Text = x.Category.CategoryName },
                                UserName=(from n in context.LoginDetails
                                          where n.LoginID== x.LoginID
                                          select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", x.DateTime),
                            }).ToList(),
                };
            }
        }
        
        //Add Product
        public Result AddProduct(ProductModel productModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            Product product = new Product();

            var pro = (from p in productModel.ProductDetails
                       select new Product()
                       {
                           ProductName = char.ToUpper(p.ProductName[0]) + p.ProductName.Substring(1).ToLower(),
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = p.categoryType.Id,
                           LoginID=1,
                           TotalProductQuantity=0,
                           DateTime=DateTime.Now,
                           UnitID = p.type.Id
                       }).ToList();
            foreach (var item in pro)
            {
                var pname = context.Products.Where(name => name.ProductName == item.ProductName).SingleOrDefault();
                if (pname != null)
                {
                    throw new ArgumentException($"{item.ProductName} Are Alredy Exits.");
                }
            }

            context.Products.InsertAllOnSubmit(pro);
            context.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"{product.ProductName} Added Successfully."),
                Status = Result.ResultStatus.success,
                Data = product.ProductName,
            };
        }

        //Edit Product 
        public Result EditProduct(ProductDetail productDetail, int productID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var product = context.Products.SingleOrDefault(id => id.ProductID == productID);
                if (product == null)
                {
                    throw new Exception("Product Does Not Exist.");
                }
                if (product.ProductName != productDetail.ProductName)
                {
                    var _product = context.Products.SingleOrDefault(name => name.ProductName == productDetail.ProductName);
                    if (_product != null)
                    {
                        throw new Exception("Product Alredy Exits.");
                    }
                }
                product.ProductName = char.ToUpper(productDetail.ProductName[0]) + productDetail.ProductName.Substring(1).ToLower();
                product.Variety = productDetail.Variety;
                product.Company = productDetail.Company;
                product.Description = productDetail.Description;
                product.CategoryID = (int)productDetail.categoryType.Id;
                product.LoginID = 1;
                product.DateTime = DateTime.Now;
                product.UnitID = (int)productDetail.type.Id;
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{productDetail.ProductName} Updated Successfully."),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        //Get Unit
        public async Task<IEnumerable> GetUnit()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.ProductUnits
                        select new IntegerNullString()
                        {
                            Text = x.Type,
                            Id = x.UnitID
                        }).ToList();
            }
        }

    }
}
