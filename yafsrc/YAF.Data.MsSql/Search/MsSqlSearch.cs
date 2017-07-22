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

namespace YAF.Data.MsSql.Search
{
    using System.Collections.Generic;

    using YAF.Types.Attributes;
    using YAF.Types.Interfaces.Data;
    using YAF.Types.Models;

    /// <summary>
    /// 
    /// </summary>
    [ExportService(ServiceLifetimeScope.OwnedByContainer, new[] { typeof(ISearch) })]
    public class MsSqlSearch : ISearch
    {
        #region Fields

        /// <summary>
        /// The _DB function
        /// </summary>
        private readonly IDbFunction _dbFunction;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlSearch"/> class.
        /// </summary>
        /// <param name="dbFunction">The database function.</param>
        public MsSqlSearch(IDbFunction dbFunction)
        {
            this._dbFunction = dbFunction;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IEnumerable<SearchResult> Execute(ISearchContext context)
        {
            using (var session = this._dbFunction.CreateSession())
            {
                return session.GetTyped<SearchResult>(r => r.executesearch(context));
            }
        }

        #endregion
    }
}