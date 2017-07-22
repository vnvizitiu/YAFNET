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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using YAF.Classes;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Utils;

    #endregion

    /// <summary>
    /// The Local Avatar Page.
    /// </summary>
    public partial class avatar : ForumPage
    {
        #region Constants and Fields

        /// <summary>
        ///   The page size.
        /// </summary>
        public int Pagesize = 20;

        /// <summary>
        ///   The title.
        /// </summary>
        protected Label title;

        /// <summary>
        ///   The return user id.
        /// </summary>
        private int returnUserID;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "avatar" /> class.
        /// </summary>
        public avatar()
            : base("AVATAR")
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Page Number
        /// </summary>
        public int Pagenum { get; set; }

        /// <summary>
        ///   Gets or sets CurrentDirectory.
        /// </summary>
        protected string CurrentDirectory
        {
            get
            {
                return this.ViewState["CurrentDir"] != null ? (string)this.ViewState["CurrentDir"] : string.Empty;
            }

            set
            {
                this.ViewState["CurrentDir"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the Bind event of the Directories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataListItemEventArgs"/> instance containing the event data.</param>
        public void Directories_Bind([NotNull] object sender, [NotNull] DataListItemEventArgs e)
        {
            var directory = string.Concat(YafForumInfo.ForumClientFileRoot, YafBoardFolders.Current.Avatars, "/");

            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var dirName = e.Item.FindControl("dirName") as LinkButton;
            dirName.CommandArgument = directory + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "name"));
            dirName.Text =
                @"<p style=""text-align:center""><img src=""{0}images/folder.gif"" alt=""{1}"" title=""{1}"" /><br />{1}</p>"
                    .FormatWith(
                        YafForumInfo.ForumClientFileRoot,
                        Convert.ToString(DataBinder.Eval(e.Item.DataItem, "name")));
        }

        /// <summary>
        /// Handles the Bind event of the Files control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataListItemEventArgs"/> instance containing the event data.</param>
        public void Files_Bind([NotNull] object sender, [NotNull] DataListItemEventArgs e)
        {
            var directoryPath = Path.Combine(YafForumInfo.ForumClientFileRoot, YafBoardFolders.Current.Avatars);

            var fname = (Literal)e.Item.FindControl("fname");

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var finfo = new FileInfo(
                    this.Server.MapPath(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "name"))));

                if (this.CurrentDirectory.IsSet())
                {
                    directoryPath = this.CurrentDirectory;
                }

                string tmpExt = finfo.Extension.ToLower();

                if (tmpExt == ".gif" || tmpExt == ".jpg" || tmpExt == ".jpeg" || tmpExt == ".png" || tmpExt == ".bmp")
                {
                    string link;
                    var encodedFileName = finfo.Name.Replace(".", "%2E");

                    if (this.returnUserID > 0)
                    {
                        link = YafBuildLink.GetLink(
                            ForumPages.admin_edituser,
                            "u={0}&av={1}",
                            this.returnUserID,
                            this.Server.UrlEncode("{0}/{1}".FormatWith(directoryPath, encodedFileName)));
                    }
                    else
                    {
                        link = YafBuildLink.GetLink(
                            ForumPages.cp_editavatar,
                            "av={0}",
                            this.Server.UrlEncode("{0}/{1}".FormatWith(directoryPath, encodedFileName)));
                    }

                    fname.Text =
                        @"<div style=""text-align:center""><a href=""{0}""><img src=""{1}"" alt=""{2}"" title=""{2}"" class=""borderless"" /></a><br /><small>{2}</small></div>{3}"
                            .FormatWith(
                                link,
                                "{0}/{1}".FormatWith(directoryPath, finfo.Name),
                                finfo.Name,
                                Environment.NewLine);
                }
            }

            if (e.Item.ItemType != ListItemType.Header)
            {
                return;
            }

            // get the previous directory...
            string previousDirectory = Path.Combine(YafForumInfo.ForumClientFileRoot, YafBoardFolders.Current.Avatars);

            var up = e.Item.FindControl("up") as LinkButton;
            up.CommandArgument = previousDirectory;
            up.Text =
                @"<p style=""text-align:center""><img src=""{0}images/folder.gif"" alt=""Up"" /><br />UP</p>".FormatWith
                    (YafForumInfo.ForumClientFileRoot);
            up.ToolTip = this.GetText("UP_TITLE");

            // Hide if Top Folder
            if (this.CurrentDirectory.Equals(previousDirectory))
            {
                up.Visible = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the Clean directory list
        /// </summary>
        /// <param name="baseDir">The base directory.</param>
        /// <returns>
        /// Returns the Clean directory list
        /// </returns>
        [NotNull]
        protected List<DirectoryInfo> DirectoryListClean([NotNull] DirectoryInfo baseDir)
        {
            DirectoryInfo[] avatarDirectories = baseDir.GetDirectories();

            return
                avatarDirectories.Where(
                    dir =>
                    (dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                    && (dir.Attributes & FileAttributes.System) != FileAttributes.System).ToList();
        }

        /// <summary>
        /// Gets the Clean files list
        /// </summary>
        /// <param name="baseDir">The base directory.</param>
        /// <returns>Returns the Clean files list</returns>
        [NotNull]
        protected List<FileInfo> FilesListClean([NotNull] DirectoryInfo baseDir)
        {
            FileInfo[] avatarfiles = baseDir.GetFiles("*.*");

            return
                avatarfiles.Where(
                    file =>
                    (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                    && (file.Attributes & FileAttributes.System) != FileAttributes.System
                    && this.IsValidAvatarExtension(file.Extension.ToLower())).ToList();
        }

        /// <summary>
        /// Determines whether [is valid avatar extension] [the specified extension].
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        /// The is valid avatar extension.
        /// </returns>
        protected bool IsValidAvatarExtension([NotNull] string extension)
        {
            return extension == ".gif" || extension == ".jpg" || extension == ".jpeg" || extension == ".png"
                   || extension == ".bmp";
        }

        /// <summary>
        /// Items the command.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="DataListCommandEventArgs"/> instance containing the event data.</param>
        protected void ItemCommand([NotNull] object source, [NotNull] DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "directory":
                    this.CurrentDirectory = e.CommandArgument.ToString();
                    this.BindData(e.CommandArgument.ToString());
                    break;
            }
        }

        /// <summary>
        /// Returns to the Edit Avatar Page
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void BtnCancel_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            // Redirect to the edit avatar page
            YafBuildLink.Redirect(ForumPages.cp_editavatar);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.Request.QueryString.GetFirstOrDefault("u") != null)
            {
                this.returnUserID = this.Request.QueryString.GetFirstOrDefault("u").ToType<int>();
            }

            if (this.IsPostBack)
            {
                return;
            }

            this.PageLinks.AddRoot();

            if (this.returnUserID > 0)
            {
                this.PageLinks.AddLink(this.GetText("ADMIN_ADMIN", "Administration"), string.Empty);
                this.PageLinks.AddLink("Users", YafBuildLink.GetLink(ForumPages.admin_users));
            }
            else
            {
                this.PageLinks.AddLink(
                    this.HtmlEncode(this.PageContext.PageUserName),
                    YafBuildLink.GetLink(ForumPages.cp_profile));
                this.PageLinks.AddLink(
                    this.GetText("CP_EDITAVATAR", "TITLE"),
                    YafBuildLink.GetLink(ForumPages.cp_editavatar));
            }

            this.PageLinks.AddLink(this.GetText("TITLE"), string.Empty);

            this.CurrentDirectory = Path.Combine(YafForumInfo.ForumClientFileRoot, YafBoardFolders.Current.Avatars);

            this.BindData(this.CurrentDirectory);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="currentFolder">The current Folder.</param>
        private void BindData([NotNull] string currentFolder)
        {
            var directoryPath = Path.Combine(YafForumInfo.ForumClientFileRoot, YafBoardFolders.Current.Avatars);

            if (currentFolder.IsSet())
            {
                directoryPath = currentFolder;
            }

            var baseDirectory = new DirectoryInfo(this.Server.MapPath(directoryPath));

            var avatarSubDirs = this.DirectoryListClean(baseDirectory);

            if (avatarSubDirs.Count > 0)
            {
                this.directories.Visible = true;
                this.directories.DataSource = avatarSubDirs;
                this.directories.DataBind();
            }
            else
            {
                this.directories.Visible = false;
            }

            this.files.DataSource = this.FilesListClean(baseDirectory);
            this.files.Visible = true;
            this.files.DataBind();
        }

        #endregion
    }
}