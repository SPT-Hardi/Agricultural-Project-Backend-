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
    public class PurchaseRepository : IPurchaseRepository
    {
        //View Purchase Details
        public Result GetPurchaseDetails()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from obj in context.PurchaseDetails
                            join obj2 in context.Products
                            on obj.ProductID equals obj2.ProductID into JoinTablePN
                            from PN in JoinTablePN.DefaultIfEmpty()
                            orderby obj.PurchaseID descending 
                            select new
                            {
                                PurchaseID = obj.PurchaseID,
                                ProductName = new IntegerNullString() { Id = obj.Product.ProductID, Text = obj.Product.ProductName },
                                TotalQuantity = obj.TotalQuantity,
                                TotalCost = obj.TotalCost,
                                Unit = obj.Unit,//new IntegerNullString() { Id=obj.ProductUnit.UnitID,Text=obj.ProductUnit.Type},
                                Remark = obj.Remark,
                                VendorName = obj.VendorName,
                                PurchaseDate = String.Format("{0:dd-mm-yyyy hh:mm tt}", obj.PurchaseDate),
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == obj.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-mm-yyyy hh:mm tt}", obj.DateTime),
                            }).ToList(),

                };
            }

        }

        //Add Purchase Details
        public Result AddPurchaseDetails(PurchaseModel purchaseModel,int LoginId)
        {
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
                                        TotalQuantity = obj.totalquantity,
                                        TotalCost = obj.totalcost,
                                        Remark = obj.remarks,
                                        VendorName = char.ToUpper(obj.vendorname[0]) + obj.vendorname.Substring(1).ToLower(),
                                        LoginID = LoginId,
                                        DateTime = DateTime.Now


                                    }).ToList();
                foreach (var item in purchaselist)
                {
                    var qs = (from obj in context.Products
                              where obj.ProductID == item.ProductID
                              select obj).SingleOrDefault();
                    qs.TotalProductQuantity = (int)qs.TotalProductQuantity + item.TotalQuantity;
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
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var funit = (from obj in purchaseModel.purchaseList
                             from u in context.Products
                             where obj.productname.Id == u.ProductID
                             select u.ProductUnit.Type).SingleOrDefault();
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID == ID
                          select obj).SingleOrDefault();

                var q = (from obj in purchaseModel.purchaseList
                         select obj).SingleOrDefault();

                var issue = (from i in context.Issues
                             where i.ProductID == qs.ProductID
                             select i).ToList();
                if(issue!=null)
                {
                    throw new ArgumentException($"Product Already Issue Can not be Update.");
                }
                if (qs.ProductID == q.productname.Id)
                {
                    var db = (from obj in context.Products
                              where obj.ProductID == qs.ProductID
                              select obj).SingleOrDefault();
                    var updatedQuantity = db.TotalProductQuantity - qs.TotalQuantity;
                    db.TotalProductQuantity = updatedQuantity + q.totalquantity;
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
                    udb.TotalProductQuantity = udb.TotalProductQuantity + q.totalquantity;
                    context.SubmitChanges();
                }
                qs.ProductID = q.productname.Id;
                qs.TotalQuantity = q.totalquantity;
                qs.TotalCost = q.totalcost;
                qs.Unit = funit;
                qs.Remark = q.remarks;
                qs.LoginID = LoginId;
                qs.VendorName = char.ToUpper(q.vendorname[0]) + q.vendorname.Substring(1).ToLower();
                qs.PurchaseDate = q.Purchasedate.ToLocalTime();
                context.SubmitChanges();

                return new Result()
                {
                    Message = string.Format("Purchase Updated Successfully"),
                    Status = Result.ResultStatus.success,

                };
            }
        }
        
        
    }
}