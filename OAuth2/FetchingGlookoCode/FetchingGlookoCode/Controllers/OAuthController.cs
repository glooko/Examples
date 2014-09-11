using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FetchingGlookoCode.Controllers
{
    public class OAuthController : Controller
    {
        public ActionResult Callback([Bind(Prefix = "code")]String authorization_code = null, String error = null, [Bind(Prefix = "error_description")]String errorDescription = null)
        {
            // check if authorization code is received. If not, an error has occurred.
            if (String.IsNullOrWhiteSpace(authorization_code))
            {
                ViewBag.Error = error;
                ViewBag.ErrorDescription = errorDescription;
                return View("CallbackError");
            }

            // exchange authorization code for authentication token, refresh token and glooko code
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
                {
                    {"client_id", WebConfigurationManager.AppSettings["GlookoApplicationID"] },
                    {"client_secret", WebConfigurationManager.AppSettings["GlookoApplicationSecret"]},
                    {"redirect_uri", Url.Action("Callback", "OAuth", null, Request.Url.Scheme)},
                    {"grant_type", "authorization_code"},
                    {"code", authorization_code}
                };
                try
                {
                    var response = webClient.UploadValues("https://staging.glooko.com/oauth/token", postData);
                    ViewBag.Response = PrettifyIfJSON(System.Text.Encoding.UTF8.GetString(response));
                }
                catch (WebException e)
                {
                    ViewBag.Error = e.ToString();
                    using (StreamReader reader = new StreamReader(e.Response.GetResponseStream()))
                    {
                        ViewBag.Response = PrettifyIfJSON(reader.ReadToEnd());
                    }
                }
            }
            return View();
        }

        private String PrettifyIfJSON(String toPrettify)
        {
            try
            {
                return Newtonsoft.Json.Linq.JObject.Parse(toPrettify).ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                return toPrettify;
            }
        }
    }
}