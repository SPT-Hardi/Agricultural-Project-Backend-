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
        private float diff;

        //All Issue Details
        public async Task<IEnumerable> ViewAllIssue()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from i in context.Issues
                        join p in context.Products
                        on i.ProductID equals p.ProductID
                        join m in context.MainAreas
                        on i.MainAreaID equals m.MainAreaID
                        join s in context.SubAreas
                        on i.SubAreaID equals s.SubAreaID
                        select new
                        {
                            IssueID = i.IssueID,
                            Date = i.Date,
                            ProductName = p.ProductName,
                            MainAreaName = m.MainAreaName,
                            SubAreaName = s.SubAreaName,
                            PurchaseQuantity = i.PurchaseQuantity
                        }).ToList();
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
                Issue issue = new Issue();
                var i = (from r in issueModel.issueDetails
                         select new Issue()
                         {
                             PurchaseQuantity = r.IssueQuantity,
                             Date = r.Date.ToLocalTime(),
                             MainAreaID = r.MainArea.Id,
                             SubAreaID = r.SubArea.Id,
                             ProductID = r.Product.Id,
                         }).ToList();
                foreach (var item in i)
                {
                    var query1 = (from r in context.PurchaseDetails
                                 where r.ProductID == item.ProductID
                                 select r.TotalQuantity).ToList();
                    double sum = 0;
                    foreach (var q1 in query1)
                    {
                        sum = sum + q1;
                    }

                    var query2 = (from r in context.Issues
                                  where r.ProductID == item.ProductID
                                  select r.PurchaseQuantity).ToList();
                    double p = 0;
                    foreach (var q2 in query2)
                    {
                        p = p + q2;
                    }
                    var diff = sum - p;
                    var name = (from pro in context.Products
                                  where pro.ProductID == item.ProductID
                                  select new { ProdcutName=pro.ProductName }).FirstOrDefault();

                    if (diff<item.PurchaseQuantity)
                    {
                        throw new ArgumentException($"{name.ProdcutName} Out of Stock. Total Quantity is {diff}");
                    }
                }
                context.Issues.InsertAllOnSubmit(i);
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
                            Date = i.Date,
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
                        select new IntegerNullString()
                        {
                            Text = x.ProductName,
                            Id = x.ProductID
                        }).ToList();
            }
        }
        //Get Product With Total Quantity Dropdown
        public async Task<IEnumerable> GetProductTotalQuantity()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                
                var result = (from p in context.Products
                          join pur in context.PurchaseDetails
                          on p.ProductID equals pur.ProductID
                           //join i in context.Issues
                           //on p.ProductID equals i.ProductID
                           group pur by new { p.ProductID, p.ProductName } into newg
                          select new IntegerNullString
                          {
                              Text = newg.Key.ProductName,
                              //Total = (float)newg.Sum(g => g.TotalQuantity),
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
