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
namespace YAF.Core.BBCode.ReplaceRules
{
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    /// <summary>
  /// Not regular expression, just a simple replace
  /// </summary>
  public class SimpleReplaceRule : BaseReplaceRule
  {
    #region Constants and Fields

    /// <summary>
    ///   The _find.
    /// </summary>
    private readonly string _find;

    /// <summary>
    ///   The _replace.
    /// </summary>
    private readonly string _replace;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleReplaceRule"/> class.
    /// </summary>
    /// <param name="find">
    /// The find.
    /// </param>
    /// <param name="replace">
    /// The replace.
    /// </param>
    public SimpleReplaceRule(string find, string replace)
    {
      this._find = find;
      this._replace = replace;

      // lower the rank by default
      this.RuleRank = 100;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets RuleDescription.
    /// </summary>
    public override string RuleDescription
    {
      get
      {
        return "Find = \"{0}\"".FormatWith(this._find);
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// The replace.
    /// </summary>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="replacement">
    /// The replacement.
    /// </param>
    public override void Replace(ref string text, IReplaceBlocks replacement)
    {
      int index = -1;

      do
      {
        
        index = text.FastIndexOf(this._find);

        if (index >= 0)
        {
          // replace it...
          int replaceIndex = replacement.Add(this._replace);
          text = text.Substring(0, index) + replacement.Get(replaceIndex) +
                 text.Substring(index + this._find.Length);
        }
      }
      while (index >= 0);
    }

    #endregion
  }
}