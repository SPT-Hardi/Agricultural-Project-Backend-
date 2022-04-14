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
    public class AreaRepository : IAreaRepository
    {
        //View All Main And Sub Area
        public Result ViewAllArea()
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Data=(from s in context.SubAreas
                          orderby s.SubAreaID descending
                          select new
                          {
                              MainAreaID=s.MainArea.MainAreaID,
                              MainAreaName=s.MainArea.MainAreaName,
                              SubAreaID=s.SubAreaID,
                              SubAreaName=s.SubAreaName,
                              UserName=(from u in context.LoginDetails
                                        where u.LoginID==s.LoginID
                                        select u.UserName).FirstOrDefault(),
                              DateTime=string.Format("{0:dd-MM-yyyy hh:mm tt}",s.DateTime),
                          }).ToList(),
                };
            }
        }

        //Add New Main And Sub Area
        public Result AddMainAreaAsync(AreaModel areaModel, int LoginId)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MainArea mainArea = new MainArea();
                var mn1 = (from m in areaModel.arealist
                           join y in context.MainAreas
                           on m.mname equals y.MainAreaName
                           //from y in context.MainAreas
                           where m.mname == y.MainAreaName
                           select new
                           {
                               MainAreaID = y.MainAreaID,
                               MainAreaname = y.MainAreaName
                           }).ToList();

                foreach (var item in mn1)
                {
                    if (mn1.Count() > 0)
                    {
                        throw new ArgumentException($"MainAreaName {item.MainAreaname} already Exist");
                    }
                }
                LoginDetail lgin = new LoginDetail();
            

                var mainarea = (from m in areaModel.arealist
                                select new MainArea()
                                {
                                    MainAreaName = m.mname,
                                    LoginID = LoginId,
                                    DateTime = DateTime.Now
                                }).ToList();
                context.MainAreas.InsertAllOnSubmit(mainarea);
                context.SubmitChanges();

                var mainarea2 = (from m in mainarea
                                 select new
                                 {
                                     MainAreaID = m.MainAreaID,
                                     MainAreaname = m.MainAreaName
                                 }).ToList();

                foreach (var item in mainarea2)
                {
                    var SD1 = (from m in areaModel.arealist
                               from y in m.subarea
                               where m.mname == item.MainAreaname
                               select new SubArea()
                               {
                                   MainAreaID = item.MainAreaID,
                                   SubAreaName = y.sname,
                                   LoginID = LoginId,
                                   DateTime = DateTime.Now
                               }).ToList();
                    context.SubAreas.InsertAllOnSubmit(SD1);
                    context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"Area Added Successfully."),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        //Edit Main And Sub Area
        public Result EditArea(UpdateAreaModel value, int mid, int sid,int LoginId)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var m = (from x in context.MainAreas where x.MainAreaID == mid select x).FirstOrDefault();

                if (m is null)
                {
                    throw new ArgumentException("MainArea Doesn't Exist");
                }

                var s = (from x in context.SubAreas where x.SubAreaID == sid select x).FirstOrDefault();

                if (s is null)
                {
                    throw new ArgumentException("SubArea Doesn't Exist");
                }
                var checkMainArea = (from x in context.MainAreas where x.MainAreaName == value.MainAreaName select x).ToList();

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
                        m.DateTime = DateTime.Now;
                        m.LoginID = LoginId;
                        s.SubAreaName =value.SubAreaName; 
                        s.DateTime = DateTime.Now;
                        s.LoginID = LoginId;
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
                    m.DateTime = DateTime.Now;
                    m.LoginID = LoginId;
                    s.SubAreaName = char.ToUpper(value.SubAreaName[0]) + value.SubAreaName.Substring(1).ToLower();
                    s.DateTime = DateTime.Now;
                    s.LoginID = LoginId;
                    context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"Area Updated Successfully."),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        
    }
}
