using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
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
    public static class TWPSDK
    {
        public const int API_Page_Size = 50;

        public static async Task<string> GetJWTToken(string APISecret, int partnerId, int siteId, APIProduct product = APIProduct.TWP_Partner)
        {
            return await GetJWTToken(APISecret, new JWT_Payload(partnerId, siteId, product));
        }

        public static async Task<string> GetJWTToken(string APISecret, JWT_Payload payload)
        {
            JwtEncoder jwt = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
            string requestToken = jwt.Encode(payload, APISecret);

            HttpWebResponse response = await SendWebRequest(HttpMethod.Post, TWPApiUtil.Auth_Service_EndPoint, requestToken, null);

            if (response.StatusCode == HttpStatusCode.Created)
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

            throw new InvalidOperationException($"Received an error while requesting a JWT token: {response.StatusCode} - {response.StatusDescription}");
        }

        public static async Task<string> CallTWPAPI(int siteId, string APIToken, string APIEndpoint, HttpMethod method = null, object content = null)
        {
            string API_Url = null;
            method = method ?? HttpMethod.Get;

            if (APIEndpoint.StartsWith("http"))
            {
                API_Url = APIEndpoint;
            }
            else
            {
                API_Url = $"{TWPApiUtil.Base_Api_Url}/{siteId}/{APIEndpoint}";
            }

            HttpWebResponse wr = await SendWebRequest(method, API_Url, APIToken, content);

            if (wr.StatusCode == HttpStatusCode.OK || wr.StatusCode == HttpStatusCode.Created)
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();

                    return result;
                }
            }
            else
            {
                throw new InvalidOperationException($"Received an error while calling Client API '{API_Url}': {wr.StatusCode} - {wr.StatusDescription}");
            }
        }

        public static async Task<HttpWebResponse> SendWebRequest(HttpMethod method, string url, string authToken, object content = null)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = method.ToString();
            request.ContentType = "application/json";
            request.Headers.Set("Authorization", String.Format("Bearer {0}", authToken));

            if (content != null)
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

        #region Employee

        public static async Task<List<TWP_Employee_Schema>> listEESchema(int siteId, string APIToken)
        {
            List<TWP_Employee_Schema> EESchema = new List<TWP_Employee_Schema>();
            string pagedURL = $"{TWPApiUtil.Employee_Schema_EndPoint}";
            //while(!String.IsNullOrEmpty(pagedURL))
            //{
                string APIResponse = await CallTWPAPI(siteId, APIToken, pagedURL, HttpMethod.Get);

                TWP_Employee_Schema result = JsonConvert.DeserializeObject<TWP_Employee_Schema>(APIResponse);

                EESchema.Add(result);
            //}
            return EESchema;
        }

        public static async Task<List<TWP_Employee>> ListEmployees(int siteId, string APIToken)
        {
            List<TWP_Employee> AllEmployees = new List<TWP_Employee>();
            string pagedURL = $"{TWPApiUtil.Employees_EndPoint}?pageSize={API_Page_Size}";

            while (!String.IsNullOrEmpty(pagedURL))
            {
                string APIResponse = await CallTWPAPI(siteId, APIToken, pagedURL, HttpMethod.Get);

                TWP_API_List_Response result = JsonConvert.DeserializeObject<TWP_API_List_Response>(APIResponse);

                AllEmployees.AddRange(result.Results.Select(empJson => empJson.ToObject<TWP_Employee>()));
                pagedURL = result.NextPageLink;
            }
            return AllEmployees;
        }

        public static async Task UpsertEmployee(int siteId, string APIToken, string eeID, TWP_EE_Upsert updateEmployee)
        {
            string upsertURL = $"{TWPApiUtil.Employees_EndPoint}/{eeID}?idType=recordNumber&upsert=true";

            await CallTWPAPI(siteId, APIToken, upsertURL, HttpMethod.Post, updateEmployee);
        }

        public static async Task<List<TWP_Group>> getEEGroups(int siteId, string APIToken, string showGroupsBy = "")
        {
            List<TWP_Group> allGroups = new List<TWP_Group>();

            string pagedURL = $"{TWPApiUtil.Employees_Groups_EndPoint}";
            if (!string.IsNullOrEmpty(showGroupsBy)) pagedURL += $"?showGroupsBy={showGroupsBy}";

            while (!String.IsNullOrEmpty(pagedURL))
            {
                string APIResponse = await CallTWPAPI(siteId, APIToken, pagedURL, HttpMethod.Get);

                TWP_API_List_Response result = JsonConvert.DeserializeObject<TWP_API_List_Response>(APIResponse);

                allGroups.AddRange(result.Results.Select(groupJson => groupJson.ToObject<TWP_Group>()));
                pagedURL = result.NextPageLink;
            }
            return allGroups;
        }

        public static async void connectEEManLogin(int siteId, string APIToken)
        {

        }

        public static async void disconnectEEManLogin(int siteId, string APIToken)
        {

        }

        public static async void resetEEPassword(int siteId, string APIToken)
        {

        }

        public static async void setEEPassword(int siteId, string APIToken)
        {

        }

        public static async void updateEEPassword(int siteId, string APIToken)
        {

        }
        #endregion

        #region PayrollActivities

        public static async Task<List<string>> getPayrollFormats(int siteId, string APIToken)
        {
            string schemaResult = await CallTWPAPI(siteId, APIToken, TWPApiUtil.Payroll_Schema_EndPoint);
            return JArray.Parse(schemaResult).Select(jo => jo["format"].ToString()).ToList();
        }

        public static async Task<TWP_PayrollActivities> GetPayrollActivities(int siteId, string APIToken, DateTime? payPeriodDate = null, List<string> employeeIds = null, string payrollFormat = null)
        {
            string dateParam = TWPApiUtil.FormatAPIDate(payPeriodDate ?? DateTime.Now);

            string payrollActivityURL = $"{TWPApiUtil.Payroll_Activities_EndPoint}?periodDate={dateParam}";

            if (!String.IsNullOrEmpty(payrollFormat))
            {
                payrollActivityURL += $"&format={payrollFormat}";
            }

            TWP_PayrollActivitiesRequest requestBody = null;

            if (employeeIds.safeCount() > 0)
            {
                requestBody = new TWP_PayrollActivitiesRequest(employeeIds);
            }
            else
            {
                List<string> eeid = new List<string>();
                var response = await ListEmployees(siteId, APIToken);
                foreach(TWP_Employee ee in response)
                {
                    eeid.Add(ee.EmployeeCode);
                }
                requestBody = new TWP_PayrollActivitiesRequest(eeid);
            }

            return JsonConvert.DeserializeObject<TWP_PayrollActivities>(
                await CallTWPAPI(siteId, APIToken, payrollActivityURL, HttpMethod.Post, requestBody));
        }

        public static async Task<TWP_PayrollActivities>getActivitiesBySDED (int siteId, string APIToken, DateTime? startDate = null,DateTime? endDate = null, string payrollFormat = null)
        {
            string startdate = TWPApiUtil.FormatAPIDate(startDate ?? DateTime.Now);
            string enddate = TWPApiUtil.FormatAPIDate(endDate ?? DateTime.Now);
            string payrollActivityURL = $"{TWPApiUtil.Payroll_Activities_EndPoint}?beginDate={startdate}&endDate={enddate}";
            if(!string.IsNullOrEmpty(payrollFormat))
            {
                payrollActivityURL += $"&format={payrollFormat}";
            }

            
            return JsonConvert.DeserializeObject<TWP_PayrollActivities>( await CallTWPAPI(siteId, APIToken, payrollActivityURL, HttpMethod.Post));
        }

        #endregion

        #region TimeCard

        public static async Task<JObject> getTimeCardDetails(int siteId, string APIToken, DateTime? payPeriodDate = null, List<string> employeeIds = null)
        {
            string dateParam = TWPApiUtil.FormatAPIDate(payPeriodDate ?? DateTime.Now);

            string timecardDetailsURL = $"{TWPApiUtil.TimeCard_Details_EndPoint}?periodDate={dateParam}";

            if (employeeIds.safeCount() > 0)
            {
                timecardDetailsURL += $"&ids={String.Join(",", employeeIds)}";
            }

            string timecardDetailsJSON = await CallTWPAPI(siteId, APIToken, timecardDetailsURL);

            return JObject.Parse(timecardDetailsJSON);
        }

        public static async Task<JObject> getTimeCardSum(int siteId, string APIToken, DateTime? payPeriodDate = null, List<string> employeeIds = null)
        {
            string dateParam = TWPApiUtil.FormatAPIDate(payPeriodDate ?? DateTime.Now);

            string timecardSumURL = $"{TWPApiUtil.getTimeCardSum}?periodDate={dateParam}";

            if(employeeIds.safeCount() > 0)
            {
                timecardSumURL += $"&ids={String.Join(",", employeeIds)}";
            }

            string timecardsumJSON = await CallTWPAPI(siteId, APIToken, timecardSumURL);

            return JObject.Parse(timecardsumJSON);
        }

        public static async void deleteTimeCardLine(int siteId, string APIToken)
        {

        }

        public static async void approveTimeCard(int siteId, string APIToken)
        {

        }

        public static async void editTimeCardLine(int siteId, string APIToken)
        {

        }

        public static async void addNote(int siteId, string APIToken)
        {

        }

        public static async void addPunch(int siteId, string APIToken)
        {

        }

        #endregion

        public static async Task<string> getSSOLink(string APISecret, int partnerId, int siteId, string APIToken, APIProduct ssoType, string ssoUserIdenfitier)
        {
            JWT_Payload ssoPayload = new JWT_Payload(partnerId, siteId, ssoType)
            {
                User = new JWT_UserInfo(ssoUserIdenfitier)
            };

            string ssoURL = TWPApiUtil.Base_Employee_SSO_EndPoint;

            if (ssoType == APIProduct.TWP_Supervisor_SSO)
            {
                ssoURL = TWPApiUtil.Base_Supervisor_SSO_EndPoint;
                ssoPayload.User.Type = JWT_Payload.JWT_Supervisor_Type_ID;
            }

            return ssoURL + await GetJWTToken(APISecret, ssoPayload);
        }

        #region Accruals

        public static async Task<List<TWP_AccrualsSchema>> getAccrualSchema(int siteId, string APIToken)
        {
            string schemaResult = await CallTWPAPI(siteId, APIToken, TWPApiUtil.Accruals_Schema_EndPoint);

            return JsonConvert.DeserializeObject<List<TWP_AccrualsSchema>>(schemaResult);
        }

        public static async Task UpdateAccrual(int siteId, string APIToken, List<TWP_AccrualUpdate> updateAccrual)
        {
            string upsertURL = $"{TWPApiUtil.Accruals_EndPoint}";

            await CallTWPAPI(siteId, APIToken, upsertURL, HttpMethod.Post, updateAccrual);
        }

        public static async Task<List<TWP_Accruals>> getAccruals(int siteId, string APIToken, DateTime? asOfDate = null, string category = null)
        {
            List<TWP_Accruals> allAccruals = new List<TWP_Accruals>();
            string pagedURL = $"{TWPApiUtil.Accruals_EndPoint}?pageSize={API_Page_Size}";
            if (asOfDate != null)
            {
                pagedURL += $"&asOfDate={TWPApiUtil.FormatAPIDate(asOfDate.Value)}";
            }

            if (category != null)
            {
                pagedURL += $"&category={category}";
            }

            while (!String.IsNullOrEmpty(pagedURL))
            {
                string apiResponse = await CallTWPAPI(siteId, APIToken, pagedURL, HttpMethod.Get);

                TWP_API_List_Response result = JsonConvert.DeserializeObject<TWP_API_List_Response>(apiResponse);

                allAccruals.AddRange(result.Results.Select(accJson => accJson.ToObject<TWP_Accruals>()));
                pagedURL = result.NextPageLink;
            }

            return allAccruals;
        }

        public static async Task<List<TWP_AccrualActivities>> getAccrualActivity(int siteId, string APIToken, DateTime? startDate = null, DateTime? endDate = null, string category = null, List<string> employeeIds = null)
        {
            List<TWP_AccrualActivities> allAccruals = new List<TWP_AccrualActivities>();
            string pagedURL = $"{TWPApiUtil.Accruals_Activity_EndPoint}?pageSize={API_Page_Size}";

            if (startDate != null)
            {
                pagedURL += $"&startDate={TWPApiUtil.FormatAPIDate(startDate.Value)}";
            }

            if (endDate != null)
            {
                pagedURL += $"&endDate={TWPApiUtil.FormatAPIDate(endDate.Value)}";
            }

            if (category != null)
            {
                pagedURL += $"&category={category}";
            }

            if (employeeIds.safeCount() > 0)
            {
                pagedURL += $"ids={String.Join(",", employeeIds)}";
            }

            while (!String.IsNullOrEmpty(pagedURL))
            {
                string apiResponse = await CallTWPAPI(siteId, APIToken, pagedURL, HttpMethod.Get);

                TWP_API_List_Response result = JsonConvert.DeserializeObject<TWP_API_List_Response>(apiResponse);

                allAccruals.AddRange(result.Results.Select(accJson => accJson.ToObject<TWP_AccrualActivities>()));
                pagedURL = result.NextPageLink;
            }

            return allAccruals;
        }

        #endregion

        #region TORS

        public static async Task<List<JObject>> getTORCats(int siteId, string APIToken)
        {
            string apiResponse = await CallTWPAPI(siteId, APIToken, $"{TWPApiUtil.TORCats}", HttpMethod.Get);

            return JsonConvert.DeserializeObject<List<JObject>>(apiResponse);
        }

        public static async void getTORbyEEId(int siteId, string APIToken)
        {

        }

        public static async void getTORbyId (int siteId, string APIToken)
        {

        }

        public static async void getTORbyEEIdDept (int siteId, string APIToken)
        {

        }

        public static async void getTORSchema (int siteId, string APIToken)
        {

        }

        public static async void getTORBySup(int siteId, string APIToken)
        {

        }

        #endregion

        #region Logins

        public static async Task<List<TWP_Login>> getLogins (int siteId, string APIToken, string loginName = null)
        {
            List<TWP_Login> AllLogins = new List<TWP_Login>();

            string url = $"{TWPApiUtil.Logins}";
            if (!string.IsNullOrEmpty(loginName)) url += $"?ids={loginName}";
            string apiResponse = await CallTWPAPI(siteId, APIToken, url, HttpMethod.Get);

            TWP_API_List_Response result = JsonConvert.DeserializeObject<TWP_API_List_Response>(apiResponse);

            AllLogins.AddRange(result.Results.Select(loginJson => loginJson.ToObject<TWP_Login>()));

            return AllLogins;
        }

        #endregion

        #region Rules

        public static async Task<List<TWP_Rules>> getRules(int siteId, string APIToken)
        {
            string rulesResult = await CallTWPAPI(siteId, APIToken, TWPApiUtil.Rules);
            return JsonConvert.DeserializeObject<List<TWP_Rules>>(rulesResult);
        }

        #endregion

        #region Schedules

        #endregion
    }
}