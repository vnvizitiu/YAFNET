<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.Admin.eventloggroups" Codebehind="eventloggroups.ascx.cs" %>

<YAF:PageLinks runat="server" ID="PageLinks" />
<YAF:AdminMenu runat="server">
    <div class="row">
    <div class="col-xl-12">
        <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_EVENTLOGGROUPS" /></h1>
    </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-group fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_EVENTLOGGROUPS" />
                </div>
                <div class="card-block">
                    <div class="alert alert-info hidden-sm-up" role="alert">
                            <YAF:LocalizedLabel ID="LocalizedLabel220" runat="server" LocalizedTag="TABLE_RESPONSIVE" LocalizedPage="ADMIN_COMMON" />
                            <span class="pull-right"><i class="fa fa-hand-o-left fa-fw"></i></span>
                        </div><div class="table-responsive">
                        <table class="table">
                            <tr>
                                <thead>
                                <th>
				<YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="GROUPNAME"  LocalizedPage="ADMIN_EVENTLOGGROUPS" />
			</th>
            <th colspan="2">
				<YAF:LocalizedLabel ID="BoardNameLabel" runat="server" LocalizedTag="BOARDNAME"  LocalizedPage="ADMIN_EVENTLOGGROUPS" />
			</th>
                                    </thead>
		</tr>
		<asp:Repeater ID="List" runat="server" OnItemCommand="List_ItemCommand">
			<ItemTemplate>
				<tr class="post">
				    <td>
					    <!-- Group Name -->
					  <i class="fa fa-group fa-fw"></i>&nbsp;<%# this.HtmlEncode(this.Eval("Name"))%>
					</td>
                    	<td>
                    	 <%# this.HtmlEncode(this.Eval( "BoardName")) %>
                        </td>
					<td>
					    <span class="pull-right">
						  <YAF:ThemeButton ID="ThemeButtonEdit" CssClass="btn btn-info btn-sm"
                              TitleLocalizedPage="ADMIN_EVENTLOGGROUPS" TitleLocalizedTag="EDIT"
                              CommandName='edit' CommandArgument='<%# this.Eval( "GroupID") %>' TextLocalizedTag="EDIT"
                              Icon="edit" runat="server"></YAF:ThemeButton>
                            </span>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater></table></div>
                </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
