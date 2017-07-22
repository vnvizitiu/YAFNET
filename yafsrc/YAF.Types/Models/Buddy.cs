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
namespace YAF.Types.Models
{
    using System;

    using ServiceStack.DataAnnotations;

    using YAF.Types.Interfaces.Data;

    /// <summary>
    /// A class which represents the Buddy table.
    /// </summary>
    [Serializable]
    public partial class Buddy : IEntity, IHaveID
    {
        partial void OnCreated();

        /// <summary>
        /// Initializes a new instance of the <see cref="Buddy"/> class.
        /// </summary>
        public Buddy()
        {
            this.OnCreated();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [AutoIncrement]
        [Alias("BuddyID")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets from user identifier.
        /// </summary>
        /// <value>
        /// From user identifier.
        /// </value>
        public int FromUserID { get; set; }

        /// <summary>
        /// Gets or sets to user identifier.
        /// </summary>
        /// <value>
        /// To user identifier.
        /// </value>
        public int ToUserID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Buddy"/> is approved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if approved; otherwise, <c>false</c>.
        /// </value>
        public bool Approved { get; set; }

        /// <summary>
        /// Gets or sets the requested.
        /// </summary>
        /// <value>
        /// The requested.
        /// </value>
        public DateTime Requested { get; set; }

        #endregion
    }
}