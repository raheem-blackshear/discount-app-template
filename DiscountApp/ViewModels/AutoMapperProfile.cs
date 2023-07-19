using AutoMapper;
using DAL.Core;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountApp.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                   .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserPatchViewModel>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap();
            CreateMap<RoleViewModel, ApplicationRole>();

            CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionViewModel>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
                .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));


            //CreateMap<LocationModel, LocationViewModel>()
            //  .ForMember(d => d.LocationId, map => map.MapFrom(s => s.Location_Id))
            //  .ForMember(d => d.LocationName, map => map.MapFrom(s => s.Location_Name))
            //  .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive));

            //CreateMap<LocationViewModel, LocationModel>()
            //    .ForMember(d => d.Location_Id, map => map.MapFrom(s => s.LocationId))
            //    .ForMember(d => d.Location_Name, map => map.MapFrom(s => s.LocationName))
            //    .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive))
            // .ReverseMap();

            //CreateMap<LocationModel, LocationViewModel>()
            //  .ForMember(d => d.LocationId, map => map.MapFrom(s => s.Location_Id))
            //  .ForMember(d => d.LocationName, map => map.MapFrom(s => s.Location_Name))
            //  .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive));

            //CreateMap<LocationViewModel, LocationModel>()
            //    .ForMember(d => d.Location_Id, map => map.MapFrom(s => s.LocationId))
            //    .ForMember(d => d.Location_Name, map => map.MapFrom(s => s.LocationName))
            //    .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive))
            // .ReverseMap();

            //CreateMap<NationalityModel, NationalityViewModel>()
            // .ForMember(d => d.NationalityId, map => map.MapFrom(s => s.Nationality_Id))
            // .ForMember(d => d.NationalityName, map => map.MapFrom(s => s.Nationality_Name))
            // .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive));

            //CreateMap<NationalityViewModel, NationalityModel>()
            //    .ForMember(d => d.Nationality_Id, map => map.MapFrom(s => s.NationalityId))
            //    .ForMember(d => d.Nationality_Name, map => map.MapFrom(s => s.NationalityName))
            //    .ForMember(d => d.IsActive, map => map.MapFrom(s => s.IsActive))
            // .ReverseMap();



        }
    }
}
