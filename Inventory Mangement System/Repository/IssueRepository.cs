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
                            where obj.IsEditable==true
                            orderby obj.IssueID descending
                            select new
                            {
                                IssueID = obj.IssueID,
                                Product = new { Id = obj.Product.ProductID, Text = obj.Product.ProductName, Unit = obj.Product.ProductUnit.Type },
                                Area = new IntegerNullString() { Id = obj.AreaDetail.AreaId, Text = obj.AreaDetail.AreaName },
                                IssueQuantity = obj.PurchaseQuantity,
                                Remark = obj.Remark,
                                UserName = obj.LoginDetail.UserName,
                                IssueDate = obj.IssueDate.ToString("dd-MM-yyyy"),
                                LastUpdated = obj.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                IsEditable=obj.IsEditable,
                                EditedList=(from x in context.Issues
                                            where x.ParentId==obj.IssueID
                                            orderby x.IssueID descending
                                            select new 
                                            {
                                                IssueID = x.IssueID,
                                                Product = new { Id = x.Product.ProductID, Text = x.Product.ProductName, Unit = x.Product.ProductUnit.Type },
                                                Area = new IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                                IssueQuantity = x.PurchaseQuantity,
                                                Remark = x.Remark,
                                                UserName = x.LoginDetail.UserName,
                                                IssueDate = x.IssueDate,
                                                LastUpdated =x.LastUpdated,
                                                IsEditable = x.IsEditable,

                                            }).ToList()
                            }).ToList(),
                };
            }
        }
        public Result GetEditIssueDetails(int Id) 
        {
            using (ProductInventoryDataContext c = new ProductInventoryDataContext())
            {

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Edited Isuue details get successfully!",
                    Data =(from obj in c.Issues
                           where obj.ParentId==Id
                           select new 
                           {
                               IssueID = obj.IssueID,
                               Product = new { Id = obj.Product.ProductID, Text = obj.Product.ProductName, Unit = obj.Product.ProductUnit.Type },
                               Area = new IntegerNullString() { Id = obj.AreaDetail.AreaId, Text = obj.AreaDetail.AreaName },
                               IssueQuantity = obj.PurchaseQuantity,
                               Remark = obj.Remark,
                               UserName = obj.LoginDetail.UserName,
                               IssueDate = obj.IssueDate.ToString("dd-MM-yyyy"),
                               LastUpdated = obj.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                               IsEditable = obj.IsEditable,
                           }).ToList()
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
                                  AreaId=issueModel.Area.Id,
                                  Remark = obj.Remark,
                                  LoginID = LoginId,
                                  IssueDate = issueModel.IssueDate.ToLocalTime(),
                                  LastUpdated = ISDT,
                                  PurchaseQuantity = (decimal)obj.IssueQuantity
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
                    backup.AreaId = qs.AreaId;
                    backup.ParentId = qs.IssueID;
                    backup.Remark = qs.Remark;
                    backup.LoginID = LoginId;
                    backup.IssueDate = qs.IssueDate;
                    backup.LastUpdated = ISDT;
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
                if (qs.PurchaseQuantity < (decimal)p.IssueQuantity)
                {
                    if (pd.TotalProductQuantity+qs.PurchaseQuantity < ((decimal)p.IssueQuantity))
                    {
                        throw new ArgumentException($"Product name :{pd.ProductID} ," +
                            $"Enter quantity{p.IssueQuantity} more than existing quantity{pd.TotalProductQuantity}");
                    }
                }
                    if (qs.ProductID == p.Product.Id)
                    {
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        RemainQuantity = (float)temp - p.IssueQuantity;
                        qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                        qs.LastUpdated = ISDT;
                        qs.ProductID = p.Product.Id;
                        qs.AreaId = issueModel.Area.Id;
                        qs.LoginID = LoginId;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = ((decimal)p.IssueQuantity);

                        ps.TotalProductQuantity = ((decimal)RemainQuantity);
                        context.SubmitChanges();

                        var res = new
                        {
                            IssueID = qs.IssueID,
                            Product = new { Id = p.Product.Id, Text = p.Product.Text },
                            Area = new IntegerNullString() { Id=issueModel.Area.Id,Text=issueModel.Area.Text},
                            IssueQuantity = qs.PurchaseQuantity,
                            Remark = qs.Remark,
                            UserName = (from n in context.LoginDetails
                                        where n.LoginID == LoginId
                                        select n.UserName).FirstOrDefault(),
                            IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                            LastUpdated = qs.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                            IsEditable = qs.IsEditable,
                            EditedList = (from x in context.Issues
                                          where x.ParentId == qs.IssueID
                                          orderby x.IssueID descending
                                          select new
                                          {
                                              IssueID = x.IssueID,
                                              Product = new { Id = x.Product.ProductID, Text = x.Product.ProductName, Unit = x.Product.ProductUnit.Type },
                                              Area = new IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                              IssueQuantity = x.PurchaseQuantity,
                                              Remark = x.Remark,
                                              UserName = x.LoginDetail.UserName,
                                              IssueDate = x.IssueDate,
                                              LastUpdated = x.LastUpdated,
                                              IsEditable = x.IsEditable,

                                          }).ToList()

                        };

                        scope.Complete();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Issue Update Successfully",
                            Data = res
                        };
                    }
                    else
                    {
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                        qs.LastUpdated = ISDT;
                        qs.ProductID = p.Product.Id;
                        qs.AreaId = issueModel.Area.Id;
                        qs.LoginID = LoginId;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = ((decimal)p.IssueQuantity);
                        RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                        ps.TotalProductQuantity = ((decimal)RemainQuantity);
                        pd.TotalProductQuantity = temp;
                        context.SubmitChanges();

                        var res = new
                        {
                            IssueID = qs.IssueID,
                            Product = new { Id = p.Product.Id, Text = p.Product.Text },
                            Area = new IntegerNullString() {Id=issueModel.Area.Id,Text=issueModel.Area.Text },
                            IssueQuantity = qs.PurchaseQuantity,
                            Remark = qs.Remark,
                            UserName = (from n in context.LoginDetails
                                        where n.LoginID == LoginId
                                        select n.UserName).FirstOrDefault(),
                            IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                            LastUpdated = qs.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                            IsEditable = qs.IsEditable,
                            EditedList = (from x in context.Issues
                                          where x.ParentId == qs.IssueID
                                          orderby x.IssueID descending
                                          select new
                                          {
                                              IssueID = x.IssueID,
                                              Product = new { Id = x.Product.ProductID, Text = x.Product.ProductName, Unit = x.Product.ProductUnit.Type },
                                              Area = new IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                              IssueQuantity = x.PurchaseQuantity,
                                              Remark = x.Remark,
                                              UserName = x.LoginDetail.UserName,
                                              IssueDate = x.IssueDate,
                                              LastUpdated = x.LastUpdated,
                                              IsEditable = x.IsEditable,

                                          }).ToList()
                        };

                        scope.Complete();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Issue Update Successfully",
                            Data = res,
                        };
                    }
                    /*if (qs.SubAreaID != null)
                    {

                        if (qs.SubAreaID == issueModel.SubArea.Id)
                        {
                            if (qs.ProductID == p.Product.Id)
                            {
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                RemainQuantity = (float)temp - p.IssueQuantity;
                                qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = ((decimal)p.IssueQuantity);

                                ps.TotalProductQuantity = ((decimal)RemainQuantity);
                                context.SubmitChanges();

                                var res = new
                                {
                                    IssueID = qs.IssueID,
                                    Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                    MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                    SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                    IssueQuantity = qs.PurchaseQuantity,
                                    Remark = qs.Remark,
                                    UserName = (from n in context.LoginDetails
                                                where n.LoginID == qs.LoginID
                                                select n.UserName).FirstOrDefault(),
                                    IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                    //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                    //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                    IsEditable = qs.IsEditable,
                                };
                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = res
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
                                qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = ((decimal)p.IssueQuantity);
                                RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                                ps.TotalProductQuantity = ((decimal)RemainQuantity);
                                pd.TotalProductQuantity = temp;
                                context.SubmitChanges();

                                var res = new
                                {
                                    IssueID = qs.IssueID,
                                    Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                    MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                    SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                    IssueQuantity = qs.PurchaseQuantity,
                                    Remark = qs.Remark,
                                    UserName = (from n in context.LoginDetails
                                                where n.LoginID == qs.LoginID
                                                select n.UserName).FirstOrDefault(),
                                    IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                    //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                    //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                    IsEditable = qs.IsEditable,
                                };
                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = res,
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
                            qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                            qs.LastUpdated = ISDT;
                            qs.ProductID = p.Product.Id;
                            qs.MainAreaID = issueModel.MainArea.Id;
                            qs.SubAreaID = issueModel.SubArea.Id;
                            qs.LoginID = LoginId;
                            qs.Remark = p.Remark;
                            qs.PurchaseQuantity = ((decimal)p.IssueQuantity);
                            RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                            ps.TotalProductQuantity = ((decimal)RemainQuantity);
                            context.SubmitChanges();

                            var res = new
                            {
                                IssueID = qs.IssueID,
                                Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                IssueQuantity = qs.PurchaseQuantity,
                                Remark = qs.Remark,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == qs.LoginID
                                            select n.UserName).FirstOrDefault(),
                                IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                IsEditable = qs.IsEditable,
                            };
                            scope.Complete();
                            return new Result()
                            {
                                Status = Result.ResultStatus.success,
                                Message = "Issue Update Successfully",
                                Data = res,
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
                                qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                //qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = ((decimal)p.IssueQuantity);

                                ps.TotalProductQuantity = ((decimal)RemainQuantity);
                                context.SubmitChanges();

                                var res = new
                                {
                                    IssueID = qs.IssueID,
                                    Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                    MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                    SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                    IssueQuantity = qs.PurchaseQuantity,
                                    Remark = qs.Remark,
                                    UserName = (from n in context.LoginDetails
                                                where n.LoginID == qs.LoginID
                                                select n.UserName).FirstOrDefault(),
                                    IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                    //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                    //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                    IsEditable = qs.IsEditable,
                                };
                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = res,
                                };
                            }
                            else
                            {
                               *//* var pid = (from obj in context.Issues
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
                                }*//*
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                                qs.LastUpdated = ISDT;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                              //qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = LoginId;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = ((decimal)p.IssueQuantity);
                                RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                                ps.TotalProductQuantity = ((decimal)RemainQuantity);
                                pd.TotalProductQuantity = temp;
                                context.SubmitChanges();

                                var res = new
                                {
                                    IssueID = qs.IssueID,
                                    Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                    MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                    SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                    IssueQuantity = qs.PurchaseQuantity,
                                    Remark = qs.Remark,
                                    UserName = (from n in context.LoginDetails
                                                where n.LoginID == qs.LoginID
                                                select n.UserName).FirstOrDefault(),
                                    IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                    //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                    //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                    IsEditable = qs.IsEditable,
                                };
                                scope.Complete();
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = "Issue Update Successfully",
                                    Data = res,
                                };
                            }
                        }
                        else
                        {
                           *//* var pid = (from obj in context.Issues
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
                            }*//*
                            var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                            pd.TotalProductQuantity = temp;
                            qs.IssueDate = issueModel.IssueDate.ToLocalTime();
                            qs.LastUpdated = ISDT;
                            qs.ProductID = p.Product.Id;
                            qs.MainAreaID = issueModel.MainArea.Id;
                            //qs.SubAreaID = issueModel.SubArea.Id;
                            qs.LoginID = LoginId;
                            qs.Remark = p.Remark;
                            qs.PurchaseQuantity = ((decimal)p.IssueQuantity);
                            RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                            ps.TotalProductQuantity = ((decimal)RemainQuantity);
                            context.SubmitChanges();
                            var res = new
                            {
                                IssueID = qs.IssueID,
                                Product = new { Id = p.Product.Id, Text = p.Product.Text },
                                MainArea = new IntegerNullString() { Id = issueModel.MainArea.Id, Text = issueModel.MainArea.Text },
                                SubArea = new IntegerNullString() { Id = issueModel.SubArea.Id == 0 ? 0 : issueModel.SubArea.Id, Text = issueModel.SubArea.Id == 0 ? null : issueModel.SubArea.Text },
                                IssueQuantity = qs.PurchaseQuantity,
                                Remark = qs.Remark,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == qs.LoginID
                                            select n.UserName).FirstOrDefault(),
                                IssueDate = qs.IssueDate.ToString("dd-MM-yyyy"),
                                //LastUpdated = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.LastUpdated),
                                //BackupDate = String.Format("{0:dd-MM-yyyy hh:mm tt}", obj.BackupDate),
                                IsEditable = qs.IsEditable,
                            };
                            scope.Complete();
                            return new Result()
                            {
                                Status = Result.ResultStatus.success,
                                Message = "Issue Update Successfully",
                                Data = res,
                            };

                        }
                    }*/


                }

                          

            }
        }

        //Get MainArea Dropdown
        public async Task<IEnumerable> GetArea()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.AreaDetails
                        select new IntegerNullString()
                        {
                            Text = x.AreaName,
                            Id = x.AreaId
                        }).ToList();
            }
        }

        //Get SubArea Dropdown
    /*    public async Task<IEnumerable> GetSubArea(int id)
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
        }*/

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
