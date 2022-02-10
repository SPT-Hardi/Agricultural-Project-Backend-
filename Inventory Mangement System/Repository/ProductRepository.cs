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

            var pro = (from p in productModel.ProductDetails
                       select new Product()
                       {
                           ProductName = p.ProductName,
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = (int)p.categorytype.Id,
                           Unit = (string)p.type.Text,
                           TotalProductQuantity = 0
                       }).ToList();
            foreach (var item in pro)
            {
                var pname = context.Products.Where(name => name.ProductName == item.ProductName).SingleOrDefault();
                if (pname != null)
                {
                    throw new ArgumentException($"{item.ProductName} product Alredy Exits.");
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

            //var pname = context.Products.Where(name => name.ProductName == productModel.ProductName).SingleOrDefault();
            //if (pname != null)
            //{
            //    throw new ArgumentException("Alredy Exits");
            //}
            //product.ProductName = productModel.ProductName;
            //product.Variety = productModel.Variety;
            //product.Company = productModel.Company;
            //product.Description = productModel.Description;
            //product.Unit = (string)productModel.type.Text;
            //product.CategoryID = (int)productModel.categorytype.Id;
            //context.Products.InsertOnSubmit(product);
            //context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            //return new Result()
            //{
            //    //Message = string.Format($"{productModel.ProductName} Added successfully!"),
            //    Status = Result.ResultStatus.success,
            //    //Data= productModel.ProductName,
            //};
        }

        //async Task<IEnumerable>
        public async Task<IEnumerable> GetUnit()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return(from x in context.ProductUnits 
                        select new IntegerNullString()
                        {
                            Text = x.Type,
                            Id = x.UnitID 
                        }).ToList();
            }
        }

        public async Task<IEnumerable> ViewAllProduct()
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext())
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

        public async Task<IEnumerable> ViewProductById(int productID)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(x => x.ProductID == productID);
                if(product == null)
                {
                    throw new ArgumentException("Product Does Not Exist");
                }
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

        public Result EditProduct(ProductDetail productDetail,int id)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(x => x.ProductID == id);
                if(product == null)
                {
                    throw new ArgumentException("Product doesn't exist");
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
                product.CategoryID = productDetail.categorytype.Id;
                product.Unit = productDetail.type.Text;
                product.Description = productDetail.Description;
                context.Products.InsertOnSubmit(product);
                context.SubmitChanges();
                return new Result()
                {
                    Message = "Updated Successfully",
                    Data = product.ProductName,
                    Status =  Result.ResultStatus.success,
                };
            }
        }
    }
}
