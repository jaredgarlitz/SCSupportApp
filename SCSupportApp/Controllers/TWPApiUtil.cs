using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SCSupportApp.Controllers
{
    public enum APIProduct
    {
        TWP_Partner = 1,
        TWP_Employee_SSO, 
        TWP_Supervisor_SSO
    }

    public static class TWPApiUtil
    {
        public const string Api_Date_Format = "yyyy-MM-dd";
        public const string Api_Unset_Token = "<unset>";

        public const string Auth_Service_EndPoint = "https://clock.payrollservers.us/AuthenticationService/oauth2/userToken";
        public const string Base_Api_Url = "https://twpapi.payrollservers.us/api";
        public const string Base_Employee_SSO_EndPoint = "https://clock.payrollservers.us/?jwt=";
        public const string Base_Supervisor_SSO_EndPoint = "https://payrollservers.us/pg/Login.aspx?jwt=";

        public const string Employee_Schema_EndPoint = "employees/schema";
        public const string Employees_EndPoint = "employees";

        public const string Payroll_Schema_EndPoint = "payrollactivities/schema";
        public const string Payroll_Activities_EndPoint = "payrollactivities";
        public const string TimeCard_Details_EndPoint = "timecards";

        public const string Accruals_Schema_EndPoint = "accruals/schema";
        public const string Accruals_EndPoint = "accruals";
        public const string Accruals_Activity_EndPoint = "accruals/activity";

        private static readonly string[] API_Product_Token = new string[]
        {
            "", "twppartner", "twpemp", "twplogin"
        };

        public class APIProductTokenClass
        {
            public string this[APIProduct product] => API_Product_Token[(int)product];
        }

        public static APIProductTokenClass aPIProductToken { get; private set; } = new APIProductTokenClass();

        public static readonly DateTime Unix_Epoch_Start = new DateTime(1970, 1, 1);
        public static readonly TimeSpan Unix_Epoch_Offset = new TimeSpan(0, 4, 30);

        public static long GetUnixEpochTimeStamp(DateTime? timestamp = null)
        {
            timestamp = timestamp ?? DateTime.UtcNow;

            return (long)timestamp.Value.Add(Unix_Epoch_Offset).Subtract(Unix_Epoch_Start).TotalSeconds;
        }

        public static string SerializeAPIBody (object bodyObject)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(bodyObject, settings);
        }

        public static string FormatAPIDate(this DateTime dateTime)
        {
            return dateTime.ToString(Api_Date_Format);
        }

        public static string FormatAPIDate (this DateTime? dateTime)
        {
            return dateTime?.ToString(Api_Date_Format) ?? Api_Unset_Token;
        }

        public static int safeCount<T>(this IEnumerable<T> source)
        {
            return source?.Count() ?? 0;
        }

        public static IEnumerable<T> SafeEnumeration<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}