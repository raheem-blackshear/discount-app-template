using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DiscountApp.App_GlobalResources;

namespace DiscountApp.ViewModels
{
    public class ContactViewModels
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Mobile { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceName = nameof(GeneralResources.EmailError), ErrorMessageResourceType = typeof(GeneralResources))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        [Display(Name = "Role")]
        public int? ROLE_ID { get; set; }
        [Required(ErrorMessageResourceName = nameof(GeneralResources.UsernameError),
       ErrorMessageResourceType = typeof(GeneralResources))]
        [Display(Name = "Email")]
        [EmailAddress]
        public string EMAIL { get; set; }
        [Required(ErrorMessageResourceName = nameof(GeneralResources.PasswordError),
           ErrorMessageResourceType = typeof(GeneralResources))]
        [DataType(DataType.Password)]
        //[Display(Name = nameof(GeneralResources.EmailError), ResourceType = typeof(GeneralResources))]
        [Display(Name ="Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        //[Display(Name = nameof(GeneralResources.EmailError), ResourceType = typeof(GeneralResources))]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool Rememberme { get; set; } = false;
        
        

        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    public class RegistrationViewModel
    {
        //[Required]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Enter only alphabets and numbers of Full Name")]
        [Display(Name = "Full Name")]
        public string FULLNAME { get; set; }

        [Required(ErrorMessageResourceName = nameof(GeneralResources.UsernameError), ErrorMessageResourceType = typeof(GeneralResources))]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EMAIL { get; set; }

        [Required(ErrorMessageResourceName = nameof(GeneralResources.PasswordError), ErrorMessageResourceType = typeof(GeneralResources))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Display(Name ="Date Of Birth")]
        public DateTime DATE_OF_BIRTH { get; set; }
        //[Required]
        [Display(Name ="Phone Number")]
        public string PHONE_NUMBER { get; set; }
        //[Required]
        public String STP_GENDER_ID { get; set; }
        //[Required]
        //[Display(Name ="Country")]
        //public String COUNTRY_ID { get; set; }
        public string ROLE_ID { get; set; }
        public string FACEBOOK_ID { get; set; }

        [Display(Name = "You must agree to the terms of services")]
        public bool AcceptTerms { get; set; }

      
    }
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = nameof(GeneralResources.EmailError), ErrorMessageResourceType = typeof(GeneralResources))]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EMAIL { get; set; }

        [Required(ErrorMessageResourceName = nameof(GeneralResources.PasswordError), ErrorMessageResourceType = typeof(GeneralResources))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} charachters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = nameof(GeneralResources.EmailError), ErrorMessageResourceType = typeof(GeneralResources))]
        [EmailAddress]
        public string EMAIL { get; set; }

    }
 
}
