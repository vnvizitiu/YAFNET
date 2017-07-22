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

namespace YAF.Core.Services.Import
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;

    using YAF.Classes.Data;
    using YAF.Core.Extensions;
    using YAF.Core.Model;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Data;
    using YAF.Types.Models;

    /// <summary>
    /// The data import.
    /// </summary>
    public static class DataImport
    {
        /// <summary>
        /// The bb code extension import.
        /// </summary>
        /// <param name="boardId">
        /// The board id.
        /// </param>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <returns>
        /// Returns How Many Extensions where imported.
        /// </returns>
        /// <exception cref="Exception">Import stream is not expected format.
        /// </exception>
        public static int BBCodeExtensionImport(int boardId, Stream inputStream)
        {
            int importedCount = 0;

            var repository = YafContext.Current.Get<IRepository<BBCode>>();

            // import extensions...
            var dsBBCode = new DataSet();
            dsBBCode.ReadXml(inputStream);

            if (dsBBCode.Tables["YafBBCode"] != null && dsBBCode.Tables["YafBBCode"].Columns["Name"] != null &&
                dsBBCode.Tables["YafBBCode"].Columns["SearchRegex"] != null &&
                dsBBCode.Tables["YafBBCode"].Columns["ExecOrder"] != null)
            {
                var bbcodeList = repository.ListTyped(boardId: boardId);

                // import any extensions that don't exist...
                foreach (DataRow row in dsBBCode.Tables["YafBBCode"].Rows)
                {
                    var name = row["Name"].ToString();

                    var bbCodeExtension = bbcodeList.FirstOrDefault(b => b.Name.Equals(name));

                    if (bbCodeExtension != null)
                    {
                        // update this bbcode...
                        repository.Save(
                            bbCodeExtension.ID,
                            row["Name"].ToString(),
                            row["Description"].ToString(),
                            row["OnClickJS"].ToString(),
                            row["DisplayJS"].ToString(),
                            row["EditJS"].ToString(),
                            row["DisplayCSS"].ToString(),
                            row["SearchRegex"].ToString(),
                            row["ReplaceRegex"].ToString(),
                            row["Variables"].ToString(),
                            row["UseModule"].ToType<bool>(),
                            row["ModuleClass"].ToString(),
                            row["ExecOrder"].ToType<int>(),
                            boardId);
                    }
                    else
                    {
                        // add this bbcode...
                        repository.Save(
                            null,
                            row["Name"].ToString(),
                            row["Description"].ToString(),
                            row["OnClickJS"].ToString(),
                            row["DisplayJS"].ToString(),
                            row["EditJS"].ToString(),
                            row["DisplayCSS"].ToString(),
                            row["SearchRegex"].ToString(),
                            row["ReplaceRegex"].ToString(),
                            row["Variables"].ToString(),
                            row["UseModule"].ToType<bool>(),
                            row["ModuleClass"].ToString(),
                            row["ExecOrder"].ToType<int>(),
                            boardId);

                        importedCount++;
                    }
                }
            }
            else
            {
                throw new Exception("Import stream is not expected format.");
            }

            return importedCount;
        }

        /// <summary>
        /// The file extension import.
        /// </summary>
        /// <param name="boardId">
        /// The board id.
        /// </param>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <returns>
        /// Returns How Many Extensions where imported.
        /// </returns>
        /// <exception cref="Exception">Import stream is not expected format.
        /// </exception>
        public static int FileExtensionImport(int boardId, Stream inputStream)
        {
            int importedCount = 0;

            var dsExtensions = new DataSet();
            dsExtensions.ReadXml(inputStream);

            if (dsExtensions.Tables["YafExtension"] != null &&
                dsExtensions.Tables["YafExtension"].Columns["Extension"] != null)
            {
                var repository = YafContext.Current.Get<IRepository<FileExtension>>();

                var extensionList = repository.ListTyped(boardId: boardId);

                // import any extensions that don't exist...
                var extensionsToImport = dsExtensions.Tables["YafExtension"].Rows.Cast<DataRow>().Select(row => row["Extension"].ToString()).ToList();

                foreach (
                    string newExtension in
                        extensionsToImport.Where(ext => !extensionList.Any(e => string.Equals(e.Extension, ext, StringComparison.OrdinalIgnoreCase))))
                {
                    try
                    {
                        // add this...
                        repository.Insert(new FileExtension { BoardId = boardId, Extension = newExtension });
                    }
                    catch
                    {
                        // LAZY CODER ALERT: Ignore errors, probably duplicates.
                    }

                    importedCount++;
                }
            }
            else
            {
                throw new Exception("Import stream is not expected format.");
            }

            return importedCount;
        }

        /// <summary>
        /// Topics the status import.
        /// </summary>
        /// <param name="boardId">
        /// The board id.
        /// </param>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <exception cref="Exception">
        /// Import stream is not expected format.
        /// </exception>
        /// <returns>
        /// Returns the Number of Imported Items.
        /// </returns>
        public static int TopicStatusImport(int boardId, Stream inputStream)
        {
            int importedCount = 0;

            // import extensions...
            var dsStates = new DataSet();
            dsStates.ReadXml(inputStream);

            if (dsStates.Tables["YafTopicStatus"] != null &&
                dsStates.Tables["YafTopicStatus"].Columns["TopicStatusName"] != null &&
                dsStates.Tables["YafTopicStatus"].Columns["DefaultDescription"] != null)
            {
                var topicStatusList = LegacyDb.TopicStatus_List(boardId);

                // import any topic status that don't exist...
                foreach (
                    DataRow row in
                        dsStates.Tables["YafTopicStatus"].Rows.Cast<DataRow>().Where(
                            row =>
                            topicStatusList.Select("TopicStatusName = '{0}'".FormatWith(row["TopicStatusName"])).Length ==
                            0))
                {
                    // add this...
                    LegacyDb.TopicStatus_Save(
                        null, boardId, row["TopicStatusName"].ToString(), row["DefaultDescription"].ToString());
                    importedCount++;
                }
            }
            else
            {
                throw new Exception("Import stream is not expected format.");
            }

            return importedCount;
        }

        /// <summary>
        /// Import List of Banned Email Addresses
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>
        /// Returns the Number of Imported Items.
        /// </returns>
        /// <exception cref="Exception">
        /// Import stream is not expected format.
        /// </exception>
        public static int BannedEmailAdressesImport(int boardId, int userId, Stream inputStream)
        {
            int importedCount = 0;

            var repository = YafContext.Current.Get<IRepository<BannedEmail>>();
            var existingBannedEmailList = repository.List(boardId: boardId);

            using (var streamReader = new StreamReader(inputStream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    if (line.IsNotSet() || !line.Contains("@"))
                    {
                        continue;
                    }

                    if (existingBannedEmailList.Select("Mask = '{0}'".FormatWith(line)).Length != 0)
                    {
                        continue;
                    }

                    repository.Save(null, line, "Imported Email Adress", boardId);
                    importedCount++;
                }
            }

            return importedCount;
        }

        /// <summary>
        /// Import List of Banned IP Addresses
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>
        /// Returns the Number of Imported Items.
        /// </returns>
        /// <exception cref="Exception">
        /// Import stream is not expected format.
        /// </exception>
        public static int BannedIpAdressesImport(int boardId, int userId, Stream inputStream)
        {
            int importedCount = 0;

            var repository = YafContext.Current.Get<IRepository<BannedIP>>();
            var existingBannedIPList = repository.List(boardId: boardId);

            using (var streamReader = new StreamReader(inputStream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    IPAddress importAddress;

                    if (line.IsNotSet() || !IPAddress.TryParse(line, out importAddress))
                    {
                        continue;
                    }

                    if (existingBannedIPList.Select("Mask = '{0}'".FormatWith(importAddress.ToString())).Length != 0)
                    {
                        continue;
                    }

                    repository.Save(null, importAddress.ToString(), "Imported IP Adress", userId, boardId);
                    importedCount++;
                }
            }

            return importedCount;
        }
        
        /// <summary>
        /// Import List of Banned User Names
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>
        /// Returns the Number of Imported Items.
        /// </returns>
        /// <exception cref="Exception">
        /// Import stream is not expected format.
        /// </exception>
        public static int BannedNamesImport(int boardId, int userId, Stream inputStream)
        {
            int importedCount = 0;

            var repository = YafContext.Current.Get<IRepository<BannedName>>();
            var existingBannedNameList = repository.List(boardId: boardId);

            using (var streamReader = new StreamReader(inputStream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    if (line.IsNotSet())
                    {
                        continue;
                    }

                    if (existingBannedNameList.Select("Mask = '{0}'".FormatWith(line)).Length != 0)
                    {
                        continue;
                    }

                    repository.Save(null, line, "Imported User Name", boardId);
                    importedCount++;
                }
            } 

            return importedCount;
        }

        /// <summary>
        /// Import List of Spam Words
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>Returns the Number of Imported Items.</returns>
        public static int SpamWordsImport(int boardId, Stream inputStream)
        {
            var importedCount = 0;

            var repository = YafContext.Current.Get<IRepository<Spam_Words>>();

            // import spam words...
            var dsSpamWords = new DataSet();
            dsSpamWords.ReadXml(inputStream);

            if (dsSpamWords.Tables["YafSpamWords"] == null
                || dsSpamWords.Tables["YafSpamWords"].Columns["spamword"] == null)
            {
                return importedCount;
            }

            var spamWordsList = repository.List();

            // import any extensions that don't exist...
            foreach (DataRow row in
                dsSpamWords.Tables["YafSpamWords"].Rows.Cast<DataRow>()
                    .Where(row => spamWordsList.Select("spamword = '{0}'".FormatWith(row["spamword"])).Length == 0))
            {
                // add this...
                repository.Save(spamWordID: null, spamWord: row["spamword"].ToString());
                importedCount++;
            }

            return importedCount;
        }
    }
}