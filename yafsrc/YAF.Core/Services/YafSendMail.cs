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

namespace YAF.Core.Services
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using YAF.Classes;
    using YAF.Types;
    using YAF.Types.Interfaces;

    #endregion

    /// <summary>
    ///     Functions to send email via SMTP
    /// </summary>
    public class YafSendMail : ISendMail
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sends all MailMessages via the SmtpClient. Doesn't handle any exceptions.
        /// </summary>
        /// <param name="messages">
        ///     The messages.
        /// </param>
        public void SendAll([NotNull] IEnumerable<MailMessage> messages, [CanBeNull] Action<MailMessage, Exception> handleException = null)
        {
            CodeContracts.VerifyNotNull(messages, "messages");

            using (var smtpClient = new SmtpClient { EnableSsl = Config.UseSMTPSSL })
            {
                // send the message...
                foreach (var m in messages.ToList())
                {
                    try
                    {
                        // send the message...
                        smtpClient.Send(m);
                    }
                    catch (Exception ex)
                    {
                        if (handleException != null)
                        {
                            handleException(m, ex);
                        }
                        else
                        {
                            // don't handle here...
                            throw;
                        }
                    }
                }
            }
        }

        #endregion
    }
}