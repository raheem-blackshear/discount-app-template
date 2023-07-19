using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
      
        IProductRepository Products { get; }
       
        IUserRepository User { get; }

        ICategoryRepository ProductCategory { get; }

        IStoreRepository Store { get; }

        IUploadRepository Image { get; }

        int SaveChanges();
    }
}
