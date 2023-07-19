using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Interfaces
{
   public interface ICategoryRepository : IRepository<Category>
    {

        IEnumerable<Category> GetAllCategories();

        Category GetCategoryById(int categoryId);

        void CreateCategory(Category category);

        void UpdateCategory(Category category);

        void DeleteCategory(Category category);

    }
}