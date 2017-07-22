﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="YAF.Pages.Admin.restartapp" Codebehind="restartapp.ascx.cs" %>

<YAF:PageLinks runat="server" ID="PageLinks" />
<YAF:AdminMenu ID="adminmenu1" runat="server">
    <div class="row">
        <div class="col-xl-12">
            <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_RESTARTAPP" /></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-refresh fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel3" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_RESTARTAPP" />
                </div>
                <div class="card-block text-center">
                    <p class="card-text">
                        <YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="INFO" LocalizedPage="ADMIN_RESTARTAPP" />
                    </p>
                </div>
                <div class="card-footer text-center">
                    <asp:LinkButton ID="RestartApp" runat="server" CssClass="btn btn-primary" OnClick="RestartApp_Click">
                        <i class="fa fa-refresh fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_RESTARTAPP" />
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
