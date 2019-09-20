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
        public const string Alpha_Auth_Service_EndPoint = "https://a.payrollservers.us/AuthenticationService/oauth2/userToken";

        public const string Base_Api_Url = "https://twpapi.payrollservers.us/api";
        public const string Alpha_Base_API_Url = "https://alpha_twpapi.wfmclock.net/api";

        public const string Base_Employee_SSO_EndPoint = "https://clock.payrollservers.us/?jwt=";
        public const string Base_Supervisor_SSO_EndPoint = "https://payrollservers.us/pg/Login.aspx?jwt=";

        public const string Accruals_Schema_EndPoint = "accruals/schema";
        public const string Accruals_EndPoint = "accruals";
        public const string Accruals_Activity_EndPoint = "accruals/activity";

        public const string Employee_Schema_EndPoint = "employees/schema";
        public const string Employees_EndPoint = "employees";
        public const string Employees_Groups_EndPoint = "employees/groups";
        public const string Employee_Connect_Man_Login_EndPoint = "employee/connect_manager";
        public const string Employee_Disconnect_Man_Login_EndPoint = "employee/connect_manager";
        public const string Employee_Reset_PW_EndPoint = "employees/reset_password";
        public const string Employee_Set_PW_EndPoint = "employees/set_password";
        public const string Employee_Update_PW_EndPoint = "employees/update_password";

        public const string Groups_EndPoint = "groups";

        public const string Logins = "logins";

        public const string Payroll_Schema_EndPoint = "payrollactivities/schema";
        public const string Payroll_Activities_EndPoint = "payrollactivities";

        public const string Rules = "rules";
        public const string Int_Sched_Rule_EndPoint = "rules/integratedscheduling";
        public const string Ext_EE_ID_EndPoint = "rules/extemployeeid";
        public const string Integration_Fields_EndPoint = "rules/integrationfields";
        public const string Integration_Rule_EndPoint = "rules/integrationfields";

        public const string Schedules_EndPoint = "schedules";

        public const string TimeCard_Details_EndPoint = "timecards";
        public const string getTimeCardSum = "timecards/summary";
        public const string approveTimeCard = "timecards/approve";
        public const string editTimeCardLine = "timecards/line";
        public const string addTimeCardNote = "timecards/note";
        public const string addPunch = "timecards/punch";
        
        public const string TORCats = "timeoffrequests/categories";
        public const string TOR_EndPoint = "timeoffrequests";
        public const string TOR_By_Dept_EndPoint = "timeoffrequests/department";
        public const string TOR_Schema_EndPoint = "timeoffrequests/schema";
        public const string TOR_Sup_EndPoint = "timeoffrequests/supervisor";

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