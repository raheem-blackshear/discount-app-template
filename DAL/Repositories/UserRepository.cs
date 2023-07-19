using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        { }

        //List<UserModel> IUserRepository.GetAllUserData(UserParamModel mydata)
        //{
        //    List<UserModel> lst = new List<UserModel>();

        //    UserParamModel ItUserContentModel = new UserParamModel();


        //    var query = (from entUser in _appContext.TBL_User
        //                 join entAgeLang in _appContext.TBL_AgeByLanguage on entUser.Age_Id equals entAgeLang.AgeId
                          
        //                 select new UserModel()
        //                 {
        //                     FullName = entUser.FullName,
        //                     User_Id = entUser.User_Id,
        //                     IsActive = entUser.IsActive,
        //                     CreatedBy = entUser.CreatedBy,
        //                     CreatedDate = entUser.CreatedDate,
        //                     DateModified = entUser.DateModified,
        //                     DateCreated = entUser.DateCreated

        //                 }
        //     ).ToList();

        //    if (query != null && query.Count > 0)
        //        lst = query;

          
        //    return lst;
        //}

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;

       
    }
}
