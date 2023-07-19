using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(DbContext context) : base(context)
        { }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;


        public IEnumerable<Store> GetAllStores(int page, int limit,  int userId)
        {

         


            IEnumerable<Store> stores = _appContext.Stores.Include("Location").Include("Lookbook").Skip(page * limit).Take(limit).ToList();
            foreach (Store element in stores)
            {
                element.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == element.Id) > 0);
              
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();  
               element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();

            }
            return stores;
        }

        public IEnumerable<Store> GetFeaturedStores(int page, int limit, int userId) {
            

            IEnumerable<Store> stores = _appContext.Stores.Where(x => x.Featured == true).Include("Lookbook").Include("Location").Skip(page * limit).Take(limit).ToList();
            foreach (Store element in stores)
            {
                element.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == element.Id) > 0);
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();
                element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();
            }
            return stores;
        }

        public IEnumerable<Store> GetRecommendedStores(int page, int limit, int userId) {

            IEnumerable<Store> stores = _appContext.Stores.Where(x => x.Recommended == true).Include("Lookbook").Include("Location").Skip(page * limit).Take(limit).ToList();
            foreach (Store element in stores)
            {
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();
                element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();
                element.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == element.Id) > 0);
            }
            return stores;
         }

        public IEnumerable<Store> GetFavoriteStores(int page, int limit, int userId)
        {
            IEnumerable <Store> stores = _appContext.Stores.Where(x => x.StoreUsers.Any(c => c.UserId == userId)).Include("Lookbook").Include("Location").Skip(page * limit).Take(limit);
            foreach (Store element in stores)
            {
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();
                element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();
                element.isFollowed = true;
            }

            return stores;
          
        }

        public void FollowStore(StoreUser storeUser)
        {

            _appContext.StoreUsers.Add(storeUser);
            _appContext.SaveChanges();
        }

        public IEnumerable<Store> Search(string keyword)
        {
            var query = _appContext.Stores.Include("Products").Include("Location").Include("Lookbook").AsQueryable();

            var filterBy = keyword.Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(filterBy))
            {
                query = query
                       .Where(m => m.Name.ToLowerInvariant().Contains(filterBy)).Take(15);
            }

            return query.ToList();
        }
        public Store GetStoreById(int storeId, int userId)
        {
            Store store = _appContext.Stores.Include("Location").Include("Lookbook").FirstOrDefault(p => p.Id == storeId);
            store.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == storeId) > 0);
            var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == store.Id).ToList();
            store.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();


            return store;
        }

        public void RemoveProduct(int storeId, int userId)
        {
            StoreUser pu = _appContext.StoreUsers.Where(x => x.UserId == userId && x.StoreId == storeId).FirstOrDefault();
            _appContext.StoreUsers.Remove(pu);
            _appContext.SaveChanges();
        }

        public void CreateStore(Store store)
        {

            Add(store);
            _appContext.SaveChanges();
        }
        public void UpdateStore(Store store)
        {

            Update(store);
            _appContext.SaveChanges();
        }

        public void DeleteStore(Store store)
        {

            Remove(store);
            _appContext.SaveChanges();
        }

        
        public Tuple<List<Store>, List<Store>, List<Store>> GetHomePage( int page, int limit, int userId)
        {
           var allStores = _appContext.Stores;
            foreach (Store element in allStores)
            {
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();
                element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();
                element.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == element.Id) > 0);
            }


            List<Store> stores = allStores.Include("Location").Include("Lookbook").Include("Category").Skip(page * limit).Take(limit).ToList();
          
            List<Store> recommendedStores = allStores.Include("Location").Include("Lookbook").Include("Category").Skip(page * limit).Take(limit).Where(x => x.Recommended == true).ToList();
          
            List<Store> featuredStores = allStores.Skip(page * limit).Take(limit).Where(x => x.Featured == true).Include("Lookbook").Include("Location").Include("Category").ToList();
           

            Tuple<List<Store>, List<Store>, List<Store>> d = new Tuple<List<Store>, List<Store>, List<Store>>(stores, recommendedStores, featuredStores);
            return d;
        }

        public List<Store> GetStoresByCategory(int page, int limit, int categoryId, int userId)
        {
            List<Store> stores = _appContext.Stores.Where(x => x.CategoryId == categoryId).Include("Lookbook").Include("Location").Include("Category").Skip(page * limit).Take(limit).ToList();
            foreach (Store element in stores)
            {
                var storeStyles = _appContext.StoreStyles.Where(o => o.StoreId == element.Id).ToList();
                element.Styles = _appContext.Styles.Where(p => p.StoreStyles.Intersect(storeStyles).Any()).ToList();
                element.isFollowed = (bool)(_appContext.StoreUsers.Count(o => o.UserId == userId && o.StoreId == element.Id) > 0);
            }
            return stores;
        }
    }
}
