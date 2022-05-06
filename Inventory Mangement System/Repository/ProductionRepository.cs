using Inventory_Mangement_System.Middleware;
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
    public class ProductionRepository : IProductionRepository
    {
        //View All Production Details 
        public Result ViewAllProductionDetails()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from pd in context.ProductionDetails
                            orderby pd.ProductionID descending
                            select new
                            {
                                ProductionID = pd.ProductionID,
                                MainAreaDetails = new Model.Common.IntegerNullString() { Id = pd.MainArea.MainAreaID, Text = pd.MainArea.MainAreaName },
                                SubAreaDetails = new Model.Common.IntegerNullString() { Id = pd.SubAreaID==null ? 0 : pd.SubArea.SubAreaID, Text = pd.SubAreaID==null ? null : pd.SubArea.SubAreaName },
                                Vegetablenm = pd.Vegetable.VegetableName,
                                Quantity = pd.Quantity,
                                Remark = pd.Remark,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == pd.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", pd.LastUpdateDate),
                                ProductionDate= String.Format("{0:dd-MM-yyyy hh:mm tt}", pd.Date),
                            }).ToList()
                };
            }
        }

        //Add Production Details
        public Result AddProductionDetails(ProductionModel productionModel,int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                ProductionDetail productionDetail = new ProductionDetail();
               /* var v = (from x in context.Vegetables
                         where x.VegetableName.ToLower() == productionModel.Vegetablenm.ToLower()
                         select x).FirstOrDefault();
                if(v == null)
                {
                    v = new Vegetable()
                    {
                        VegetableName = char.ToUpper(productionModel.Vegetablenm[0]) + productionModel.Vegetablenm.Substring(1).ToLower()
                    };
                    context.Vegetables.InsertOnSubmit(v);
                    context.SubmitChanges();
                }
                int vg = (from obj in context.Vegetables
                          where obj.VegetableName == productionModel.Vegetablenm
                          select obj.VegetableID).SingleOrDefault();*/
                
                productionDetail.MainAreaID = productionModel.MainAreaDetails.Id;
                productionDetail.SubAreaID = productionModel.SubAreaDetails.Id==0? null: productionModel.SubAreaDetails.Id; 
                productionDetail.VegetableId = productionModel.Vegetable.Id;
                productionDetail.Quantity = productionModel.Quantity;
                productionDetail.LastUpdateDate =ISDT;
                productionDetail.Date = productionModel.ProductionDate;
                productionDetail.LoginID = LoginId;
                productionDetail.Remark = productionModel.Remark;
                context.ProductionDetails.InsertOnSubmit(productionDetail);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{productionModel.Vegetable.Text} Production DSetails Added Successfully."),
                    Status = Result.ResultStatus.success,
                };
            }
        }
        
        //Edit Production
        public Result Editproduction(ProductionModel productionModel, int id,int LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope()) 
            {

                using (ProductInventoryDataContext context = new ProductInventoryDataContext())
                {
                    ProductionDetail productionDetail = new ProductionDetail();
                    var pro = context.ProductionDetails.SingleOrDefault(x => x.ProductionID == id);
                    if (pro == null)
                    {
                        throw new ArgumentException("Production Details Does Not Exits!.");
                    }

                    /* var v = (from x in context.Vegetables
                              where x.VegetableName.ToLower() == productionModel.Vegetablenm.ToLower()
                              select x).FirstOrDefault();
                     if (v == null)
                     {
                         v = new Vegetable()
                         {
                             VegetableName = char.ToUpper(productionModel.Vegetablenm[0]) + productionModel.Vegetablenm.Substring(1).ToLower()
                         };
                         context.Vegetables.InsertOnSubmit(v);
                         context.SubmitChanges();
                     }

                     int vg = (from obj in context.Vegetables
                               where obj.VegetableName == productionModel.Vegetablenm
                               select obj.VegetableID).SingleOrDefault();
     */
                    pro.MainAreaID = productionModel.MainAreaDetails.Id;
                    if (pro.SubAreaID != null) 
                    {
                       pro.SubAreaID = productionModel.SubAreaDetails.Id;
                    }
                    pro.VegetableId = productionModel.Vegetable.Id;
                    pro.Quantity = productionModel.Quantity;
                    pro.LastUpdateDate = ISDT;
                    pro.Date = productionModel.ProductionDate;
                    pro.LoginID = LoginId;
                    pro.Remark = productionModel.Remark;
                    context.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"{productionModel.Vegetable.Text} Production Details Updated Successfully."),
                        Status = Result.ResultStatus.success,
                    };
                }
            }
        }
        
    }
}