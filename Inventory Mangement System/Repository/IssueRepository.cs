using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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
                                Product = new { Id = obj.Product.ProductID, Text = obj.Product.ProductName, Unit = obj.Product.ProductUnit.Type },
                                MainArea = new IntegerNullString() { Id = obj.MainArea.MainAreaID, Text = obj.MainArea.MainAreaName },
                                SubArea = new IntegerNullString() { Id = obj.SubAreaID==null? 0 : obj.SubArea.SubAreaID, Text = obj.SubAreaID==null ? null : obj.SubArea.SubAreaName },
                                IssueQuantity = obj.PurchaseQuantity,
                                Remark = obj.Remark,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == obj.LoginID
                                            select n.UserName).FirstOrDefault(),
                                IssueDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.IssueDate),
                                LastUpdated= String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                BackupDate= String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                            }).ToList(),
                };
            }
        }

        //Issue Products
        public Result IssueProduct(IssueModel issueModel, int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    Issue i = new Issue();
                    var qs = (from obj in issueModel.issueDetails
                              select new Issue()
                              {
                                  ProductID = obj.Product.Id,
                                  MainAreaID = issueModel.MainArea.Id,
                                  SubAreaID = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Id,
                                  Remark = obj.Remark,
                                  LoginID = LoginId,
                                  IssueDate = issueModel.Date.ToLocalTime(),
                                  LastUpdated = ISDT,
                                  BackupDate = ISDT,
                                  PurchaseQuantity = obj.IssueQuantity
                              }).ToList();

                    foreach (var item in qs)
                    {
                        var p = (from obj in context.Products
                                 where obj.ProductID == item.ProductID
                                 select new
                                 {
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
                    scope.Complete();
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
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            float RemainQuantity = 0;
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope()) 
                {
                    
                    //var MacAddress = context.LoginDetails.FirstOrDefault(c => c.SystemMac == macObj);
                    var qs = (from obj in context.Issues
                              where obj.IssueID == ID
                              select obj).SingleOrDefault();
                    if (qs.IsEditable == false)
                    {
                        throw new ArgumentException("Not editable!");
                    }
                    // backup entry not editable
                    Issue backup = new Issue();
                    backup.ProductID = qs.ProductID;
                    backup.MainAreaID = qs.MainAreaID;
                    backup.SubAreaID = qs.SubAreaID;
                    backup.Remark = qs.Remark;
                    backup.LoginID = qs.LoginID;
                    backup.IssueDate = qs.IssueDate;
                    backup.LastUpdated = qs.LastUpdated;
                    backup.BackupDate = qs.BackupDate;
                    backup.PurchaseQuantity = qs.PurchaseQuantity;
                    backup.IsEditable = false;
                    context.Issues.InsertOnSubmit(backup);
                    context.SubmitChanges();


                var p = (from obj in issueModel.issueDetails
                         select obj).SingleOrDefault();
                var pd = (from obj in context.Products
                          where obj.ProductID == qs.ProductID
                          select obj).SingleOrDefault();
                var ps = (from obj in context.Products
                          where obj.ProductID == p.Product.Id
                          select obj).SingleOrDefault();
                
                
                if (p.IssueQuantity == 0)
                {
                    throw new ArgumentException($"Please Enter {p.Product.Text} Issue Quantity More Than Zero");
                }
                if (qs.PurchaseQuantity < p.IssueQuantity)
                {
                    if (pd.TotalProductQuantity+qs.PurchaseQuantity < p.IssueQuantity)
                    {
                        throw new ArgumentException($"Product name :{pd.ProductID} ," +
                            $"Enter quantity{p.IssueQuantity} more than existing quantity{pd.TotalProductQuantity}");
                    }
                }
                    if (qs.SubAreaID != null)
                    {

                        if (qs.SubAreaID == issueModel.SubArea.Id)
                        {
                            if (qs.ProductID == p.Product.Id)
                            {
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                RemainQuantity = (float)temp - p.IssueQuantity;
                                qs.IssueDate = issueModel.Date.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = p.IssueQuantity;

                                ps.TotalProductQuantity = RemainQuantity;
                                context.SubmitChanges();

                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = $"Issue ID : {ID} updated successfully",
                                };
                            }
                            else
                            {
                                var pid = (from obj in context.Issues
                                           where obj.SubAreaID == issueModel.SubArea.Id
                                           select new
                                           {
                                               ProductID = obj.ProductID
                                           }).ToList();

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
                                qs.LastUpdated = ISDT;
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

                                scope.Complete();
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
                            var pid = (from obj in context.Issues
                                       where obj.SubAreaID == issueModel.SubArea.Id
                                       select new
                                       {
                                           ProductID = obj.ProductID
                                       }).ToList();

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
                            qs.IssueDate = issueModel.Date.ToLocalTime();
                            qs.LastUpdated = ISDT;
                            qs.ProductID = p.Product.Id;
                            qs.MainAreaID = issueModel.MainArea.Id;
                            qs.SubAreaID = issueModel.SubArea.Id;
                            qs.LoginID = LoginId;
                            qs.Remark = p.Remark;
                            qs.PurchaseQuantity = p.IssueQuantity;
                            RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                            ps.TotalProductQuantity = RemainQuantity;
                            context.SubmitChanges();

                            scope.Complete();
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
                        if (qs.MainAreaID == issueModel.MainArea.Id)
                        {
                            if (qs.ProductID == p.Product.Id)
                            {
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                RemainQuantity = (float)temp - p.IssueQuantity;
                                qs.IssueDate = issueModel.Date.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                //qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = p.IssueQuantity;

                                ps.TotalProductQuantity = RemainQuantity;
                                context.SubmitChanges();

                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = $"Issue ID : {ID} updated successfully",
                                };
                            }
                            else
                            {
                                var pid = (from obj in context.Issues
                                           where obj.MainAreaID == issueModel.MainArea.Id
                                           select new
                                           {
                                               ProductID = obj.ProductID
                                           }).ToList();

                                foreach (var item in pid)
                                {
                                    if (p.Product.Id == item.ProductID)
                                    {
                                        throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                            $" area : {issueModel.MainArea.Text}");

                                    }
                                }
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                qs.IssueDate = issueModel.Date.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                // qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = p.IssueQuantity;
                                RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                                ps.TotalProductQuantity = RemainQuantity;
                                pd.TotalProductQuantity = temp;
                                context.SubmitChanges();

                                scope.Complete();
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
                            var pid = (from obj in context.Issues
                                       where obj.MainAreaID == issueModel.MainArea.Id
                                       select new
                                       {
                                           ProductID = obj.ProductID
                                       }).ToList();

                            foreach (var item in pid)
                            {
                                if (p.Product.Id == item.ProductID)
                                {
                                    throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                        $" area : {issueModel.MainArea.Text}");

                                }
                            }
                            var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                            pd.TotalProductQuantity = temp;
                            qs.IssueDate = issueModel.Date.ToLocalTime();
                            qs.LastUpdated = ISDT;
                            qs.ProductID = p.Product.Id;
                            qs.MainAreaID = issueModel.MainArea.Id;
                            //qs.SubAreaID = issueModel.SubArea.Id;
                            qs.LoginID = LoginId;
                            qs.Remark = p.Remark;
                            qs.PurchaseQuantity = p.IssueQuantity;
                            RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                            ps.TotalProductQuantity = RemainQuantity;
                            context.SubmitChanges();

                            scope.Complete();
                            return new Result()
                            {
                                Status = Result.ResultStatus.success,
                                Message = "Issue Update Successfully",
                                Data = $"Issue ID : {ID} updated successfully",
                            };

                        }
                    }
                          

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
                            Quantity = x.TotalProductQuantity,
                            Unit = x.ProductUnit.Type
                        }).ToList();
            }
        }
    }
}
