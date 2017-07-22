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
namespace YAF.Core.BBCode
{
  #region Using

    using System.Collections.Generic;

    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    #endregion

  /// <summary>
  /// Gets an instance of replace rules and uses
  ///   caching if possible.
  /// </summary>
  public class ProcessReplaceRulesProvider : IHaveServiceLocator, IReadOnlyProvider<IProcessReplaceRules>
  {
    #region Constants and Fields

    /// <summary>
    ///   The _inject services.
    /// </summary>
    private readonly IInjectServices _injectServices;

    /// <summary>
    /// The _object store.
    /// </summary>
    private readonly IObjectStore _objectStore;

    /// <summary>
    ///   The _unique flags.
    /// </summary>
    private readonly IEnumerable<bool> _uniqueFlags;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessReplaceRulesProvider"/> class.
    /// </summary>
    /// <param name="objectStore">
    /// </param>
    /// <param name="serviceLocator">
    /// The service locator.
    /// </param>
    /// <param name="injectServices">
    /// The inject services.
    /// </param>
    /// <param name="uniqueFlags">
    /// The unique Flags.
    /// </param>
    public ProcessReplaceRulesProvider(
      [NotNull] IObjectStore objectStore, 
      [NotNull] IServiceLocator serviceLocator, 
      [NotNull] IInjectServices injectServices, 
      [NotNull] IEnumerable<bool> uniqueFlags)
    {
      this.ServiceLocator = serviceLocator;
      this._objectStore = objectStore;
      this._injectServices = injectServices;
      this._uniqueFlags = uniqueFlags;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   The Instance of this provider.
    /// </summary>
    /// <returns>
    /// </returns>
    public IProcessReplaceRules Instance
    {
      get
      {
        return this._objectStore.GetOrSet(
          Constants.Cache.ReplaceRules.FormatWith(this._uniqueFlags.ToIntOfBits()), 
          () =>
            {
              var processRules = new ProcessReplaceRules();

              // inject
              this._injectServices.Inject(processRules);

              return processRules;
            });
      }
    }

    /// <summary>
    ///   Gets or sets ServiceLocator.
    /// </summary>
    public IServiceLocator ServiceLocator { get; set; }

    #endregion
  }
}