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

    using YAF.Types.Extensions;

    #endregion

    /// <summary>
    ///     The i db function extensions.
    /// </summary>
    public static class IDbFunctionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get data set.
        /// </summary>
        /// <param name="dbFunction">
        /// The db function. 
        /// </param>
        /// <param name="function">
        /// The function. 
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/> . 
        /// </returns>
        [NotNull]
        public static DataSet GetAsDataSet([NotNull] this IDbFunction dbFunction, [NotNull] Func<dynamic, object> function)
        {
            CodeContracts.VerifyNotNull(dbFunction, "dbFunction");
            CodeContracts.VerifyNotNull(function, "function");

            return (DataSet)function(dbFunction.GetDataSet);
        }

        /// <summary>
        /// Gets a DataTable using the new dynamic Db Function
        /// </summary>
        /// <param name="dbFunction">
        /// The db function. 
        /// </param>
        /// <param name="function">
        /// The function. 
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/> . 
        /// </returns>
        [NotNull]
        public static DataTable GetAsDataTable([NotNull] this IDbFunction dbFunction, [NotNull] Func<dynamic, object> function)
        {
            CodeContracts.VerifyNotNull(dbFunction, "dbFunction");
            CodeContracts.VerifyNotNull(function, "function");

            return (DataTable)function(dbFunction.GetData);
        }

        /// <summary>
        /// The get data typed.
        /// </summary>
        /// <param name="dbFunction">
        /// The db function. 
        /// </param>
        /// <param name="function">
        /// The function. 
        /// </param>
        /// <param name="comparer">
        /// The comparer. 
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/> . 
        /// </returns>
        [CanBeNull]
        public static IEnumerable<T> GetDataTyped<T>(
            [NotNull] this IDbFunction dbFunction, 
            [NotNull] Func<object, object> function, 
            [CanBeNull] IEqualityComparer<string> comparer = null) where T : IDataLoadable, new()
        {
            CodeContracts.VerifyNotNull(dbFunction, "dbFunction");
            CodeContracts.VerifyNotNull(function, "function");

            return dbFunction.GetData(function).Typed<T>(comparer);
        }

        /// <summary>
        /// The get scalar as.
        /// </summary>
        /// <param name="dbFunction">
        /// The db function. 
        /// </param>
        /// <param name="function">
        /// The function. 
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/> . 
        /// </returns>
        [CanBeNull]
        public static T GetScalar<T>([NotNull] this IDbFunction dbFunction, [NotNull] Func<dynamic, object> function)
        {
            CodeContracts.VerifyNotNull(dbFunction, "dbFunction");
            CodeContracts.VerifyNotNull(function, "function");

            return ((object)function(dbFunction.Scalar)).ToType<T>();
        }

        #endregion
    }
}