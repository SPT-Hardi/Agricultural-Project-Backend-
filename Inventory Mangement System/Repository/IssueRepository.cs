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
        //View Issue Details
        public Result ViewAllIssue()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.Issues
                            orderby obj.IssueID descending
                            select new
                            {
                                IssueID = obj.IssueID,
                                Date = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.IssueDate),
                                Product = new { Id = obj.Product.ProductID, Text = obj.Product.ProductName , Unit = obj.Product.ProductUnit.Type },
                                MainArea = new IntegerNullString() { Id = obj.MainArea.MainAreaID, Text = obj.MainArea.MainAreaName },
                                SubArea = new IntegerNullString() { Id = obj.SubArea.SubAreaID, Text = obj.SubArea.SubAreaName },
                                IssueQuantity = obj.PurchaseQuantity,
                                Remark=obj.Remark,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == obj.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.DateTime),
                            }).ToList(),
                };
            }
        }
        
        //Issue Products
        public Result IssueProduct(IssueModel issueModel, int LoginId)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue i = new Issue();
                var qs = (from obj in issueModel.issueDetails
                          select new Issue()
                          {
                              ProductID = obj.Product.Id,
                              MainAreaID = issueModel.MainArea.Id,
                              SubAreaID = issueModel.SubArea.Id,
                              Remark = obj.Remark,
                              IssueDate = issueModel.Date.ToLocalTime(),
                              LoginID = LoginId,
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

        //Edit Issue Details
        public Result EditIssue(IssueModel issueModel, int ID, int LoginId)
        {
            float RemainQuantity = 0;
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                //var MacAddress = context.LoginDetails.FirstOrDefault(c => c.SystemMac == macObj);
                var qs = (from obj in context.Issues
                         where obj.IssueID == ID
                         select obj).SingleOrDefault();
                var p = (from obj in issueModel.issueDetails
                         select obj).SingleOrDefault();
                var pd = (from obj in context.Products
                          where obj.ProductID == qs.ProductID
                          select obj).SingleOrDefault();
                var ps = (from obj in context.Products
                          where obj.ProductID == p.Product.Id
                          select obj).SingleOrDefault();
                var pid = (from obj in context.Issues
                           where obj.SubAreaID == issueModel.SubArea.Id
                           select new
                           {
                               ProductID = obj.ProductID
                           }).ToList();
                if(qs.PurchaseQuantity< p.IssueQuantity)
                {
                    if (pd.TotalProductQuantity < p.IssueQuantity)
                    {
                        throw new ArgumentException($"Product name :{pd.ProductID} ," +
                            $"Enter quantity{p.IssueQuantity} more than existing quantity{pd.TotalProductQuantity}");
                    }
                }
                
                if (qs.SubAreaID == issueModel.SubArea.Id)
                {
                    if (qs.ProductID == p.Product.Id)
                    {
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        RemainQuantity = (float)temp - p.IssueQuantity;
                        qs.IssueDate= issueModel.Date.ToLocalTime();
                        qs.DateTime = DateTime.Now;
                        qs.ProductID = p.Product.Id;
                        qs.MainAreaID = issueModel.MainArea.Id;
                        qs.SubAreaID = issueModel.SubArea.Id;
                        qs.LoginID = LoginId;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = p.IssueQuantity;

                        ps.TotalProductQuantity = RemainQuantity;
                        context.SubmitChanges();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Issue Update Successfully",
                            Data = $"Issue ID : {ID} updated successfully",
                        };
                    }
                    else
                    {
                        foreach (var item in pid)
                        {
                            if (p.Product.Id == item.ProductID)
                            {
                                throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                    $" area : {issueModel.SubArea.Text}");

                            }
                        }
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        qs.IssueDate = issueModel.Date.ToLocalTime();
                        qs.DateTime = DateTime.Now;
                        qs.ProductID = p.Product.Id;
                        qs.MainAreaID = issueModel.MainArea.Id;
                        qs.SubAreaID = issueModel.SubArea.Id;
                        qs.LoginID = LoginId;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = p.IssueQuantity;
                        RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                        ps.TotalProductQuantity = RemainQuantity;
                        pd.TotalProductQuantity = temp;
                        context.SubmitChanges();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Issue Update Successfully",
                            Data = $"Issue ID : {ID} updated successfully",
                        };
                    }
                }
                else
                {
                    foreach (var item in pid)
                    {
                        if (p.Product.Id == item.ProductID)
                        {
                            throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                $" area : {issueModel.SubArea.Text}");

                        }
                    }
                    var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                    pd.TotalProductQuantity = temp;
                    qs.IssueDate= issueModel.Date.ToLocalTime();
                    qs.DateTime = DateTime.Now;
                    qs.ProductID = p.Product.Id;
                    qs.MainAreaID = issueModel.MainArea.Id;
                    qs.SubAreaID = issueModel.SubArea.Id;
                    qs.LoginID = LoginId;
                    qs.Remark = p.Remark;
                    qs.PurchaseQuantity = p.IssueQuantity;
                    RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                    ps.TotalProductQuantity = RemainQuantity;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Issue Update Successfully",
                        Data = $"Issue ID : {ID} updated successfully",
                    };

                }
                
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

        //Get Product Dropdown with Unit And Quantity
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
 
    }
}