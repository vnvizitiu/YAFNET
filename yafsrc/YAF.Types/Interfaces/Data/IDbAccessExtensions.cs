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
    using System;
    using System.Data;
    using System.Data.Common;

    using ServiceStack.OrmLite;

    /// <summary>
    ///     The DBAccess extensions.
    /// </summary>
    public static class IDbAccessExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The begin transaction.
        /// </summary>
        /// <param name="dbAccess">
        /// The DB access.
        /// </param>
        /// <param name="isolationLevel">
        /// The isolation level.
        /// </param>
        /// <returns>
        /// The <see cref="IDbTransaction"/> .
        /// </returns>
        public static IDbTransaction BeginTransaction([NotNull] this IDbAccess dbAccess, IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");

            return dbAccess.CreateConnectionOpen().BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="dbAccess">The DB access.</param>
        /// <returns>
        /// The <see cref="DbConnection" /> .
        /// </returns>
        [NotNull]
        public static DbConnection CreateConnection([NotNull] this IDbAccess dbAccess)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");

            var connection = dbAccess.DbProviderFactory.CreateConnection();
            connection.ConnectionString = dbAccess.Information.ConnectionString();

            return connection;
        }

        /// <summary>
        /// Get an open DB connection.
        /// </summary>
        /// <param name="dbAccess">
        /// The DB Access.
        /// </param>
        /// <returns>
        /// The <see cref="DbConnection"/> .
        /// </returns>
        [NotNull]
        public static DbConnection CreateConnectionOpen([NotNull] this IDbAccess dbAccess)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");

            var connection = dbAccess.CreateConnection();

            if (connection.State != ConnectionState.Open)
            {
                // open it up...
                connection.Open();
            }

            return connection;
        }

        /// <summary>
        /// Executes a non query in a transaction
        /// </summary>
        /// <param name="dbAccess">
        /// The DB access.
        /// </param>
        /// <param name="cmd">
        /// The command.
        /// </param>
        /// <param name="dbTransaction">
        /// The DB Transaction.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> .
        /// </returns>
        public static int ExecuteNonQuery(
            [NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, [CanBeNull] IDbTransaction dbTransaction = null)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");

            return dbAccess.Execute(c => c.ExecuteNonQuery(), cmd, dbTransaction);
        }

        /// <summary>
        /// Executes a non query in a transaction
        /// </summary>
        /// <param name="dbAccess">The database access.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="useTransaction">if set to <c>true</c> [use transaction].</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>Returns the Result</returns>
        public static int ExecuteNonQuery(
            [NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, bool useTransaction, IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");

            if (!useTransaction)
            {
                return dbAccess.ExecuteNonQuery(cmd);
            }

            using (var dbTransaction = dbAccess.BeginTransaction(isolationLevel))
            {
                var result = dbAccess.Execute(c => c.ExecuteNonQuery(), cmd, dbTransaction);
                dbTransaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="dbTransaction">The DB Transaction.</param>
        /// <returns>
        /// Returns the Data
        /// </returns>
        public static object ExecuteScalar(
            [NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, [CanBeNull] IDbTransaction dbTransaction = null)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");

            return dbAccess.Execute(c => c.ExecuteScalar(), cmd, dbTransaction);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>
        /// The <see cref="DataTable" /> .
        /// </returns>
        public static DataTable GetData(
            [NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, [CanBeNull] IDbTransaction dbTransaction = null)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");

            return dbAccess.GetDataset(cmd, dbTransaction).Tables[0];
        }

        /// <summary>
        /// Gets the dataset.
        /// </summary>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>
        /// The <see cref="DataSet" /> .
        /// </returns>
        public static DataSet GetDataset([NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, [CanBeNull] IDbTransaction dbTransaction = null)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");

            return dbAccess.Execute(
                c =>
                    {
                        var ds = new DataSet();

                        IDbDataAdapter dataAdapter = dbAccess.DbProviderFactory.CreateDataAdapter();

                        if (dataAdapter != null)
                        {
                            dataAdapter.SelectCommand = cmd;
                            dataAdapter.Fill(ds);
                        }

                        return ds;
                    }, 
                cmd, 
                dbTransaction);
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="dbTransaction">The DB transaction.</param>
        /// <returns>
        /// The <see cref="IDataReader" /> .
        /// </returns>
        public static IDataReader GetReader([NotNull] this IDbAccess dbAccess, [NotNull] IDbCommand cmd, [NotNull] IDbTransaction dbTransaction)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(cmd, "cmd");
            CodeContracts.VerifyNotNull(dbTransaction, "dbTransaction");

            return dbAccess.Execute(c => c.ExecuteReader(), cmd, dbTransaction);
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <typeparam name="T">The type parameter</typeparam>
        /// <param name="dbAccess">The DB access.</param>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        public static string GetTableName<T>(this IDbAccess dbAccess)
        {
            return OrmLiteConfig.DialectProvider.GetQuotedTableName(ModelDefinition<T>.Definition);
        }

        /// <summary>
        /// Insert the entity using the model provided.
        /// </summary>
        /// <typeparam name="T">The type parameter</typeparam>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="insert">The insert.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="selectIdentity">if set to <c>true</c> [select identity].</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public static long Insert<T>(
            [NotNull] this IDbAccess dbAccess,
            [NotNull] T insert,
            [CanBeNull] IDbTransaction transaction = null,
            bool selectIdentity = false) where T : IEntity
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");

            if (transaction != null && transaction.Connection != null)
            {
                using (var command = transaction.Connection.CreateCommand())
                {
                    OrmLiteConfig.DialectProvider.PrepareParameterizedInsertStatement<T>(command);
                    OrmLiteConfig.DialectProvider.SetParameterValues<T>(command, insert);

                    return selectIdentity
                               ? OrmLiteConfig.DialectProvider.InsertAndGetLastInsertId<T>(command)
                               : dbAccess.ExecuteNonQuery(command, transaction);
                }
            }

            // no transaction
            using (var connection = dbAccess.CreateConnectionOpen())
            {
                using (var command = connection.CreateCommand())
                {
                    OrmLiteConfig.DialectProvider.PrepareParameterizedInsertStatement<T>(command);
                    OrmLiteConfig.DialectProvider.SetParameterValues<T>(command, insert);

                    return selectIdentity
                               ? OrmLiteConfig.DialectProvider.InsertAndGetLastInsertId<T>(command)
                               : dbAccess.ExecuteNonQuery(command, transaction);
                }
            }
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="dbAccess">
        /// The DB access.
        /// </param>
        /// <param name="runFunc">
        /// The run function.
        /// </param>
        /// <typeparam name="T">The type Parameter</typeparam>
        /// <returns>
        /// The <see cref="T"/> .
        /// </returns>
        public static T Run<T>([NotNull] this IDbAccess dbAccess, Func<IDbConnection, T> runFunc)
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");
            CodeContracts.VerifyNotNull(runFunc, "runFunc");

            using (var connection = dbAccess.CreateConnectionOpen())
            {
                return runFunc(connection);
            }
        }

        /// <summary>
        /// Runs the update command.
        /// </summary>
        /// <typeparam name="T">The type Parameter</typeparam>
        /// <param name="dbAccess">The DB access.</param>
        /// <param name="update">The update.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public static int Update<T>([NotNull] this IDbAccess dbAccess, [NotNull] T update, [CanBeNull] IDbTransaction transaction = null)
            where T : IEntity
        {
            CodeContracts.VerifyNotNull(dbAccess, "dbAccess");

            if (transaction != null && transaction.Connection != null)
            {
                using (var command = transaction.Connection.CreateCommand())
                {
                    OrmLiteConfig.DialectProvider.PrepareParameterizedUpdateStatement<T>(command);
                    OrmLiteConfig.DialectProvider.SetParameterValues<T>(command, update);

                    return dbAccess.ExecuteNonQuery(command, transaction);
                }
            }

            // no transaction
            using (var connection = dbAccess.CreateConnectionOpen())
            {
                using (var command = connection.CreateCommand())
                {
                    OrmLiteConfig.DialectProvider.PrepareParameterizedUpdateStatement<T>(command);
                    OrmLiteConfig.DialectProvider.SetParameterValues<T>(command, update);

                    return dbAccess.ExecuteNonQuery(command, transaction);
                }
            }
        }

        #endregion
    }
}