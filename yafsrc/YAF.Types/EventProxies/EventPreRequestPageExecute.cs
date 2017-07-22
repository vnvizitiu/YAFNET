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
namespace YAF.Types.EventProxies
{
  #region Using

  using System.Web.UI;

  using YAF.Types.Interfaces;

  #endregion

  /// <summary>
  /// The event pre request page execute.
  /// </summary>
  public class EventPreRequestPageExecute : IAmEvent
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EventPreRequestPageExecute"/> class.
    /// </summary>
    /// <param name="requestedPage">
    /// The requested page.
    /// </param>
    public EventPreRequestPageExecute([NotNull] Page requestedPage)
    {
      this.RequestedPage = requestedPage;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets RequestedPage.
    /// </summary>
    public Page RequestedPage { get; set; }

    #endregion
  }
}