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
    public class AreaRepository : IAreaRepository
    {
        //View All Main And Sub Area
        public Result ViewAllArea(int? Id)
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                if (Id == null)
                {
                    var res = (from x in context.AreaDetails
                                   /* where (Id == null || x.AreaId == Id)*/
                               orderby x.AreaId descending
                               select new
                               {
                                   AreaId = x.AreaId,
                                   AreaName = x.AreaName,
                                   MainAreaName = x.MainAreaName,
                                   SubAreaName = x.SubAreaName,
                                   LastUpdated = x.LastUpdated.ToString("dd-MM-yyy hh:mm tt"),
                                   CreatedBy = x.LoginDetail.UserName,
                               }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Area details get successfully!",
                        Data =res,
                    };
                }
                else 
                {
                    var res = (from x in context.AreaDetails
                               where x.AreaId == Id
                               orderby x.AreaId descending
                               select new
                               {
                                   AreaId = x.AreaId,
                                   AreaName = x.AreaName,
                                   MainAreaName = x.MainAreaName,
                                   SubAreaName = x.SubAreaName,
                                   LastUpdated = x.LastUpdated.ToString("dd-MM-yyy hh:mm tt"),
                                   CreatedBy = x.LoginDetail.UserName,
                               }).FirstOrDefault();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Area details get successfully!",
                        Data = res,
                    };
                }
            }
        }

        //Add New Main And Sub Area
        public Result AddMainAreaAsync(AreaModel areaModel, object LoginId)
        {
            var ISDT = new ISDT().GetISDT(DateTime.Now);
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (LoginId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    // MainArea mainArea = new MainArea();
                    /* var mn1 = (from m in areaModel.arealist
                                join y in context.AreaDetails
                                on m.mname equals y.MainAreaName
                                //from y in context.MainAreas
                                where m.mname == y.MainAreaName
                                select new
                                {
                                    MainAreaName=y.MainAreaName,
                                    SubAreaName=y.SubAreaName,
                                    AreaName = y.AreaName
                                }).ToList();

                     foreach (var item in mn1)
                     {

                         if (mn1.Count() > 0)
                         {
                             throw new ArgumentException($"AreaName : {item.AreaName} already Exist for MainArea : {item.MainAreaName}");
                         }
                     }*/
                    LoginDetail lgin = new LoginDetail();

                   var mainarealist = (from m in areaModel.arealist
                                       select m
                                       ).ToList();
                   
                   foreach (var item in mainarealist)
                   {
                       if (item.subarea.Count() == 0)
                       {
                           AreaDetail areaDetail = new AreaDetail();
                           areaDetail.MainAreaName = item.MainAreaName;
                           areaDetail.SubAreaName = null;
                           areaDetail.AreaName = item.MainAreaName;
                           areaDetail.LastUpdated = ISDT;
                           areaDetail.LoginId = (int)LoginId;
                   
                           context.AreaDetails.InsertOnSubmit(areaDetail);
                           context.SubmitChanges();
                       }
                       else
                       {
                   
                           var SD1 = (from y in item.subarea
                                      select new AreaDetail()
                                      {
                                          MainAreaName = item.MainAreaName,
                                          SubAreaName = y.SubAreaName,
                                          LoginId = (int)LoginId,
                                          AreaName = (from x in context.AreaDetails
                                                      where x.AreaName.ToLower() == (y.SubAreaName == null ? item.MainAreaName.ToLower() : $"{item.MainAreaName.ToLower()}-{y.SubAreaName.ToLower()}")
                                                      //where (x.MainAreaName.ToLower()==item.MainAreaName.ToLower()) && (y.SubAreaName==null ? true : (x.SubAreaName==null ? null : x.SubAreaName.ToLower())==y.SubAreaName.ToLower())
                                                      select x.AreaName).Count() > 0 ? throw new ArgumentException($"AreaName : {(y.SubAreaName == null ? item.MainAreaName.Trim() : $"{item.MainAreaName.Trim()}-{y.SubAreaName.Trim()}")} already exist under MainArea: {item.MainAreaName}!") : (y.SubAreaName == null ? item.MainAreaName.Trim() : $"{item.MainAreaName.Trim()}-{y.SubAreaName.Trim()}"),
                   
                                          LastUpdated = ISDT,
                                      }).ToList();
                           context.AreaDetails.InsertAllOnSubmit(SD1);
                           context.SubmitChanges();
                       }
                   }
                   
                   
                   scope.Complete();
                   return new Result()
                   {
                       Message = string.Format($"Area Added Successfully."),
                       Status = Result.ResultStatus.success,
                       Data = areaModel,
                   };
                }  
            }
        }

        //Edit Main And Sub Area
        public Result EditArea(UpdateAreaModel value,int Id,object LoginId)
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
                    /*
                                        var m = (from x in context.MainAreas where x.MainAreaID == mid select x).FirstOrDefault();

                                        if (m is null)
                                        {
                                            throw new ArgumentException("MainArea Doesn't Exist");
                                        }

                                        var s = (from x in context.SubAreas where x.SubAreaID == sid select x).FirstOrDefault();
                                        if (sid != 0)
                                        {

                                            if (s is null)
                                            {
                                                throw new ArgumentException("SubArea Doesn't Exist");
                                            }
                                        }
                                        //var checkMainArea = (from x in context.MainAreas where x.MainAreaName == value.MainAreaName select x).ToList();


                                        if (m.MainAreaName != value.MainAreaName)
                                        {
                                            var _m = context.MainAreas.SingleOrDefault(x => x.MainAreaName == value.MainAreaName);
                                            if (_m != null)
                                            {
                                                throw new ArgumentException($"MainArea {value.MainAreaName} Already Exist");
                                            }
                                            else
                                            {
                                                m.MainAreaName = value.MainAreaName;
                                                m.DateTime = ISDT;
                                                m.LoginID = LoginId;
                                                if (s != null)
                                                {
                                                    s.SubAreaName = value.SubAreaName;
                                                    s.DateTime = ISDT;
                                                    s.LoginID = LoginId;
                                                }

                                                context.SubmitChanges();
                                            }
                                        }
                                        else
                                        {
                                            var checkSubArea = (from x in context.SubAreas where x.SubAreaName == value.SubAreaName && x.MainAreaID == mid select x).ToList();
                                            if (checkSubArea.Any())
                                            {
                                                throw new ArgumentException($"SubArea {value.SubAreaName} Already Exist");
                                            }
                                            m.MainAreaName = char.ToUpper(value.MainAreaName[0]) + value.MainAreaName.Substring(1).ToLower();
                                            m.DateTime = ISDT;
                                            m.LoginID = LoginId;
                                            if (s != null)
                                            {
                                                s.SubAreaName = char.ToUpper(value.SubAreaName[0]) + value.SubAreaName.Substring(1).ToLower();
                                                s.DateTime = ISDT;
                                                s.LoginID = LoginId;
                                            }

                                            context.SubmitChanges();
                                        }

                                        scope.Complete();
                                        return new Result()
                                        {
                                            Message = string.Format($"Area Updated Successfully."),
                                            Status = Result.ResultStatus.success,
                                            Data= new 
                                            {
                                                MainAreaID = m.MainAreaID,
                                                MainAreaName = m.MainAreaName,
                                                SubAreaID = value.SubAreaName==null ? 0 : s.SubAreaID,
                                                SubAreaName = value.SubAreaName == null ? null : s.SubAreaName,
                                            }
                                        };*/
                    var areadetail = (from x in context.AreaDetails where x.AreaId == Id select x).FirstOrDefault();
                    if (areadetail.AreaName.ToLower() == (value.SubAreaName == null ? value.MainAreaName.ToLower() : $"{value.MainAreaName.ToLower()}-{value.SubAreaName.ToLower()}"))
                    {
                        throw new ArgumentException($"AreaName : {(value.SubAreaName == null ? value.MainAreaName : $"{value.MainAreaName}-{value.SubAreaName}")} already exist under MainArea: {value.MainAreaName}!");
                    }
                    areadetail.MainAreaName = value.MainAreaName;
                    areadetail.SubAreaName = value.SubAreaName;
                    areadetail.AreaName = (value.SubAreaName == null ? value.MainAreaName : $"{value.MainAreaName}-{value.SubAreaName}");
                    areadetail.LastUpdated = ISDT;
                    areadetail.LoginId = (int)LoginId;

                    context.SubmitChanges();

                    var res = new
                    {
                        AreaId = areadetail.AreaId,
                        AreaName = areadetail.AreaName,
                        MainAreaName = areadetail.MainAreaName,
                        SubAreaName = areadetail.SubAreaName,
                        LastUpdated = areadetail.LastUpdated.ToString("dd-MM-yyy hh:mm tt"),
                        CreatedBy = areadetail.LoginDetail.UserName,
                        /*AreaId = areadetail.AreaId,
                        AreaName = areadetail.AreaName,
                        MainAreaName = areadetail.MainAreaName,
                        SubAreaName = areadetail.SubAreaName,
                        LastUpdated = areadetail.LastUpdated.ToString("dd-MM-yyy hh:mm tt"),
                        CreatedBy = (from x in context.LoginDetails where x.LoginID == (int)LoginId select x.UserName).FirstOrDefault(),*/
                    };

                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Area details updated successfully!",
                        Data = res
                    };
                }
            }
        }

        
    }
}
