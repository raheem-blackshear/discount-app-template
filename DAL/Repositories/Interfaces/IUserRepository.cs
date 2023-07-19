using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserModel>
    {
        // List<UserModel> GetAllUserData(UserParamModel mydata);
        
    }
}
