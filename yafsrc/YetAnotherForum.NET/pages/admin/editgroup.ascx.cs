/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2017 Ingo Herbote
 * http://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Pages.Admin
{
    #region Using

    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    using YAF.Classes.Data;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Core.Extensions;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Flags;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    /// Interface for creating or editing user roles/groups.
    /// </summary>
    public partial class editgroup : AdminPage
    {
        #region Methods

        /// <summary>
        /// Gets or sets the access masks list.
        /// </summary>
        public DataTable AccessMasksList { get; set; }

        /// <summary>
        /// Handles databinding event of initial access maks dropdown control.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void BindData_AccessMaskID([NotNull] object sender, [NotNull] EventArgs e)
        {
            // We don't change access masks if it's a guest
            if (this.IsGuestX.Checked)
            {
                return;
            }

            // get sender object as dropdown list
            var c = (DropDownList)sender;

            // list all access masks as data source
            c.DataSource = this.AccessMasksList;

            // set value and text field names
            c.DataValueField = "AccessMaskID";
            c.DataTextField = "Name";
        }

        /// <summary>
        /// Handles click on cancel button.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Cancel_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            // go back to roles administration
            YafBuildLink.Redirect(ForumPages.admin_groups);
        }

        /// <summary>
        /// Creates page links for this page.
        /// </summary>
        protected override void CreatePageLinks()
        {
            // forum index
            this.PageLinks.AddLink(this.PageContext.BoardSettings.Name, YafBuildLink.GetLink(ForumPages.forum));

            // admin index
            this.PageLinks.AddLink(this.GetText("ADMIN_ADMIN", "Administration"), YafBuildLink.GetLink(ForumPages.admin_admin));

            this.PageLinks.AddLink(this.GetText("ADMIN_GROUPS", "TITLE"), YafBuildLink.GetLink(ForumPages.admin_groups));

            // current page label (no link)
            this.PageLinks.AddLink(this.GetText("ADMIN_EDITGROUP", "TITLE"), string.Empty);

            this.Page.Header.Title = "{0} - {1} - {2}".FormatWith(
               this.GetText("ADMIN_ADMIN", "Administration"),
               this.GetText("ADMIN_GROUPS", "TITLE"),
               this.GetText("ADMIN_EDITGROUP", "TITLE"));
        }

        /// <summary>
        /// Handles page load event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            // this needs to be done just once, not during postbacks
            if (this.IsPostBack)
            {
                return;
            }

            // create page links
            this.CreatePageLinks();

            this.Save.Text = "<i class=\"fa fa-floppy-o fa-fw\"></i>&nbsp;{0}".FormatWith(this.GetText("SAVE"));
            this.Cancel.Text = "<i class=\"fa fa-remove fa-fw\"></i>&nbsp;{0}".FormatWith(this.GetText("CANCEL"));

            // bind data
            this.BindData();

            // is this editing of existing role or creation of new one?
            if (this.Request.QueryString.GetFirstOrDefault("i") == null)
            {
                return;
            }

            // we are not creating new role
            this.NewGroupRow.Visible = false;

            // get data about edited role
            using (
                var dt = this.GetRepository<Group>()
                    .List(
                        boardId: this.PageContext.PageBoardID,
                        groupID: this.Request.QueryString.GetFirstOrDefaultAs<int>("i")))
            {
                // get it as row
                var row = dt.Rows[0];

                // get role flags
                var flags = new GroupFlags(row["Flags"]);

                // set controls to role values
                this.Name.Text = (string)row["Name"];

                this.IsAdminX.Checked = flags.IsAdmin;
                this.IsAdminX.Enabled = !flags.IsGuest;

                this.IsStartX.Checked = flags.IsStart;
                this.IsStartX.Enabled = !flags.IsGuest;

                this.IsModeratorX.Checked = flags.IsModerator;
                this.IsModeratorX.Enabled = !flags.IsGuest;

                this.PMLimit.Text = row["PMLimit"].ToString();
                this.PMLimit.Enabled = !flags.IsGuest;

                this.StyleTextBox.Text = row["Style"].ToString();

                this.Priority.Text = row["SortOrder"].ToString();

                this.UsrAlbums.Text = row["UsrAlbums"].ToString();
                this.UsrAlbums.Enabled = !flags.IsGuest;

                this.UsrAlbumImages.Text = row["UsrAlbumImages"].ToString();
                this.UsrAlbumImages.Enabled = !flags.IsGuest;

                this.UsrSigChars.Text = row["UsrSigChars"].ToString();
                this.UsrSigChars.Enabled = !flags.IsGuest;

                this.UsrSigBBCodes.Text = row["UsrSigBBCodes"].ToString();
                this.UsrSigBBCodes.Enabled = !flags.IsGuest;

                this.UsrSigHTMLTags.Text = row["UsrSigHTMLTags"].ToString();
                this.UsrSigHTMLTags.Enabled = !flags.IsGuest;

                this.Description.Text = row["Description"].ToString();

                this.IsGuestX.Checked = flags.IsGuest;

                // IsGuest flag can be set for only one role. if it isn't for this, disable that row
                if (flags.IsGuest)
                {
                    this.IsGuestTR.Visible = true;
                    this.IsGuestX.Enabled = !flags.IsGuest;
                    this.AccessList.Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles click on save button.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Save_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (!ValidationHelper.IsValidInt(this.PMLimit.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITGROUP", "MSG_VALID_NUMBER"));
                return;
            }

            if (!ValidationHelper.IsValidInt(this.Priority.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITGROUP", "MSG_INTEGER"));
                return;
            }

            if (!ValidationHelper.IsValidInt(this.UsrAlbums.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITGROUP", "MSG_ALBUM_NUMBER"));
                return;
            }

            if (!ValidationHelper.IsValidInt(this.UsrSigChars.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITGROUP", "MSG_SIG_NUMBER"));
                return;
            }

            if (!ValidationHelper.IsValidInt(this.UsrAlbumImages.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITGROUP", "MSG_TOTAL_NUMBER"));
                return;
            }

            // Role
            long roleID = 0;

            // get role ID from page's parameter
            if (this.Request.QueryString.GetFirstOrDefault("i") != null)
            {
                roleID = long.Parse(this.Request.QueryString.GetFirstOrDefault("i"));
            }

            // get new and old name
            var roleName = this.Name.Text.Trim();
            var oldRoleName = string.Empty;

            // if we are editing exising role, get it's original name
            if (roleID != 0)
            {
                // get the current role name in the DB
                using (var dt = this.GetRepository<Group>().List(boardId: this.PageContext.PageBoardID))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        oldRoleName = row["Name"].ToString();
                    }
                }
            }

            // save role and get its ID if it's new (if it's old role, we get it anyway)
            roleID = LegacyDb.group_save(
              roleID,
              this.PageContext.PageBoardID,
              roleName,
              this.IsAdminX.Checked,
              this.IsGuestX.Checked,
              this.IsStartX.Checked,
              this.IsModeratorX.Checked,
              this.AccessMaskID.SelectedValue,
              this.PMLimit.Text.Trim(),
              this.StyleTextBox.Text.Trim(),
              this.Priority.Text.Trim(),
              this.Description.Text,
              this.UsrSigChars.Text,
              this.UsrSigBBCodes.Text,
              this.UsrSigHTMLTags.Text,
              this.UsrAlbums.Text.Trim(),
              this.UsrAlbumImages.Text.Trim());

            // empty out access table(s)
            this.GetRepository<Active>().DeleteAll();
            this.GetRepository<ActiveAccess>().DeleteAll();

            // see if need to rename an existing role...
            if (oldRoleName.IsSet() && roleName != oldRoleName && RoleMembershipHelper.RoleExists(oldRoleName) && !RoleMembershipHelper.RoleExists(roleName) && !this.IsGuestX.Checked)
            {
                // transfer users in addition to changing the name of the role...
                var users = this.Get<RoleProvider>().GetUsersInRole(oldRoleName);

                // delete the old role...
                RoleMembershipHelper.DeleteRole(oldRoleName, false);

                // create new role...
                RoleMembershipHelper.CreateRole(roleName);

                if (users.Any())
                {
                    // put users into new role...
                    this.Get<RoleProvider>().AddUsersToRoles(users, new[] { roleName });
                }
            }
            else if (!RoleMembershipHelper.RoleExists(roleName) && !this.IsGuestX.Checked)
            {
                // if role doesn't exist in provider's data source, create it

                // simply create it
                RoleMembershipHelper.CreateRole(roleName);
            }

            // Access masks for a newly created or an existing role
            if (this.Request.QueryString.GetFirstOrDefault("i") != null)
            {
                    // go trhough all forums
                    for (var i = 0; i < this.AccessList.Items.Count; i++)
                    {
                        // get current repeater item
                        var item = this.AccessList.Items[i];

                        // get forum ID
                        var forumID = int.Parse(item.FindControlAs<Label>("ForumID").Text);

                        // save forum access maks for this role
                        LegacyDb.forumaccess_save(
                            forumID,
                            roleID,
                            item.FindControlAs<DropDownList>("AccessmaskID").SelectedValue);
                    }

                YafBuildLink.Redirect(ForumPages.admin_groups);
            }

            // remove caching in case something got updated...
            this.Get<IDataCache>().Remove(Constants.Cache.ForumModerators);

            // Clearing cache with old permissions data...
            this.Get<IDataCache>().Remove(k => k.StartsWith(Constants.Cache.ActiveUserLazyData.FormatWith(string.Empty)));

            // Clear Styling Caching
            this.Get<IDataCache>().Remove(Constants.Cache.GroupRankStyles);

            // Done, redirect to role editing page
            YafBuildLink.Redirect(ForumPages.admin_editgroup, "i={0}", roleID);
        }

        /// <summary>
        /// Handles pre-render event of each forum's access mask dropdown.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void SetDropDownIndex([NotNull] object sender, [NotNull] EventArgs e)
        {
            // get dropdown which raised this event
            var list = (DropDownList)sender;

            // select value from the list
            var item = list.Items.FindByValue(list.Attributes["value"]);

            // verify something was found...
            if (item != null)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Bind data for this control.
        /// </summary>
        private void BindData()
        {
            // set datasource of access list (list of forums and role's access masks) if we are editing existing mask
            if (this.Request.QueryString.GetFirstOrDefault("i") != null)
            {
                this.AccessList.DataSource = LegacyDb.forumaccess_group(this.Request.QueryString.GetFirstOrDefault("i"));
            }

            this.AccessMasksList = this.GetRepository<AccessMask>().List();

            // bind data to controls
            this.DataBind();
        }

        #endregion
    }
}