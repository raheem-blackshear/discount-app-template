using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

      
        IProductRepository _products;
        ICategoryRepository _productsCategory;
        IStoreRepository _store;
        IUserRepository _User;
        IUploadRepository _image;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public IUserRepository User
        {
            get
            {
                if (_User == null)
                    _User = new UserRepository(_context);

                return _User;
            }
        }
        public IProductRepository Products
        {
            get
            {
                if (_products == null)
                    _products = new ProductRepository(_context);

                return _products;
            }
        }
        public ICategoryRepository ProductCategory
        {
            get
            {
                if (_productsCategory == null)
                    _productsCategory = new CategoryRepository(_context);

                return _productsCategory;
            }
        }
        public IStoreRepository Store
        {
            get
            {
                if (_store == null)
                    _store = new StoreRepository(_context);

                return _store;
            }
        }
        public IUploadRepository Image
        {
            get
            {
                if (_image == null)
                    _image = new UploadRepository(_context);

                return _image;
            }
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
