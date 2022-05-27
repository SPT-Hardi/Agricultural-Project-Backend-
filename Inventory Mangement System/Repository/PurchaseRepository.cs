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
    public class PurchaseRepository : IPurchaseRepository
    {
        //View Purchase Details
        public Result GetPurchaseDetails()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var res = (from obj in context.PurchaseDetails/*
                            join obj2 in context.Products
                            on obj.ProductID equals obj2.ProductID into JoinTablePN
                            from PN in JoinTablePN.DefaultIfEmpty()*/
                               where obj.IsEditable == true
                               orderby obj.PurchaseID descending
                               select new
                               {
                                   PurchaseID = obj.PurchaseID,
                                   ProductName = new IntegerNullString() { Id = obj.Product.ProductID, Text = obj.Product.ProductName },
                                   TotalQuantity = obj.TotalQuantity,
                                   TotalCost = obj.TotalCost,
                                   Unit = obj.Unit,
                                   Remark = obj.Remark,
                                   VendorName = obj.VendorName,
                                   PurchaseDate = obj.PurchaseDate.ToString("dd-MM-yyyy"),
                                   UserName = obj.LoginDetail.UserName,
                                   LastUpdated = obj.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                   IsEditable = obj.IsEditable,
                                   BillNumber = obj.BillNumber,
                                   PurchaseLocation = obj.PurchaseLocation,
                                   EditedList = (from x in context.PurchaseDetails
                                                 where x.ParentId == obj.PurchaseID
                                                 orderby x.PurchaseID descending
                                                 select new
                                                 {
                                                     PurchaseID = x.PurchaseID,
                                                     ProductName = new IntegerNullString() { Id = x.Product.ProductID, Text = x.Product.ProductName },
                                                     TotalQuantity = x.TotalQuantity,
                                                     TotalCost = x.TotalCost,
                                                     Unit = x.Unit,
                                                     Remark = x.Remark,
                                                     VendorName = x.VendorName,
                                                     PurchaseDate =x.PurchaseDate,
                                                     UserName = x.LoginDetail.UserName,
                                                     LastUpdated = x.LastUpdated,
                                                     IsEditable = x.IsEditable,
                                                     BillNumber = x.BillNumber.ToLower(),
                                                     PurchaseLocation = x.PurchaseLocation,


                                                 }).ToList(),
                               }).ToList();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = res

                    };
                }
            }

        }
        public Result GetPurchaseEditDetails(int Id) 
        {
            using (ProductInventoryDataContext c = new ProductInventoryDataContext())
            {
                    var res = (from obj in c.PurchaseDetails
                               where obj.ParentId==Id
                               select new
                               {
                                   PurchaseID = obj.PurchaseID,
                                   ProductName = new IntegerNullString() { Id = obj.Product.ProductID, Text = obj.Product.ProductName },
                                   TotalQuantity = obj.TotalQuantity,
                                   TotalCost = obj.TotalCost,
                                   Unit = obj.Unit,
                                   Remark = obj.Remark,
                                   VendorName = obj.VendorName,
                                   PurchaseDate = obj.PurchaseDate.ToString("dd-MM-yyyy"),
                                   UserName = obj.LoginDetail.UserName,
                                   LastUpdated = obj.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                   IsEditable = obj.IsEditable,
                                   BillNumber = obj.BillNumber,
                                   PurchaseLocation = obj.PurchaseLocation

                               }).ToList();
                   
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Edited Purchase details get successfully!",
                        Data = "Good",
                    };
                
            }
        }
        public string GetFormatedDate(DateTime date) 
        {
            return date.ToString("dd-MM-yyyy");
        }
        //Add Purchase Details
        public Result AddPurchaseDetails(PurchaseModel purchaseModel,int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            if (LoginId == 0) 
            {
                throw new ArgumentException("You are not authorized!");
            }
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                
                var purchaselist = (from obj in purchaseModel.purchaseList
                                    select new PurchaseDetail()
                                    {
                                        ProductID = obj.productname.Id,
                                        Unit = (from u in context.Products
                                                  where u.ProductID == obj.productname.Id
                                                  select u.ProductUnit.Type).SingleOrDefault(),
                                        PurchaseDate = obj.Purchasedate.ToLocalTime(),
                                        TotalQuantity = ((decimal)obj.totalquantity),
                                        TotalCost = ((decimal)obj.totalcost),
                                        Remark = obj.remarks,
                                        PurchaseLocation=obj.PurchaseLocation,
                                        BillNumber=obj.BillNumber,
                                        VendorName = (obj.vendorname == null  ? null : char.ToUpper(obj.vendorname[0]) + obj.vendorname.Substring(1).ToLower()),
                                        LoginID = LoginId,
                                        LastUpdated = ISDT,
                                    }).ToList();
                foreach (var item in purchaselist)
                {
                    var qs = (from obj in context.Products
                              where obj.ProductID == item.ProductID
                              select obj).SingleOrDefault();
                    qs.TotalProductQuantity = qs.TotalProductQuantity + item.TotalQuantity;
                    context.SubmitChanges();
                }

                context.PurchaseDetails.InsertAllOnSubmit(purchaselist);
                context.SubmitChanges();

                return new Result()
                {
                    Message = string.Format($"Total {purchaselist.Count()} Product Purchase Successfully"),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        //Edit Purchase Details
        public Result EditPurchaseProduct(PurchaseModel purchaseModel, int ID,int LoginId)
        {
            var ISDT = new ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (LoginId == 0)
                    {
                        throw new ArgumentException("You are not authorized!");
                    }
                    var funit = (from obj in purchaseModel.purchaseList
                                 from u in context.Products
                                 where obj.productname.Id == u.ProductID
                                 select u.ProductUnit.Type).SingleOrDefault();

                    var qs = (from obj in context.PurchaseDetails
                              where obj.PurchaseID == ID
                              select obj).SingleOrDefault();
                    if (qs.IsEditable == false) 
                    {
                        throw new ArgumentException("Not editable!");
                    }
                    var q = (from obj in purchaseModel.purchaseList
                             select obj).SingleOrDefault();

                    var issue = (from i in context.Issues
                                 where i.ProductID == qs.ProductID
                                 select i).ToList();
                    if (issue == null)
                    {
                        throw new ArgumentException($"Product Already Issue Can not be Update.");
                    }
                    if (qs.ProductID == q.productname.Id)
                    {
                        var db = (from obj in context.Products
                                  where obj.ProductID == qs.ProductID
                                  select obj).SingleOrDefault();
                        var updatedQuantity = db.TotalProductQuantity - qs.TotalQuantity;
                        db.TotalProductQuantity = updatedQuantity + ((decimal)q.totalquantity);
                        context.SubmitChanges();
                    }
                    else
                    {
                        var db = (from obj in context.Products
                                  where obj.ProductID == qs.ProductID
                                  select obj).SingleOrDefault();
                        var udb = (from obj in context.Products
                                   where obj.ProductID == q.productname.Id
                                   select obj).SingleOrDefault();

                        db.TotalProductQuantity = db.TotalProductQuantity - qs.TotalQuantity;
                        udb.TotalProductQuantity = udb.TotalProductQuantity + ((decimal)q.totalquantity);
                        context.SubmitChanges();
                    }
                    //enter new backup not editable
                    PurchaseDetail backup = new PurchaseDetail();
                    backup.ProductID =qs.ProductID;
                    backup.ParentId = qs.PurchaseID;
                    backup.TotalCost =qs.TotalCost;
                    backup.TotalQuantity =qs.TotalQuantity;
                    backup.Unit =qs.Unit;
                    backup.PurchaseDate =qs.PurchaseDate;
                    backup.PurchaseLocation =qs.PurchaseLocation;
                    backup.LastUpdated =ISDT;
                    backup.BillNumber =qs.BillNumber;
                    backup.Remark =qs.Remark;
                    backup.LoginID =LoginId;
                    backup.VendorName =qs.VendorName;
                    backup.IsEditable = false;
                    context.PurchaseDetails.InsertOnSubmit(backup);
                    context.SubmitChanges();


                    //update old entry editable
                    qs.PurchaseDate = q.Purchasedate.ToLocalTime();
                    qs.ProductID = q.productname.Id;
                    qs.TotalQuantity = ((decimal)q.totalquantity);
                    qs.TotalCost = ((decimal)q.totalcost);
                    qs.Unit = funit;
                    qs.PurchaseLocation = q.PurchaseLocation;
                    qs.BillNumber = q.BillNumber;
                    qs.Remark = q.remarks;
                    qs.LoginID = LoginId;
                    qs.VendorName = (q.vendorname == null ? null : char.ToUpper(q.vendorname[0]) + q.vendorname.Substring(1).ToLower());
                    qs.LastUpdated = ISDT;
                    context.SubmitChanges();

                    var res = new
                    {
                        PurchaseID = qs.PurchaseID,
                        ProductName = new IntegerNullString() { Id = q.productname.Id, Text = q.productname.Text },
                        TotalQuantity = qs.TotalQuantity,
                        TotalCost = qs.TotalCost,
                        Unit = qs.Unit,
                        Remark = qs.Remark,
                        VendorName = qs.VendorName,
                        PurchaseDate = qs.PurchaseDate.ToString("dd-MM-yyyy"),
                        UserName = (from n in context.LoginDetails
                                    where n.LoginID ==LoginId
                                    select n.UserName).FirstOrDefault(),
                        LastUpdated =  qs.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                        IsEditable = qs.IsEditable,
                        BillNumber = qs.BillNumber,
                        PurchaseLocation = qs.PurchaseLocation,
                        EditedList = (from x in context.PurchaseDetails
                                      where x.ParentId == qs.PurchaseID
                                      orderby x.PurchaseID descending
                                      select new
                                      {
                                          PurchaseID = x.PurchaseID,
                                          ProductName = new IntegerNullString() { Id = x.Product.ProductID, Text = x.Product.ProductName },
                                          TotalQuantity = x.TotalQuantity,
                                          TotalCost = x.TotalCost,
                                          Unit = x.Unit,
                                          Remark = x.Remark,
                                          VendorName = x.VendorName,
                                          PurchaseDate = x.PurchaseDate,
                                          UserName = x.LoginDetail.UserName,
                                          LastUpdated = x.LastUpdated,
                                          IsEditable = x.IsEditable,
                                          BillNumber = x.BillNumber.ToLower(),
                                          PurchaseLocation = x.PurchaseLocation,


                                      }).ToList(),

                    };
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format("Purchase Updated Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = res

                    };
                }
                
               
            }
        }
        
        
    }
}