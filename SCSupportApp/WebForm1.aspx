<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SCSupportApp.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="genTokenPlaceHolder" runat="server">
    <h1>Use this to test sendgrid</h1>
    <div class="container">
        <div class="row">
            <input type="email" placeholder="Enter Email" required/>
            <button type="submit" class="btn btn-success">Send Email</button>
        </div>
    </div>
</asp:Content>
