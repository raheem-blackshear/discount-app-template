using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Interfaces
{
   public interface IStoreRepository:IRepository<Store>
    {

        IEnumerable<Store> GetAllStores(int page, int limit, int userId);

        IEnumerable<Store> GetFeaturedStores(int page, int limit,int userId);

        IEnumerable<Store> GetRecommendedStores(int page, int limit, int userId);

        IEnumerable<Store> GetFavoriteStores(int page, int limit, int userId);

        void FollowStore(StoreUser storeUser);

        IEnumerable<Store> Search(string keyword);

        Store GetStoreById(int storeId, int userId);

        void CreateStore(Store store);
        void RemoveProduct(int storeId, int userId);
        void UpdateStore(Store store);

        void DeleteStore(Store store);

        Tuple<List<Store>, List<Store>, List<Store>> GetHomePage(int page, int limit, int userId);

        List<Store> GetStoresByCategory(int page, int limit,int categoryId, int userId);
    }
}
