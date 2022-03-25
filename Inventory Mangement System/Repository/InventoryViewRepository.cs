﻿using Inventory_Mangement_System.Model.Common;
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
        public Result GetInventoryView()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result(){
                    Status=Result.ResultStatus.success,
                    Data=(from p in context.Products
                        select new
                        {
                            ProductName = p.ProductName,
                            Category = new IntegerNullString() { Id = p.Category.CategoryID, Text = p.Category.CategoryName, },
                            Quantity = p.TotalProductQuantity,
                            UserName = (from n in context.LoginDetails
                                        where n.LoginID == p.LoginID
                                        select n.UserName).FirstOrDefault(),
                            DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", p.DateTime),
                        }).ToList(),
                };
                
            }
        }
    }
}
