using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCSupportApp.Controllers
{
    public class TWPModels
    {
    }

    public class JWT_SiteInfo
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        public override string ToString()
        {
            return $"Type: {Type}, Id: {Id}";
        }
    }

    public class JWT_UserInfo
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public JWT_UserInfo(string empCode)
        {
            Type = JWT_Payload.JWT_Employee_Type_ID;
            Id = empCode;
        }

        public override string ToString()
        {
            return $"Type: {Type}, Id: {Id}";
        }
    }

    public class JWT_Payload
    {
        public const int JWT_Expiration_Seconds = 60;

        public const string JWT_Partner_Subject = "partner";
        public const string JWT_Site_Type_ID = "id";
        public const string JWT_Supervisor_Type_ID = "login";
        public const string JWT_Employee_Type_ID = "empcode";

        [JsonProperty(PropertyName = "iss")]
        public int Iss { get; set; }

        [JsonProperty(PropertyName = "exp")]
        public long Exp { get; set; }

        [JsonProperty(PropertyName = "sub")]
        public string Sub { get; set; }

        [JsonProperty(PropertyName = "siteInfo")]
        public JWT_SiteInfo SiteInfo { get; set; } = new JWT_SiteInfo();

        [JsonProperty(PropertyName = "user")]
        public JWT_UserInfo User { get; set; } = null;

        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        public JWT_Payload(int partnerId, int siteId, APIProduct apiProduct)
        {
            Iss = partnerId;
            Exp = TWPApiUtil.GetUnixEpochTimeStamp(DateTime.UtcNow.AddSeconds(JWT_Expiration_Seconds));
            Sub = JWT_Partner_Subject;
            SiteInfo.Type = JWT_Site_Type_ID;
            SiteInfo.Id = siteId;
            Product = TWPApiUtil.aPIProductToken[apiProduct];
        }

        public override string ToString()
        {
            string retval = $"iss: {Iss}, prod: {Product}, site: {(SiteInfo?.Id.ToString() ?? "<null>")}, exp: {Exp}";

            if(User != null && User.Id != null)
            {
                retval += $", user: {User.Id}";
            }

            return retval;
        }
    }

    public class TWP_API_List_Response
    {
        public long TotalCount { get; set; }
        public long TotalPages { get; set; }
        public string PrevPageLink { get; set; }
        public string NextPageLink { get; set; }
        public long ResultCount { get; set; }
        public List<JObject> Results { get; set; }

        public override string ToString()
        {
            return $"Total Entries: {TotalCount}, Page Entries: {ResultCount}";
        }
    }

    public class TWP_Identifier
    {
        public string Id { get; set; }
    }

    public class TWP_Employee_Schema
    {
        public string RecordNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExportBlock { get; set; }
        public string WebClockEnabled { get; set; }
        public List<TWP_Identifier> Identifiers { get; set; }
        public List<TWP_State_Schema> States { get; set; }

    }

    public class TWP_State_Schema
    {
        public string EffectiveDate { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }

    public class TWP_Employee
    {
        public int RecordNumber { get; set; }
        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Name2")]
        public string FullName { get; }
        public string Designation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool ExportBlock { get; set; }
        public bool WebClockEnabled { get; set; }

        public List<TWP_Identifier> Identifiers { get; set; }

        public List<TWP_State> States { get; set; }

        public TWP_Employee()
        {

        }

        [JsonConstructor]
        public TWP_Employee(string fullName)
        {
            FullName = fullName;
        }

        public override string ToString()
        {
            return $"{FullName}({EmployeeCode})";
        }
    }

    public class TWP_Group
    {
        public List<TWP_Employee> Employees { get; set; }

    }

    public class TWP_State
    {
        public DateTime? EffectiveDate { get; set; }
        public Dictionary<string, string> Variables { get; set; }

        public override string ToString()
        {
            return $"{Variables.safeCount()} Variables as of: {EffectiveDate.FormatAPIDate() ?? TWPApiUtil.Api_Unset_Token}, ";
        }
    }

    public class TWP_ID
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }

    public class TWP_PayrollActivitiesRequest : List<TWP_ID>
    {
        public TWP_PayrollActivitiesRequest(List<string> employeeIds)
        {
            this.AddRange(employeeIds.Select(empId => new TWP_ID { Id = empId }));
        }

        public override string ToString()
        {
            return $"{Count} IDs: {String.Join(", ", this.Take(5).Select(id => id.Id))}";
        }
    }

    public class TWP_PayrollActivities
    {
        public string Error { get; set; }
        public bool? FormatBinary { get; set; }
        public string FormatString { get; set; }
        public string MimeType { get; set; }

        public override string ToString()
        {
            if(!String.IsNullOrEmpty(Error))
            {
                return $"Error: {Error}";
            }

            string shortData = FormatString?.Substring(0, 100) ?? TWPApiUtil.Api_Unset_Token;

            string retval = $"Data: {shortData}";

            if(FormatBinary ?? false)
            {
                retval = "Binary " + retval;
            }

            if(!String.IsNullOrEmpty(MimeType))
            {
                retval = $"MimeType: {MimeType}" + retval;
            }

            return retval;
        }
    }

    public class TWP_AccrualsSchema
    {
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "isHidden")]
        public string IsHidden { get; set; }

        [JsonProperty(PropertyName = "effective")]
        public string Effective { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Category: {Category}, IsHidden: {IsHidden}";
        }
    }

    public partial class TWP_AccrualUpdate
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("isHidden")]
        public string IsHidden { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("effective")]
        public string Effective { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Category: {Category}, Value: {Value}";
        }
    }

    public partial class TWP_Accruals
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [JsonProperty(PropertyName = "Name2")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "StartDate")]
        public DateTime? EmployeeStartDate { get; set; }
        [JsonProperty(PropertyName = "EndDate")]
        public DateTime? EmployeeEndDate { get; set; }
        public DateTime? AsOfDate { get; set; }
        public List<TWP_AccrualValues> Balances { get; set; }

        public override string ToString()
        {
            return $"Emp: {FullName}, AsOf: {AsOfDate.FormatAPIDate()}, Balances: {Balances.safeCount()}";
        }
    }

    public partial class TWP_AccrualActivities
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [JsonProperty(PropertyName = "Name2")]
        public string FullName { get; set; }
        public DateTime? EmployeeStartDate { get; set; }
        public DateTime? EmployeeEndDate { get; set; }
        public List<TWP_AccrualValues> StartingValues { get; set; }
        public List<TWP_AccrualValues> EndingValues { get; set; }
        public List<TWP_AccrualValues> Days { get; set; }

        public override string ToString()
        {
            return $"Employee: {FullName}, Activity Days: {Days.safeCount()}";
        }
    }

    public partial class TWP_AccrualValues
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("isHidden")]
        public string IsHidden { get; set; }

        [JsonProperty("value")]
        public decimal? Value { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        [JsonProperty("effective")]
        public DateTime? Effective { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        public List<TWP_AccrualActivity> Activity { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Category: {Category}, Date: {Date.FormatAPIDate()}, Value: {Value ?? 0}, Activity Count: {Activity.safeCount()}";
        }
    }

    public partial class TWP_AccrualActivity
    {
        public string SavedBy { get; set; }
        public string SavedFrom { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsAbsolute { get; set; }
        public string ChangeType { get; set; }
        public decimal? Delta { get; set; }

        public override string ToString()
        {
            return $"ChangeType: {ChangeType}, IsAbsolute: {IsAbsolute?.ToString() ?? TWPApiUtil.Api_Unset_Token}, Amount: {Amount ?? 0}, Delta: {Delta ?? 0}, SavedBy: {SavedBy ?? TWPApiUtil.Api_Unset_Token}";
        }
    }

    public class TWP_Rules
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string EmpSelectorConfiguration { get; set; }
        public string EnableOnOrAfterDate { get; set; }
        public string ConfigPageTitle { get; set; }
        public List<Rule_Config> XmlConfig { get; set; }
        public string xmlConfig => XmlConfig.ToString();

        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description}, Category: {Category}, Status: {Status}, EmpSelectorConfiguration: {EmpSelectorConfiguration}, EnableOnOrAfterDate: {EnableOnOrAfterDate}, ConfigPageTitle: {ConfigPageTitle}" + !string.IsNullOrEmpty(xmlConfig);

        }
    }

    public class Rule_Config
    {
        public string XmlConfiguration { get; set; }
    }
}