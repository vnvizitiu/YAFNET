﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="YAF.Pages.Admin.bannedip_import" Codebehind="bannedip_import.ascx.cs" %>

<YAF:PageLinks ID="PageLinks" runat="server" />
<YAF:AdminMenu runat="server">
    <div class="row">
    <div class="col-xl-12">
        <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="HEADER" LocalizedPage="ADMIN_BANNEDIP_IMPORT" /></h1>
    </div>
    </div>
    <div class="row">
        
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="alert alert-warning" role="alert">
                <YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" 
                    LocalizedTag="NOTE" LocalizedPage="ADMIN_BANNEDIP_IMPORT">
                </YAF:LocalizedLabel>
            </div>
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-hand-stop-o fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel3" runat="server" LocalizedTag="HEADER" LocalizedPage="ADMIN_BANNEDIP_IMPORT" />
                </div>
                <div class="card-block">
			        <h4>
                        <YAF:HelpLabel ID="HelpLabel1" runat="server" LocalizedTag="IMPORT_FILE" LocalizedPage="ADMIN_BANNEDIP_IMPORT" />
                    </h4>
			        <p>
			            <input type="file" id="importFile" class="form-control-file" runat="server" />
			        </p>
                </div>
                <div class="card-footer text-lg-center">
				    <asp:LinkButton id="Import" runat="server" CssClass="btn btn-primary" OnClick="Import_OnClick"></asp:LinkButton>
				    <asp:LinkButton id="cancel" runat="server" CssClass="btn btn-secondary" OnClick="Cancel_OnClick"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
