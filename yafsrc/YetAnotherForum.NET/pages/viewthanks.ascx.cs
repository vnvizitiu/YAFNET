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
  using System.Data;

  using YAF.Classes.Data;
  using YAF.Controls;
  using YAF.Core;
  using YAF.Types;
  using YAF.Types.Constants;
  using YAF.Types.Extensions;
  using YAF.Types.Interfaces;
  using YAF.Utilities;
  using YAF.Utils;

  #endregion

  /// <summary>
  /// Summary description for view thanks.
  /// </summary>
  public partial class ViewThanks : ForumPage
  {
    #region Constructors and Destructors

    /// <summary>
    ///   Initializes a new instance of the <see cref = "ViewThanks" /> class.
    ///   Initializes a new instance of the viewthanks class.
    /// </summary>
    public ViewThanks()
      : base("VIEWTHANKS")
    {
    }

    #endregion

    /* Public Methods */
    #region Public Methods

    /// <summary>
    /// Initializes the ThanksList controls.
    /// </summary>
    /// <param name="thanksList">
    /// The control which is being initialized.
    /// </param>
    /// <param name="currentMode">
    /// the CurrentMode property of the control.
    /// </param>
    /// <param name="userID">
    /// the UserID of the control.
    /// </param>
    /// <param name="thanksInfo">
    /// The Dataview for the control's data.
    /// </param>
    public void InitializeThanksList([NotNull] ViewThanksList thanksList, ThanksListMode currentMode, int userID, [NotNull] DataTable thanksInfo)
    {
      thanksList.CurrentMode = currentMode;
      thanksList.UserID = userID;
      thanksList.ThanksInfo = thanksInfo;
    }

    #endregion

    /* Methods */
    #region Methods

    /// <summary>
    /// The On PreRender event.
    /// </summary>
    /// <param name="e">
    /// the Event Arguments
    /// </param>
    protected override void OnPreRender([NotNull] EventArgs e)
    {
      // setup jQuery and Jquery Ui Tabs.
        YafContext.Current.PageElements.RegisterJsBlock(
            "ThanksTabsJs",
            JavaScriptBlocks.BootstrapTabsLoadJs(this.ThanksTabs.ClientID, this.hidLastTab.ClientID));

      base.OnPreRender(e);
    }

      /// <summary>
      /// The Page_ Load Event.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The e.
      /// </param>
      protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
      {
          var userID = (int)Security.StringToLongOrRedirect(this.Request.QueryString.GetFirstOrDefault("u"));

          if (!this.IsPostBack)
          {
              var displayName = this.PageContext.BoardSettings.EnableDisplayName
                                    ? UserMembershipHelper.GetDisplayNameFromID(userID)
                                    : UserMembershipHelper.GetUserNameFromID(userID);
              this.PageLinks.Clear();
              this.PageLinks.AddLink(this.PageContext.BoardSettings.Name, YafBuildLink.GetLink(ForumPages.forum));
              this.PageLinks.AddLink(
                  displayName,
                  YafBuildLink.GetLink(ForumPages.profile, "u={0}", userID, displayName));
              this.PageLinks.AddLink(this.GetText("TITLE"), string.Empty);
          }

          DataTable thanksInfo = LegacyDb.user_viewallthanks(userID, this.PageContext.PageUserID);
          this.InitializeThanksList(this.ThanksFromList, ThanksListMode.FromUser, userID, thanksInfo);
          this.InitializeThanksList(this.ThanksToList, ThanksListMode.ToUser, userID, thanksInfo);
      }

      #endregion
  }
}