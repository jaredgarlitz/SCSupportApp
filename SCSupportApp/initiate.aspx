<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="initiate.aspx.cs" Inherits="SCSupportApp.initiate" Async="true"%>

<asp:Content ID="tokenGen" ContentPlaceHolderID="genTokenPlaceHolder" runat="server">
    <%--<asp:DropDownList ID="typeSelect" runat="server" />--%>
    <div class="container">
        <asp:DropDownList ID="type" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Accountant" Value="0" />
            <asp:ListItem Text="Client" Value="1" />
        </asp:DropDownList>
        <asp:TextBox ID="Partner" runat="server" placeholder ="Accountant ID" />
        <asp:TextBox ID="Site" runat="server" placeholder="Site #" />
        <asp:TextBox ID="Secret" runat="server" placeholder="API Secret" />
        <asp:Button ID="sendTo" class="btn btn-success btn-sm" Text="Check" runat="server" />
        <asp:Label ID="loadLabel" runat="server" />
        <asp:Label ID="tokenID" runat="server" />
    </div>
    <div class="container-sm">
        <div class="accordion" id="addPunch">
            <%--<div class="card">
                <div class="card-header" id="addPunchHeader">
                    <h2 class="mb-0">
                        <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="false" aria-controls="collapseOne">
                            Add Punch
                        </button>
                    </h2>
                </div>
                <div id="collapseOne" class="collapse" aria-labelledby="addPunchHeader" data-parent="#addPunch">
                    <div class="card-body">
                        This is where we are going to add the fields for the addpunch api.
                    </div>
                </div>
            </div>--%>
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
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getAccrualSchema" runat="server" Text="Get Accrual Schema" />
                            </div>
                            <div class ="col-1">
                                <label id="accrualvalues" runat="server"></label>
                            </div>
                        </div>
                        <p />
                        <div class="row">
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getEmployeesSchema" runat="server" Text="Get Employee Schema" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getEmployees" runat="server" Text="Get Employees"></asp:Button>
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postUpdateEmployees" runat="server" Text="POST Update Employees" />
                            </div>
                        </div>
                        <p />
                        <div class="row":>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeConnectMgrLogin" runat="server" Text="Connect EE/Sup Login" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeDisconnectMangerLogin" runat="server" Text="Disconnect EE/Sup Login" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeSetPassword" runat="server" Text="Set EE Password" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeResetPassword" runat="server" Text="Reset EE Password" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEmployeeUpdatePassword" runat="server" Text="Update EE Password" />
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
                        <asp:Button type="button" class="btn btn-primary btn-sm" ID="getLogins" runat="server" Text="Get Logins"></asp:Button>
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollActivities" runat="server" Text="Get Payroll Activities" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollActivitiesPerPayPeriod" runat="server" Text="Payroll Act. by Period" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getPayrollFormats" runat="server" Text="Get Formats" />
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getRules" runat="server" Text="Get Rules" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getIntegratedSchedulingRule" runat="server" Text="Get Integrated Scheduling Rule" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteExtEmployeeIDRule" runat="server" Text="Delete ExtEmployeeID" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteIntegrationFieldsRule" runat="server" Text="Delete IntegrationFields Rule" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postExtEmployeeIDRule" runat="server" Text="Add ExtEmployeeID" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postIntegratedSchedulingRule" runat="server" Text="Add Int Scheduling" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postIntegrationRule" runat="server" Text="Post Integration Rule" />
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeWorksPlusSchedules" runat="server" Text="Get TWP Schedules"></asp:Button>
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeCards" runat="server" Text="Get Time Cards" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeCardSummary" runat="server" Text="Get Time Card Summary" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="deleteTimeCardLine" runat="server" Text="Delete Time CardLine" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postEditTimeCardLine" runat="server" Text="Edit TimeCard Line" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postTimeCardApproval" runat="server" Text="TimeCard Approval" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAddTimeCardNote" runat="server" Text="Add TimeCard Note" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAddTimeCardPunch" runat="server" Text="Add TimeCard Punch" />
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
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestsbyEEID" runat="server" Text="Get TOR by EE ID" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestCategories" runat="server" Text="Get TOR Categories" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getTimeOffRequestbyEEIDdept" runat="server" Text="Get TOR by EE ID(Dep)" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getSchemaTimeOffRequests" runat="server" Text="Get Schema TOR" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="getSupervisorTimeOffRequests" runat="server" Text="Get Supervisor TOR" />
                            </div>
                        </div>
                        <p />
                        <div class="row">
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postCreateTimeOffRequest" runat="server" Text="Create TOR" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postAcceptTimeOffRequest" runat="server" Text="Accept TOR" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postApproveTimeOffRequest" runat="server" Text="Approve TOR" />
                            </div>
                            <div class="col-1">
                                <asp:button type="button" class="btn btn-primary btn-sm" ID="postCancelTimeOffRequest" runat="server" Text="Cancel TOR" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postRejectTimeOffRequest" runat="server" Text="Reject TOR" />
                            </div>
                            <div class="col-1">
                                <asp:Button type="button" class="btn btn-primary btn-sm" ID="postUnApproveTimeOffRequest" runat="server" Text="Un-Approve TOR" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        window.jQuery || document.write('<script src="/Scripts/jquery-3.4.1.slim.js"><\/script>')
    </script>
</asp:Content>
