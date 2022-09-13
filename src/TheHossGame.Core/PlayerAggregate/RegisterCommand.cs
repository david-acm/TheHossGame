// 🃏 The HossGame 🃏
// <copyright file="RegisterCommand.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

public class RegisterCommand
{
   public RegisterCommand(PlayerName name, PlayerEmail email)
   {
      this.Name = name;
      this.Email = email;
   }

   public PlayerName Name { get; internal set; }

   public PlayerEmail Email { get; internal set; }
}
