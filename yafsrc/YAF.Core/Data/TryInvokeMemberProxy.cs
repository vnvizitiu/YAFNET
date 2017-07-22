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
    using System.Dynamic;

    using YAF.Types;

    /// <summary>
    ///     The dynamic db.
    /// </summary>
    public class TryInvokeMemberProxy : DynamicObject
    {
        #region Fields

        /// <summary>
        ///     The _try invoke func.
        /// </summary>
        private readonly TryInvokeFunc _tryInvokeFunc;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TryInvokeMemberProxy"/> class.
        /// </summary>
        /// <param name="tryInvokeFunc">
        /// The try invoke func. 
        /// </param>
        public TryInvokeMemberProxy([NotNull] TryInvokeFunc tryInvokeFunc)
        {
            CodeContracts.VerifyNotNull(tryInvokeFunc, "tryInvokeFunc");

            this._tryInvokeFunc = tryInvokeFunc;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     The try invoke func.
        /// </summary>
        /// <param name="binder"> The binder. </param>
        /// <param name="args"> The args. </param>
        /// <param name="result"> The result. </param>
        public delegate bool TryInvokeFunc(
            [NotNull] InvokeMemberBinder binder, [NotNull] object[] args, [NotNull] out object result);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The try invoke member.
        /// </summary>
        /// <param name="binder">
        /// The binder. 
        /// </param>
        /// <param name="args">
        /// The args. 
        /// </param>
        /// <param name="result">
        /// The result. 
        /// </param>
        /// <returns>
        /// The try invoke member. 
        /// </returns>
        public override bool TryInvokeMember(
            [NotNull] InvokeMemberBinder binder, [NotNull] object[] args, [NotNull] out object result)
        {
            return this._tryInvokeFunc(binder, args, out result);
        }

        #endregion
    }
}