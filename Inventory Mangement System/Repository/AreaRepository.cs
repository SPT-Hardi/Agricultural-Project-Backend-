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
    public class AreaRepository:IAreaRepository
    {
        //New Main Area Add 
        public Result AddMainAreaAsync(AreaModel areaModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MainArea mainArea = new MainArea();
                var mn1 = (from m in areaModel.arealist
                           from y in context.MainAreas
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
                };

                var mainarea = (from m in areaModel.arealist
                                select new MainArea()
                                {
                                    MainAreaName = m.mname
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
                                   SubAreaName = y.sname
                               }).ToList();
                    context.SubAreas.InsertAllOnSubmit(SD1);
                    context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"Area Added Successfully."),
                    Status = Result.ResultStatus.success,
                };

                //foreach (var item in mn1)
                //{
                //    var SA = (from m in context.MainAreas
                //              join s in context.SubAreas
                //              on m.MainAreaID equals s.MainAreaID
                //              where m.MainAreaID == item.MainAreaID
                //              select new
                //              {
                //                  MainAreaID = m.MainAreaID,
                //                  SubAreaName = s.SubAreaName
                //              }).ToList();

                //    var sd = (from m in areaModel.arealist
                //              from y in m.subarea
                //              where m.mname == item.MainAreaname
                //              select new
                //              {
                //                  MainAreaID = item.MainAreaID,
                //                  SubAreaName = y.sname
                //              }).ToList().Except(SA);

                //    var _sd = (from m in sd
                //               from y in areaModel.arealist
                //               where y.mname == item.MainAreaname
                //               select new SubArea()
                //               {
                //                   MainAreaID = item.MainAreaID,
                //                   SubAreaName = m.SubAreaName
                //               }).ToList();

                //    if (_sd.Count() == 0)
                //    {
                //        throw new ArgumentException($"MainAreaName {item.MainAreaname} already have SubAreaN{_sd}");
                //    }
                //    context.SubAreas.InsertAllOnSubmit(_sd);
                //    context.SubmitChanges();
                //}
                //return new Result()
                //{
                //    Message = string.Format($"SubArea Added Successfully."),
                //    Status = Result.ResultStatus.success,
                //}; 
            }
        }
    }
}
