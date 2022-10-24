// 🃏 The HossGame 🃏
// <copyright file="IEmailSender.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.Interfaces;

public interface IEmailSender
{
   Task SendEmailAsync(string addressee, string from, string subject, string body);
}
