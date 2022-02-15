using Inventory_Mangement_System.Middleware;
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
    public class IssueRepository : IIssueRepository
    {
        //private readonly UserLoginDetails _userLoginDetails;

        //private float diff;
        //public IssueRepository(UserLoginDetails userLoginDetails)
        //{
        //    _userLoginDetails = userLoginDetails;
        //}

        //All Issue Details
        public Result ViewAllIssue()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from i in context.Issues
                            join p in context.Products
                            on i.ProductID equals p.ProductID
                            join m in context.MainAreas
                            on i.MainAreaID equals m.MainAreaID
                            join s in context.SubAreas
                            on i.SubAreaID equals s.SubAreaID
                            select new
                            {
                                IssueID = i.IssueID,
                                Date = i.IssueDate,
                                ProductName = p.ProductName,
                                MainAreaName = m.MainAreaName,
                                SubAreaName = s.SubAreaName,
                                PurchaseQuantity = i.PurchaseQuantity,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == i.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-mm-yyyy hh:mm tt}", i.DateTime),
                            }).ToList(),
                };
            }
        }
       /* //Issue Products
        public Result IssueProduct(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue issue = new Issue();
                var query = (from r in context.PurchaseDetails
                             where r.ProductID == issueModel.Product.Id
                             select r.TotalQuantity).ToList();
                double sum = 0;
                foreach (var item in query)
                {
                    sum = sum + item;
                }

                var query2 = (from r in context.Issues
                              where r.ProductID == issueModel.Product.Id
                              select r.PurchaseQuantity).ToList();
                double p = 0;
                foreach (var item in query2)
                {
                    p = p + item;
                }

                var diff = sum - p;
                if (diff >= issueModel.IssueQuantity)
                {
                    issue.PurchaseQuantity = issueModel.IssueQuantity;
                    issue.Date = issueModel.Date.ToLocalTime();
                    issue.MainAreaID = issueModel.MainArea.Id;
                    issue.SubAreaID = issueModel.SubArea.Id;
                    issue.ProductID = issueModel.Product.Id;
                    context.Issues.InsertOnSubmit(issue);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"{issueModel.Product.Text} Issue successfully!"),
                        Status = Result.ResultStatus.success,
                        Data = issueModel.Product.Text,
                    };
                }
                else
                {
                    throw new ArgumentException($"Product Out Of Stock.Total Quantity is {diff}");
                    return new Result()
                    {
                        Message = string.Format($"Product Out Of Stock.Total Quantity is {diff}"),
                        Status = Result.ResultStatus.none,
                        Data = diff,
                    };
                }

                return $"{issueModel.Product.Text } Add Successfully";
            }
        }*/

        //Issue Products
        public Result IssueProduct(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue i = new Issue();
                UserLoginDetails userLoginDetails = new UserLoginDetails();
                var MacAddress = userLoginDetails.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMAC == MacAddress);
                var qs = (from obj in issueModel.issueDetails
                          select new Issue()
                          {
                              ProductID = obj.Product.Id,
                              MainAreaID = issueModel.MainArea.Id,
                              SubAreaID = issueModel.SubArea.Id,
                              Remark = obj.Remark,
                              IssueDate = issueModel.Date.ToLocalTime(),
                              //LoginID = LoginID.LoginID,
                              DateTime = DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity
                          }).ToList();

                foreach (var item in qs)
                {
                    var p = (from obj in context.Products
                             where obj.ProductID == item.ProductID
                             select new {
                                 obj.TotalProductQuantity,
                                 obj.ProductName
                             }).SingleOrDefault();
                    if (item.PurchaseQuantity == 0)
                    {
                        throw new ArgumentException($"Please Enter {p.ProductName} Issue Quantity More Than Zero");
                    }
                    if (p.TotalProductQuantity < item.PurchaseQuantity)
                    {
                        throw new ArgumentException($"Product name :{item.ProductID} ," +
                            $"Enter quantity{item.PurchaseQuantity} more than existing quantity{p}");
                    }
                }
                context.Issues.InsertAllOnSubmit(qs);
                context.SubmitChanges();
                foreach (var item in qs)
                {
                    var updatePQ = context.Products.FirstOrDefault(c => c.ProductID == item.ProductID);
                    updatePQ.TotalProductQuantity = updatePQ.TotalProductQuantity - item.PurchaseQuantity;
                    context.SubmitChanges();

                }
                
                return new Result()
                {
                    Message = string.Format($" Issue successfully!"),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        public Result EditIssue(IssueModel issueModel,int id)
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                var _p = (from obj in issueModel.issueDetails
                          select new Issue()
                          {
                              ProductID = obj.Product.Id,
                              MainAreaID = issueModel.MainArea.Id,
                              SubAreaID = issueModel.SubArea.Id,
                              Remark = obj.Remark,
                              IssueDate = issueModel.Date.ToLocalTime(),
                              //LoginID = LoginID.LoginID,
                              DateTime = DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity
                          });
                var product = (from p in context.Products
                               select new { 
                                   p.ProductName,
                                   p.TotalProductQuantity
                               }).FirstOrDefault();

                var _product = (from obj in issueModel.issueDetails
                                select obj).FirstOrDefault();

                if (product ==null)
                {
                    throw new ArgumentException("Product Is Not Exist.");
                }
                if (product.TotalProductQuantity<_product.IssueQuantity)
                {
                    throw new ArgumentException($"{product.ProductName} Enter quantity {_product.IssueQuantity} more than existing quantity {product.TotalProductQuantity}");
                }
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($" Issue successfully!"),
                    Status = Result.ResultStatus.success,
                };
            }
        }
        //Issue Detail By Id
        public async Task<IEnumerable> ViewIssueById(int issueID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue issue=new Issue();
                issue = context.Issues.SingleOrDefault(id => id.IssueID == issueID);
                if (issue == null)
                {
                    throw new ArgumentException("Issue Detail Does Not Exist.");
                }
                return (from i in context.Issues
                        join p in context.Products
                        on i.ProductID equals p.ProductID
                        join m in context.MainAreas
                        on i.MainAreaID equals m.MainAreaID
                        join s in context.SubAreas
                        on i.SubAreaID equals s.SubAreaID
                        where i.IssueID == issueID
                        select new
                        {
                            IssueID = i.IssueID,
                            Date = i.IssueDate,
                            ProductName = p.ProductName,
                            MainAreaName = m.MainAreaName,
                            SubAreaName = s.SubAreaName,
                            PurchaseQuantity = i.PurchaseQuantity
                        }).ToList();
            }
        }

        //Get MainArea Dropdown
        public async Task<IEnumerable> GetMainArea()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.MainAreas
                        select new IntegerNullString()
                        {
                            Text = x.MainAreaName,
                            Id = x.MainAreaID
                        }).ToList();
            }
        }

        //Get SubArea Dropdown
        public async Task<IEnumerable> GetSubArea(int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.SubAreas 
                        where x.MainAreaID == id 
                        select new IntegerNullString()
                        {
                            Text = x.SubAreaName,
                            Id = x.SubAreaID
                        }).ToList();
            }
        }

        //Get Product Dropdown
        public async Task<IEnumerable> GetProduct()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Products
                        select new 
                        {
                            Text = x.ProductName,
                            Id = x.ProductID,
                            Quantity=x.TotalProductQuantity,
                            Unit=x.ProductUnit.Type
                        }).ToList();
            }
        }
        //Get Product With Total Quantity Dropdown
 /*       public async Task<IEnumerable> GetProductTotalQuantity()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                
                var result = (from p in context.Products
                          join pur in context.PurchaseDetails
                          on p.ProductID equals pur.ProductID
                              join i in context.Issues
                              on p.ProductID equals i.ProductID
                              group new { pur, i } by new { p.ProductID, p.ProductName} into newg
                          select new// IntegerNullString
                          {
                              Text = newg.Key.ProductName,
                              Total = (float)newg.Sum(g => g.pur.TotalQuantity),
                              Issuet= (float)newg.Sum(g => g.i.PurchaseQuantity),
                          }).ToList();
                
                return result;
        //        var productListResult = orderProductVariantListResult
        //.GroupBy(pv => pv.Product)
        //.Select(g => new
        //{
        //    Product = g.Key,
        //    TotalOrderCount = g.Count(),
        //    TotalSales = g.Sum(pv => pv.Sales),
        //    TotalQuantity = g.Sum(pv => pv.Quantity),
        //})
        //.OrderByDescending(x => x.TotalOrderCount).ToList();
                

                //return (from p in context.Products
                //        join pur in context.PurchaseDetails
                //        on p.ProductID equals pur.ProductID
                //        join i in context.Issues
                //        on p.ProductID equals i.ProductID
                //        select new IntegerNullString()
                //        {
                //            Id = p.ProductID,
                //            Text = p.ProductName,
                //            Total = (float)((float)pur.TotalQuantity-i.PurchaseQuantity)
                //        }).ToList();
            }
        }
        /*
        public Result total(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue issue = new Issue();
                var query = (from r in context.PurchaseDetails
                             where r.ProductID == issueModel.Product.Id
                             select r.TotalQuantity).ToList();
                double sum = 0;
                foreach (var item in query)
                {
                    sum = sum + item;
                }

                var query2 = (from r in context.Issues
                              where r.ProductID == issueModel.Product.Id
                              select r.PurchaseQuantity).ToList();
                double p = 0;
                foreach (var item in query2)
                {
                    p = p + item;
                }

                var diff = sum - p;
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message= "Total Quantity Purchase",
                    Data = sum,
                };
            }
        }*/
    }
}
