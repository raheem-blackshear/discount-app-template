using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Interfaces;
namespace DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        { }
       
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;


        public IEnumerable<ProductReturnResult> GetAllProducts(int page,int limit,int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          select new ProductReturnResult
                          {
                              product = product,             
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                               isFollowed = (bool)(_appContext.ProductUsers.Count(o => o.UserId == userId && o.ProductId == product.Id) > 0 )  


                          }).Skip(page * limit).Take(limit).ToList();



            
            return result;


          //  return _appContext.Products.Skip(page * limit).Take(limit).ToList();
        }
        public IEnumerable<ProductReturnResult> GetFavoriteProducts(int page, int limit, int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          where(product.ProductUsers.Any(c => c.UserId == userId))
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = true

                          }).Skip(page * limit).Take(limit).ToList();

            return result;
         
            
        }

        public IEnumerable<Product> Search(string keyword)
        {
            var query = _appContext.Products.AsQueryable();

            var filterBy = keyword.Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(filterBy))
            {
                query = query
                       .Where(m => m.Name.ToLowerInvariant().Contains(filterBy)
                       || m.Description.ToLowerInvariant().Contains(filterBy)).Take(15);
            }

            return query.ToList();
        }
        
        
        public ProductReturnResult GetProductById(int productId, int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          where (product.Id == productId)
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = (bool)(_appContext.ProductUsers.Count(o => o.UserId == userId && o.ProductId == product.Id) > 0)



                          }).FirstOrDefault();

        


            return result;
        }

        public void CreateProduct(Product product)
        {

            Add(product);
            _appContext.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {

            Update(product);
            _appContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
           
            Remove(product);
            _appContext.SaveChanges();
        }

        public void FollowProduct(ProductUser productUser)
        {

            _appContext.ProductUsers.Add(productUser);
            _appContext.SaveChanges();
        }
        public void RemoveProduct(int productId , int userId)
        {
            ProductUser pu = _appContext.ProductUsers.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefault();
            _appContext.ProductUsers.Remove(pu);
            _appContext.SaveChanges();
        }
        public IEnumerable<ProductReturnResult> GetNewProducts(int page, int limit, int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = (bool)(_appContext.ProductUsers.Count(o => o.UserId == userId && o.ProductId == product.Id) > 0)
                          }).OrderByDescending(p =>p.product.CreatedDate).Skip(page * limit).Take(limit).ToList();

            return result;
           
        }
        public IEnumerable<ProductReturnResult> GetSaleProducts(int page, int limit, int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          where (product.IsOnSale == true)
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = (bool)(_appContext.ProductUsers.Count(o => o.UserId == userId && o.ProductId == product.Id) > 0)
                          }).Skip(page * limit).Take(limit).ToList();
            
            return result;
         
        }
        public IEnumerable<ProductReturnResult> GetTrendingProducts(int page, int limit, int userId)
        {
            var result = (from product in _appContext.Products
                          join store in _appContext.Stores on product.StoreId equals store.Id into Stores
                          from m in Stores.DefaultIfEmpty()
                          join category in _appContext.ProductCategories on product.CategoryId equals category.Id into Category
                          from c in Category.DefaultIfEmpty()
                          where (product.IsTrending == true)
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = (bool)(_appContext.ProductUsers.Count(o => o.UserId == userId && o.ProductId == product.Id) > 0)
                          }).Skip(page * limit).Take(limit).ToList();

            return result;
        }
        public Tuple<List<ProductReturnResult>, List<ProductReturnResult>, List<ProductReturnResult>> GetSaleTrendingNewProducts(int page, int limit,int userId, int categoryId)
        {
           
            List<Product> list = _appContext.Products.ToList();
            List<Store> stores = _appContext.Stores.ToList();
            List<ProductUser> productUsers = _appContext.ProductUsers.ToList();
            List<Category> categories = _appContext.ProductCategories.ToList();

            List<ProductReturnResult> a = (from product in list
                                           join store in stores on product.StoreId equals store.Id into Stores
                                           from m in Stores.DefaultIfEmpty()
                                           join category in categories on product.CategoryId equals category.Id into Category
                                           from cat in Category.DefaultIfEmpty()
                                           where (product.IsOnSale == true && product.CategoryId == categoryId)
                                           select new ProductReturnResult
                                           {
                                               product = product,
                                               storeName = m.Name,
                                               storeId = m.Id,
                                               location = m.Location,
                                               categoryId = cat.Id,
                                               categoryName = cat.Name,
                                               isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))


                                           }).OrderByDescending(p => p.product.CreatedDate).Skip(page * limit).Take(limit).ToList();
            List<ProductReturnResult> b = (from product in list
                                           join store in stores on product.StoreId equals store.Id into Stores
                                           from m in Stores.DefaultIfEmpty()
                                           join category in categories on product.CategoryId equals category.Id into Category
                                           from cat in Category.DefaultIfEmpty()
                                           where (product.IsOnSale == true && product.CategoryId == categoryId)
                                           select new ProductReturnResult
                                           {
                                               product = product,
                                               storeName = m.Name,
                                               storeId = m.Id,
                                               location = m.Location,
                                               categoryId = cat.Id,
                                               categoryName = cat.Name,
                                               isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))


                                           }).Skip(page * limit).Take(limit).ToList();
            List<ProductReturnResult> c = (from product in list
                                           join store in stores on product.StoreId equals store.Id into Stores
                                           from m in Stores.DefaultIfEmpty()
                                           join category in categories on product.CategoryId equals category.Id into Category
                                           from cat in Category.DefaultIfEmpty()
                                           where (product.IsTrending == true && product.CategoryId == categoryId)
                                           select new ProductReturnResult
                                           {
                                               product = product,
                                               storeName = m.Name,
                                               storeId = m.Id,
                                               location = m.Location,
                                               categoryId = cat.Id,
                                               categoryName = cat.Name,
                                                isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))
                                           
            
        }).Skip(page * limit).Take(limit).ToList(); ;

            Tuple<List<ProductReturnResult>, List<ProductReturnResult>, List<ProductReturnResult>> d = new Tuple<List<ProductReturnResult>, List<ProductReturnResult>, List<ProductReturnResult>>(a, b, c);
            return d;
        }
        public Tuple<List<ProductReturnResult>, List<ProductReturnResult>, ProductReturnResult> GetHomePage(DateTime date, int page, int limit,int userId)
        {
            List<Product> list = _appContext.Products.ToList();
            List<Store> stores = _appContext.Stores.ToList();
            List<ProductUser> productUsers = _appContext.ProductUsers.ToList();
            List<Category> categories = _appContext.ProductCategories.ToList();
            date = DateTime.Now.Date;

            var a = (from product in list
                     join store in stores on product.StoreId equals store.Id into Stores
                     from m in Stores.DefaultIfEmpty()
                     join category in categories on product.CategoryId equals category.Id into Category
                     from c in Category.DefaultIfEmpty()
                     where (product.IsOnSale == true)
                          select new ProductReturnResult
                          {
                              product = product,
                              storeName = m.Name,
                              storeId = m.Id,
                              location = m.Location,
                              categoryId = c.Id,
                              categoryName = c.Name,
                              isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))


                          }).Skip(page * limit).Take(limit).ToList();

           
            var b = (from product in list
                     join store in stores on product.StoreId equals store.Id into Stores
                     from m in Stores.DefaultIfEmpty()
                     join category in categories on product.CategoryId equals category.Id into Category
                     from c in Category.DefaultIfEmpty()
                     where (product.IsTrending == true)
                     select new ProductReturnResult
                     {
                         product = product,
                         storeName = m.Name,
                         storeId = m.Id,
                         location = m.Location,
                         categoryId = c.Id,
                         categoryName = c.Name,
                         isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))


                     }).Skip(page * limit).Take(limit).ToList();
            var e = (from product in list
                     join store in stores on product.StoreId equals store.Id into Stores
                     from m in Stores.DefaultIfEmpty()
                     join category in categories on product.CategoryId equals category.Id into Category
                     from c in Category.DefaultIfEmpty()
                     where (product.DealDate == date)
                     select new ProductReturnResult
                     {
                         product = product,
                         storeName = m.Name,
                         storeId = m.Id,
                         location = m.Location,
                         categoryId = c.Id,
                         categoryName = c.Name,
                         isFollowed = (bool)(productUsers.Any(p => p.UserId == userId && p.ProductId == product.Id))


                     }).FirstOrDefault();
            Tuple<List<ProductReturnResult>, List<ProductReturnResult>, ProductReturnResult> d = new Tuple<List<ProductReturnResult>, List<ProductReturnResult>, ProductReturnResult>(a, b, e);
            return d;
        }
    }
}
