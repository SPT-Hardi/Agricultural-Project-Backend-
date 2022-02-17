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
        public async Task<IEnumerable> GetunitByid(int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Products
                        where x.ProductID == id
                        select new
                        {
                            Unit = x.ProductUnit.Type,
                        }).ToList();
            }
        }
        public Result AddPurchaseDetails(PurchaseModel purchaseModel)
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
                                        VendorName = obj.vendorname,
                                        LoginID = 1,
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
                    Message = string.Format($" Purchase successfully!"),
                    Status = Result.ResultStatus.success,
                    Data = $"Total {purchaselist.Count()} Product Purchase Successfully",
                };
            }
        }
        /*public Result AddPurchaseDetails(PurchaseModel purchaseModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                purchaseDetail.ProductID = purchaseModel.productname.Id;
                var funit = (from u in context.Products
                             where u.ProductID == purchaseModel.productname.Id
                             select u.ProductUnit.Type).SingleOrDefault();
                purchaseDetail.Unit = funit;
                purchaseDetail.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();
                purchaseDetail.TotalQuantity = purchaseModel.totalquantity;
                purchaseDetail.TotalCost = purchaseModel.totalcost;
                purchaseDetail.Remark = purchaseModel.remarks;
                purchaseDetail.VendorName = purchaseModel.vendorname;

                context.PurchaseDetails.InsertOnSubmit(purchaseDetail);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{purchaseModel.productname.Text} Purchase successfully!"),
                    Status = Result.ResultStatus.success,
                    Data = purchaseModel.productname.Text,
                };
            }
        }*/

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
                            select new
                            {
                                PurchaseID = obj.PurchaseID,
                                ProductName = PN.ProductName,
                                TotalQuantity = obj.TotalQuantity,
                                TotalCost = obj.TotalCost,
                                Unit = obj.Unit,
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

        public Result EditPurchaseProduct(PurchaseModel purchaseModel, int ID)
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
                qs.VendorName = q.vendorname;
                qs.PurchaseDate = q.Purchasedate.ToLocalTime();
                context.SubmitChanges();

                return new Result()
                {
                    Message = "Purchase Updated Successfully",
                    Status = Result.ResultStatus.success,
                    Data = q.productname.Text,
                };
            }
        }
        
        /*public Result Update(PurchaseModel purchaseModel, int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var funit = (from obj in purchaseModel.purchaseList
                             from u in context.Products
                             where obj.productname.Id == u.ProductID
                             select u.ProductUnit.UnitID).SingleOrDefault();
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID == ID
                          select obj).SingleOrDefault();

                var q = (from obj in purchaseModel.purchaseList
                         select obj).SingleOrDefault();


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
                qs.UnitID = funit;
                qs.Remark = q.remarks;
                qs.VendorName = q.vendorname;
                qs.PurchaseDate = q.Purchasedate.ToLocalTime();
                context.SubmitChanges();

                return new Result()
                {
                    Message = "Purchase Updated Successfully",
                    Status = Result.ResultStatus.success,
                    Data = q.productname.Text,
                };
            }
        }*/
    }
}
