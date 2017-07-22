﻿/* Yet Another Forum.NET
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

namespace YAF.Core.BBCode
{
    #region Using

    using System;
    using System.Text;

    using YAF.Types.Extensions;

    #endregion

    /// <summary>
    /// The high lighter.
    /// </summary>
    public class HighLighter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "HighLighter" /> class.
        /// </summary>
        public HighLighter()
        {
            this.ReplaceEnter = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether ReplaceEnter.
        /// </summary>
        public bool ReplaceEnter { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Colors the text.
        /// </summary>
        /// <param name="codeText">The code to highlight.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// The color text.
        /// </returns>
        public string ColorText(string codeText, string language)
        {
            language = language.ToLower();

            language = language.Replace("\"", string.Empty);

            switch (language)
            {
                case "cs":
                    language = "csharp";
                    break;
                case "xml":
                    language = "markup";
                    break;
                case "plain":
                    language = "markup";
                    break;
                case "":
                    language = "markup";
                    break;
            }

            var tmpOutput = new StringBuilder();

            var highlight = string.Empty;

            // extract highlight
            if (language.Contains(";"))
            {
                highlight = language.Substring(language.IndexOf(";") + 1);
                language = language.Remove(language.IndexOf(";"));
            }

            // Create Output
            tmpOutput.AppendFormat(
                "<pre class=\"line-numbers\"{1}><code class=\"language-{0}\">",
                language,
                highlight.IsSet() ? " data-line=\"{0}\"".FormatWith(highlight) : string.Empty);

            tmpOutput.Append(codeText);

            tmpOutput.AppendFormat("</code></pre>{0}", Environment.NewLine);

            return tmpOutput.ToString();
        }

        #endregion
    }
}