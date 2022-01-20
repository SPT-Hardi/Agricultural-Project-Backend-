﻿using Inventory_Mangement_System.Model;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class ProductionRepository : IProductionRepository
    {
        public async Task<string> AddProductionDetails(ProductionModel productionModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                ProductionDetail productionDetail = new ProductionDetail();
                var v = (from x in context.Vegetables
                         where x.VegetableName.ToLower() == productionModel.vegetablenm.ToLower()
                         select x).FirstOrDefault();
                if(v == null)
                {
                    v = new Vegetable()
                    {
                        VegetableName = productionModel.vegetablenm
                    };
                    context.Vegetables.InsertOnSubmit(v);
                    context.SubmitChanges();
                }

                int vg = (from obj in context.Vegetables
                          where obj.VegetableName == productionModel.vegetablenm
                          select obj.VegetableID).SingleOrDefault();

                productionDetail.MainAreaID = productionModel.mainAreaDetails.Id;
                productionDetail.SubAreaID = productionModel.subAreaDetails.Id;
                productionDetail.VegetableID = vg;
                context.ProductionDetails.InsertOnSubmit(productionDetail);
                context.SubmitChanges();
                return "Product details added succesfully ";
            }
        }
    }
}
