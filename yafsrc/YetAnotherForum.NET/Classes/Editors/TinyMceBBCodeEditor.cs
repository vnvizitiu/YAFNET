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
namespace YAF.Editors
{
    using System.Web.UI;

    using YAF.Classes;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    /// <summary>
    /// The tinyMCE bb code editor.
    /// </summary>
    public class TinyMceBBCodeEditor : TinyMceEditor
    {
        #region Properties

        /// <summary>
        ///   Gets Description.
        /// </summary>
        [NotNull]
        public override string Description => "TinyMCE (BBCode)";

        /// <summary>
        ///   Gets ModuleId.
        /// </summary>
        public override string ModuleId => this.Description.GetHashCode().ToString();

        /// <summary>
        ///   Gets a value indicating whether UsesBBCode.
        /// </summary>
        public override bool UsesBBCode => true;

        /// <summary>
        ///   Gets a value indicating whether UsesHTML.
        /// </summary>
        public override bool UsesHTML => false;

        #endregion

        #region Methods

        /// <summary>
        /// The register Smiley script.
        /// </summary>
        protected override void RegisterSmilieyScript()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page,
                this.Page.GetType(),
                "insertsmiley",
                @"function insertsmiley(code,img) {tinyMCE.execCommand('mceInsertContent',false,code);}
                  function insertAttachment(id,url) {tinyMCE.execCommand('mceInsertContent',false,'[attach]' + id + '[/attach]');}",
                true);
        }

        /// <summary>
        /// The register tiny mce custom js.
        /// </summary>
        protected override void RegisterTinyMceCustomJS()
        {
            YafContext.Current.PageElements.RegisterJsBlock(
                "editorlang",
                @"var editorLanguage = ""{0}"";".FormatWith(
                    YafContext.Current.CultureUser.IsSet()
                        ? YafContext.Current.CultureUser.Substring(0, 2)
                        : this.Get<YafBoardSettings>().Culture.Substring(0, 2)));

            ScriptManager.RegisterClientScriptInclude(
                this.Page,
                this.Page.GetType(),
                "tinymceinit",
                this.ResolveUrl("tinymce/tinymce_initbbcode.js"));
        }

        #endregion
    }
}