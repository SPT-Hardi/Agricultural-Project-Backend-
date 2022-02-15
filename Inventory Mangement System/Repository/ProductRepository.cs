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
                                DateTime = String.Format("{0:dd-mm-yyyy hh:mm tt}", x.DateTime),
                            }).ToList(),
                };
            }
        }
        /*
        //Add Product
        public Result AddProduct(ProductModel productModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            Product product = new Product();

            var pname = context.Products.Where(name => name.ProductName == productModel.ProductName).SingleOrDefault();
            if (pname != null)
            {
                throw new ArgumentException("Product Alredy Exits.");
            }
            product.ProductName = productModel.ProductName;
            product.Variety = productModel.Variety;
            product.Company = productModel.Company;
            product.Description = productModel.Description;
            product.Unit = (string)productModel.type.Text;
            product.CategoryID = (int)productModel.categoryType.Id;
            context.Products.InsertOnSubmit(product);
            context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            return new Result()
            {
                Message = string.Format($"{productModel.ProductName} Added Successfully."),
                Status = Result.ResultStatus.success,
                Data = productModel.ProductName,
            };
        } 
        */
        //Add Product
        public Result AddProduct(ProductModel productModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            Product product = new Product();

            var pro = (from p in productModel.ProductDetails
                       select new Product()
                       {
                           ProductName = p.ProductName,
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = p.categoryType.Id,
                           LoginID=1,
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

        //View Product By Id
        public Result ViewProductById(int productID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                /*product = context.Products.SingleOrDefault(id => id.ProductID == productID);
                if (product == null)
                {
                    throw new ArgumentException("Product Does Not Exist.");
                }*/
                return new Result()
                {
                    Status= Result.ResultStatus.success,
                    Data = (from x in context.Products
                            join catid in context.Categories
                            on x.CategoryID equals catid.CategoryID
                            where x.ProductID == productID
                            select new
                            {
                                ProductID = x.ProductID,
                                ProductName = x.ProductName,
                                Varitey = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Type = new Model.Common.IntegerNullString() { Id = x.ProductUnit.UnitID, Text = x.ProductUnit.Type },
                                CategoryType = new Model.Common.IntegerNullString() { Id = x.Category.CategoryID, Text = x.Category.CategoryName },
                            }).ToList(),
                };
            }
        }

        //Edit Product Using Put Method
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
                product.ProductName = productDetail.ProductName;
                product.Variety = productDetail.Variety;
                product.Company = productDetail.Company;
                product.Description = productDetail.Description;
                product.CategoryID = (int)productDetail.categoryType.Id;
                product.UnitID = (int)productDetail.type.Id;
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{productDetail.ProductName} Product Update Successfully."),
                    Status = Result.ResultStatus.success,
                    Data = productDetail.ProductName,
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

        public Result UpdateProduct(JsonPatchDocument productModel, int productID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(id => id.ProductID == productID);
                if (product == null)
                {
                    throw new Exception("");
                }
                productModel.ApplyTo(product);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format("fully!"),
                    Status = Result.ResultStatus.success,
                };
            }
        }
    }
}
