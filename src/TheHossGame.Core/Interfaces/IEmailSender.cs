// 🃏 The HossGame 🃏
// <copyright file="IEmailSender.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Interfaces;

using System.Threading.Tasks;

public interface IEmailSender
{
  Task SendEmailAsync(string destinatary, string from, string subject, string body);
}
