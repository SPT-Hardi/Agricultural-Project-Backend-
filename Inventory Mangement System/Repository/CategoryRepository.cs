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
        public Result AddCategory(CategoryModel categoryModel, int Uid)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            var res = context.Categories.FirstOrDefault(x => x.CategoryName == categoryModel.CategoryName);
            if(res != null )
            {
                throw new ArgumentException( "Category Already Exist");
            }
            else
            {
                category.CategoryName = categoryModel.CategoryName;
                category.Description = categoryModel.Descritption;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {categoryModel.CategoryName } Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = categoryModel.CategoryName,
                }; 
                //return $"Category {categoryModel.CategoryName } Added Successfully";
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
    }
}
