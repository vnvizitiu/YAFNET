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

namespace YAF.Pages
{
  // YAF.Pages
  #region Using

  using System;
  using System.Web.Security;

  using YAF.Controls;
  using YAF.Core;
  using YAF.Types;
  using YAF.Types.Constants;
  using YAF.Types.Extensions;
  using YAF.Types.Interfaces;
  using YAF.Utils;

  #endregion

  /// <summary>
  /// Class to communicate in XMPP.
  /// </summary>
  public partial class im_xmpp : ForumPage
  {
    #region Constructors and Destructors

    /// <summary>
    ///   Initializes a new instance of the <see cref = "im_xmpp" /> class.
    /// </summary>
    public im_xmpp()
      : base("IM_XMPP")
    {
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets UserID.
    /// </summary>
    public int UserID
    {
      get
      {
        return (int)Security.StringToLongOrRedirect(this.Request.QueryString.GetFirstOrDefault("u"));
      }
    }

    #endregion

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
      if (this.User == null)
      {
        YafBuildLink.AccessDenied();
      }

      if (!this.IsPostBack)
      {
        // get user data...
        MembershipUser userHe = UserMembershipHelper.GetMembershipUserById(this.UserID);

        string displayNameHe = UserMembershipHelper.GetDisplayNameFromID(this.UserID);

        this.PageLinks.AddLink(this.PageContext.BoardSettings.Name, YafBuildLink.GetLink(ForumPages.forum));
          this.PageLinks.AddLink(
              this.PageContext.BoardSettings.EnableDisplayName ? displayNameHe : userHe.UserName,
              YafBuildLink.GetLink(
                  ForumPages.profile,
                  "u={0}&name={1}",
                  this.UserID,
                  this.PageContext.BoardSettings.EnableDisplayName ? displayNameHe : userHe.UserName));
        this.PageLinks.AddLink(this.GetText("TITLE"), string.Empty);

        if (this.UserID == this.PageContext.PageUserID)
        {
          this.NotifyLabel.Text = this.GetText("SERVERYOU");
        }
        else
        {
          if (userHe == null)
          {
            YafBuildLink.AccessDenied( /*No such user exists*/);
          }

          // Data for current page user
          MembershipUser userMe = UserMembershipHelper.GetMembershipUserById(this.PageContext.PageUserID);

          // get full user data...
          var userDataHe = new CombinedUserDataHelper(userHe, this.UserID);
          var userDataMe = new CombinedUserDataHelper(userMe, this.PageContext.PageUserID);

          string serverHe = userDataHe.Profile.XMPP.Substring(userDataHe.Profile.XMPP.IndexOf("@") + 1).Trim();
          string serverMe = userDataMe.Profile.XMPP.Substring(userDataMe.Profile.XMPP.IndexOf("@") + 1).Trim();
          if (serverMe == serverHe)
          {
            this.NotifyLabel.Text = this.GetTextFormatted("SERVERSAME", userDataHe.Profile.XMPP);
          }
          else
          {
            this.NotifyLabel.Text = this.GetTextFormatted("SERVEROTHER", "http://" + serverHe);
          }
        }
      }
    }

    #endregion
  }
}