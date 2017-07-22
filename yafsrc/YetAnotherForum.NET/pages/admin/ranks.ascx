<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.Admin.ranks" Codebehind="ranks.ascx.cs" %>

<%@ Import Namespace="YAF.Core" %>
<%@ Import Namespace="YAF.Types.Flags" %>
<%@ Import Namespace="YAF.Types.Extensions" %>
<YAF:PageLinks runat="server" ID="PageLinks" />
<YAF:AdminMenu runat="server">
    <div class="row">
    <div class="col-xl-12">
        <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_RANKS" /></h1>
    </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-mortar-board fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_RANKS" />
                </div>
                <div class="card-block">
		<asp:Repeater ID="RankList" OnItemCommand="RankList_ItemCommand" runat="server">
			<HeaderTemplate>
			    <div class="table-responsive">
                    <table class="table">
				<tr>
					<td>
						<YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="NAME" LocalizedPage="ADMIN_RANKS" />
                    </td>
					<td>
						&nbsp;
                    </td>
				</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
                    <i class="fa fa-mortar-board fa-fw"></i>&nbsp;
						<%# this.Eval( "Name") %>
					</td>
                    <td class="header2">
						&nbsp;
                    </td>
                 </tr>
                 <tr>
					<td>
                     <YAF:LocalizedLabel ID="HelpLabel6" Visible='<%# this.Eval("Description").ToString().IsSet() %>' runat="server" LocalizedTag="DESCRIPTION" LocalizedPage="ADMIN_EDITGROUP">
                         </YAF:LocalizedLabel>
                          &nbsp;<%# this.Eval("Description").ToString() %>&nbsp;
                    <br />
                    <YAF:LocalizedLabel  ID="HelpLabel12" runat="server" LocalizedTag="PRIORITY" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label11" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "SortOrder" ).ToString()) %>'><%# this.Eval("SortOrder").ToString()%></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="IS_START" LocalizedPage="ADMIN_RANKS" />
                     <asp:Label ID="Label4" runat="server" CssClass='<%# this.GetItemColor(this.Eval( "Flags" ).BinaryAnd(1)) %>'><%# this.GetItemName(this.Eval( "Flags" ).BinaryAnd(1)) %></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel ID="LocalizedLabel5" runat="server" LocalizedTag="IS_LADDER" LocalizedPage="ADMIN_RANKS" />
                    <asp:Label ID="Label1" runat="server" CssClass='<%# this.GetItemColor(this.Eval( "Flags" ).BinaryAnd(RankFlags.Flags.IsLadder)) %>'><%# this.LadderInfo(Container.DataItem) %></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel ID="LocalizedLabel6" runat="server" LocalizedTag="PM_LIMIT" LocalizedPage="ADMIN_RANKS" />
					<asp:Label ID="Label6" runat="server" CssClass='<%# this.GetItemColorString((Convert.ToInt32(this.Eval("PMLimit")) == int.MaxValue) ? "\u221E" : this.Eval("PMLimit").ToString()) %>'><%# ((Convert.ToInt32(this.Eval("PMLimit")) == int.MaxValue) ? "\u221E": this.Eval("PMLimit").ToString())%></asp:Label>&nbsp;|&nbsp;
                    <br />
                    <YAF:LocalizedLabel  ID="HelpLabel10" runat="server" LocalizedTag="ALBUM_NUMBER" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label9" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "UsrAlbums" ).ToString()) %>'><%# this.Eval("UsrAlbums").ToString()%></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel  ID="HelpLabel11" runat="server" LocalizedTag="IMAGES_NUMBER" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label10" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "UsrAlbumImages" ).ToString()) %>'><%# this.Eval("UsrAlbumImages").ToString()%></asp:Label>&nbsp;|&nbsp;
                    <br />
                    <YAF:LocalizedLabel  ID="HelpLabel13" runat="server" LocalizedTag="STYLE" LocalizedPage="ADMIN_EDITGROUP" />&nbsp;
                    <asp:Label ID="Label12" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "Style" ).ToString()) %>'><%# this.Eval("Style").ToString().IsSet() && (this.Eval("Style").ToString().Trim().Length > 0) ? "" : this.GetItemName(false)%></asp:Label>&nbsp;
                    <YAF:RoleRankStyles ID="RoleRankStylesRanks" RawStyles='<%# this.Eval( "Style" ).ToString() %>' runat="server" />
                    <br />
					<YAF:LocalizedLabel ID="HelpLabel7" runat="server" LocalizedTag="SIGNATURE_LENGTH" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label5" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "UsrSigChars" ).ToString()) %>'><%# this.Eval("UsrSigChars").ToString().IsSet() ? this.Eval("UsrSigChars").ToString() : this.GetItemName(false) %></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel ID="HelpLabel8" runat="server" LocalizedTag="SIG_BBCODES" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label7" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "UsrSigBBCodes" ).ToString()) %>'><%# this.Eval("UsrSigBBCodes").ToString().IsSet() ? this.Eval("UsrSigBBCodes").ToString() : this.GetItemName(false) %></asp:Label>&nbsp;|&nbsp;
                    <YAF:LocalizedLabel ID="HelpLabel9" runat="server"  LocalizedTag="SIG_HTML" LocalizedPage="ADMIN_EDITGROUP" />
                    <asp:Label ID="Label8" runat="server" CssClass='<%# this.GetItemColorString(this.Eval( "UsrSigHTMLTags" ).ToString()) %>'><%#  this.Eval("UsrSigHTMLTags").ToString().IsSet() ? this.Eval("UsrSigHTMLTags").ToString() : this.GetItemName(false)%></asp:Label>&nbsp;|&nbsp;
                    </td>
					<td>
					    <span class="pull-right">
					    <YAF:ThemeButton ID="ThemeButtonEdit" CssClass="btn btn-info btn-sm"
                            CommandName='edit' CommandArgument='<%# this.Eval( "RankID") %>'
                            TitleLocalizedTag="EDIT"
                            Icon="edit"
                            TextLocalizedTag="EDIT"
                            runat="server">
					    </YAF:ThemeButton>
						<YAF:ThemeButton ID="ThemeButtonDelete" CssClass="btn btn-danger btn-sm"
                                    CommandName='delete' CommandArgument='<%# this.Eval( "RankID") %>'
                                    TitleLocalizedTag="DELETE"
                                    Icon="trash"
                                    TextLocalizedTag="DELETE"
                                    OnLoad="Delete_Load"  runat="server">
                                </YAF:ThemeButton>
                            </span>
					</td>
				</tr>
			</ItemTemplate>
            <FooterTemplate>
                </table></div>
            </FooterTemplate>
		</asp:Repeater>
                </div>
                <div class="card-footer text-lg-center">
				   <asp:LinkButton ID="NewRank" runat="server" OnClick="NewRank_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
