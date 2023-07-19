using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
//using Torch.Model.ViewModel.Profile;

namespace DiscountApp.Helpers
{
  public static class Social
    {
        public static FacebookResult CheckFacebookToken(string accesstoken)
        {       

            FacebookResult FBresult = new FacebookResult();


            string fbresponse = string.Empty;

            string longurl = string.Empty;

            try
            {


            // https://graph.facebook.com/me?fields=id&access_token=@accesstoken
           
            string BaseURL = "https://graph.facebook.com/me";

            var uriBuilder = new UriBuilder(BaseURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["fields"] = "id,name,email,picture.type(large)";
            query["access_token"] = accesstoken;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();



            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(longurl);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
               
                fbresponse = reader.ReadToEnd();
            }
          
          

            
            if (fbresponse.Contains("id"))
            {
             FBresult = JsonConvert.DeserializeObject<FacebookResult>(fbresponse);
                
         
               
            }
            }
            catch (Exception ex)
            {
                return FBresult;
            }
            return FBresult;
        }



        public static bool CheckGoogleToken(string tokenid,string email="")
        {
            bool isAuthentic = false;


            string googleresponse = string.Empty;

            string longurl = string.Empty;

            try
            {
                string BaseURL = " https://www.googleapis.com/oauth2/v2/tokeninfo";

                var uriBuilder = new UriBuilder(BaseURL);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                //query["fields"] = "id";
                query["id_token"] = tokenid;
                uriBuilder.Query = query.ToString();
                longurl = uriBuilder.ToString();





                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(longurl);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    // should parse as json object
                    googleresponse = reader.ReadToEnd();
                }



                // need to check googleresponse content
                if (googleresponse.Contains("error_description"))
                {


                }
                else
                {
                    if (!string.IsNullOrEmpty(email))
                    {

                        //GoogleResult oGoogleResult = new GoogleResult();


                        //oGoogleResult = JsonConvert.DeserializeObject<GoogleResult>(email);
                        //// check to see if the email send by the user is the same as the one returned by google
                        //if (oGoogleResult.verified_email && oGoogleResult.email == email)
                        //{
                          
                        //}


                    }
                    isAuthentic = true;
                }

          
            }

            catch(Exception ex)
            {
                isAuthentic = false;
                return isAuthentic;

            }

            return isAuthentic;




        }
    }
}
