// 🃏 The HossGame 🃏
// <copyright file="PlayerEmail.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;

public class PlayerEmail
{
   public PlayerEmail(string address)
   {
      Guard.Against.NullOrEmpty(address);

      this.Address = address;
   }

   public string Address { get; }
}