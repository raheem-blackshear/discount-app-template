using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Interfaces
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context)
        { }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;

        public void CreateCategory(Category category)
        {
             Add(category);
            _appContext.SaveChanges();
        }

        public Category GetCategoryById(int categoryId)
        {
            return Get(categoryId);
        }

        public void DeleteCategory(Category category)
        {
             Remove(category);
            _appContext.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return GetAll();
        }

        public void UpdateCategory(Category category)
        {
             Update(category);
            _appContext.SaveChanges();
        }
    }
}
