using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FetchingGlookoCode.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams["client_id"] = WebConfigurationManager.AppSettings["GlookoApplicationID"];
            queryParams["redirect_uri"] = Url.Action("Callback", "OAuth", null, Request.Url.Scheme);
            queryParams["response_type"] = "code";
            queryParams["scope"] = "glooko_code";
            UriBuilder glookoAuthorizationURI = new UriBuilder("https", "staging.glooko.com", 443, "/oauth/authorize", BuildQueryString(queryParams));

            ViewBag.GlookoAuthorizationURI = glookoAuthorizationURI.ToString();
            return View();
        }

        private String BuildQueryString(Dictionary<String, String> queryParams)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < queryParams.Count; i++)
            {
                var param = queryParams.ElementAt(i);
                if (i == 0)
                {
                    result.Append("?");
                }
                else
                {
                    result.Append("&");
                }
                result.Append(param.Key);
                result.Append("=");
                result.Append(HttpUtility.UrlEncode(param.Value));

            }
            return result.ToString();
        }
    }
}