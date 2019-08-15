using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SCSupportApp.Controllers.Accruals
{
    public class AccrualsSchema
    {
        public int siteId;
        
        public static async void GetAccrualsSchema(int SiteId)
        {
            var client = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", ""),
            });

            HttpResponseMessage response = await client.PostAsync(
                "https://payrollservers.us/api/{SiteId}/accruals/schema", requestContent);
            HttpContent responseContent = response.Content;
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                Console.WriteLine(await reader.ReadToEndAsync());
            }
        }
    }
}