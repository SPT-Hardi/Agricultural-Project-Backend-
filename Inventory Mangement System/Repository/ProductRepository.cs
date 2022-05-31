using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.JsonPatch;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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
        public Result AddProduct(ProductModel productModel,int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();

            var pro = (from p in productModel.ProductDetails
                       select new Product()
                       {
                           ProductName = char.ToUpper(p.ProductName[0]) + p.ProductName.Substring(1).ToLower(),
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = p.categoryType.Id,
                           LoginID=LoginId,
                           TotalProductQuantity=0,
                           DateTime=ISDT,
                           UnitID = p.type.Id,
             
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
                Message = string.Format($"Product Added Successfully."),
                Status = Result.ResultStatus.success
            };
        }

        //Edit Product 
        public Result EditProduct(ProductDetail productDetail, int productID,int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
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
                    /*if (product.IsEditable == false) 
                    {
                        throw new ArgumentException("Not editable!");
                    }
                    //backup entry
                    product.IsEditable = false;
                    context.SubmitChanges();*/

                    //update
                    //Product p = new Product();
                    product.ProductName = char.ToUpper(productDetail.ProductName[0]) + productDetail.ProductName.Substring(1).ToLower();
                    product.Variety = productDetail.Variety;
                    product.Company = productDetail.Company;
                    product.Description = productDetail.Description;
                    product.CategoryID = (int)productDetail.categoryType.Id;
                    product.LoginID = LoginId;
                    product.DateTime = ISDT;
                    product.UnitID = (int)productDetail.type.Id;
                    //context.Products.InsertOnSubmit(p);
                    context.SubmitChanges();

                    var res = new
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Variety = product.Variety,
                        Company = product.Company,
                        Description = product.Description,
                        Type = new Model.Common.IntegerNullString() { Id = productDetail.type.Id, Text = productDetail.type.Text },
                        CategoryType = new Model.Common.IntegerNullString() { Id = productDetail.categoryType.Id, Text = productDetail.categoryType.Text },
                        UserName = (from n in context.LoginDetails
                                    where n.LoginID == product.LoginID
                                    select n.UserName).FirstOrDefault(),
                        DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", product.DateTime),
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"{productDetail.ProductName} Updated Successfully."),
                        Status = Result.ResultStatus.success,
                        Data=res
                    };
                }
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
