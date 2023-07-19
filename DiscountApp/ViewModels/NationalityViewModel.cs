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
    public class NationalityViewModel
    {
        public string NationalityId { get; set; }
       
        public string NationalityName { get; set; }

        public bool IsActive { get; set; }

    }

}
