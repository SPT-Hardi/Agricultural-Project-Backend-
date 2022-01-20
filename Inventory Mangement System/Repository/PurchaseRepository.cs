﻿using Inventory_Mangement_System.Model;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        public async Task<string> AddPurchaseDetails(PurchaseModel purchaseModel)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext ())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                purchaseDetail.ProductID = purchaseModel.productname.Id;
                var funit = (from u in context.Products
                             where u.ProductID == purchaseModel.productname.Id
                             select u.Unit).SingleOrDefault ();
                purchaseDetail.Unit = funit;
                purchaseDetail.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();
                purchaseDetail.TotalQuantity = purchaseModel.totalquantity;
                purchaseDetail.TotalCost  = purchaseModel.totalcost;
                purchaseDetail.Remark  = purchaseModel.remarks;
                purchaseDetail.VendorName = purchaseModel.vendorname;
                context.PurchaseDetails.InsertOnSubmit(purchaseDetail);
                context.SubmitChanges();
                return "Product Purchase Successfully";
            }
        }
    }
}
