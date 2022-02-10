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
        //public async Task<IEnumerable> GetunitByid(int id)
        //{
        //    using (ProductInventoryDataContext context = new ProductInventoryDataContext())
        //    {
        //        return (from x in context.Products
        //                where x.ProductID == id
        //                select new
        //                {
        //                    Unit = x.Unit,
        //                }).ToList();
        //    }
        //}

        public Result  AddPurchaseDetails(PurchaseModel purchaseModel)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext ())
            {
                //PurchaseDetail purchaseDetail = new PurchaseDetail();
                //purchaseDetail.ProductID = purchaseModel.productname.Id;
                //var funit = (from u in context.Products
                //             where u.ProductID == purchaseModel.productname.Id
                //             select u.Unit).SingleOrDefault ();
                //purchaseDetail.Unit = funit;
                //purchaseDetail.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();
                //purchaseDetail.TotalQuantity = purchaseModel.totalquantity;
                //purchaseDetail.TotalCost  = purchaseModel.totalcost;
                //purchaseDetail.Remark  = purchaseModel.remarks;
                //purchaseDetail.VendorName = purchaseModel.vendorname;

                //context.PurchaseDetails.InsertOnSubmit(purchaseDetail);
                //context.SubmitChanges();
                //return new Result()
                //{
                //    Message = string.Format($"Product Purchase Successfully"),
                //    Status = Result.ResultStatus.success,
                //    Data = purchaseModel.productname.Text,
                //};

                var purchaselist = (from obj in purchaseModel.purchaseList
                                    select new PurchaseDetail()
                                    {
                                        ProductID = obj.productname.Id,
                                        Unit = (from u in context.Products
                                                where u.ProductID == obj.productname.Id
                                                select u.Unit).SingleOrDefault(),
                                        PurchaseDate = obj.Purchasedate.ToLocalTime(),
                                        TotalQuantity = obj.totalquantity,
                                        TotalCost = obj.totalcost,
                                        Remark = obj.remarks,
                                        VendorName = obj.vendorname
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
                    Message = string.Format($" Purchase successfully!"),
                    Status = Result.ResultStatus.success,
                    Data = DateTime.Now,
                };
            }
        }
    }
}
