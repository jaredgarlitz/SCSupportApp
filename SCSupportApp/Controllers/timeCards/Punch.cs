using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCSupportApp.Controllers;
using System.Net.Http;
using System.IO;

namespace SCSupportApp.Controllers.timeCards
{
    public class Punch
    {
        public string token { get; set; }
        Dictionary<string, object> punchData = new Dictionary<string, object>();
        Dictionary<string, object> PromptData = new Dictionary<string, object>();
        public static async void AddPunchAsync(string date, string time, string type, object promptData = null, string category = null)
        {
            var client = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", ""),
            });

            HttpResponseMessage response = await client.PostAsync(
                "https://payrollservers.us/api/", requestContent);

            HttpContent responseContent = response.Content;

            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                Console.WriteLine(await reader.ReadToEndAsync());
            }
        }
    }
}