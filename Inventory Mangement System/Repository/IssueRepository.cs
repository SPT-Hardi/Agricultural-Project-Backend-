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

        public async Task<IEnumerable> GetProductwithquantity()
        {

            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                return (from x in context.Products
                        select new IntegerNullString()
                        {
                            Text = x.ProductName,
                            Id = x.ProductID,
                            Total = (float)x.TotalProductQuantity,
                        }).ToList();

                //var q = context.Products.Select(x => x.ProductName);

                //    var q2 = (from m in context.Products
                //              join p in context.PurchaseDetails
                //              on m.ProductID equals p.ProductID
                //              group p by new { m.ProductName, m.ProductID } into g
                //              select new
                //              {
                //                  Productname = g.Key.ProductName,
                //                  PurchaseQuantity = (float)g.Sum(x => x.TotalQuantity)
                //              }).ToList();

                //    var q3 = (from m in context.Products
                //              join i in context.Issues
                //              on m.ProductID equals i.ProductID
                //              group i by new { m.ProductName, m.ProductID } into g
                //              select new
                //              {
                //                  Productname = g.Key.ProductName,
                //                  IssueQuantity = (float)g.Sum(x => x.PurchaseQuantity)
                //              }).ToList();

                //var result = (from p in context.Products
                //              join pur in context.PurchaseDetails
                //              on p.ProductID equals pur.ProductID
                //              join i in context.Issues
                //              on p.ProductID equals i.ProductID
                //              group new { pur, i } by new { p.ProductID, p.ProductName, i.PurchaseQuantity } into newg
                //              select new
                //              {
                //                  Text = newg.Key.ProductName,
                //                  Total = (float)newg.Sum(g => g.pur.TotalQuantity) - (float)newg.Sum(g => g.i.PurchaseQuantity),
                //                  //Issuet = (float)newg.Sum(g => g.i.PurchaseQuantity),
                //              }).ToList();

                //return new Result()
                //{
                //    Status = Result.ResultStatus.success,

                //};
            }
        }


        //public Result IssueProduct(IssueModel issueModel)
        //{
        //    using (ProductInventoryDataContext context = new ProductInventoryDataContext())
        //    {
        //        Issue issue = new Issue();
        //        var i = (from r in issueModel.issueDetails
        //                 select new Issue()
        //                 {
        //                     PurchaseQuantity = r.IssueQuantity,
        //                     Date = r.Date.ToLocalTime(),
        //                     MainAreaID = r.MainArea.Id,
        //                     SubAreaID = r.SubArea.Id,
        //                     ProductID = r.Product.Id,
        //                 }).ToList();
        //        foreach (var item in i)
        //        {
        //            var query1 = (from r in context.PurchaseDetails
        //                          where r.ProductID == item.ProductID
        //                          select r.TotalQuantity).ToList();
        //            double sum = 0;
        //            foreach (var q1 in query1)
        //            {
        //                sum = sum + q1;
        //            }

        //            var query2 = (from r in context.Issues
        //                          where r.ProductID == item.ProductID
        //                          select r.PurchaseQuantity).ToList();
        //            double p = 0;
        //            foreach (var q2 in query2)
        //            {
        //                p = p + q2;
        //            }
        //            var diff = sum - p;
        //            var name = (from pro in context.Products
        //                        where pro.ProductID == item.ProductID
        //                        select new { ProdcutName = pro.ProductName }).FirstOrDefault();

        //            if (diff < item.PurchaseQuantity)
        //            {
        //                throw new ArgumentException($"{name.ProdcutName} Out of Stock. Total Quantity is {diff}");
        //            }
        //        }
        //        context.Issues.InsertAllOnSubmit(i);
        //        context.SubmitChanges();
        //        foreach (var item in i)
        //        {
        //            var updatePQ = context.Products.SingleOrDefault(x => x.ProductID == item.ProductID);
        //            updatePQ.TotalProductQuantity = updatePQ.TotalProductQuantity  - item.PurchaseQuantity;
        //            context.SubmitChanges();
        //        }
        //        return new Result()
        //        {
        //            Message = string.Format($" Issue successfully!"),
        //            Status = Result.ResultStatus.success,
        //        };
        //    }
        //}

        public Result IssueProduct(IssueModel issueModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue i = new Issue();
                UserLoginDetails login = new UserLoginDetails();
                var MacAddress = login.GetMacAddress().Result;

                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == MacAddress);
                var qs = (from obj in issueModel.issueDetails
                          select new Issue()
                          {
                              ProductID = obj.Product.Id,
                              MainAreaID = issueModel.MainArea.Id,
                              SubAreaID = issueModel.SubArea.Id,
                              Remark = obj.Remark,
                              LoginID = 1,
                              Date = DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity
                          }).ToList();
                foreach (var item in qs)
                {
                    var p = (from obj in context.Products
                             where obj.ProductID == item.ProductID
                             select obj.TotalProductQuantity).SingleOrDefault();
                    if (p < item.PurchaseQuantity)
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
                    Message = "successfully",
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
