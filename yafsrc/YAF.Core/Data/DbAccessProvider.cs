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

namespace YAF.Core.Data
{
    #region Using

    using Autofac.Features.Indexed;

    using YAF.Classes;
    using YAF.Core.Helpers;
    using YAF.Types;
    using YAF.Types.Exceptions;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Data;

    #endregion

    /// <summary>
    ///     The db connection provider base.
    /// </summary>
    public class DbAccessProvider : IDbAccessProvider
    {
        #region Fields

        private readonly IIndex<string, IDbAccess> _dbAccessProviders;

        private readonly SafeReadWriteProvider<IDbAccess> _dbAccessSafe;

        private readonly IServiceLocator _serviceLocator;

        private string _providerName = null;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbAccessProvider" /> class.
        /// </summary>
        /// <param name="dbAccessProviders">
        ///     The db access providers.
        /// </param>
        /// <param name="serviceLocator">
        ///     The service locator.
        /// </param>
        public DbAccessProvider(IIndex<string, IDbAccess> dbAccessProviders, IServiceLocator serviceLocator)
        {
            this._dbAccessProviders = dbAccessProviders;
            this._serviceLocator = serviceLocator;

            this._dbAccessSafe = new SafeReadWriteProvider<IDbAccess>(
                () =>
                    {
                        IDbAccess dbAccess;

                        // attempt to get the provider...
                        if (this._dbAccessProviders.TryGetValue(this.ProviderName, out dbAccess))
                        {
                            // first time...
                            this._serviceLocator.Get<IRaiseEvent>()
                                .Raise(new InitDatabaseProviderEvent(this.ProviderName, dbAccess));
                        }
                        else
                        {
                            throw new NoValidDbAccessProviderFoundException(
                                @"Unable to Locate Provider Named ""{0}"" in Data Access Providers (DLL Not Located in Bin Directory?)."
                                    .FormatWith(this.ProviderName));
                        }

                        return dbAccess;
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the instance of the IDbAccess provider.
        /// </summary>
        /// <returns> </returns>
        /// <exception cref="NoValidDbAccessProviderFoundException">
        ///     <c>NoValidDbAccessProviderFoundException</c>
        ///     .
        /// </exception>
        [CanBeNull]
        public IDbAccess Instance
        {
            get
            {
                return this._dbAccessSafe.Instance;
            }

            set
            {
                this._dbAccessSafe.Instance = value;
                if (value != null)
                {
                    this.ProviderName = value.Information.ProviderName;
                }
            }
        }

        /// <summary>
        ///     Gets or sets ProviderName.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return this._providerName ?? (this._providerName = Config.ConnectionProviderName);
            }
            set
            {
                this._providerName = value;
                this._dbAccessSafe.Instance = null;
            }
        }

        #endregion
    }
}