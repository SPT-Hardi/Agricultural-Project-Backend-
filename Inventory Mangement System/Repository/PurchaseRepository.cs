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
            using(ProductInventoryDataContext context = new ProductInventoryDataContext ())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                purchaseDetail.ProductID = purchaseModel.productname.Id;
                var funit = (from u in context.Products
                             where u.ProductID == purchaseModel.productname.Id
                             select u.ProductUnit.Type).SingleOrDefault ();
                purchaseDetail.Unit = funit;
                purchaseDetail.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();
                purchaseDetail.TotalQuantity = purchaseModel.totalquantity;
                purchaseDetail.TotalCost  = purchaseModel.totalcost;
                purchaseDetail.Remark  = purchaseModel.remarks;
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
        }

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
                                TotalQuantiry = obj.TotalQuantity,
                                TotalCost = obj.TotalCost,
                                Unit = obj.Unit,
                                Remark = obj.Remark,
                                VendorName = obj.VendorName,
                                PurchaseDate = obj.PurchaseDate,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == obj.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-mm-yyyy hh:mm tt}", obj.DateTime),
                            }).ToList(),
                };
            }

        }
    }
}
