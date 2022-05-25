using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Inventory_Mangement_System.Repository
{
    public class Vegetables
    {
        public Result AddList(Model.Vegetable value,object loginid) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (ProductInventoryDataContext c = new ProductInventoryDataContext())
                {
                    if (loginid == null) 
                    {
                        throw new ArgumentException("Yor are not authorized!");
                    }
                    ProductInventoryContext.Vegetable v = new Vegetable();
                    
                    var checkexist = (from x in c.Vegetables where x.VegetableName.ToLower() == value.VegetableName.ToLower() select x).FirstOrDefault();
                    if (checkexist != null) 
                    {
                        throw new ArgumentException($"Vegetable name: {value.VegetableName} already exist!");
                    }

                    v.VegetableName = value.VegetableName;
                    
                    c.Vegetables.InsertOnSubmit(v);
                    c.SubmitChanges();

                    scope.Complete();
                    
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Vegetables added successfully!",
                        Data =new 
                        {
                            VegetableId=v.VegetableId,
                            VegetableName=value.VegetableName,

                        },
                    };
                }
                }
            }
       
            public Result Drop() 
            {
                using (ProductInventoryDataContext c = new ProductInventoryDataContext())
                {
                
                    var vegetablelist = (from x in c.Vegetables select new IntegerNullString() { Id = x.VegetableId, Text = x.VegetableName }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Vegetables drop geted successfully!",
                        Data = vegetablelist,
                    };
                }
            
            }
        public Result Update(int Id,Model.Vegetable value,object loginid) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (ProductInventoryDataContext c = new ProductInventoryDataContext())
                {
                    if (loginid == null) 
                    {
                        throw new ArgumentException("You are not authorized!");
                    }
                    var vegetable = (from x in c.Vegetables where x.VegetableId == Id select x).FirstOrDefault();
                    if (vegetable== null) 
                    {
                        throw new ArgumentException("Vegetable details not exist for current Id!");
                    }
                    var vegetableexist = (from x in c.Vegetables where x.VegetableName.ToLower() == value.VegetableName.ToLower() select x).FirstOrDefault();
                    if (vegetableexist!=null)
                    {
                        throw new ArgumentException($"Vegetable name: {value.VegetableName} already exist!");
                    }
                    vegetable.VegetableName = value.VegetableName;
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Vegetable updated successfully!",
                        Data = new IntegerNullString() 
                        {
                            Id=vegetable.VegetableId,
                            Text=vegetable.VegetableName,
                        },
                    };
                }
            }

        }
        public Result View() 
        {
            using (ProductInventoryDataContext c = new ProductInventoryDataContext())
            {

                var vegetablelist = (from x in c.Vegetables select new { x.VegetableId,x.VegetableName }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Vegetables viewed successfully!",
                    Data = vegetablelist,
                };
            }
        }
        public Result Delete(int Id,object loginid) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (ProductInventoryDataContext c = new ProductInventoryDataContext())
                {
                    if (loginid == null) 
                    {
                        throw new ArgumentException("You are not authorized!");
                    }
                    var vegetable = (from x in c.Vegetables where x.VegetableId == Id select x).FirstOrDefault();
                    if (vegetable == null) 
                    {
                        throw new ArgumentException("Vegetable details not exist for current Id!");
                    }
                    c.Vegetables.DeleteOnSubmit(vegetable);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Vegetables details deleted successfully!",
                        Data = new IntegerNullString { Id=vegetable.VegetableId,Text=vegetable.VegetableName},
                    };
                }
            }
        }
    }
}
