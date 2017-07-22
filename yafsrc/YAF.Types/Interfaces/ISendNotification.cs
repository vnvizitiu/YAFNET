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
namespace YAF.Types.Interfaces
{
    using System;
    using System.Web.Security;

    /// <summary>
    /// The SendNotification Interface
    /// </summary>
    public interface ISendNotification
    {
        /// <summary>
        /// Sends Notifications to Moderators that Message Needs Approval
        /// </summary>
        /// <param name="forumId">The forum id.</param>
        /// <param name="newMessageId">The new message id.</param>
        /// <param name="isSpamMessage">if set to <c>true</c> [is spam message].</param>
        void ToModeratorsThatMessageNeedsApproval(int forumId, int newMessageId, bool isSpamMessage);

        /// <summary>
        /// Sends Notifications to Moderators that a Message was Reported
        /// </summary>
        /// <param name="pageForumID">
        /// The page Forum ID.
        /// </param>
        /// <param name="reportedMessageId">
        /// The reported message id.
        /// </param>
        /// <param name="reporter">
        /// The reporter.
        /// </param>
        /// <param name="reportText">
        /// The report Text.
        /// </param>
        void ToModeratorsThatMessageWasReported(int pageForumID, int reportedMessageId, int reporter, string reportText);

        /// <summary>
        /// Sends notification about new PM in user's inbox.
        /// </summary>
        /// <param name="toUserId">
        /// User supposed to receive notification about new PM.
        /// </param>
        /// <param name="subject">
        /// Subject of PM user is notified about.
        /// </param>
        void ToPrivateMessageRecipient(int toUserId, [NotNull] string subject);

        /// <summary>
        /// The to watching users.
        /// </summary>
        /// <param name="newMessageId">
        /// The new message id.
        /// </param>
        void ToWatchingUsers(int newMessageId);

        /// <summary>
        /// Send an Email to the Newly Created User with
        /// his Account Info (Pass, Security Question and Answer)
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="pass">
        /// The pass.
        /// </param>
        /// <param name="securityAnswer">
        /// The security answer.
        /// </param>
        /// <param name="templateName">
        /// The template Name.
        /// </param>
        void SendRegistrationNotificationToUser(
            [NotNull] MembershipUser user,
            [NotNull] string pass,
            [NotNull] string securityAnswer,
            string templateName);

        /// <summary>
        /// Sends notification that the User was awarded with a Medal
        /// </summary>
        /// <param name="toUserId">To user id.</param>
        /// <param name="medalName">Name of the medal.</param>
        void ToUserWithNewMedal([NotNull] int toUserId, [NotNull] string medalName);

        /// <summary>
        /// The send registration notification email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userId">The user id.</param>
        void SendRegistrationNotificationEmail([NotNull] MembershipUser user, int userId);

        /// <summary>
        /// Sends a spam bot notification to admins.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userId">The user id.</param>
        void SendSpamBotNotificationToAdmins([NotNull] MembershipUser user, int userId);

        /// <summary>
        /// Sends the user welcome notification.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userId">The user identifier.</param>
        void SendUserWelcomeNotification([NotNull] MembershipUser user, int? userId);

        /// <summary>
        /// Sends the verification email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="email">The email.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="newUsername">The new username.</param>
        void SendVerificationEmail(
            [NotNull] MembershipUser user,
            [NotNull] string email,
            int? userID,
            string newUsername = null);

        /// <summary>
        /// Sends the user a suspension notification.
        /// </summary>
        /// <param name="suspendedUntil">The suspended until.</param>
        /// <param name="suspendReason">The suspend reason.</param>
        /// <param name="email">The email.</param>
        /// <param name="userName">Name of the user.</param>
        void SendUserSuspensionNotification(
            [NotNull] DateTime suspendedUntil,
            [NotNull] string suspendReason,
            [NotNull] string email,
            [NotNull] string userName);

        /// <summary>
        /// Sends the user a suspension ended notification.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="userName">Name of the user.</param>
        void SendUserSuspensionEndedNotification(
            [NotNull] string email,
            [NotNull] string userName);
    }
}