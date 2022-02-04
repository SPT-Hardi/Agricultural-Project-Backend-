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
        //GetMainArea Dropdown
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

        //GetSubArea Dropdown
        public async Task<IEnumerable> GetSubArea(int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.SubAreas where x.MainAreaID == id select new IntegerNullString()
                {
                Text = x.SubAreaName,
                Id = x.SubAreaID
                }).ToList();
            }
        }

        //GetProduct Dropdown
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

        float diff;
        private readonly float df;

        public Result GetProductwithquantity()
        {

            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();

                var q2 = (from m in context.Products
                          join p in context.PurchaseDetails
                          on m.ProductID equals p.ProductID
                          group  p  by new { m.ProductName, m.ProductID }  into g
                          select new
                          {
                              Productname = g.Key.ProductName,
                              PurchaseQuantity = (float)g.Sum(x => x.TotalQuantity)
                          }).Select (s => s).ToList();

                var q3 = (from m in context.Products
                          join i in context.Issues
                          on m.ProductID equals i.ProductID
                          group i  by new { m.ProductName, m.ProductID} into g
                          select new
                          {
                              Productname = g.Key.ProductName,
                              IssueQuantity = (float)g.Sum(x => x.PurchaseQuantity) 
                          }).ToList();
                
                

                return new Result()
                {
                    Status = Result.ResultStatus.success,

                };
            }
        }

        //Issue Details
        public Result IssueProduct(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue issue = new Issue();
                //var query = (from r in context.PurchaseDetails
                //             where r.ProductID == issueModel.Product.Id
                //             select r.TotalQuantity).ToList();
                //double sum = 0;
                //foreach (var item in query)
                //{
                //    sum = sum + item;
                //}

                //var query2 = (from r in context.Issues
                //              where r.ProductID == issueModel.Product.Id
                //              select r.PurchaseQuantity).ToList();
                //double p = 0;
                //foreach (var item in query2)
                //{
                //    p = p + item;
                //}
                //var diff = sum - p;
                //if (diff >= issueModel.IssueQuantity)
                //{
                //}
                //    else
                //{
                //    throw new ArgumentException($"Product Out Of Stock.Total Quantity is {diff}");
                //}
                var query = (from i in issueModel.issueDetails
                             select new Issue
                             {
                                 PurchaseQuantity = i.IssueQuantity,
                                 Date = i.Date.ToLocalTime(),
                                 MainAreaID = i.MainArea.Id,
                                 SubAreaID = i.SubArea.Id,
                                 ProductID = i.Product.Id
                             }).ToList();
                context.Issues.InsertAllOnSubmit(query);
                context.SubmitChanges();
                return new Result()
                {
                   Message = string.Format($"Product Issue successfully!"),
                   Status = Result.ResultStatus.success,
                };
            }
        }

        public Result total(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue issue = new Issue();
                var query = (from p in context.PurchaseDetails
                             join i in context.Issues
                             on p.ProductID equals i.ProductID
                             select new
                             {
                                 ProductID = p.ProductID,
                                 PurchaseQuantity = p.TotalQuantity,
                                 IssueQuantity = i.PurchaseQuantity
                             }).ToList();
                foreach (var item in query)
                {
                    double sum = 0;
                    sum = sum + item.PurchaseQuantity;
                    double p = 0;
                    p = p + item.IssueQuantity;
                    double diff = sum - p;
                }


                //var query = (from r in context.PurchaseDetails
                //             where r.ProductID == issueModel.Product.Id
                //             select r.TotalQuantity).ToList();
                //double sum = 0;
                //foreach (var item in query)
                //{
                //    sum = sum + item;
                //}

                //var query2 = (from r in context.Issues
                //              where r.ProductID == issueModel.Product.Id
                //              select r.PurchaseQuantity).ToList();
                //double p = 0;
                //foreach (var item in query2)
                //{
                //    p = p + item;
                //}

               // var diff = sum - p;
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Total Quantity Purchase",
                   // Data = sum,
                };
            }
        }
    }
}
