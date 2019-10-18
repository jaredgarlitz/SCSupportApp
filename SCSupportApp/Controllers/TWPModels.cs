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

            if (User != null && User.Id != null)
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

    public class TWP_EE_Upsert
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool ExportBlock { get; set; }
        public bool WebClockEnabled { get; set; }
        //[JsonProperty(PropertyName = "Id")]
        public List<TWP_Identifier> Identifiers { get; set; }
        public TWP_upsert_State States { get; set; }
        public TWP_EE_Upsert() { }

    }
    public class TWP_Employee
    {
        public string RecordNumber { get; set; }
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

        [JsonProperty(PropertyName = "Id")]
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

    public class TWP_upsert_State
    {
        public string EffectiveDate { get; set; }

        public List<Variable> Variables { get; set; }
    }

    public class Variable
    {
        public KeyValuePair<string, string> Variables { get; set; }
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

    public class TWP_Login
    {
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Guid { get; set; }
        public string SiteNumber { get; set; }
        public string SiteName { get; set; }
        public string AccountType { get; set; }
        public int AccountTypeNumber { get; set; }

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
            if (!String.IsNullOrEmpty(Error))
            {
                return $"Error: {Error}";
            }

            string shortData = FormatString?.Substring(0, 100) ?? TWPApiUtil.Api_Unset_Token;

            string retval = $"Data: {shortData}";

            if (FormatBinary ?? false)
            {
                retval = "Binary " + retval;
            }

            if (!String.IsNullOrEmpty(MimeType))
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

    public class TimeCard
    {
        public string SiteTimeZone { get; set; }
        public string LocalizationInfo { get; set; }
        public DateTime? PayPeriodBeginDate { get; set; }
        public DateTime? PayPeriodEneDate { get; set; }
        public DateTime? PayPeriodPrevBeginDate { get; set; }
        public DateTime? PayPeriodPrevEndDate { get; set; }
        public string[] PunchInOutTypes { get; set; }
        public string ClockPrompts { get; set; }
        public TWPPunchCategory PunchCategories { get; set; }
        public bool IntelligentClock { get; set; }
        public string TimeCards { get; set; }
    }

    public class TWPPunchCategory
    {
        public bool Accruable { get; set; }
        public string _baseCategory { get; set; }
        public bool IsRegular { get; set; }
        public string OT1Name { get; set; }
        public string OT2Name { get; set; }
        public bool OTEligible { get; set; }
        public string OutputValueColumn { get; set; }
        public bool SelectableAmount { get; set; }
        public bool SelectableHours { get; set; }
        public bool SelectableTimes { get; set; }
        public int SortKey { get; set; }
        public bool TimeOffRequestable { get; set; }
        public string UnpaidName { get; set; }
        public bool UserSelectable { get; set; }
        public string m_BaseCategoryLong { get; set; }
    }

    public class TimeCards
    {
        public TWP_Employee Employee { get; set; }
        public TWPSummary Summary { get; set; }
        public Dates[] Dates { get; set; }
        public Notifications[] Notifications { get; set; }
    }

    public class TWPSummary
    {
        public CategoryLaborPromptGrouping CategoryLaborPromptGrouping { get; set; }
        public SumLines[] Lines { get; set; }
        public Categories[] Categories { get; set; }

    }

    public class CategoryLaborPromptGrouping
    {
        public BaseCategory[] BaseCategory { get; set; }
    }

    public class BaseCategory
    {
        public string GroupedBy { get; set; }
        public string GroupValue { get; set; }
        public string BaseCategory1 { get; set; }
        public string OT1Category { get; set; }
        public string OT2Category { get; set; }
        public string UnpaidCategory { get; set; }
        public int TotalUnpaidSeconds { get; set; }
        public int TotalSeconds { get; set; }
        public int TotalBaseAmount { get; set; }
        public int TotalAdditionalPay { get; set; }
        public int TotalBaseSeconds { get; set; }
        public int TotalOT1Seconds { get; set; }
        public int TotalOT2Seconds { get; set; }
    }

    public class SumLines
    {
        public string BaseCategory { get; set; }
        public string OT1Category { get; set; }
        public string OT2Category { get; set; }
        public string UnpaidCategory { get; set; }
        public int TotalUnpaidSeconds { get; set; }
        public string Totals { get; set; }
        public int TotalSeconds { get; set; }
        public int TotalBaseAmount { get; set; }
        public int TotalBaseSeconds { get; set; }
        public int TotalOT1Seconds { get; set; }
        public int TotalOT2Seconds { get; set; }
        public string Properties { get; set; }
    }

    public class Categories
    {
        public string BaseCategory { get; set; }
        public string OT1Category { get; set; }
        public string OT2Category { get; set; }
        public string UnpaidCategory { get; set; }
        public int TotalBAseAmount { get; set; }
        public int TotalOt1Seconds { get; set; }
        public int TotalOT2Seconds { get; set; }
        public int TotalUnpaidSeconds { get; set; }
        public int TotalSeconds { get; set; }
        public int TotalBaseSeconds { get; set; }
        public string TotalTimeCardValueFields { get; set; }
    }

    public class Dates
    {
        public string[] Alerts { get; set; }
        public string ApprovalStatus { get; set; }
        public string DatedNote { get; set; }
        public string approvalDesc { get; set; }
        public bool softLocked { get; set; }
        public string Date { get; set; }
        public string PayPeriodBeginDate { get; set; }
        public string PayPeriodEndDate { get; set; }
        public string FirstDayOfWeek { get; set; }
        public bool IsFromFinal { get; set; }
        public string[] Lines { get; set; }
        public string[] Categories { get; set; }
        public string LinesTotal { get; set; }

    }

    public class Notifications
    {
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Seconds { get; set; }
        public bool NotConsideredOtInSeconds { get; set; }
        public string Message { get; set; }
        public string ShortMessage { get; set; }
        public string Subject { get; set; }

    }

    public class TWP_Format_Response
    {
        public string Error { get; set; }
        public string FormatBinary { get; set; }
        public string FormatString { get; set; }
        public string MimeType { get; set; }
    }

}