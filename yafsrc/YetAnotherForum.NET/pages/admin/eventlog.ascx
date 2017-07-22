<%@ Control Language="c#" AutoEventWireup="True" EnableViewState="true" Inherits="YAF.Pages.Admin.eventlog"
    CodeBehind="eventlog.ascx.cs" %>

<%@ Import Namespace="YAF.Types.Interfaces" %>
<%@ Import Namespace="YAF.Types.Extensions" %>

<YAF:PageLinks runat="server" ID="PageLinks" />

<script type="text/javascript">
function toggleItem(detailId)
{
    var show = '<i class="fa fa-toggle-down fa-fw"></i>&nbsp;<%# this.GetText("ADMIN_EVENTLOG", "SHOW")%>';
    var hide = '<i class="fa fa-toggle-up fa-fw"></i>&nbsp;<%# this.GetText("ADMIN_EVENTLOG", "HIDE")%>';

    jQuery('#Show'+ detailId).html($('#Show'+ detailId).html() == show ? hide : show);

	jQuery('#eventDetails' + detailId).slideToggle('slow');

	return false;

}
</script>

<YAF:AdminMenu runat="server" ID="AdminMenu1">
    <div class="row">
    <div class="col-xl-12">
        <h1><YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_EVENTLOG" /></h1>
    </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-info-outline">
                <div class="card-header card-info">
                    <i class="fa fa-book fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel7" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_EVENTLOG" />
            </div>
                <div class="card-block">
                    <h4>
                        <YAF:HelpLabel ID="SinceDateLabel" runat="server" LocalizedPage="ADMIN_EVENTLOG" LocalizedTag="SINCEDATE" />
                    </h4>
                    <div class="form-group">
                        <div class='input-group date datepickerinput'>
                            <span class="input-group-addon">
                                <span class="fa fa-calendar fa-fw"></span>
                            </span>
                            <asp:TextBox ID="SinceDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                   </div>
                   <hr />
                    <h4>
                <YAF:HelpLabel ID="ToDateLabel" runat="server" LocalizedPage="ADMIN_EVENTLOG" LocalizedTag="TODATE" />
                        </h4>
                    <div class="form-group">
                        <div class='input-group date datepickerinput'>
                            <span class="input-group-addon">
                                <span class="fa fa-calendar fa-fw"></span>
                            </span>
                            <asp:TextBox ID="ToDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                   </div>
                   <hr />
            <h4>
                <YAF:HelpLabel ID="HelpLabel1" runat="server" LocalizedPage="ADMIN_EVENTLOG" LocalizedTag="TYPES" />
            </h4>
            <p>
                <asp:DropDownList ID="Types" runat="server" CssClass="custom-select"></asp:DropDownList>
            </p>
        </div>
                <div class="card-footer text-lg-center">
                <YAF:ThemeButton ID="ApplyButton" CssClass="btn btn-primary" OnClick="ApplyButton_Click"
                    TextLocalizedPage="ADMIN_EVENTLOG" TextLocalizedTag="APPLY" Icon="check" runat="server"></YAF:ThemeButton>
            </div>
           </div>
         </div>
    </div>
        <div class="row">
        <div class="col-xl-12">
             <YAF:Pager ID="PagerTop" runat="server" OnPageChange="PagerTop_PageChange" />
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-book fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel8" runat="server" LocalizedTag="TITLE" LocalizedPage="ADMIN_EVENTLOG" />
            </div>
                <div class="card-block">
        <asp:Repeater runat="server" ID="List">
            <HeaderTemplate>
                <div class="alert alert-info hidden-sm-up" role="alert">
                            <YAF:LocalizedLabel ID="LocalizedLabel220" runat="server" LocalizedTag="TABLE_RESPONSIVE" LocalizedPage="ADMIN_COMMON" />
                            <span class="pull-right"><i class="fa fa-hand-o-left fa-fw"></i></span>
                        </div><div class="table-responsive"><table class="table">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td colspan="5">
                      <div onclick="javascript:toggleItem(<%# this.Eval("EventLogID") %>);">
                        <div class="table-responsive"><table class="table">
                          <tr class="table-<%# this.EventCssClass(Container.DataItem) %>">
                            <td>
                                <a name="event<%# this.Eval("EventLogID")%>" ></a>
                              <asp:HiddenField ID="EventTypeID" Value='<%# this.Eval("Type")%>' runat="server"/>
                            </td>
                              <td>
                                <strong><YAF:LocalizedLabel ID="LocalizedLabel3" runat="server" LocalizedTag="NAME" LocalizedPage="ADMIN_EVENTLOG" />:</strong>&nbsp;
                                <%# this.HtmlEncode(this.Eval( "UserName")).IsSet() ? this.HtmlEncode(this.Eval( "UserName")) : "N/A" %>&nbsp;
                              <strong><YAF:LocalizedLabel ID="LocalizedLabel6" runat="server" LocalizedTag="TYPE" LocalizedPage="ADMIN_EVENTLOG" />:</strong>&nbsp;
                                <%# this.HtmlEncode(this.Eval( "Name")).IsSet() ? this.HtmlEncode(this.Eval( "Name")) : "N/A" %>&nbsp;

                               <strong><YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="TIME" LocalizedPage="ADMIN_EVENTLOG" />:</strong>&nbsp;
                              <%# this.Get<IDateTime>().FormatDateTimeTopic(Container.DataItemToField<DateTime>("EventTime")) %>&nbsp;

                              <strong><YAF:LocalizedLabel ID="LocalizedLabel5" runat="server" LocalizedTag="SOURCE" LocalizedPage="ADMIN_EVENTLOG" />:</strong>&nbsp;
                              <%# this.HtmlEncode(this.Eval( "Source")).IsSet() ? this.HtmlEncode(this.Eval( "Source")) : "N/A" %>
                              <td>
                                <span class="pull-right">
                                    <a class="showEventItem btn btn-info btn-sm" href="#event<%# this.Eval("EventLogID")%>" id="Show<%# this.Eval("EventLogID") %>"><i class="fa fa-toggle-down fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="SHOW" LocalizedPage="ADMIN_EVENTLOG" /></a>&nbsp;&nbsp;
                                    <asp:LinkButton runat="server" OnLoad="Delete_Load" CssClass="deleteEventItem btn btn-danger btn-sm" CommandName="delete" CommandArgument='<%# this.Eval( "EventLogID") %>'>
                                         <i class="fa fa-trash fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="DELETE" />
                                    </asp:LinkButton>
                                </span>
                            </td>
                          </tr>
                        </table>
                            </div>
                      </div>
                      <div class="EventDetails" id="eventDetails<%# this.Eval("EventLogID") %>" style="display: none;margin:0;padding:0;">
                            <pre class="pre-scrollable">
                                <code>
                                    <%# this.HtmlEncode(this.Eval( "Description")) %>
                                </code>
                            </pre>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table></div>
                </div>
                <div class="card-footer text-lg-center">
                        <YAF:ThemeButton runat="server" Visible="<%# this.List.Items.Count > 0 %>" OnLoad="DeleteAll_Load" CssClass="btn btn-primary"
                            Icon="trash" OnClick="DeleteAll_Click" TextLocalizedPage="ADMIN_EVENTLOG" TextLocalizedTag="DELETE_ALLOWED">
                        </YAF:ThemeButton>
                </div>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    <YAF:Pager ID="PagerBottom" runat="server" LinkedPager="PagerTop" />
                </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
