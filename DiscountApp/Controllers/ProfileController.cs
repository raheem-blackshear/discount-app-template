using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DiscountApp.Authorization;
using DiscountApp.Helpers;
using DiscountApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using DAL.Core;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using DiscountApp.ViewModels;
using System.Net.Mail;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscountApp.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
    
        private readonly IAccountManager _accountManager;
        private readonly IAuthorizationService _authorizationService;
        private const string GetUserByIdActionName = "GetUserById";
        private const string GetRoleByIdActionName = "GetRoleById";
        private IUnitOfWork _unitOfWork;
        readonly IEmailer _emailer;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public const string SessionKeyName = "_Login";

        private readonly IHttpContextAccessor _httpContextAccessor;



        public ProfileController(IAccountManager accountManager, IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailer emailer)
        {
            _accountManager = accountManager;
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailer = emailer;
            _signInManager = signInManager;
        }

        [HttpPost("users")]
        public async Task<ResultApi> Register([FromBody] UserEditViewModel user)
        {
            ResultApi ItResultApi = new ResultApi();
            FacebookResult oFacebookResult = new FacebookResult();
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    ItResultApi.Result = "failure";
                    ItResultApi.Data = "Wrong input parameters";
                    return ItResultApi;
                }
                ApplicationUser appUser = Mapper.Map<ApplicationUser>(user);

                string email = user.Email;
                var x = email.Split("@");
                user.UserName = x.First();
                appUser.UserName = x.First();

                var result = await _accountManager.CreateUserAsync(appUser, user.Roles, user.NewPassword);

               
                if (result.Item1)
                {
                    UserModel ItUserModel = new UserModel();
                    ItUserModel.FullName = appUser.FullName;
                    ItUserModel.Email = appUser.Email;

                    ItUserModel.AspNetUsersId = appUser.Id;
                    ItUserModel.IsActive = true;
                    user.ItUserModel = ItUserModel;
                   
                    //if (user.ItUserModel.Location_id != null && user.ItUserModel.Location_id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Location_id = user.ItUserModel.Location_id;
                    //}
                    //if (user.ItUserModel.Age_Id != null && user.ItUserModel.Age_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Age_Id = user.ItUserModel.Age_Id;
                    //}
                    //if (user.ItUserModel.Gender_Id != null && user.ItUserModel.Gender_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Gender_Id = user.ItUserModel.Gender_Id;
                    //}
                   
                    
                    //if (user.ItUserModel.Nationality_Id != null && user.ItUserModel.Nationality_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Nationality_Id = user.ItUserModel.Nationality_Id;
                    //}
                    //if (user.ItUserModel.SchoolType_id != null && user.ItUserModel.SchoolType_id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.SchoolType_id = user.ItUserModel.SchoolType_id;
                    //}
                    //if (user.ItUserModel.IMe != null && user.ItUserModel.IMe.ToString() != string.Empty)
                    //{
                    //    ItUserModel.IMe = user.ItUserModel.IMe;
                    //}
                    //if (user.ItUserModel.School_Id != null && user.ItUserModel.School_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.School_Id = user.ItUserModel.School_Id;
                    //}
                    _unitOfWork.User.Add(ItUserModel);
                    _unitOfWork.SaveChanges();
                    ApplicationUser appUser1 = await _userManager.FindByIdAsync(appUser.Id);
                    string code = await _accountManager.GenerateEmailConfirmationTokenAsync(appUser1);
                    string longurl = string.Empty;
                    string BaseURL = "http://Discountapp.com/api/Profile/users/ConfirmEmail";
                    var ConfirmEmailURL = string.Format("{0}?userId={1}&code={2}", BaseURL, appUser1.Id.ToString(), code.ToString());
                    ConfirmEmailURL = "<a href =\"" + ConfirmEmailURL + "\">here</a>";

                    string Title = null;
                    string emailId = string.Empty;

                    Title = "Verification Email From Discount App";
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("administrator@Discountapp.com");
                    mail.To.Add(appUser1.Email);

                    mail.Subject = Title;
                    mail.Body = "Please Clique in The link " + ConfirmEmailURL;



                    //  mail.Body = messageTraining;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("administrator@Discountapp.com", "dL&A5$5^rryD9Fn!");
                    SmtpServer.EnableSsl = true;

                    //TODO: email verification
                   // SmtpServer.Send(mail);

                    //Task<string> m = Email(ConfirmEmailURL, appUser.Email);


                    UserViewModel userVM = await GetUserViewModelHelper(appUser.Id);
                    ItResultApi.Result = "Success";
                    ItResultApi.Data = appUser1.Id.ToString();
                    ItResultApi.Message = "Waiting The confirm Email";

                }
                else
                {
                    ItResultApi.Result = "Error";
                    ItResultApi.Data = result.Item2[0].ToString();
                    return ItResultApi;
                }



            }
            else
            {
                string errorMessage = string.Join("; ", ModelState.Values
                                                      .SelectMany(x => x.Errors)
                                                      .Select(x => x.ErrorMessage));



                ItResultApi.Result = "Error";
                ItResultApi.Data = errorMessage;
                return ItResultApi;
            }

            return ItResultApi;
        }
        [HttpPost("RegisterSocialMedia")]
        public async Task<ResultApi> RegisterSocialMedia([FromBody] SocialLoginViewModel user)
        {
            ResultApi ItResultApi = new ResultApi();
            FacebookResult oFacebookResult = new FacebookResult();
            if (user == null)
            {
                ItResultApi.Result = "failure";
                ItResultApi.Data = "Wrong input parameters";
                return ItResultApi;
            }
            if (user.FACEBOOK_TOKEN != "")
            { // we do not need email confirmation in that case

                //Check facebook token authenticity 
                oFacebookResult = Social.CheckFacebookToken(user.FACEBOOK_TOKEN);

                if (string.IsNullOrEmpty(oFacebookResult.id))
                {
                    ItResultApi.Result = "Failure";
                    ItResultApi.Data = "Your facebook account was not confirmed, please try again";
                    return ItResultApi;
                }

            }
            if (oFacebookResult.id != string.Empty)
            {


                UserEditViewModel Aspnetuser = new UserEditViewModel();

                Aspnetuser.FullName = oFacebookResult.id.ToString();
                Aspnetuser.Email = oFacebookResult.id.ToString() + "@hotmail.com";
                Aspnetuser.UserName = oFacebookResult.id.ToString() + "@hotmail.com";
                Aspnetuser.JobTitle = "Mr";
                Aspnetuser.PhoneNumber = "961123456";
                Aspnetuser.NewPassword = "#$@SQL$R";
                Aspnetuser.Roles = new string[] { "user" };


                ApplicationUser signedUser = new ApplicationUser();

                signedUser.UserName = Aspnetuser.UserName;

                var ItSignInResult = await _signInManager.CheckPasswordSignInAsync(signedUser, Aspnetuser.NewPassword, false);

                signedUser = await _accountManager.GetUserByUserNameAsync(Aspnetuser.UserName);
                if (signedUser != null && signedUser.Email != string.Empty)
                {
                    ItResultApi.Result = "Success";
                    ItResultApi.Data = signedUser.Id.ToString();
                    ItResultApi.Message = "the registration is successful Already";
                    return ItResultApi;
                }

                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        ItResultApi.Result = "failure";
                        ItResultApi.Data = "Wrong input parameters";
                        return ItResultApi;
                    }
                    ApplicationUser appUser = new ApplicationUser();
                    appUser.Email = user.EMAIL;
                    appUser.FullName = oFacebookResult.name.ToString();
                    appUser.UserName = oFacebookResult.id.ToString() + "@hotmail.com";
                    appUser .Email = oFacebookResult.id.ToString() + "@hotmail.com";
                    appUser.EmailConfirmed = true;
                    appUser.EmailConfirmed = true;
                    var result = await _accountManager.CreateUserAsync(appUser, Aspnetuser.Roles, Aspnetuser.NewPassword);


                    UserModel ItUserModel = new UserModel();
                    ItUserModel.AspNetUsersId = appUser.Id;
                    ItUserModel.FullName = oFacebookResult.name;
                    ItUserModel.FaceBook_Id = oFacebookResult.id;
                    ItUserModel.IsActive = true;
                    //if (user.ItUserModel.Location_id != null && user.ItUserModel.Location_id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Location_id = user.ItUserModel.Location_id;
                    //}
                    //if (user.ItUserModel.Age_Id != null && user.ItUserModel.Age_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Age_Id = user.ItUserModel.Age_Id;
                    //}
                    //if (user.ItUserModel.Gender_Id != null && user.ItUserModel.Gender_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Gender_Id = user.ItUserModel.Gender_Id;
                    //}

                    //if (user.ItUserModel.Nationality_Id != null && user.ItUserModel.Nationality_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.Nationality_Id = user.ItUserModel.Nationality_Id;
                    //}
                    //if (user.ItUserModel.SchoolType_id != null && user.ItUserModel.SchoolType_id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.SchoolType_id = user.ItUserModel.SchoolType_id;
                    //}
                    //if (user.ItUserModel.IMe != null && user.ItUserModel.IMe.ToString() != string.Empty)
                    //{
                    //    ItUserModel.IMe = user.ItUserModel.IMe;
                    //}
                    //if (user.ItUserModel.School_Id != null && user.ItUserModel.School_Id.ToString() != string.Empty)
                    //{
                    //    ItUserModel.School_Id = user.ItUserModel.School_Id;
                    //}
                    _unitOfWork.User.Add(ItUserModel);
                    _unitOfWork.SaveChanges();

                    ItResultApi.Result = "Success";
                    ItResultApi.Data = ItUserModel.FaceBook_Id;
                    ItResultApi.Message = "Success";

                }
                else
                {
                    ItResultApi.Result = "Error";
                    ItResultApi.Data = "Your facebook account was not confirmed, please try again";
                    return ItResultApi;
                }

                return ItResultApi;
            }
            return ItResultApi;
        }
        public async Task<string> Email(string FullName,string senderEmail)
        {
            string recepientName = FullName; //         <===== Put the recepient's name here
            string recepientEmail = senderEmail; //   <===== Put the recepient's email here

            string message = EmailTemplates.GetTestEmail(recepientName, DateTime.UtcNow);

            (bool success, string errorMsg) response = await _emailer.SendEmailAsync(recepientName, recepientEmail, "Test Email from Discount", message);

            if (response.success)
                return "Success";

            return "Error: " + response.errorMsg;
        }
        [HttpPost("usersConfirm")]
        public async Task<ResultApi> CheckConfirmationEmail([FromBody] LoginResult user)
        {
            ApplicationUser appUser = await _accountManager.GetUserByIdAsync(user.USER_ID);
            Boolean ConfirmEmail = await _userManager.IsEmailConfirmedAsync(appUser);
            ResultApi ItResultApi = new ResultApi();
            if (ConfirmEmail != true)
            {
                ItResultApi.Result = "Failure";
                ItResultApi.Data = ConfirmEmail.ToString();
            }
            else
            {
                ItResultApi.Result = "success";
                ItResultApi.Data = ConfirmEmail.ToString();
            }

            return ItResultApi;
        }
        [HttpPost("ChangePassword")]
        public async Task<ResultApi> ChangePassword([FromBody] LoginResult user)
        {
            ResultApi ItResultApi = new ResultApi();
            ApplicationUser appUser = await _accountManager.GetUserByIdAsync(user.USER_ID);
            if (appUser == null)
            {
                ItResultApi.Result = "Failure";
                ItResultApi.Data = "Please Login";
            }
            else
            {
                IdentityResult ItIdentityUser = await _userManager.ChangePasswordAsync(appUser, user.OldPassword.ToString(), user.NewPassword.ToString());
               if( ItIdentityUser.Succeeded == false)
                {
                    ItResultApi.Result = "Error";
                    ItResultApi.Data = "Incorrect password.";
                }
                else {
                ItResultApi.Result = "success";
                ItResultApi.Data = "The password was change";
                }

            }
            return ItResultApi;
        }
        [HttpPost("ResendEmail")]
        public async Task<ResultApi> ResendEmail([FromBody] LoginResult user)
        {
            ApplicationUser appUser = await _accountManager.GetUserByIdAsync(user.USER_ID);

            ResultApi ItResultApi = new ResultApi();
            if (appUser == null)
            {
                ItResultApi.Result = "Failure";
                ItResultApi.Data = "Please Login";
            }
            else
            {
                string code = await _accountManager.GenerateEmailConfirmationTokenAsync(appUser);
                string longurl = string.Empty;
                string BaseURL = "http://Discountapp.com/api/Profile/users/ConfirmEmail";
                var ConfirmEmailURL = string.Format("{0}?userId={1}&code={2}", BaseURL, appUser.Id.ToString(), code.ToString());
                ConfirmEmailURL = "<a href =\"" + ConfirmEmailURL + "\">here</a>";
                Task<string> m = Email(ConfirmEmailURL, appUser.Email);
                ItResultApi.Result = "success";
                ItResultApi.Data = ConfirmEmailURL;
            }


            return ItResultApi;
        }
        //[HttpPost("RegisterTempUser")]
        //public async Task<ResultApi> RegisterTempUser([FromBody] LoginResult Tempuser)
        //{
        //    UserEditViewModel user = new UserEditViewModel();
           
        //        user.FullName = Tempuser.IMe.ToString();
        //        user.Email = Tempuser.IMe.ToString() + "@hotmail.com";
        //        user.UserName = Tempuser.IMe.ToString() + "@hotmail.com";
        //        user.JobTitle = "Mr";
        //        user.PhoneNumber = "961123456";
        //        user.NewPassword = "#$@SQL$R";
        //        user.Roles = new string[] { "user" };
        //    }
        //    ResultApi ItResultApi = new ResultApi();

        //    ApplicationUser signedUser = new ApplicationUser();
         
        //    signedUser.UserName = user.UserName;

        //    var ItSignInResult = await _signInManager.CheckPasswordSignInAsync(signedUser, user.NewPassword, false);

        //     signedUser = await _accountManager.GetUserByUserNameAsync(user.UserName);
        //    if(signedUser !=null && signedUser.Email !=string .Empty)
        //    {
        //        ItResultApi.Result = "Success";
        //        ItResultApi.Data = signedUser.Id.ToString();
        //        ItResultApi.Message = "the registration is successful Already";
        //        return ItResultApi;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        if (user == null)
        //        {
        //            ItResultApi.Result = "failure";
        //            ItResultApi.Data = "Wrong input parameters";
        //            return ItResultApi;
        //        }
        //        ApplicationUser appUser = Mapper.Map<ApplicationUser>(user);
        //        appUser.EmailConfirmed = true;
        //        var result = await _accountManager.CreateUserAsync(appUser, user.Roles, user.NewPassword);

        //        if (result.Item1)
        //        {
        //            UserModel ItUserModel = new UserModel();
        //            ItUserModel.FullName = appUser.FullName;
        //            ItUserModel.AspNetUsersId = appUser.Id;
        //            ItUserModel.IsActive = true;

        //          //  ItUserModel.Location_id = 1;


        //           // ItUserModel.Age_Id = 1;
        //            ItUserModel.FaceBook_Id = "";
        //         //   ItUserModel.Google_Id = "";

        //         //   ItUserModel.Gender_Id = 1;

        //         //   ItUserModel.Nationality_Id = 1;

        //            //ItUserModel.SchoolType_id = 1;
        //            //if (Tempuser.IMe != null && Tempuser.IMe.ToString() != string.Empty)
        //            //{
        //            //    ItUserModel.IMe = Tempuser.IMe;
        //            //}

        //            //ItUserModel.School_Id = 1;

        //            _unitOfWork.User.Add(ItUserModel);
        //            _unitOfWork.SaveChanges();
        //            ItResultApi.Result = "Success";
        //            ItResultApi.Data = appUser.Id.ToString();
        //            ItResultApi.Message = "the registration is successful";

        //        }
        //        else
        //        {
        //            ItResultApi.Result = "Error";
        //            ItResultApi.Data = result.Item2[0].ToString();
        //            return ItResultApi;
        //        }



        //    }
        //    else
        //    {
        //        string errorMessage = string.Join("; ", ModelState.Values
        //                                              .SelectMany(x => x.Errors)
        //                                              .Select(x => x.ErrorMessage));
        //        ItResultApi.Result = "Error";
        //        ItResultApi.Data = errorMessage;
        //        return ItResultApi;
        //    }

        //    return ItResultApi;
        //}


        [HttpPost("UpdateUser"),Authorize]
        public async Task<ResultApi> UpdateUser([FromBody] UpdateUsers ItParamtUserMode)
        {

            var userid = ((System.Security.Claims.ClaimsIdentity)this.User.Identity).FindFirst("sub").Value;
            ResultApi ItResultApi = new ResultApi();
            UserModel ItUserModel = new UserModel();
            var LstUser = _unitOfWork.User.GetAll().ToList();
            if (LstUser.Count > 0)
            {

                ItUserModel = LstUser.Where(p => p.AspNetUsersId == userid).FirstOrDefault();
            }
        
            if (ItUserModel == null)
            {
                ItResultApi.Result = "Failure";
                ItResultApi.Data = "";
                ItResultApi.Message = "User Not Exist";
                return ItResultApi;
            }
            if (ItParamtUserMode.FullName != null && ItParamtUserMode.FullName.ToString() != string.Empty)
            {
                ItUserModel.FullName = ItParamtUserMode.FullName;
                ApplicationUser appUser = await _userManager.FindByIdAsync(ItUserModel.AspNetUsersId.ToString());
                appUser.FullName = ItParamtUserMode.FullName;
                var result = await _userManager.UpdateAsync(appUser);
            }
            ItUserModel.IsActive = true;
            //if (ItParamtUserMode.Location_id != null && ItParamtUserMode.Location_id.ToString() != string.Empty)
            //{
            //    ItUserModel.Location_id = ItParamtUserMode.Location_id;
            //}
            if (ItParamtUserMode.Age != null && ItParamtUserMode.Age.ToString() != string.Empty)
            {
                ItUserModel.Age = ItParamtUserMode.Age;
            }
            if (ItParamtUserMode.ProfileImage != null && ItParamtUserMode.ProfileImage.ToString() != string.Empty)
            {
                ItUserModel.ProfilePicture = ItParamtUserMode.ProfileImage;
            }
            if (ItParamtUserMode.FaceBook_Id != null && ItParamtUserMode.FaceBook_Id.ToString() != string.Empty)
            {
                ItUserModel.FaceBook_Id = ItParamtUserMode.FaceBook_Id;
            }

            if (ItParamtUserMode.Email != null && ItParamtUserMode.Email.ToString() != string.Empty)
            {
                ItUserModel.Email = ItParamtUserMode.Email;
            }
            if (ItParamtUserMode.GenderId != null && ItParamtUserMode.GenderId.ToString() != string.Empty)
            {
                ItUserModel.GenderId = ItParamtUserMode.GenderId;
            }
            //if (ItParamtUserMode.Nationality_Id != null && ItParamtUserMode.Nationality_Id.ToString() != string.Empty)
            //{
            //    ItUserModel.Nationality_Id = ItParamtUserMode.Nationality_Id;
            //}

            //if (ItParamtUserMode.SchoolType_id != null && ItParamtUserMode.SchoolType_id.ToString() != string.Empty)
            //{
            //    ItUserModel.SchoolType_id = ItParamtUserMode.SchoolType_id;
            //}
            //if (ItParamtUserMode.IMe != null && ItParamtUserMode.IMe.ToString() != string.Empty)
            //{
            //    ItUserModel.IMe = ItParamtUserMode.IMe;
            //}
            //if (ItParamtUserMode.School_Id != null && ItParamtUserMode.School_Id.ToString() != string.Empty)
            //{
            //    ItUserModel.School_Id = ItParamtUserMode.School_Id;
            //}
            _unitOfWork.User.Update(ItUserModel);
            _unitOfWork.SaveChanges();

            ItResultApi.Result = "Success";
            ItResultApi.Data = ItParamtUserMode.UserId.ToString();
            ItResultApi.Message = "Save Successeful";
            return ItResultApi;
        }

        [HttpPost("SocialLogin")]
        public async Task<ProfileResult> SocialLogin([FromBody] SocialLoginViewModel model)
        {


            ResultApi etResultApi = new ResultApi();

            ProfileResult etProfileResult = new ProfileResult();


            bool IsValid = false;

            FacebookResult oFacebookResult = new FacebookResult();


            try
            {
                if (model.FACEBOOK_TOKEN != "") // if the user logged in with facebook
                {

                    oFacebookResult = Social.CheckFacebookToken(model.FACEBOOK_TOKEN);
                    

                    if (!string.IsNullOrEmpty(oFacebookResult.id)) // if the user provided valid social id, check for email existance
                    {

                        // UserModel user = _unitOfWork.User.GetAll().Where(p => p.FaceBook_Id == oFacebookResult.id).FirstOrDefault();
                        UserModel user = new UserModel();
                        var LstUser = _unitOfWork.User.GetAll().ToList();
                        if (LstUser.Count > 0)
                        {

                            user = LstUser.Where(p => p.FaceBook_Id != null && p.FaceBook_Id.ToString() == oFacebookResult.id).FirstOrDefault();
                        }

                        if (user == null) // user does not exist
                        {
                            etProfileResult.Result = "Register";

                            etProfileResult.ErrorMessage = "Please register";
                            return etProfileResult;
                        }
                        else // user already exists, meaning he wants to login 
                        {
                            etProfileResult.Data = new LoginResult();
                            etProfileResult.Result = "Success";

                            etProfileResult.Data.FULLNAME = user.FullName;

                            etProfileResult.Data.USER_ID = user.User_Id.ToString();

                            //etProfileResult.Data.Age_Id = user.Age_Id.ToString();
                            etProfileResult.Data.AspNetUsersId = user.AspNetUsersId.ToString();

                            //etProfileResult.Data.Gender_Id = user.Gender_Id.ToString();
                            ////  etProfileResult.Data.USERNAME = profile[0].USERNAME;
                            //etProfileResult.Data.Location_id = user.Location_id.ToString();
                            //etProfileResult.Data.Nationality_Id = user.Nationality_Id.ToString();
                            //etProfileResult.Data.School_Id = user.School_Id.ToString();
                            //if (user.IMe != null) {
                            //etProfileResult.Data.IMe = user.IMe.ToString();
                            //}
                            return etProfileResult;
                        }
                    }
                    else if (model.GOOGLE_TOKEN != "")
                    {
                        IsValid = Social.CheckGoogleToken(model.GOOGLE_TOKEN, model.EMAIL);


                        if (IsValid) // if the user provided valid social id, check for email existance
                        {
                            ApplicationUser Appuser = await _accountManager.GetUserByEmailAsync(model.EMAIL);


                            if (Appuser == null) // user does not exist
                            {
                                etProfileResult.Result = "Register";

                                etProfileResult.ErrorMessage = "Please register";

                                return etProfileResult;
                            }
                            //else // user already exists, meaning he wants to login 
                            //{

                            //    var user = _unitOfWork.User.GetAll().Where(p => p.Google_Id.ToString() == model.GOOGLE_TOKEN.ToString()).FirstOrDefault();

                            //    if (user != null) // the user was saved in aspnetusers but not in TBL_USER
                            //    {

                            //        etProfileResult.Result = "Success";

                            //        etProfileResult.Data.FULLNAME = user.FullName;

                            //        etProfileResult.Data.USER_ID = user.User_Id.ToString();

                            //        etProfileResult.Data.Age_Id = user.Age_Id.ToString();

                            //        etProfileResult.Data.Gender_Id = user.Gender_Id.ToString();
                            //        //  etProfileResult.Data.USERNAME = profile[0].USERNAME;
                            //        etProfileResult.Data.Location_id = user.Location_id.ToString();
                            //        etProfileResult.Data.Nationality_Id = user.Nationality_Id.ToString();
                            //        etProfileResult.Data.School_Id = user.School_Id.ToString();
                            //        etProfileResult.Data.IMe = user.IMe.ToString();
                            //    }

                            //}

                        }

                        return etProfileResult;
                    }


                }
                return etProfileResult;
            }

            catch (Exception ex)
            {
                etProfileResult.Result = "Error";
                etProfileResult.ErrorMessage = ex.ToString();

                return etProfileResult;
            }


        }


        //[HttpPost("LoginUser")]
        //public async Task<ResultApi> LoginUser([FromBody] LoginResult user)
        //{
        //    ResultApi ItResultApi = new ResultApi();
        //    ApplicationUser signedUser = new ApplicationUser();
        //    UserModel ItUserModel = new UserModel();

        //   // signedUser.UserName = user.Username;

        //    var ItSignInResult = await _signInManager.CheckPasswordSignInAsync(signedUser, user.NewPassword, false);



        //  //  signedUser = await _accountManager.GetUserByUserNameAsync(user.Username);
        //    if (signedUser != null)
        //    {

        //        //TODO: After Confirm Email
        //        //if (signedUser.EmailConfirmed == false)
        //        //{
        //        //    ItResultApi.Result = "ConfirmEmail";
        //        //    ItResultApi.Data = "";
        //        //    ItResultApi.Message = "Please confirm the email";
        //        //    return ItResultApi;
        //        //}

        //            var result = await _signInManager.PasswordSignInAsync(user.Username, user.NewPassword, false, false);
        //    if (result.Succeeded == true)
        //    {
                

        //        // HttpContext.Session.SetString("Login", signedUser.Id);
        //        LoginSession.HttpContext.Session.SetString("Login", signedUser.Id);

        //        if (LoginSession.HttpContext.Session.GetString("Login") != null)
        //        {
        //            String LogInd = LoginSession.HttpContext.Session.GetString("Login");
        //            // String LogInd1 = LoginSession.GetSession("Login");
        //            // do something here
        //            // string connection = Configuration.GetConnectionString("MyDb");
        //        }
        //            var LstUser = _unitOfWork.User.GetAll().ToList();
        //           if(LstUser.Count > 0)
        //            {
                    
        //            ItUserModel = LstUser.Where(p => p.AspNetUsersId !=null && p.AspNetUsersId.ToString() == signedUser.Id.ToString()).FirstOrDefault();
        //            }
        //            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
        //        {
        //            HttpContext.Session.SetString(SessionKeyName, signedUser.Id);
        //        }
        //        ItResultApi.Result = "Success";
        //        ItResultApi.LstData = ItUserModel;
        //        ItResultApi.Message = "Login Success";

        //      }
            
        //    else
        //    {
        //        ItResultApi.Result = "Failure";
        //        ItResultApi.Data = "";
        //        ItResultApi.Message = "User Name And The Password Does Not match";

        //    }
        //    }
        //    else {
        //        ItResultApi.Result = "Failure";
        //        ItResultApi.Data = "";
        //        ItResultApi.Message = "User Name And The Password Does Not match";

        //    }
        //    return ItResultApi;
        //}
        

        [HttpGet("GetUser"),Authorize]
        public LoginResult FindListUser()
        {
            var userid = ((System.Security.Claims.ClaimsIdentity)this.User.Identity).FindFirst("sub").Value;
            LoginResult Loginuser = new LoginResult();
            ResultApi ItResultApi = new ResultApi();
            // UserModel ItUserModel = _unitOfWork.User.GetAll().Where(p => p.AspNetUsersId.ToString() == user.USER_ID.ToString()).FirstOrDefault();
            UserModel ItUserModel = new UserModel();
            var LstUser = _unitOfWork.User.GetAll().ToList();
            if (LstUser.Count > 0)
            {

                ItUserModel = LstUser.Where(p => p.AspNetUsersId.ToString() == userid).FirstOrDefault();
            }
            Loginuser.USER_ID = ItUserModel.User_Id.ToString ();
            Loginuser.FULLNAME = ItUserModel.FullName.ToString();
          

            Loginuser.Age = ItUserModel.Age;
            Loginuser.GenderId = ItUserModel.GenderId;
            Loginuser.ProfilePicture =  ItUserModel.ProfilePicture;
            Loginuser.Email = ItUserModel.Email;
            
            //Loginuser.Location_id = ItUserModel.Location_id.ToString();
            //Loginuser.Nationality_Id = ItUserModel.Nationality_Id.ToString();
            //Loginuser.SchoolType_id = ItUserModel.SchoolType_id.ToString();
            //Loginuser.School_Id = ItUserModel.School_Id.ToString();
            //Loginuser.Gender_Id = ItUserModel.Gender_Id.ToString();
            //if (Loginuser.Age_Id != null && Loginuser.Age_Id.ToString() != "0") {

            //    Loginuser.Age_Name = _unitOfWork.Age.GetAll().Where(p => p.Age_Id.ToString() == ItUserModel.Age_Id.ToString()).FirstOrDefault().Age_Name;
            //}
            //if (Loginuser.Location_id != null && Loginuser.Location_id.ToString() != "0")
            //{
            //    Loginuser.Location_Name = _unitOfWork.Location.GetAll().Where(p => p.Location_Id.ToString() == ItUserModel.Location_id.ToString()).FirstOrDefault().Location_Name;
            //}
            //if (Loginuser.Nationality_Id != null && Loginuser.Nationality_Id.ToString() != "0")
            //{
            //    Loginuser.Nationality_Name = _unitOfWork.Nationality.GetAll().Where(p => p.Nationality_Id.ToString() == ItUserModel.Nationality_Id.ToString()).FirstOrDefault().Nationality_Name;
            //}
            //if (Loginuser.SchoolType_id != null && Loginuser.SchoolType_id.ToString() != "0")
            //{
            //    Loginuser.SchoolType_Name = _unitOfWork.SchoolType.GetAll().Where(p => p.SchoolType_Id.ToString() == ItUserModel.SchoolType_id.ToString()).FirstOrDefault().SchoolType_Name;
            //}
            //if (Loginuser.School_Id != null && Loginuser.School_Id.ToString() != "0")
            //{
            //    Loginuser.School_Name = _unitOfWork.School.GetAll().Where(p => p.School_Id.ToString() == ItUserModel.School_Id.ToString()).FirstOrDefault().School_Name;

            //}

            return Loginuser;
        }
        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
        private async Task<UserViewModel> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _accountManager.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = Mapper.Map<UserViewModel>(userAndRoles.Item1);
            userVM.Roles = userAndRoles.Item2;

            return userVM;
        }


        [HttpGet("users/{id}", Name = "ConfirmEmail")]
       public async Task<ActionResult> ConfirmEmail(string Userid, string code)
        {
            if (Userid == null || code == null)
            {
                return View("Error");
            }
            Userid = Userid.Split("-")[0].TrimStart() + "-"+ Userid.Split("-")[1].TrimStart() + "-" + Userid.Split("-")[2].TrimStart() + "-" + Userid.Split("-")[3].TrimStart() + "-" + Userid.Split("-")[4].TrimStart();
            ApplicationUser appUser = await _userManager.FindByIdAsync(Userid);
            appUser.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(appUser);

            return View("Index");
        }
     
        public IActionResult Index()
        {
            return View();
        }
    }
}
