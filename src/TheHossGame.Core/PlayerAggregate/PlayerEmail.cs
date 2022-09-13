// 🃏 The HossGame 🃏
// <copyright file="PlayerEmail.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;

public class PlayerEmail : ValueObject
{
   public PlayerEmail(string address)
   {
      Guard.Against.NullOrEmpty(address);

      this.Address = address;
   }

   public string Address { get; private set; }
}