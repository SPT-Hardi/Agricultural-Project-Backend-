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

        //Issue Details
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
                }
            }
        }

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
                    Status = Result.ResultStatus.success ,
                    Message = "Total Quantity Purchase",
                    Data = sum,
                };
            }
        }
    }
}
