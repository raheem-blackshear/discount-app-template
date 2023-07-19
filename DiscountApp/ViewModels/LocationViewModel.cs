using DAL.Models;
using FluentValidation;
using DiscountApp.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace DiscountApp.ViewModels
{
    public class LocationViewModel
    {
        public string LocationId { get; set; }
       
        public string LocationName { get; set; }

        public bool IsActive { get; set; }

    }

}
