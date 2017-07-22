<%@ Control Language="c#" AutoEventWireup="True"
    Inherits="YAF.Pages.Admin.editcategory" Codebehind="editcategory.ascx.cs" %>

<YAF:PageLinks ID="PageLinks" runat="server" />
<YAF:AdminMenu runat="server">
    <div class="row">
        <div class="col-xl-12">
            <h1><YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="HEADER" LocalizedPage="ADMIN_EDITCATEGORY" />&nbsp;<asp:Label ID="Label1" runat="server"></asp:Label></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3 card-outline-primary">
                <div class="card-header card-primary">
                    <i class="fa fa-comments-o fa-fw"></i>&nbsp;<YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" LocalizedTag="HEADER" LocalizedPage="ADMIN_EDITCATEGORY" />&nbsp;<asp:Label ID="CategoryNameTitle" runat="server"></asp:Label>
                </div>
                <div class="card-block">
			<h4>
			  <YAF:HelpLabel ID="HelpLabel1" runat="server" LocalizedTag="CATEGORY_NAME" LocalizedPage="ADMIN_EDITCATEGORY" />
			</h4>
			<p>
			<asp:TextBox ID="Name" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox></p><hr />
		  <h4>
			  <YAF:HelpLabel ID="HelpLabel2" runat="server" LocalizedTag="CATEGORY_IMAGE" LocalizedPage="ADMIN_EDITCATEGORY" />
			</h4>
			<p>
				<img align="middle" alt="Preview" runat="server" id="Preview" />
                <asp:DropDownList ID="CategoryImages" runat="server" CssClass="custom-select" />
		</p><hr />
		   <h4>
			  <YAF:HelpLabel ID="HelpLabel3" runat="server" LocalizedTag="SORT_ORDER" LocalizedPage="ADMIN_EDITCATEGORY" />
			</h4>
			<p>
			<asp:TextBox ID="SortOrder" runat="server" MaxLength="5" CssClass="form-control" TextMode="Number"></asp:TextBox></p>
                </div>
                <div class="card-footer text-lg-center">
			  <asp:LinkButton ID="Save" runat="server" OnClick="Save_Click" CssClass="btn btn-primary"></asp:LinkButton>
			  <asp:LinkButton ID="Cancel" runat="server" OnClick="Cancel_Click" CssClass="btn btn-secondary"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</YAF:AdminMenu>
<YAF:SmartScroller ID="SmartScroller1" runat="server" />
