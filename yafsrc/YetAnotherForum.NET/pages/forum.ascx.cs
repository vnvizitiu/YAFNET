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
    #region Using

    using System;

    using YAF.Classes;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Interfaces;
    using YAF.Utils;

    #endregion

    /// <summary>
    /// The Main Forum Page.
    /// </summary>
    public partial class forum : ForumPage
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "forum" /> class.
        /// </summary>
        public forum()
            : base("DEFAULT")
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            this.PollList.Visible = this.Get<YafBoardSettings>().BoardPollID > 0;
            this.PollList.PollGroupId = this.Get<YafBoardSettings>().BoardPollID;
            this.PollList.BoardId = this.PageContext.Settings.BoardID;

            // Since these controls have EnabledViewState=false, set their visibility on every page load so that this value is not lost on postback.
            // This is important for another reason: these are board settings; values in the view state should have no impact on whether these controls are shown or not.
            this.ShoutBox1.Visible = this.Get<YafBoardSettings>().ShowShoutbox
                                     && this.Get<IPermissions>()
                                            .Check(this.Get<YafBoardSettings>().ShoutboxViewPermissions);
            this.ForumStats.Visible = this.Get<YafBoardSettings>().ShowForumStatistics;
            this.ActiveDiscussions.Visible = this.Get<YafBoardSettings>().ShowActiveDiscussions;

            if (this.IsPostBack)
            {
                return;
            }

            if (this.PageContext.Settings.LockedForum == 0)
            {
                this.PageLinks.AddRoot();
                if (this.PageContext.PageCategoryID != 0)
                {
                    this.PageLinks.AddLink(
                        this.PageContext.PageCategoryName,
                        YafBuildLink.GetLink(ForumPages.forum, "c={0}", this.PageContext.PageCategoryID));
                    this.Welcome.Visible = false;
                }
            }
        }

        #endregion
    }
}