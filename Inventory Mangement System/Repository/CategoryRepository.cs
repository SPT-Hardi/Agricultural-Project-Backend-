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
        //public Result AddCategory(CategoryModel categoryModel, int Uid)
        public Result AddCategory(CategoryModel categoryModel)
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
                category.CategoryName = categoryModel.CategoryName;
                category.Description = categoryModel.Description;
                category.LoginID = 1;
                category.DateTime = DateTime.Now;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {categoryModel.CategoryName } Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = categoryModel.CategoryName,
                };
            }
        }

        public async Task<IEnumerable> GetCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                 var res= (from x in context.Categories
                        select new IntegerNullString()
                        {
                            Text = x.CategoryName,
                            Id = x.CategoryID
                        }).ToList();
                return res;
            }
        }

        public Result ViewCategory()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Data = (from x in context.Categories
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
                                  //DateTime = String.Format("{0:dd-MM-yyyy hh:mm tt}", x.DateTime),
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

        public Result EditCategory(CategoryModel categoryModel, int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var ck = context.Categories.SingleOrDefault(x => x.CategoryID == id);
                if (ck == null)
                {
                    throw new ArgumentException("Category doesn't Exist");
                }
                if (ck.CategoryName != categoryModel.CategoryName)
                {
                    var _c = context.Categories.SingleOrDefault(x => x.CategoryName == categoryModel.CategoryName);
                    if (_c != null)
                    {
                        throw new ArgumentException("Category already Exist");
                    }
                }
                ck.CategoryName = categoryModel.CategoryName;
                ck.Description = categoryModel.Description;
                ck.DateTime = DateTime.Now;
                ck.LoginID = 1;
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
