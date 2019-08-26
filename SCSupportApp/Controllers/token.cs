using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SCSupportApp.Controllers
{
    public class token
    {
        public static string creator(int site, string secret, bool isAccountant = false, string accountant = "")
        {
            if (isAccountant)
            {
                Dictionary<string, object> siteInfo = new Dictionary<string, object>();
                siteInfo.Add("type", "id");
                if (!siteInfo.ContainsValue(site))
                    siteInfo.Add("id", site);
                var clientToken = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .AddClaim("iss", accountant)
                    .AddClaim("sub", "partner")
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeSeconds())
                    .AddClaim("product", "twppartner")
                    .AddClaim("siteInfo", siteInfo)
                    .Build();
                return clientToken.ToString();
            }
            else
            {
                Dictionary<string, object> siteInfo = new Dictionary<string, object>();
                siteInfo.Add("type", "id");
                if (!siteInfo.ContainsValue(site))
                    siteInfo.Add("id", site);
                var clientToken = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .AddClaim("iss", site)
                    .AddClaim("sub", "client")
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeSeconds())
                    .AddClaim("product", "twpclient")
                    .AddClaim("siteInfo", siteInfo)
                    .Build();
                return clientToken.ToString();
            }
        }

        public static async Task<string> PostAuth (string token, string apiSecret)
        {
            HttpWebResponse response = await SendWebRequest(HttpMethod.Post, TWPApiUtil.Auth_Service_EndPoint, token, null);

            if(response.StatusCode == HttpStatusCode.Created)
            {
                JObject result = null;

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string responseString = await sr.ReadToEndAsync();
                    result = JObject.Parse(responseString);

                    string authToken = result["token"].ToString();

                    return authToken;
                }
            }

            throw new InvalidOperationException("$Received an error while requesting a JWT token: " + $"{response.StatusCode} - {response.StatusDescription}");
        }

        public static async Task<string> CallTWPAPI(int siteID, string apiToken, string API_Endpoint, HttpMethod method = null, object content = null)
        {
            string apiUrl = null;

            method = method ?? HttpMethod.Get;
            if(API_Endpoint.StartsWith("http"))
            {
                apiUrl = API_Endpoint;
            }
            else
            {
                apiUrl = $"{TWPApiUtil.Base_Api_Url}/{siteID}/{API_Endpoint}";
            }

            HttpWebResponse wr = await SendWebRequest(method, apiUrl, apiToken, content);

            if(wr.StatusCode == HttpStatusCode.OK || wr.StatusCode == HttpStatusCode.Created)
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();

                    return result;
                }
            }
            else
            {
                throw new InvalidOperationException($"Received an error while calling Client API '{apiUrl}': {wr.StatusCode} - {wr.StatusDescription}");
            }
        }

        public static async Task<HttpWebResponse> SendWebRequest(HttpMethod method, string url, string authToken, object content = null)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = method.ToString();
            request.ContentType = "application/json";
            request.Headers.Set("Authorization", string.Format("Bearer {0}", authToken));

            if(content != null)
            {
                string stringContent = TWPApiUtil.SerializeAPIBody(content);
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(stringContent);
                    streamWriter.Flush();
                }
            }
            return (HttpWebResponse)(await request.GetResponseAsync());
        }

        //public static async Task<List<TWP_Employee>> ListEmployees(int siteID, string apiToken)
        //{

        //}
    }
}