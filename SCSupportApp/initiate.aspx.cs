using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCSupportApp.Controllers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace SCSupportApp
{
    public partial class initiate : System.Web.UI.Page
    {
        public static int siteId;
        public string siteIdString;
        public static string apiSecret = "";
        public static int accountantId;
        public string accountantIDString = "";
        public static string token = "";
        public static string returnAuthToken = "";
        public bool alpha = true;
        public static bool tokenValid = false;
        private static string partnerAPIToken { get; set; } = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            getAccrualSchema.Enabled = tokenValid;

            if (!Page.IsPostBack)
            {
                loadLabel.Text = token;
            }
            else
            {
                if (type.SelectedValue == "1") Partner.Enabled = false;
                else Partner.Enabled = true;
                siteIdString = Site.Text;
                siteId = toNumber(siteIdString);
                apiSecret = Secret.Text;
                sendTo.Click += new EventHandler(this.sendTo_Click);

            }
        }

        public static int toNumber(string str, int defaultValue = 0)
        {
            int result;
            return Int32.TryParse(str, out result) ? result : defaultValue;
        }

        protected async void sendTo_Click(object sender, EventArgs e)
        {
            siteIdString = Site.Text;
            siteId = toNumber(siteIdString);
            apiSecret = Secret.Text;
            bool partner = type.SelectedValue == "0";
            string partnerId = Partner.Text;
            if (server.SelectedValue == "1")
            {
                alpha = false;
            }
            try
            {
                returnAuthToken = await Controllers.token.PostAuth(Controllers.token.creator(siteId, apiSecret, partner, partnerId), alpha);
            }
            catch
            {
                var error = new Label();
                error.Text = "Please check your credentials and try again";
                error.ID = "tokenFailure";
                error.BorderColor = System.Drawing.Color.Red;
                tokenError.Controls.Add(error);
            }

            if (!string.IsNullOrEmpty(returnAuthToken)) tokenValid = true;
            getAccrualSchema.Enabled = tokenValid;
        }

        private void catButtonHandle(object sender, EventArgs e)
        {
            var update = new Button();
            update.Click += new EventHandler(updateCat_Click);
            update.Text = $"Update Balance";
            modal_panel.Controls.Add(update);

            var getBal = new Button();
            getBal.Click += new EventHandler(getBal_Click);
            getBal.Text = $"Get Balance";
            modal_panel.Controls.Add(getBal);

            var getActivity = new Button();
            getActivity.Click += new EventHandler(getAct_Click);
            getActivity.Text = $"Activity";
            modal_panel.Controls.Add(getActivity);
        }

        private void getAct_Click(object sender, EventArgs e)
        {

        }

        private void getBal_Click(object sender, EventArgs e)
        {

        }

        private void updateCat_Click(object sender, EventArgs e)
        {

        }

        private void Load_Items(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
        }

        public static async Task AuthorizeAPI()
        {
            if (partnerAPIToken == null)
            {
                partnerAPIToken = await TWPSDK.GetJWTToken(apiSecret, accountantId, siteId, APIProduct.TWP_Partner);

                if(!String.IsNullOrEmpty(partnerAPIToken))
                {
                    Console.WriteLine($"Partner Authorization succeeded.");
                }
            }
        }

        protected void createButton(string Name, string text, bool success = true)
        {
            var button = new Button();

            if (success == true) button.CssClass = "btn btn-success";
            else button.CssClass = "btn btn-danger";
            button.Text = text;
            button.ID = Name;
            resultsPanel.Controls.Add(button);
        }
        protected void downloadFile(string name, StringBuilder body)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(body.ToString());

            if (bytes != null)
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.AddHeader("Content-disposition", "attachment; filename=\""+ name + ".csv" + "\"");
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }

        #region Accruals

        protected async void getAccrualSchema_Click(object sender, EventArgs e)
        {
            siteIdString = Site.Text;
            siteId = toNumber(siteIdString);
            string values = "";

            List<TWP_AccrualsSchema> accSchema = await Controllers.TWPSDK.getAccrualSchema(siteId, returnAuthToken);

            try
            {
                foreach (TWP_AccrualsSchema value in accSchema)
                {
                    createButton(value.Category, value.Category);
                }
                accrualvalues.InnerText = $"{values}";
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Validation Client Accrual Schema: an exception occurred: {ex.Message}");
            }
        }

        protected async void getAccrualBalance_Click(object sender, EventArgs e)
        {
            try
            {
                List<TWP_Accruals> accBalances = await TWPSDK.getAccruals(siteId, returnAuthToken);

                foreach(TWP_Accruals results in accBalances)
                {
                    foreach(TWP_AccrualValues balances in results.Balances)
                    {
                        string name = $"{results.FirstName} {results.LastName} {balances.Category}";
                        string text = $"{results.FirstName} {results.LastName} {balances.Category} {balances.Value}";
                        createButton(name, text);
                    }
                }
            }
            catch
            {

            }

        }
        
        protected async void getAccrualActivity_Click(object sender, EventArgs e)
        {
            List<TWP_AccrualActivities> accrualActivity = await TWPSDK.getAccrualActivity(siteId, returnAuthToken);
        }

        protected void postUpdateAccrual_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Employees

        protected async void getEmployeesSchema_Click(object sender, EventArgs e)
        {
            List<TWP_Employee> eeSchema = await TWPSDK.ListEmployees(siteId, returnAuthToken);
        }

        protected async void getEmployees_Click(object sender, EventArgs e)
        {
            List<TWP_Employee> employeesList = await Controllers.TWPSDK.ListEmployees(siteId, returnAuthToken);
            try
            {
                foreach(TWP_Employee employee in employeesList)
                {
                    createButton(employee.FullName, $"{employee.FullName}");
                }
            }
            catch
            {

            }
        }

        protected async void downloadEmployees_Click(object sender, EventArgs e)
        {

            List<TWP_Employee_Schema> eeSchema = await TWPSDK.listEESchema(siteId, returnAuthToken);
            IList<string> schemaState = eeSchema[0].States[0].Variables.Keys.ToList();

            List<TWP_Employee> employees = await TWPSDK.ListEmployees(siteId, returnAuthToken);
            StringBuilder sb = new StringBuilder();

            int idCount = 3;
            int statesCount = 0;

            string header = "RecordNumber,First Name,Middle Name,Last Name,Code,Designation,Phone,Email,Start Date,Separation Date," +
                "Export Block,WebClock Enabled,login1,login2,login3,";

            foreach(TWP_Employee_Schema results in eeSchema)
            {
                foreach(TWP_State_Schema state in results.States)
                {
                    foreach (KeyValuePair<string, string> kvp in state.Variables)
                    {
                        ++statesCount;
                        header += $"{kvp.Key.ToString()},";
                    }
                }
            }

            sb.AppendLine(header);
            string body = "";
            foreach (TWP_Employee emp in employees)
            {
                body = $"{emp.RecordNumber},{emp.FirstName},{emp.MiddleName},{emp.LastName},{emp.EmployeeCode},{emp.Designation},{emp.Phone},{emp.Email},{emp.StartDate},{emp.EndDate},{emp.ExportBlock},{emp.WebClockEnabled},";

                try
                {
                    foreach (TWP_Identifier id in emp.Identifiers)
                    {
                        if (!string.IsNullOrEmpty(id.Id))
                            body += $"{id.Id},";
                    }
                    if (emp.Identifiers.Count < idCount) body += string.Concat(Enumerable.Repeat(",", idCount - emp.Identifiers.Count));
                }
                catch
                {
                    body += $",,,";
                }



                try
                {
                    foreach (TWP_State state in emp.States)
                    {
                        foreach (string varName in schemaState)
                        {
                            foreach (KeyValuePair<string, string> kvp in state.Variables)
                            {
                                if (kvp.Key == varName)
                                {
                                    body += $"{kvp.Value}";
                                }

                            }
                            body += ",";
                        }
                    }
                }
                catch
                {
                    body += string.Concat(Enumerable.Repeat(",", statesCount));
                }
                sb.AppendLine(body);
            }

            downloadFile("EmployeeDetail", sb);
        }

        protected async void bulkUpsertEmployees_Click(object sender, EventArgs e)
        {
            List<TWP_EE_Upsert> updatedEE = new List<TWP_EE_Upsert>();
            
            
            if (EEImport.HasFile)
            {
                StreamReader csvreader = new StreamReader(EEImport.FileContent);
                while(!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    var values = line.Split(',');
                    
                    
                    
                    List<Variable> varsList = new List<Variable>();
                    List<TWP_Employee_Schema> eeSchema = await TWPSDK.listEESchema(siteId, returnAuthToken);

                    

                    
                    
                    IList<string> varValue = new List<string>();
                    

                    int valCount = 15;

                    

                    if (values[0] != "RecordNumber")
                    {
                        List<TWP_Identifier> idList = new List<TWP_Identifier>();
                        List<string> testid = new List<string>();
                        Variable vars = new Variable();
                        
                        if (values.Length > 13)
                        {
                            for (int i = 12; i <= 14; i++)
                            {
                                TWP_Identifier id = new TWP_Identifier();
                                id.Id = !string.IsNullOrEmpty(values[i]) ? values[i].ToString() : "";
                                idList.Add(id);
                                testid.Add(id.Id);
                            }

                        }

                        foreach (string key in eeSchema[0].States[0].Variables.Keys)
                        {
                            Variable varItem = new Variable();
                            
                            string value = values[valCount];
                            KeyValuePair<string, string> variablesDic = new KeyValuePair<string, string>(key, value);
                            if (value.Contains("$")) value = value.Replace("$", "");
                            //if (!variablesDic(key)) variablesDic.Add(key, value);
                            varItem.Variables = variablesDic;
                            varsList.Add(varItem);
                            //if (!vars.Variables.ContainsKey(key)) vars.Variables.Add(key, value);
                            valCount++;
                        }


                        TWP_EE_Upsert ee = new TWP_EE_Upsert()
                        {
                            FirstName = values[1],
                            MiddleName = values[2],
                            LastName = values[3],
                            EmployeeCode = values[4],
                            Designation = values[5],
                            Phone = values[6],
                            Email = values[7],
                            StartDate = values[8],
                            EndDate = values[9],
                            ExportBlock = Convert.ToBoolean(values[10]),
                            WebClockEnabled = Convert.ToBoolean(values[11]),
                            //Identifiers = idList,
                            //States = new TWP_upsert_State()
                            //{
                            //    EffectiveDate = DateTime.Now.ToString("yyyy-MM-dd"),
                            //    Variables = varsList
                            //}
                        };

                        try
                        {
                            await TWPSDK.UpsertEmployee(siteId, returnAuthToken, values[0], ee);
                            createButton(ee.EmployeeCode, $"{ee.FirstName} {ee.LastName}");

                        }
                        catch
                        {
                            createButton(ee.EmployeeCode, $"{ee.FirstName} {ee.LastName}", false);
                        }
                    }
                }
            }
            
        }

        protected void postUpdateEmployees_Click(object sender, EventArgs e)
        {

        }
        
        protected void postEmployeeConnectMgrLogin_Click(object sender, EventArgs e)
        {

        }

        protected void postEmployeeDisconnectMangerLogin_Click(object sender, EventArgs e)
        {

        }

        protected void postEmployeeSetPassword_Click(object sender, EventArgs e)
        {

        }

        protected void postEmployeeResetPassword_Click(object sender, EventArgs e)
        {

        }

        protected void postEmployeeUpdatePassword_Click(object sender, EventArgs e)
        {

        }

        #endregion

        protected async void getLogins_Click(object sender, EventArgs e)
        {
            List<TWP_Login> logins = await TWPSDK.getLogins(siteId, returnAuthToken);
            foreach(TWP_Login login in logins)
            {
                createButton(login.LoginName, login.LoginName);
            }
        }

        #region PayrollActivities

        protected async void getPayrollActivities_Click(object sender, EventArgs e)
        {
            var response = await TWPSDK.getActivitiesBySDED(siteId, returnAuthToken);
            if (string.IsNullOrEmpty(response.Error))
            {
                if (!string.IsNullOrEmpty(response.FormatString))
                {
                    string file = response.FormatString.Replace("\"", "");
                    string[] strSeparators = new string[] { "\r\n" };
                    string[] result;
                    result = file.Split(strSeparators, StringSplitOptions.None);
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in result)
                    {
                        sb.AppendLine(line);
                    }
                    downloadFile("payroll", sb);
                }
            }
        }

        protected async void getPayrollActivitiesPerPayPeriod_Click(object sender, EventArgs e)
        {
            var response = await TWPSDK.GetPayrollActivities(siteId, returnAuthToken, null, null, "sum3");
            if(string.IsNullOrEmpty(response.Error))
            {
                if (!string.IsNullOrEmpty(response.FormatString))
                {
                    string file = response.FormatString.Replace("\"", "");
                    string[] strSeparators = new string[] { "\r\n" };
                    string[] result;
                    result = file.Split(strSeparators, StringSplitOptions.None);
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in result)
                    {
                        sb.AppendLine(line);
                    }
                    downloadFile("payroll", sb);
                }
            }
        }

        protected async void getPayrollFormats_Click(object sender, EventArgs e)
        {
            List<string> formats = await TWPSDK.getPayrollFormats(siteId, returnAuthToken);
            var formatLabel = new Label()
            {

            };
            resultsPanel.Controls.Add(resultsLabel);
        }

        #endregion

        #region Rules

        protected async void getRules_Click(object sender, EventArgs e)
        {
            List<TWP_Rules> rules = await TWPSDK.getRules(siteId, returnAuthToken);
        }

        protected void getIntegratedSchedulingRule_Click(object sender, EventArgs e)
        {

        }

        protected void deleteExtEmployeeIDRule_Click(object sender, EventArgs e)
        {

        }

        protected void deleteIntegrationFieldsRule_Click(object sender, EventArgs e)
        {

        }

        protected void postExtEmployeeIDRule_Click(object sender, EventArgs e)
        {

        }

        protected void postIntegratedSchedulingRule_Click(object sender, EventArgs e)
        {

        }

        protected void postIntegrationRule_Click(object sender, EventArgs e)
        {

        }

        #endregion

        protected void getTimeWorksPlusSchedules_Click(object sender, EventArgs e)
        {

        }

        #region TimeCard

        protected void getTimeCards_Click(object sender, EventArgs e)
        {

        }

        protected void getTimeCardSummary_Click(object sender, EventArgs e)
        {

        }

        protected void deleteTimeCardLine_Click(object sender, EventArgs e)
        {

        }

        protected void postEditTimeCardLine_Click(object sender, EventArgs e)
        {

        }

        protected void postTimeCardApproval_Click(object sender, EventArgs e)
        {

        }

        protected void postAddTimeCardNote_Click(object sender, EventArgs e)
        {

        }

        protected void postAddTimeCardPunch_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region TOR

        protected void getTimeOffRequestsbyEEID_Click(object sender, EventArgs e)
        {

        }

        protected void getTimeOffRequestCategories_Click(object sender, EventArgs e)
        {

        }

        protected void getTimeOffRequestbyEEIDdept_Click(object sender, EventArgs e)
        {

        }

        protected async void getSchemaTimeOffRequests_Click(object sender, EventArgs e)
        {
            List<JObject> TORs = await TWPSDK.getTORCats(siteId, returnAuthToken);

            foreach (JObject Cat in TORs)
            {
                string category = Cat.ToString();
                var label = new Label
                {
                    ID = category,
                    Text = category
                };
                resultsPanel.Controls.Add(label);
            }
        }

        protected void getSupervisorTimeOffRequests_Click(object sender, EventArgs e)
        {

        }

        protected void postCreateTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        protected void postAcceptTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        protected void postApproveTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        protected void postCancelTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        protected void postRejectTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        protected void postUnApproveTimeOffRequest_Click(object sender, EventArgs e)
        {

        }

        #endregion

    }
}