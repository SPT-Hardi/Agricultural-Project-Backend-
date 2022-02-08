using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class ProductionRepository : IProductionRepository
    {
        public Result AddProductionDetails(ProductionModel productionModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
                {
                    ProductionDetail productionDetail = new ProductionDetail();
                    UserLoginDetails login = new UserLoginDetails();
                    ProductionModel pm = new ProductionModel();
                    var UserMACAddress = login.GetMacAddress().Result;

                    var vn1 = (from m in productionModel.productionLists
                               from y in context.Vegetables
                               where m.vegetablenm.ToLower() == y.VegetableName.ToLower()
                               select new
                               {
                                   VegetableName = y.VegetableName
                               }).ToList();
                    if (vn1.Count() == 0)
                    {
                        var vegetablename = (from m in productionModel.productionLists
                                             select new Vegetable()
                                             {
                                                 VegetableName = m.vegetablenm
                                             }).ToList();
                        context.Vegetables.InsertAllOnSubmit(vegetablename);
                        context.SubmitChanges();
                    }

                    var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                    var pd = (from obj in pm.productionLists
                              select obj).ToList();

                    var productionlist = (from m in productionModel.productionLists

                                          select new ProductionDetail()
                                          {
                                              MainAreaID = m.mainAreaDetails.Id,
                                              SubAreaID = m.subAreaDetails.Id,
                                              VegetableID = (from obj in context.Vegetables
                                                             where obj.VegetableName == m.vegetablenm
                                                             select obj.VegetableID).SingleOrDefault(),
                                              Quantity = m.Quantity,
                                              Remark = m.Remark,
                                              DateTime = DateTime.Now,
                                              LoginID = mac .LoginID 
                                          }).ToList();
                    context.ProductionDetails.InsertAllOnSubmit(productionlist);
                    context.SubmitChanges();

                    return new Result()
                    {
                        Message = string.Format($"Production details Added Successfully."),
                        Status = Result.ResultStatus.success,
                        Data = DateTime.Now,
                    };
                }

                //ProductionDetail productionDetail = new ProductionDetail();
                //var v = (from x in context.Vegetables
                //         where x.VegetableName.ToLower() == productionModel.vegetablenm.ToLower()
                //         select x).FirstOrDefault();
                //if(v == null)
                //{
                //    v = new Vegetable()
                //    {
                //        VegetableName = productionModel.vegetablenm
                //    };
                //    context.Vegetables.InsertOnSubmit(v);
                //    context.SubmitChanges();
                //}
                //int vg = (from obj in context.Vegetables
                //          where obj.VegetableName == productionModel.vegetablenm
                //          select obj.VegetableID).SingleOrDefault();

                //productionDetail.MainAreaID = productionModel.mainAreaDetails.Id;
                //productionDetail.SubAreaID = productionModel.subAreaDetails.Id;
                //productionDetail.VegetableID = vg;
                //productionDetail.Quantity = productionModel.Quantity;
                //context.ProductionDetails.InsertOnSubmit(productionDetail);
                //context.SubmitChanges();
        }
    }
}
