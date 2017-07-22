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
  using System.Data;

  using YAF.Classes.Data;
  using YAF.Core;
  using YAF.Types;
  using YAF.Types.Extensions;
  using YAF.Types.Interfaces;
  using YAF.Utils;

  #endregion

  /// <summary>
  /// The profile your account.
  /// </summary>
  public partial class ProfileYourAccount : BaseUserControl
  {
    #region Methods

    /// <summary>
    /// The page_ load.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
    {
      if (!this.IsPostBack)
      {
        this.BindData();
      }
    }

    /// <summary>
    /// The bind data.
    /// </summary>
    private void BindData()
    {
      DataTable dt = LegacyDb.usergroup_list(this.PageContext.PageUserID);

      if (YafContext.Current.BoardSettings.UseStyledNicks)
      {
        this.Get<IStyleTransform>().DecodeStyleByTable(dt, false);
      }

      this.Groups.DataSource = dt;

      // Bind			
      this.DataBind();

      // TitleUserName.Text = HtmlEncode( userData.Membership.UserName );
      this.AccountEmail.Text = this.PageContext.CurrentUserData.Membership.Email;
      this.Name.Text = this.HtmlEncode(this.PageContext.CurrentUserData.Membership.UserName);
      this.Joined.Text = this.Get<IDateTime>().FormatDateTime(this.PageContext.CurrentUserData.Joined);
      this.NumPosts.Text = "{0:N0}".FormatWith(this.PageContext.CurrentUserData.NumPosts);

      this.DisplayNameHolder.Visible = this.PageContext.BoardSettings.EnableDisplayName;

      if (this.PageContext.BoardSettings.EnableDisplayName)
      {
        this.DisplayName.Text =
          this.HtmlEncode(this.Get<IUserDisplayName>().GetName(this.PageContext.PageUserID));
      }

      string avatarImg = this.Get<IAvatars>().GetAvatarUrlForCurrentUser();

      if (avatarImg.IsSet())
      {
        this.AvatarImage.ImageUrl = avatarImg;
      }
      else
      {
        this.AvatarImage.Visible = false;
      }
    }

    #endregion
  }
}