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
        //View Category
        public Result ViewCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from x in context.Categories
                            orderby x.CategoryID descending
                            select new
                            {
                                CategoryID = x.CategoryID,
                                CategoryName = x.CategoryName,
                                Description = x.Description,
                                UserName = (from n in context.LoginDetails
                                            where n.LoginID == x.LoginID
                                            select n.UserName).FirstOrDefault(),
                                DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", x.DateTime),
                            }).ToList(),
                };
            }
        }

        //Add New Category
        public Result AddCategory(CategoryModel categoryModel, int LoginId)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            var res = context.Categories.FirstOrDefault(x => x.CategoryName == categoryModel.CategoryName);
            if (res != null)
            {
                throw new ArgumentException("Category Already Exist");
            }
            else
            {
                category.CategoryName = char.ToUpper(categoryModel.CategoryName[0]) + categoryModel.CategoryName.Substring(1).ToLower(); 
                category.Description = categoryModel.Description;
                category.LoginID = LoginId;
                category.DateTime = DateTime.Now;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {category.CategoryName } Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = categoryModel.CategoryName,
                };
            }
        }
        
        //Edit Category
        public Result EditCategory(CategoryModel categoryModel, int id, int LoginId)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var ck = context.Categories.SingleOrDefault(x => x.CategoryID == id);
                if (ck == null)
                {
                    throw new ArgumentException("Category Doesn't Exist");
                }
                if (ck.CategoryName != categoryModel.CategoryName)
                {
                    var _c = context.Categories.SingleOrDefault(x => x.CategoryName == categoryModel.CategoryName);
                    if (_c != null)
                    {
                        throw new ArgumentException("Category Already Exist");
                    }
                }
                ck.CategoryName = char.ToUpper(categoryModel.CategoryName[0]) + categoryModel.CategoryName.Substring(1).ToLower();
                ck.Description = categoryModel.Description;
                ck.DateTime = DateTime.Now;
                ck.LoginID = LoginId;
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {ck.CategoryName} Update Successfully"),
                    Status = Result.ResultStatus.success,
                };
            }
        }

        //DropDown For Category
        public async Task<IEnumerable> GetCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var res = (from x in context.Categories
                           select new IntegerNullString()
                           {
                               Text = x.CategoryName,
                               Id = x.CategoryID
                           }).ToList();
                return res;
            }
        }

        //View Category With Paging
        public Result ViewCategorys(Paging value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var source = (from x in context.Categories
                              select new
                              {
                                  CategoryID = x.CategoryID,
                                  CategoryName = x.CategoryName,
                                  Description = x.Description,
                                  UserName = (from n in context.LoginDetails
                                              where n.LoginID == x.LoginID
                                              select n.UserName).FirstOrDefault(),
                                  DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", x.DateTime),
                              }).AsQueryable();

                int count = source.Count();
                int CurrentPage = value.pageNumber;

                int PageSize = value.pageSize;
                int TotalCount = count;
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
                var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                var previousPage = CurrentPage > 1 ? "Yes" : "No";
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                // Setting Header
                //_httpContext.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                return new Result()
                {
                    Data = items,
                    Status = Result.ResultStatus.success
                };
            }

        }

    }
}
