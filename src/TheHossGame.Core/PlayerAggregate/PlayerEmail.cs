// 🃏 The HossGame 🃏
// <copyright file="PlayerEmail.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using Ardalis.GuardClauses;
using System.ComponentModel.DataAnnotations;
using TheHossGame.SharedKernel;

public record PlayerEmail : ValueObject
{
   public static PlayerEmail FromString(string email) =>
      new (email);

   private PlayerEmail(string address)
   {
      Guard.Against.NullOrEmpty(address);
      Guard.Against.InvalidInput(address, nameof(address), a => new EmailAddressAttribute().IsValid(address));

      this.Address = address;
   }

   public string Address { get; private set; }
}