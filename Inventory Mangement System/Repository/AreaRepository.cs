﻿using Inventory_Mangement_System.Model;
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
        public Result AddMainAreaAsync(MainAreaModel mainAreaModel)
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                MainArea mainArea = new MainArea();
                //SubArea subArea = new SubArea();
                var MA = context.MainAreas.SingleOrDefault(x => x.MainAreaName == mainAreaModel.mname);
                if(MA != null)
                {
                    var SA = (from m in context.MainAreas
                              join s in context.SubAreas
                              on m.MainAreaID equals s.MainAreaID
                              where m.MainAreaID == MA.MainAreaID
                              select new
                              {
                                  MainAreaID = m.MainAreaID,
                                  SubAreaName = s.SubAreaName
                              }).ToList();

                    var sd = (from m in mainAreaModel.subarea
                                 select new 
                                 {
                                   MainAreaID = MA.MainAreaID,
                                   SubAreaName = m.sname
                                 }).ToList().Except (SA);
                    
                    var _sd = (from m in sd
                               select new SubArea()
                               {
                                   MainAreaID = MA.MainAreaID,
                                   SubAreaName = m.SubAreaName
                               }).ToList(); 
                    if (_sd.Count() == 0)
                    {
                        throw new ArgumentException("Already Exist");
                    }
                    context.SubAreas.InsertAllOnSubmit(_sd);
                     context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"{mainAreaModel.mname} Already Added succesfully"),
                        Status = Result.ResultStatus.success,
                        Data = mainAreaModel.mname,
                    };
                }
                else
                {
                    mainArea.MainAreaName = mainAreaModel.mname;
                    context.MainAreas.InsertOnSubmit(mainArea);
                    context.SubmitChanges();

                    var Maid = context.MainAreas.SingleOrDefault(x => x.MainAreaName == mainAreaModel.mname);
                    var sd = mainAreaModel.subarea.Select(x => new SubArea()
                    {
                        MainAreaID = Maid.MainAreaID,
                        SubAreaName = x.sname
                    }).ToList();
                    context.SubAreas.InsertAllOnSubmit(sd);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"{mainAreaModel.mname} Already Added succesfully"),
                        Status = Result.ResultStatus.success,
                        Data = mainAreaModel.mname,
                    };
                }
            }
        }
    }
}
