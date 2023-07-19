using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {

        IEnumerable<ProductReturnResult> GetAllProducts(int page, int limit, int userId);

        IEnumerable<Product> Search(string keyword);

        IEnumerable<ProductReturnResult> GetFavoriteProducts(int page, int limit, int userId);

        void FollowProduct(ProductUser productUser);

        ProductReturnResult GetProductById(int productId, int userId);

        void CreateProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);
         void RemoveProduct(int productId, int userId);
        IEnumerable<ProductReturnResult> GetNewProducts(int page, int limit, int userId);
        IEnumerable<ProductReturnResult> GetSaleProducts(int page, int limit, int userId);
        IEnumerable<ProductReturnResult> GetTrendingProducts(int page, int limit, int userId);
        Tuple<List<ProductReturnResult>, List<ProductReturnResult>, List<ProductReturnResult>> GetSaleTrendingNewProducts(int page, int limit, int userId,int categoryId);

        Tuple<List<ProductReturnResult>, List<ProductReturnResult>, ProductReturnResult> GetHomePage(DateTime date, int page, int limit,int userId);
    }
}
