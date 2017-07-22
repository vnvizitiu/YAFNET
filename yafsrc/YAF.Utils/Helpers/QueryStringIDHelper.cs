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
namespace YAF.Utils.Helpers
{
  #region Using

  using System;
  using System.Collections.Generic;
  using System.Web;

  using YAF.Types.Extensions;
  using YAF.Utils;
  using YAF.Utils.Helpers.StringUtils;
  using YAF.Types;

  #endregion

  /// <summary>
  /// The query string id helper.
  /// </summary>
  public class QueryStringIDHelper
  {
    #region Constants and Fields

    /// <summary>
    ///   The _id dictionary.
    /// </summary>
    private Dictionary<string, long> _idDictionary;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringIDHelper"/> class. 
    ///   False to ErrorOnInvalid
    /// </summary>
    /// <param name="idName">
    /// </param>
    public QueryStringIDHelper(string idName)
      : this(idName, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringIDHelper"/> class. 
    ///   False on ErrorOnInvalid
    /// </summary>
    /// <param name="idNames">
    /// </param>
    public QueryStringIDHelper(string[] idNames)
      : this(idNames, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringIDHelper"/> class.
    /// </summary>
    /// <param name="idName">
    /// The id name.
    /// </param>
    /// <param name="errorOnInvalid">
    /// The error on invalid.
    /// </param>
    public QueryStringIDHelper([NotNull] string idName, bool errorOnInvalid)
    {
      CodeContracts.VerifyNotNull(idName, "idName");

      this.InitIDs(new[] { idName }, new[] { errorOnInvalid });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringIDHelper"/> class.
    /// </summary>
    /// <param name="idNames">
    /// The id names.
    /// </param>
    /// <param name="errorOnInvalid">
    /// The error on invalid.
    /// </param>
    public QueryStringIDHelper([NotNull] string[] idNames, bool errorOnInvalid)
    {
      CodeContracts.VerifyNotNull(idNames, "idNames");

      var failInvalid = new bool[idNames.Length];

      for (int i = 0; i < failInvalid.Length; i++)
      {
        failInvalid[i] = errorOnInvalid;
      }

      this.InitIDs(idNames, failInvalid);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringIDHelper"/> class.
    /// </summary>
    /// <param name="idNames">
    /// The id names.
    /// </param>
    /// <param name="errorOnInvalid">
    /// The error on invalid.
    /// </param>
    public QueryStringIDHelper(string[] idNames, bool[] errorOnInvalid)
    {
      this.InitIDs(idNames, errorOnInvalid);
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets Params.
    /// </summary>
    public Dictionary<string, long> Params
    {
      get
      {
        if (this._idDictionary == null)
        {
          this._idDictionary = new Dictionary<string, long>();
        }

        return this._idDictionary;
      }
    }

    #endregion

    #region Indexers

    /// <summary>
    ///   The this.
    /// </summary>
    /// <param name = "idName">
    ///   The id name.
    /// </param>
    public long? this[string idName]
    {
      get
      {
        if (this.Params.ContainsKey(idName))
        {
          return this.Params[idName];
        }

        return null;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// The contains key.
    /// </summary>
    /// <param name="idName">
    /// The id name.
    /// </param>
    /// <returns>
    /// The contains key.
    /// </returns>
    public bool ContainsKey(string idName)
    {
      return this.Params.ContainsKey(idName);
    }

    #endregion

    #region Methods

    /// <summary>
    /// The init i ds.
    /// </summary>
    /// <param name="idNames">
    /// The id names.
    /// </param>
    /// <param name="errorOnInvalid">
    /// The error on invalid.
    /// </param>
    /// <exception cref="Exception">
    /// </exception>
    private void InitIDs([NotNull] string[] idNames, [NotNull] bool[] errorOnInvalid)
    {
      CodeContracts.VerifyNotNull(idNames, "idNames");
      CodeContracts.VerifyNotNull(errorOnInvalid, "errorOnInvalid");

      if (idNames.Length != errorOnInvalid.Length)
      {
        throw new Exception("idNames and errorOnInvalid variables must be the same array length.");
      }

      for (int i = 0; i < idNames.Length; i++)
      {
        if (this.Params.ContainsKey(idNames[i]))
        {
          continue;
        }

        long idConverted = -1;

        if (HttpContext.Current.Request.QueryString.GetFirstOrDefault(idNames[i]).IsSet() && long.TryParse(HttpContext.Current.Request.QueryString.GetFirstOrDefault(idNames[i]), out idConverted))
        {
          this.Params.Add(idNames[i], idConverted);
        }
        else if (errorOnInvalid[i])
        {
          // fail, see if it should be valid...
          YafBuildLink.RedirectInfoPage(InfoMessage.Invalid);
        }
      }
    }

    #endregion
  }
}