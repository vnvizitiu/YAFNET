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

    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using YAF.Types.Extensions;

    #endregion

    /// <summary>
    /// Custom DropDown List Controls with Images
    /// </summary>
    public class ImageListBox : DropDownList
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image location.
        /// </summary>
        /// <value>
        /// The image location.
        /// </value>
        public string ImageLocation
        {
            get
            {
                return this.ViewState["ImageLocation"].ToString();
            }

            set
            {
                this.ViewState["ImageLocation"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add Flag Images to Items
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (var item in this.Items.Cast<ListItem>().Where(item => item.Value.IsSet()))
            {
                item.Attributes.Add(
                    "data-content",
                    "<image src=\"{0}\" alt=\"{1}\" class=\"standardSelectMenu-Icon\" /><span>&nbsp;{1}</span>".FormatWith(
                        this.ImageLocation.FormatWith(item.Value),
                        item.Text));
            }

            base.Render(writer);
        }

        #endregion
    }
}