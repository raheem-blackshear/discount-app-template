using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DiscountApp.ViewModels
{
    public class UserEditViewModel : UserViewModel
    {
        public string CurrentPassword { get; set; }

     
        public string NewPassword { get; set; }
        new private bool IsLockedOut { get; } //Hide base member

        public UserModel ItUserModel { get; set; }
      
    }
}
