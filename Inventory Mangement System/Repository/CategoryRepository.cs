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
    public class CategoryRepository : ICategoryRepository
    {
        public Result AddCategory(CategoryModel categoryModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            var res = context.Categories.FirstOrDefault(x => x.CategoryName == categoryModel.CategoryName);
            if(res != null )
            {
                throw new ArgumentException("Category Already Exist");
            }
            else
            {
                category.CategoryName = (categoryModel.CategoryName[0] + categoryModel.CategoryName.Substring(1));
                category.Description = categoryModel.Description;
                category.LoginID = 1;
                category.DateTime = DateTime.Now;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {categoryModel.CategoryName} Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = categoryModel.CategoryName,
                };
            }
        }

        public async Task<IEnumerable> GetCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Categories
                        select new IntegerNullString()
                        {
                            Text = x.CategoryName,
                            Id = x.CategoryID
                        }).ToList();
            } 
        }

        public async Task<IEnumerable> ViewCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Categories
                        select new 
                        {
                            CategoryID = x.CategoryID ,
                            CategoryName = x.CategoryName,//string.Format(char.ToUpper(x.CategoryName[0]) + x.CategoryName.Substring(1)),
                            Description = x.Description,
                            DateTime = x.DateTime ,
                            UserName = (from y in context.LoginDetails
                                        where y.LoginID == x.LoginID 
                                        select y.UserName).SingleOrDefault(),
                        }).ToList();
            }
        }

        public async Task<IEnumerable> ViewCategoryById(int cid)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.Categories
                        where x.CategoryID == cid
                        select new
                        {
                            CategoryName = x.CategoryName,
                            Description = x.Description
                        }).ToList();
            }
        }   

        public Result EditCategory(CategoryModel categoryModel,int id)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var ck = context.Categories.SingleOrDefault(x => x.CategoryID == id);
                if(ck == null)
                {
                    throw new ArgumentException("Category doesn't Exist");
                }
                if (ck.CategoryName != categoryModel.CategoryName)
                {
                    var _c = context.Categories.SingleOrDefault(x => x.CategoryName == categoryModel.CategoryName);
                    if(_c != null)
                    {
                        throw new ArgumentException("Category already Exist");

                    }
                }
                
                ck.CategoryName = categoryModel.CategoryName;
                ck.Description = categoryModel.Description;
                ck.LoginID = 1;
                ck.DateTime = DateTime.Now;
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {categoryModel.CategoryName} Update Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = categoryModel.CategoryName,
                };
            }
        }
    }
}
