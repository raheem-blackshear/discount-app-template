using DAL;
using DAL.Core.Interfaces;
using DAL.Models;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Core;

namespace DAL.Core
{
    public class Setup : ISetup
    {
        private readonly ApplicationDbContext _context;   


        public Setup(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
            
        }

        //public List<TypeConsumes> GetTypeConsumesLoadRelatedAsync(int page, int pageSize)
        //{
        //    List<TypeConsumes> typeConsumes = new List<TypeConsumes>();

           

        //    return typeConsumes;
        //}


    }
}
