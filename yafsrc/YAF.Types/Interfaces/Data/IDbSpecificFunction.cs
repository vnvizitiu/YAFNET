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
namespace YAF.Types.Interfaces.Data
{
	#region Using

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion

    /// <summary>
	/// The db specific function.
	/// </summary>
    public interface IDbSpecificFunction : IDbSortableOperation
	{
		#region Properties

		/// <summary>
		/// Gets ProviderName.
		/// </summary>
		string ProviderName { get; }

		#endregion

		#region Public Methods

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="dbfunctionType">
        /// The dbfunction type.
        /// </param>
        /// <param name="operationName">
        /// The operation name.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="transaction"></param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The execute.
        /// </returns>
        bool Execute(
	        DbFunctionType dbfunctionType,
	        string operationName,
	        IEnumerable<KeyValuePair<string, object>> parameters,
	        out object result,
            IDbTransaction transaction = null);

		#endregion
	}
}