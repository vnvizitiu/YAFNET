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

namespace YAF.Controls
{
    #region Using

    using System;
    using System.Linq;
    using System.Web.UI.WebControls;

    using YAF.Core;
    using YAF.Core.Extensions;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.EventProxies;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Data;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    /// The edit users groups.
    /// </summary>
    public partial class EditUsersGroups : BaseUserControl
    {
        #region Properties

        /// <summary>
        ///   Gets user ID of edited user.
        /// </summary>
        protected int CurrentUserID
        {
            get
            {
                return this.PageContext.QueryIDs["u"].ToType<int>();
            }
        }

        #endregion

        #region Methods

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
            // redirect to user admin page.
            YafBuildLink.Redirect(ForumPages.admin_users);
        }

        /// <summary>
        /// Checks if user is member of role or not depending on value of parameter.
        /// </summary>
        /// <param name="o">
        /// Parameter if 0, user is not member of a role.
        /// </param>
        /// <returns>
        /// True if user is member of role (o &gt; 0), false otherwise.
        /// </returns>
        protected bool IsMember([NotNull] object o)
        {
            return long.Parse(o.ToString()) > 0;
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
            this.PageContext.QueryIDs = new QueryStringIDHelper("u", true);

            // this needs to be done just once, not during postbacks
            if (this.IsPostBack)
            {
                return;
            }

            this.Save.Text = "<i class=\"fa fa-floppy-o fa-fw\"></i>&nbsp;{0}".FormatWith(this.GetText("COMMON", "SAVE"));

            // bind data
            this.BindData();
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
            // go through all roles displayed on page
            for (var i = 0; i < this.UserGroups.Items.Count; i++)
            {
                // get current item
                var item = this.UserGroups.Items[i];

                // get role ID from it
                var roleID = int.Parse(((Label)item.FindControl("GroupID")).Text);

                // get role name
                var roleName = this.GetRepository<Group>().ListTyped(boardId: this.PageContext.PageBoardID, groupID: roleID).FirstOrDefault().Name;

                // is user supposed to be in that role?
                var isChecked = ((CheckBox)item.FindControl("GroupMember")).Checked;

                // save user in role
                this.Get<IDbFunction>().Query.usergroup_save(this.CurrentUserID, roleID, isChecked);

                // empty out access table(s)
                this.GetRepository<Active>().DeleteAll();
                this.GetRepository<ActiveAccess>().DeleteAll();

                // update roles if this user isn't the guest
                if (UserMembershipHelper.IsGuestUser(this.CurrentUserID))
                {
                    continue;
                }

                // get user's name
                var userName = UserMembershipHelper.GetUserNameFromID(this.CurrentUserID);

                // add/remove user from roles in membership provider
                if (isChecked && !RoleMembershipHelper.IsUserInRole(userName, roleName))
                {
                    RoleMembershipHelper.AddUserToRole(userName, roleName);
                }
                else if (!isChecked && RoleMembershipHelper.IsUserInRole(userName, roleName))
                {
                    RoleMembershipHelper.RemoveUserFromRole(userName, roleName);
                }

                // Clearing cache with old permisssions data...
                this.Get<IDataCache>().Remove(Constants.Cache.ActiveUserLazyData.FormatWith(this.CurrentUserID));
            }

            // update forum moderators cache just in case something was changed...
            this.Get<IDataCache>().Remove(Constants.Cache.ForumModerators);

            // clear the cache for this user...
            this.Get<IRaiseEvent>().Raise(new UpdateUserEvent(this.CurrentUserID));

            this.BindData();
        }

        /// <summary>
        /// Bind data for this control.
        /// </summary>
        private void BindData()
        {
            // get user roles
            this.UserGroups.DataSource = this.Get<IDbFunction>().GetAsDataTable(cdb => cdb.group_member(this.PageContext.PageBoardID, this.CurrentUserID));

            // bind data to controls
            this.DataBind();
        }

        #endregion
    }
}