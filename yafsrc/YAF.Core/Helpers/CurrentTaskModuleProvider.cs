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
namespace YAF.Core
{
  #region Using

  using System.Web;

  using YAF.Types;
  using YAF.Types.Constants;
  using YAF.Types.Interfaces;

  #endregion

  /// <summary>
  /// The current task module provider.
  /// </summary>
  public class CurrentTaskModuleProvider : IReadWriteProvider<ITaskModuleManager>
  {
    #region Constants and Fields

    /// <summary>
    /// The _http application state.
    /// </summary>
    private readonly HttpApplicationStateBase _httpApplicationState;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentTaskModuleProvider"/> class.
    /// </summary>
    /// <param name="httpApplicationState">
    /// The http application state.
    /// </param>
    public CurrentTaskModuleProvider([NotNull] HttpApplicationStateBase httpApplicationState)
    {
      CodeContracts.VerifyNotNull(httpApplicationState, "httpApplicationState");

      this._httpApplicationState = httpApplicationState;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   The create.
    /// </summary>
    /// <returns>
    /// </returns>
    [CanBeNull]
    public ITaskModuleManager Instance
    {
      get
      {
        // Note: not treated with "BoardID" at all -- only one instance per application.
        return this._httpApplicationState[Constants.Cache.TaskModule] as ITaskModuleManager;
      }

      set
      {
        CodeContracts.VerifyNotNull(value, "value");

        this._httpApplicationState[Constants.Cache.TaskModule] = value;
      }
    }

    #endregion
  }
}