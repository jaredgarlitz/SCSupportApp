<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="initiate.aspx.cs" Inherits="SCSupportApp.initiate" Async="true" %>

<asp:Content ID="tokenGen" ContentPlaceHolderID="genTokenPlaceHolder" runat="server">
    <div class="container">
        <asp:DropDownList ID="type" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Accountant" Value="0" />
            <asp:ListItem Text="Client" Value="1" />
        </asp:DropDownList>
        <asp:DropDownList ID="server" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Alpha" Value="0" />
            <asp:ListItem Text="Live" Value="1" />
        </asp:DropDownList>
        <asp:TextBox ID="Partner" runat="server" placeholder="Accountant ID" />
        <asp:TextBox ID="Site" runat="server" placeholder="Site #" />
        <asp:TextBox ID="Secret" runat="server" placeholder="API Secret" />
        <asp:Button ID="sendTo" class="btn btn-success btn-sm" Text="Check" runat="server" />
        <asp:Label ID="loadLabel" runat="server" />
        <asp:Label ID="tokenID" runat="server" />
        <div class="container">
            <div class="col">
                <asp:Panel ID="tokenError" runat="server">

                </asp:Panel>
            </div>
        </div>
    </div>
    <div class="container-sm">
        <div class="container">
            <div class="row">
                <div class="col-6">
                    <div class="accordion" id="addPunch">
                        <div class="card">
                            <div class="card-header" id="headingTwo">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                        Accruals
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-1">
                                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getAccrualSchema" runat="server" Text="Get Schema" OnClick="getAccrualSchema_Click"/>
                                            </div>
                                            <div class="col-1">
                                                <label id="accrualvalues" runat="server"></label>
                                            </div>
                                        </div>
                                        <p />
                                        <div class="row">
                                            <div class="col-1">
                                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getAccrualBalance" runat="server" Text="Get Balances" OnClick="getAccrualBalance_Click" />
                                            </div>
                                        </div>
                                        <p />
                                        <%--<div class="row">
                                <div class="col-1">
                                    <asp:Button type="button" class="btn btn-primary btn-sm" ID="getAccrualBalance" runat="server" Text="Get Accrual Balance" />
                                </div>
                                <div class="col-1">
                                    <asp:Button type="button" class="btn btn-primary btn-sm" ID="getAccrualActivity" runat="server" Text="Get Accrual Activity" />
                                </div>
                            </div>
                            <p />
                            <div class="row">
                                <div class="col-1">
                                    <asp:Button type="button" class="btn btn-primary btn-sm" ID="postUpdateAccrual" runat="server" Text="Update Accrual Balance" />
                                </div>
                            </div>--%>
                                    </div>
                                    <div class="container">
                                        <asp:Panel ID="accrualCatPanel" runat="server">
                                        </asp:Panel>
                                    </div>
                                    <div class="container">
                                        <asp:Panel ID="modal_panel" runat="server" Width="500px" data-target="accrualCatModal">
                                            <div class="modal" tabindex="-1" role="button" id="accrualCatModal">
                                                <div class="modal-dialog" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                                <span aria-hidden="true">&times;</span>
                                                            </button>
                                                        </div>
                                                        <div class="modal-body">
                                                        </div>
                                                        <div class="modal-footer">
                                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                                            <button type="button" class="btn btn-primary">Save changes</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingThree">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                        Employees
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseThree" class="collapse" aria-labelledby="headingThree" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getEmployeesSchema" runat="server" Text="Get Employee Schema" OnClick="getEmployeesSchema_Click"/>
                                        </div>
<%--                                    </div>
                                    <p />
                                    <div class="row">--%>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getEmployees" runat="server" Text="Get Employees" OnClick="getEmployees_Click"></asp:Button>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-success btn-sm" ID="downloadEmployees" runat="server" Text="Download .csv" OnClick="downloadEmployees_Click" />
                                        </div>
                                    </div>
                                    <p></p>
                                    <div class="row">
                                        <div class="col">
                                            <asp:FileUpload type="file" class="btn btn-dark btn-sm" text="Import" id="EEImport" runat="server" />
                                            <asp:Button type="button" class="btn btn-dark" Text="Bulk Import EE's" ID="bulkImport" runat="server" OnClick="bulkUpsertEmployees_Click" />
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col-sm">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postUpdateEmployees" runat="server" Text="POST Update Employees" OnClick="postUpdateEmployees_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col-sm">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeConnectMgrLogin" runat="server" Text="Connect EE/Sup Login" OnClick="postEmployeeConnectMgrLogin_Click" Visible ="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeDisconnectMangerLogin" runat="server" Text="Disconnect EE/Sup Login" OnClick="postEmployeeDisconnectMangerLogin_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col-sm">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeSetPassword" runat="server" Text="Set EE Password" OnClick="postEmployeeSetPassword_Click" Visible="false"/>
                                        </div>
                                        <div class="col-sm">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeResetPassword" runat="server" Text="Reset EE Password" OnClick="postEmployeeResetPassword_Click" Visible="false"/>
                                        </div>
                                        <div class="col-sm">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeUpdatePassword" runat="server" Text="Update EE Password" OnClick="postEmployeeUpdatePassword_Click" Visible="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingFour">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                                        Logins
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseFour" class="collapse" aria-labelledby="headingFour" data-parent="#addPunch">
                                <div class="card-body">
                                    <asp:Button type="button" class="btn btn-primary btn-sm" ID="getLogins" runat="server" Text="Get Logins" OnClick="getLogins_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingFive">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseFive" aria-expanded="false" aria-controls="collapseFive">
                                        PayRollActivities
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseFive" class="collapse" aria-labelledby="headingFive" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollActivities" runat="server" Text="Get Payroll Activities" OnClick="getPayrollActivities_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollActivitiesPerPayPeriod" runat="server" Text="Payroll Act. by Period" OnClick="getPayrollActivitiesPerPayPeriod_Click"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollFormats" runat="server" Text="Get Formats" OnClick="getPayrollFormats_Click" Visible="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingSix">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseSix" aria-expanded="false" aria-controls="collapseSix">
                                        Rules
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseSix" class="collapse" aria-labelledby="headingSix" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getRules" runat="server" Text="Get Rules" OnClick="getRules_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getIntegratedSchedulingRule" runat="server" Text="Get Integrated Scheduling Rule" OnClick="getIntegratedSchedulingRule_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteExtEmployeeIDRule" runat="server" Text="Delete ExtEmployeeID" OnClick="deleteExtEmployeeIDRule_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteIntegrationFieldsRule" runat="server" Text="Delete IntegrationFields Rule" OnClick="deleteIntegrationFieldsRule_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postExtEmployeeIDRule" runat="server" Text="Add ExtEmployeeID" OnClick="postExtEmployeeIDRule_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postIntegratedSchedulingRule" runat="server" Text="Add Int Scheduling" OnClick="postIntegratedSchedulingRule_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postIntegrationRule" runat="server" Text="Post Integration Rule" OnClick="postIntegrationRule_Click" Visible="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingSeven">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseSeven" aria-expanded="false" aria-controls="collapseSeven">
                                        Schedules
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseSeven" class="collapse" aria-labelledby="headingSeven" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeWorksPlusSchedules" runat="server" Text="Get TWP Schedules" OnClick="getTimeWorksPlusSchedules_Click" Visible="false"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingEight">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseEight" aria-expanded="false" aria-controls="collapseEight">
                                        TimeCards
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseEight" class="collapse" aria-labelledby="headingEight" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeCards" runat="server" Text="Get Time Cards" OnClick="getTimeCards_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeCardSummary" runat="server" Text="Get Time Card Summary" OnClick="getTimeCardSummary_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteTimeCardLine" runat="server" Text="Delete Time CardLine" OnClick="deleteTimeCardLine_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEditTimeCardLine" runat="server" Text="Edit TimeCard Line" OnClick="postEditTimeCardLine_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postTimeCardApproval" runat="server" Text="TimeCard Approval" OnClick="postTimeCardApproval_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAddTimeCardNote" runat="server" Text="Add TimeCard Note" OnClick="postAddTimeCardNote_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAddTimeCardPunch" runat="server" Text="Add TimeCard Punch" OnClick="postAddTimeCardPunch_Click" Visible="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingNine">
                                <h2 class="mb-0">
                                    <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseNine" aria-expanded="false" aria-controls="collapseNine">
                                        TimeOffRequestsRequests
                                    </button>
                                </h2>
                            </div>
                            <div id="collapseNine" class="collapse" aria-labelledby="headingNine" data-parent="#addPunch">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestsbyEEID" runat="server" Text="Get TOR by EE ID" OnClick="getTimeOffRequestsbyEEID_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestCategories" runat="server" Text="Get TOR Categories" OnClick="getTimeOffRequestCategories_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestbyEEIDdept" runat="server" Text="Get TOR by EE ID(Dep)" OnClick="getTimeOffRequestbyEEIDdept_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getSchemaTimeOffRequests" runat="server" Text="Get Schema TOR" OnClick="getSchemaTimeOffRequests_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="getSupervisorTimeOffRequests" runat="server" Text="Get Supervisor TOR" OnClick="getSupervisorTimeOffRequests_Click" Visible="false"/>
                                        </div>
                                    </div>
                                    <p />
                                    <div class="row">
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postCreateTimeOffRequest" runat="server" Text="Create TOR" OnClick="postCreateTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAcceptTimeOffRequest" runat="server" Text="Accept TOR" OnClick="postAcceptTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postApproveTimeOffRequest" runat="server" Text="Approve TOR" OnClick="postApproveTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postCancelTimeOffRequest" runat="server" Text="Cancel TOR" OnClick="postCancelTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postRejectTimeOffRequest" runat="server" Text="Reject TOR" OnClick="postRejectTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                        <div class="col">
                                            <asp:Button type="button" class="btn btn-primary btn-sm" ID="postUnApproveTimeOffRequest" runat="server" Text="Un-Approve TOR" OnClick="postUnApproveTimeOffRequest_Click" Visible="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-6">
                    <asp:Panel ID="resultsPanel" runat="server">
                        <asp:Label ID="resultsLabel" Text="Results" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <script>
        window.jQuery || document.write('<script src="/Scripts/jquery-3.4.1.slim.js"><\/script>')
    </script>
</asp:Content>
