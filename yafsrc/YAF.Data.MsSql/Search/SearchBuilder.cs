﻿/* Yet Another Forum.NET
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces.Data;

    #region Using

    #endregion

    #region Enums

    #endregion

    /// <summary>
    ///     The search builder.
    /// </summary>
    public class SearchBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the search SQL.
        /// </summary>
        /// <param name="context">The search context.</param>
        /// <returns>Returns the full SQL script</returns>
        public string BuildSearchSql([NotNull] ISearchContext context)
        {
            CodeContracts.VerifyNotNull(context, "context");

            var builtStatements = new List<string>();

            if (context.MaxResults > 0)
            {
                builtStatements.Add("SET ROWCOUNT {0};".FormatWith(context.MaxResults));
            }

            string searchSql =
                "SELECT a.ForumID, a.TopicID, a.Topic, b.UserID, IsNull(c.Username, b.Name) as Name, c.MessageID, c.Posted, [Message] = '', c.Flags ";
            searchSql +=
                "\r\nfrom {databaseOwner}.{objectQualifier}topic a left join {databaseOwner}.{objectQualifier}message c on a.TopicID = c.TopicID left join {databaseOwner}.{objectQualifier}user b on c.UserID = b.UserID join {databaseOwner}.{objectQualifier}vaccess x on x.ForumID=a.ForumID ";
            searchSql +=
                "\r\nwhere x.ReadAccess<>0 AND x.UserID={0} AND c.IsApproved = 1 AND a.TopicMovedID IS NULL AND a.IsDeleted = 0 AND c.IsDeleted = 0"
                    .FormatWith(context.UserID);

            if (context.ForumIDs.Any())
            {
                searchSql += " AND a.ForumID IN ({0})".FormatWith(context.ForumIDs.ToDelimitedString(","));
            }

            if (context.ToSearchFromWho.IsSet())
            {
                searchSql +=
                    "\r\nAND ({0})".FormatWith(
                        this.BuildWhoConditions(
                            context.ToSearchFromWho,
                            context.SearchFromWhoMethod,
                            context.SearchDisplayName).BuildSql(true));
            }

            if (context.ToSearchWhat.IsSet())
            {
                if (!context.SearchTitleOnly)
                {
                    builtStatements.Add(searchSql);

                    builtStatements.Add(
                        "AND ({0})".FormatWith(
                            this.BuildWhatConditions(
                                context.ToSearchWhat,
                                context.SearchWhatMethod,
                                "c.Message",
                                context.UseFullText).BuildSql(true)));

                    builtStatements.Add("UNION");
                }

                builtStatements.Add(searchSql);

                builtStatements.Add(
                    "AND ({0})".FormatWith(
                        this.BuildWhatConditions(
                            context.ToSearchWhat,
                            context.SearchWhatMethod,
                            "a.Topic",
                            context.UseFullText).BuildSql(true)));
            }
            else
            {
                builtStatements.Add(searchSql);
            }

            builtStatements.Add("ORDER BY c.Posted DESC");

            string builtSql = builtStatements.ToDelimitedString("\r\n");

            Debug.WriteLine("Build Sql: [{0}]".FormatWith(builtSql));

            return builtSql;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The build search who conditions.
        /// </summary>
        /// <param name="toSearchWhat">
        ///     The to Search What.
        /// </param>
        /// <param name="searchWhatMethod">
        ///     The search What Method.
        /// </param>
        /// <param name="dbField">
        ///     The DB Field.
        /// </param>
        /// <param name="useFullText">
        ///     The use Full Text.
        /// </param>
        /// <returns>
        /// </returns>
        [NotNull]
        protected IEnumerable<SearchCondition> BuildWhatConditions(
            [NotNull] string toSearchWhat,
            SearchWhatFlags searchWhatMethod,
            [NotNull] string dbField,
            bool useFullText)
        {
            CodeContracts.VerifyNotNull(toSearchWhat, "toSearchWhat");
            CodeContracts.VerifyNotNull(dbField, "dbField");

            toSearchWhat = toSearchWhat.Replace("'", "''").Trim();

            var conditions = new List<SearchCondition>();

            var conditionType = SearchConditionType.AND;

            if (searchWhatMethod == SearchWhatFlags.AnyWords)
            {
                conditionType = SearchConditionType.OR;
            }

            var wordList = new List<string> { toSearchWhat };

            if (searchWhatMethod == SearchWhatFlags.AllWords || searchWhatMethod == SearchWhatFlags.AnyWords)
            {
                wordList =
                    toSearchWhat.Replace(@"""", string.Empty)
                        .Split(' ')
                        .Where(x => x.IsSet())
                        .Select(x => x.Trim())
                        .ToList();
            }

            if (useFullText)
            {
                var list = new List<SearchCondition>();

                list.AddRange(
                    wordList.Select(
                        word =>
                        new SearchCondition
                            {
                                Condition = @"""{0}""".FormatWith(word), 
                                ConditionType = conditionType
                            }));

                conditions.Add(
                    new SearchCondition
                        {
                            Condition =
                                "CONTAINS ({1}, N' {0} ')".FormatWith(list.BuildSql(false), dbField),
                            ConditionType = conditionType
                        });
            }
            else
            {
                conditions.AddRange(
                    wordList.Select(
                        word =>
                        new SearchCondition
                            {
                                Condition = "{1} LIKE N'%{0}%'".FormatWith(word, dbField),
                                ConditionType = conditionType
                            }));
            }

            return conditions;
        }

        /// <summary>
        ///     The build search who conditions.
        /// </summary>
        /// <param name="toSearchFromWho">
        ///     The to search from who.
        /// </param>
        /// <param name="searchFromWhoMethod">
        ///     The search from who method.
        /// </param>
        /// <param name="searchDisplayName">
        ///     The search display name.
        /// </param>
        /// <returns>
        /// </returns>
        [NotNull]
        protected IEnumerable<SearchCondition> BuildWhoConditions(
            [NotNull] string toSearchFromWho,
            SearchWhatFlags searchFromWhoMethod,
            bool searchDisplayName)
        {
            CodeContracts.VerifyNotNull(toSearchFromWho, "toSearchFromWho");

            toSearchFromWho = toSearchFromWho.Replace("'", "''").Trim();

            var conditions = new List<SearchCondition>();

            var conditionType = SearchConditionType.AND;

            if (searchFromWhoMethod == SearchWhatFlags.AnyWords)
            {
                conditionType = SearchConditionType.OR;
            }

            var wordList = new List<string> { toSearchFromWho };

            if (searchFromWhoMethod == SearchWhatFlags.AllWords || searchFromWhoMethod == SearchWhatFlags.AnyWords)
            {
                wordList =
                    toSearchFromWho.Replace(@"""", string.Empty)
                        .Split(' ')
                        .Where(x => x.IsSet())
                        .Select(x => x.Trim())
                        .ToList();
            }

            foreach (string word in wordList)
            {
                int userId;

                string conditionSql;
                if (int.TryParse(word, out userId))
                {
                    conditionSql = "c.UserID IN ({0})".FormatWith(userId);
                }
                else
                {
                    if (searchFromWhoMethod == SearchWhatFlags.ExactMatch)
                    {
                        conditionSql =
                            "(c.Username IS NULL AND b.{1} = N'{0}') OR (c.Username = N'{0}')".FormatWith(
                                word,
                                searchDisplayName ? "DisplayName" : "Name");
                    }
                    else
                    {
                        conditionSql =
                            "(c.Username IS NULL AND b.{1} LIKE N'%{0}%') OR (c.Username LIKE N'%{0}%')".FormatWith(
                                word,
                                searchDisplayName ? "DisplayName" : "Name");
                    }
                }

                conditions.Add(new SearchCondition { Condition = conditionSql, ConditionType = conditionType });
            }

            return conditions;
        }

        #endregion
    }
}