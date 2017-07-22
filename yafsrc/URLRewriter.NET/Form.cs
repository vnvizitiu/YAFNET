namespace Intelligencia.UrlRewriter
{
    using System.Runtime.InteropServices;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using Intelligencia.UrlRewriter.Utilities;

    /// <summary>
    /// Replacement for &lt;asp:form&gt; to handle rewritten form postback.
    /// </summary>
    /// <remarks>
    /// <p>This form should be used for pages that use the url rewriter and have
    /// forms that are posted back.  If you use the normal ASP.NET <see cref="System.Web.UI.HtmlControls.HtmlForm">HtmlForm</see>,
    /// then the postback will not be able to correctly resolve the postback data to the form data.
    /// </p>
    /// <p>This form is a direct replacement for the &lt;asp:form&gt; tag.
    /// </p>
    /// <p>The following code demonstrates the usage of this control.</p>
    /// <code>
    /// &lt;%@ Page language="c#" Codebehind="MyPage.aspx.cs" AutoEventWireup="false" Inherits="MyPage" %&gt;
    /// &lt;%@ Register TagPrefix="url" Namespace="Intelligencia.UrlRewriter" Assembly="Intelligencia.UrlRewriter" %&gt;
    /// &lt;html&gt;
    /// ...
    /// &lt;body&gt;
    /// &lt;url:form id="MyForm" runat="server"&gt;
    /// ...
    /// &lt;/url:form&gt;
    /// &lt;/body&gt;
    /// &lt;/html&gt;
    /// </code>
    /// </remarks>
    [ComVisible(false)]
    [ToolboxData("<{0}:Form runat=server></{0}:Form>")]
    public class Form : HtmlForm
    {
        /// <summary>
        /// Renders children of the form control.
        /// </summary>
        /// <param name="writer">The output writer.</param>
        /// <exclude />
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderChildren(writer);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders attributes.
        /// </summary>
        /// <param name="writer">The output writer.</param>
        /// <exclude />
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            writer.WriteAttribute(Constants.AttrName, this.GetName());
            this.Attributes.Remove(Constants.AttrName);

            writer.WriteAttribute(Constants.AttrMethod, this.GetMethod());
            this.Attributes.Remove(Constants.AttrMethod);

            writer.WriteAttribute(Constants.AttrAction, this.GetAction(), true);
            this.Attributes.Remove(Constants.AttrAction);

            this.Attributes.Render(writer);

            if (this.ID != null)
            {
                writer.WriteAttribute(Constants.AttrID, this.GetID());
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        private string GetID()
        {
            return this.ClientID;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        private string GetName()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <returns></returns>
        private string GetMethod()
        {
            return this.Method;
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <returns></returns>
        private string GetAction()
        {
            return RewriterHttpModule.RawUrl;
        }
    }
}