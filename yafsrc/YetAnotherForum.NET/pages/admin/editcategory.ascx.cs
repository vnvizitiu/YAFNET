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
    using System.IO;
    using System.Linq;
    using System.Web.UI.WebControls;

    using YAF.Classes;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    /// Class for the Edit Category Page
    /// </summary>
    public partial class editcategory : AdminPage
    {
        #region Methods

        /// <summary>
        /// The cancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Cancel_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            YafBuildLink.Redirect(ForumPages.admin_forums);
        }

        /// <summary>
        /// The create images data table.
        /// </summary>
        protected void CreateImagesDataTable()
        {
            using (var dt = new DataTable("Files"))
            {
                dt.Columns.Add("FileID", typeof(long));
                dt.Columns.Add("FileName", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                DataRow dr = dt.NewRow();
                dr["FileID"] = 0;
                dr["FileName"] = YafForumInfo.GetURLToContent("images/spacer.gif"); // use spacer.gif for Description Entry
                dr["Description"] = "None";
                dt.Rows.Add(dr);

                var dir =
                  new DirectoryInfo(
                    this.Request.MapPath(
                      "{0}{1}".FormatWith(YafForumInfo.ForumServerFileRoot, YafBoardFolders.Current.Categories)));
                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles("*.*");
                    long nFileID = 1;

                    foreach (FileInfo file in from file in files
                                              let sExt = file.Extension.ToLower()
                                              where sExt == ".png" || sExt == ".gif" || sExt == ".jpg"
                                              select file)
                    {
                        dr = dt.NewRow();
                        dr["FileID"] = nFileID++;
                        dr["FileName"] = file.Name;
                        dr["Description"] = file.Name;
                        dt.Rows.Add(dr);
                    }
                }

                this.CategoryImages.DataSource = dt;
                this.CategoryImages.DataValueField = "FileName";
                this.CategoryImages.DataTextField = "Description";
                this.CategoryImages.DataBind();
            }
        }

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
            if (this.IsPostBack)
            {
                return;
            }

            this.PageLinks.AddRoot();
            this.PageLinks.AddLink(this.GetText("ADMIN_ADMIN", "Administration"), YafBuildLink.GetLink(ForumPages.admin_admin));

            this.PageLinks.AddLink(this.GetText("TEAM", "FORUMS"), YafBuildLink.GetLink(ForumPages.admin_forums));
            this.PageLinks.AddLink(this.GetText("ADMIN_EDITCATEGORY", "TITLE"), string.Empty);

            this.Page.Header.Title = "{0} - {1} - {2}".FormatWith(
                 this.GetText("ADMIN_ADMIN", "Administration"),
                 this.GetText("TEAM", "FORUMS"),
                 this.GetText("ADMIN_EDITCATEGORY", "TITLE"));

            this.Save.Text = "<i class=\"fa fa-floppy-o fa-fw\"></i>&nbsp;{0}".FormatWith(this.GetText("SAVE"));
            this.Cancel.Text = "<i class=\"fa fa-remove fa-fw\"></i>&nbsp;{0}".FormatWith(this.GetText("CANCEL"));

            // Populate Category Table
            this.CreateImagesDataTable();

            this.CategoryImages.Attributes["onchange"] =
                "getElementById('{1}').src='{0}{2}/' + this.value".FormatWith(
                    YafForumInfo.ForumClientFileRoot, this.Preview.ClientID, YafBoardFolders.Current.Categories);

            this.BindData();
        }

        /// <summary>
        /// The save_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Save_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            int categoryID = 0;

            if (this.Request.QueryString.GetFirstOrDefault("c") != null)
            {
                categoryID = int.Parse(this.Request.QueryString.GetFirstOrDefault("c"));
            }

            short sortOrder;
            string name = this.Name.Text.Trim();
            string categoryImage = null;

            if (this.CategoryImages.SelectedIndex > 0)
            {
                categoryImage = this.CategoryImages.SelectedValue;
            }

            if (!ValidationHelper.IsValidPosShort(this.SortOrder.Text.Trim()))
            {
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITCATEGORY", "MSG_POSITIVE_VALUE"), MessageTypes.danger);
                return;
            }

            if (!short.TryParse(this.SortOrder.Text.Trim(), out sortOrder))
            {
                // error...
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITCATEGORY", "MSG_NUMBER"), MessageTypes.danger);
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                // error...
                this.PageContext.AddLoadMessage(this.GetText("ADMIN_EDITCATEGORY", "MSG_VALUE"), MessageTypes.danger);
                return;
            }

            // save category
            this.GetRepository<Category>().Save(categoryID, name, categoryImage, sortOrder);

            // remove category cache...
            this.Get<IDataCache>().Remove(Constants.Cache.ForumCategory);

            // redirect
            YafBuildLink.Redirect(ForumPages.admin_forums);
        }

        /// <summary>
        /// The bind data.
        /// </summary>
        private void BindData()
        {
            this.Preview.Src = "{0}Content/images/spacer.gif".FormatWith(YafForumInfo.ForumClientFileRoot);

            if (this.Request.QueryString.GetFirstOrDefault("c") == null)
            {
                // Currently creating a New Category, and auto fill the Category Sort Order + 1
                using (DataTable dt = this.GetRepository<Category>().List())
                {
                    int sortOrder = 1;

                    try
                    {
                        DataRow highestRow = dt.Rows[dt.Rows.Count - 1];

                        sortOrder = (short)highestRow["SortOrder"] + sortOrder;
                    }
                    catch
                    {
                        sortOrder = 1;
                    }

                    this.SortOrder.Text = sortOrder.ToString();

                    return;
                }
            }

            using (DataTable dt = this.GetRepository<Category>().List(this.Request.QueryString.GetFirstOrDefaultAs<int>("c")))
            {
                DataRow row = dt.Rows[0];
                this.Name.Text = (string)row["Name"];
                this.SortOrder.Text = row["SortOrder"].ToString();
                this.CategoryNameTitle.Text = this.Label1.Text = this.Name.Text;

                ListItem item = this.CategoryImages.Items.FindByText(row["CategoryImage"].ToString());

                if (item == null)
                {
                    return;
                }

                item.Selected = true;
                this.Preview.Src = "{0}{2}/{1}".FormatWith(
                    YafForumInfo.ForumClientFileRoot, row["CategoryImage"], YafBoardFolders.Current.Categories);

                // path corrected
            }
        }

        #endregion
    }
}