<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.Admin.pageaccesslist" Codebehind="pageaccesslist.ascx.cs" %>

<%@ Import Namespace="YAF.Types.Interfaces" %>
<YAF:PageLinks runat="server" ID="PageLinks" />
<YAF:AdminMenu runat="server">
    <div class="row">
    <div class="col-xl-12">
        <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_PAGEACCESSLIST" /></h1>
    </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-building fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_PAGEACCESSLIST" />
                </div>
                <div class="card-block">
                    <div class="table-responsive">
                        <table class="table">
		<tr>
		    <thead>
			<th>
				<YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="HEADER"  LocalizedPage="ADMIN_PAGEACCESSLIST" />
			</th>
            <th colspan="2">
				<YAF:LocalizedLabel ID="BoardNameLabel" runat="server" LocalizedTag="BOARDnAME"  LocalizedPage="ADMIN_PAGEACCESSLIST" />
			</th>
            </thead>
		</tr>
		<asp:Repeater ID="List" runat="server" OnItemCommand="List_ItemCommand">
			<ItemTemplate>
				<tr class="post">
					<td>
					    <!-- User Name -->
					  <img alt='<%# this.HtmlEncode(this.Get<YafBoardSettings>().EnableDisplayName ? this.Eval( "DisplayName") : this.Eval( "Name")) %>'
                                    title='<%# this.HtmlEncode(this.Get<YafBoardSettings>().EnableDisplayName ? this.Eval( "DisplayName") : this.Eval( "Name")) %>'
                                    src='<%# this.Get<ITheme>().GetItem("ICONS","USER_BUSINESS") %>' />&nbsp;<%# this.HtmlEncode(this.Get<YafBoardSettings>().EnableDisplayName ? this.Eval("DisplayName") : this.Eval("Name"))%>
					</td>
                    	<td>
                    	 <%# this.HtmlEncode(this.Eval( "BoardName")) %>
                        </td>
                    <td>
                        <span class="pull-right">
						  <YAF:ThemeButton ID="ThemeButtonEdit" CssClass="btn btn-info btn-sm"
                              TitleLocalizedPage="ADMIN_PAGEACCESSLIST" CommandName='edit' CommandArgument='<%# this.Eval( "UserID") %>'
                              TitleLocalizedTag="EDIT"
                              Icon="edit"
                              TextLocalizedTag="EDIT"
                              runat="server">
						  </YAF:ThemeButton>
                            </span>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater>
                            </table></div>
                    </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
