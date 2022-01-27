using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class InventoryViewRepository: IInventoryViewRepository
    {
        public async Task<IEnumerable> GetInventoryView()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                //var query = (from r in context.PurchaseDetails
                //             join c in context.Products
                //             on r.ProductID equals c.ProductID
                //             select r.TotalQuantity).ToList();
                //double sum = 0;
                //foreach (var item in query)
                //{
                //    sum = sum + item;
                //}

                //var query2 = (from r in context.Issues
                //              join c in context.Products
                //             on r.ProductID equals c.ProductID
                //              select r.PurchaseQuantity).ToList();
                //double cu = 0;
                //foreach (var item in query2)
                //{
                //    cu= cu + item;
                //}

                //var diff = sum - cu;
                return (from p in context.Products
                        join c in context.Categories
                        on p.CategoryID equals c.CategoryID
                        join r in context.PurchaseDetails
                        on p.ProductID equals r.ProductID
                        //join i in context.Issues
                        //on p.ProductID equals i.ProductID
                        select new
                        {
                            ProductName=p.ProductName,
                            Variety=p.Variety,
                            Company=p.Company,
                            Category=c.CategoryName,
                            Quantity = r.TotalQuantity//-i.PurchaseQuantity
                        }).ToList();
                //return (from x in context.Products
                //        select new IntegerNullString()
                //        {
                //            Text = x.ProductName,
                //            Id = x.ProductID
                //        }).ToList();
                //return new Result()
                //{
                //    Message = string.Format("fully!"),
                //    Status = Result.ResultStatus.success,
                //    Data = result,
                //};
            }
        }
    }
}
