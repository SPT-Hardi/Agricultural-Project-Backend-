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
        public async Task<IEnumerable> ViewAllProduct()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Products
                        join catid in context.Categories
                        on x.CategoryID equals catid.CategoryID
                        select new
                        {
                            ProductID = x.ProductID,
                            ProductName = x.ProductName,
                            Varitey = x.Variety,
                            Company = x.Company,
                            Description = x.Description,
                            Unit = x.Unit,
                            CategoryName = catid.CategoryName
                        }).ToList();
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
            product.CategoryID = (int)productModel.categorytype.Id;
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
                           Description=p.Description,
                           CategoryID = p.categorytype.Id,
                           Unit=p.type.Text                           
                       }).ToList();
            foreach (var item in pro)
            {
                var pname = context.Products.Where(name => name.ProductName == item.ProductName).SingleOrDefault();
                if (pname != null)
                {
                    throw new ArgumentException($"{item.ProductName} Are Alredy Exits.");
                }
            }
            
            //product.ProductName = productModel.ProductName;
            //product.Variety = productModel.Variety;
            //product.Company = productModel.Company;
            //product.Description = productModel.Description;
            //product.Unit = (string)productModel.type.Text;
            //product.CategoryID = (int)productModel.categorytype.Id;
            context.Products.InsertAllOnSubmit(pro);
            context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            return new Result()
            {
                Message = string.Format($"{product.ProductName} Added Successfully."),
                Status = Result.ResultStatus.success,
                Data = product.ProductName,
            };
        }

        //View Product By Id
        public async Task<IEnumerable> ViewProductById(int productID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(id => id.ProductID == productID);
                if (product == null)
                {
                    throw new ArgumentException("Product Does Not Exist.");
                }
                return (from x in context.Products
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
                            Unit = x.Unit,
                            CategoryName = catid.CategoryName
                        }).ToList();
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
                if(product.ProductName != productDetail.ProductName)
                {
                    var _product= context.Products.SingleOrDefault(name => name.ProductName == productDetail.ProductName);
                    if (_product != null)
                    {
                        throw new Exception("Product Alredy Exits.");
                    }
                }
                product.ProductName = productDetail.ProductName;
                product.Variety = productDetail.Variety;
                product.Company = productDetail.Company;
                product.Description = productDetail.Description;
                product.CategoryID = (int)productDetail.categorytype.Id;
                product.Unit = (string)productDetail.type.Text;
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
                            Text = x.Type ,
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
                    //Data = product,
                };
            }
        }
    }
}
