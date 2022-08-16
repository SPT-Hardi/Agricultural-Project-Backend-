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
        public Result ViewAllProductionDetails(int? Id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                if (Id == null)
                {

                    var res = (from pd in context.ProductionDetails
                               where pd.IsEditable == true
                               orderby pd.ProductionID descending
                               select new
                               {
                                   ProductionID = pd.ProductionID,
                                   Area = pd.AreaDetail.AreaName,
                                   //Area = new Model.Common.IntegerNullString() {Id=pd.AreaDetail.AreaId,Text=pd.AreaDetail.AreaName },
                                   //Vegetable = new IntegerNullString() { Id=pd.Vegetable.VegetableId,Text=pd.Vegetable.VegetableName},
                                   Vegetable = pd.Vegetable.VegetableName,
                                   Quantity = pd.Quantity,
                                   Remark = pd.Remark,
                                   CreatedBy = pd.LoginDetail.UserName,
                                   LastUpdated = pd.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                   ProductionDate = pd.ProductionDate.ToString("dd-MM-yyyy"),
                                   IsEditable = pd.IsEditable
                                   /*EditedList=(from x in context.ProductionDetails
                                               where x.ParentId==pd.ProductionID
                                               orderby x.ProductionID descending
                                               select new 
                                               {
                                                   ProductionID = x.ProductionID,
                                                   Area = new Model.Common.IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                                   Vegetable = new IntegerNullString() { Id = x.Vegetable.VegetableId, Text = x.Vegetable.VegetableName },
                                                   Quantity = x.Quantity,
                                                   Remark = x.Remark,
                                                   CreatedBy = x.LoginDetail.UserName,
                                                   LastUpdated = x.LastUpdated,
                                                   ProductionDate = x.ProductionDate,
                                                   IsEditable = x.IsEditable,

                                               }).ToList(),
                                               HaveEditedList=(from x in context.ProductionDetails
                                               where x.ParentId == pd.ProductionID select x).ToList().Count()>0 ? true :false,*/
                               }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = res,
                    };
                }
                else 
                {
                    var res = (from pd in context.ProductionDetails
                               where pd.IsEditable == true && pd.ProductionID==Id
                               orderby pd.ProductionID descending
                               select new
                               {
                                   ProductionID = pd.ProductionID,
                                   //Area = pd.AreaDetail.AreaName,
                                   Area = new Model.Common.IntegerNullString() {Id=pd.AreaDetail.AreaId,Text=pd.AreaDetail.AreaName },
                                   Vegetable = new IntegerNullString() { Id=pd.Vegetable.VegetableId,Text=pd.Vegetable.VegetableName},
                                   //Vegetable = pd.Vegetable.VegetableName,
                                   Quantity = pd.Quantity,
                                   Remark = pd.Remark,
                                   CreatedBy = pd.LoginDetail.UserName,
                                   LastUpdated = pd.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                   ProductionDate = pd.ProductionDate.ToString("dd-MM-yyyy"),
                                   IsEditable = pd.IsEditable
                                   /*EditedList=(from x in context.ProductionDetails
                                               where x.ParentId==pd.ProductionID
                                               orderby x.ProductionID descending
                                               select new 
                                               {
                                                   ProductionID = x.ProductionID,
                                                   Area = new Model.Common.IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                                   Vegetable = new IntegerNullString() { Id = x.Vegetable.VegetableId, Text = x.Vegetable.VegetableName },
                                                   Quantity = x.Quantity,
                                                   Remark = x.Remark,
                                                   CreatedBy = x.LoginDetail.UserName,
                                                   LastUpdated = x.LastUpdated,
                                                   ProductionDate = x.ProductionDate,
                                                   IsEditable = x.IsEditable,

                                               }).ToList(),
                                               HaveEditedList=(from x in context.ProductionDetails
                                               where x.ParentId == pd.ProductionID select x).ToList().Count()>0 ? true :false,*/
                               }).FirstOrDefault();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = res,
                    };
                }
            }
        }
        public Result GetEditProductionDetails(int Id) 
        {
            using (ProductInventoryDataContext c = new ProductInventoryDataContext())
            {

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Edited Production details get successfully!",
                    Data = new 
                    {
                        ProductionDetails=ViewAllProductionDetails(Id).Data,
                        EditedList= (from pd in c.ProductionDetails
                                     where pd.ParentId == Id
                                     orderby pd.ProductionID descending
                                     select new
                                     {
                                         ProductionID = pd.ProductionID,
                                         Area = pd.AreaDetail.AreaName,
                                         //Area = new Model.Common.IntegerNullString() {Id=pd.AreaDetail.AreaId,Text=pd.AreaDetail.AreaName },
                                         //Vegetable = new IntegerNullString() { Id=pd.Vegetable.VegetableId,Text=pd.Vegetable.VegetableName},
                                         Vegetable = pd.Vegetable.VegetableName,
                                         Quantity = pd.Quantity,
                                         Remark = pd.Remark,
                                         CreatedBy = pd.LoginDetail.UserName,
                                         LastUpdated = pd.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                                         ProductionDate = pd.ProductionDate.ToString("dd-MM-yyyy"),
                                         IsEditable = pd.IsEditable
                                     }).ToList(),
                    }
                };
            }
        }

        //Add Production Details
        public Result AddProductionDetails(ProductionModel productionModel,object LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (LoginId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
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

                    productionDetail.AreaId = productionModel.Area.Id;
                    productionDetail.VegetableId = productionModel.Vegetable.Id;
                    productionDetail.Quantity = ((decimal)productionModel.Quantity);
                    productionDetail.LastUpdated = ISDT;
                    productionDetail.ProductionDate = productionModel.ProductionDate.ToLocalTime();
                    productionDetail.LoginID = (int)LoginId;
                    productionDetail.Remark = productionModel.Remark;
                    productionDetail.IsEditable = true;

                    context.ProductionDetails.InsertOnSubmit(productionDetail);
                    context.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"{productionModel.Vegetable.Text} Production DSetails Added Successfully."),
                        Status = Result.ResultStatus.success
                    };
                }
            }
        }
        
        //Edit Production
        public Result Editproduction(ProductionModel productionModel, int id,object LoginId)
        {
            var ISDT = new Repository.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope()) 
            {

                using (ProductInventoryDataContext context = new ProductInventoryDataContext())
                {
                    if (LoginId == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    ProductionDetail backup = new ProductionDetail();
                    var pro = context.ProductionDetails.SingleOrDefault(x => x.ProductionID == id);
                    if (pro == null)
                    {
                        throw new ArgumentException("Production Details does Not Exits!.");
                    }
                    if (pro.IsEditable == false) 
                    {
                        throw new ArgumentException("Not editable!");
                    }

                    //backup new entry not editable
                    backup.AreaId = pro.AreaId;
                    backup.ParentId = pro.ProductionID;
                    backup.VegetableId = pro.VegetableId;
                    backup.Quantity = pro.Quantity;
                    backup.LastUpdated = ISDT;
                    backup.ProductionDate = pro.ProductionDate;
                    backup.LoginID = (int)LoginId;
                    backup.Remark = pro.Remark;
                    backup.IsEditable = false;

                    context.ProductionDetails.InsertOnSubmit(backup);
                    context.SubmitChanges();

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
                    pro.ProductionDate = productionModel.ProductionDate.ToLocalTime();
                    pro.AreaId = productionModel.Area.Id;
                    pro.VegetableId = productionModel.Vegetable.Id;
                    pro.Quantity = ((decimal)productionModel.Quantity);
                    pro.LastUpdated= ISDT;
                    pro.LoginID = (int)LoginId;
                    pro.Remark = productionModel.Remark;
                    context.SubmitChanges();
                    

                    
                    var res = new
                    {
                        ProductionID = pro.ProductionID,
                        Area = pro.AreaDetail.AreaName,
                        //Area = new Model.Common.IntegerNullString() {Id=pd.AreaDetail.AreaId,Text=pd.AreaDetail.AreaName },
                        //Vegetable = new IntegerNullString() { Id=pd.Vegetable.VegetableId,Text=pd.Vegetable.VegetableName},
                        Vegetable = pro.Vegetable.VegetableName,
                        Quantity = pro.Quantity,
                        Remark = pro.Remark,
                        CreatedBy = pro.LoginDetail.UserName,
                        LastUpdated = pro.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                        ProductionDate = pro.ProductionDate.ToString("dd-MM-yyyy"),
                        IsEditable = pro.IsEditable
                        /*  ProductionID = pro.ProductionID,
                          Area = new Model.Common.IntegerNullString() { Id=productionModel.Area.Id,Text=productionModel.Area.Text},
                          Vegetable = new IntegerNullString() { Id = productionModel.Vegetable.Id, Text = productionModel.Vegetable.Text },
                          Quantity = pro.Quantity,
                          Remark = pro.Remark,
                          CreatedBy = (from n in context.LoginDetails
                                      where n.LoginID == (int)LoginId
                                      select n.UserName).FirstOrDefault(),
                          LastUpdated = pro.LastUpdated.ToString("dd-MM-yyyy hh:mm tt"),
                          ProductionDate = pro.ProductionDate.ToString("dd-MM-yyyy"),
                          IsEditable = pro.IsEditable,
                          EditedList = (from x in context.ProductionDetails
                                        where x.ParentId == pro.ProductionID
                                        orderby x.ProductionID descending
                                        select new
                                        {
                                            ProductionID = x.ProductionID,
                                            Area = new Model.Common.IntegerNullString() { Id = x.AreaDetail.AreaId, Text = x.AreaDetail.AreaName },
                                            Vegetable = new IntegerNullString() { Id = x.Vegetable.VegetableId, Text = x.Vegetable.VegetableName },
                                            Quantity = x.Quantity,
                                            Remark = x.Remark,
                                            CreatedBy = x.LoginDetail.UserName,
                                            LastUpdated = x.LastUpdated,
                                            ProductionDate = x.ProductionDate,
                                            IsEditable = x.IsEditable,

                                        }).ToList(),
                                        HaveEditedList = (from x in context.ProductionDetails
                                                          where x.ParentId == pro.ProductionID
                                                          select x).ToList().Count() > 0 ? true : false,*/
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format($"{productionModel.Vegetable.Text} Production Details Updated Successfully."),
                        Status = Result.ResultStatus.success,
                        Data=res,
                    };
                }
            }
        }
        
    }
}