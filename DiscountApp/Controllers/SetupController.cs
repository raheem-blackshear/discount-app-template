using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DiscountApp.ViewModels;
using AutoMapper;
using DAL.Models;
using DAL.Core.Interfaces;
using DiscountApp.Authorization;
using DiscountApp.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using DAL.Core;
namespace DiscountApp.Controllers
{
    [Route("api/[controller]")]
    public class SetupController : Controller
    {
        private readonly ISetup _setupManager;
        private readonly IAuthorizationService _authorizationService;


        public SetupController(ISetup setupManager, IAuthorizationService authorizationService)
        {
            _setupManager = setupManager;
            _authorizationService = authorizationService;
        }


       
      

          }
}
