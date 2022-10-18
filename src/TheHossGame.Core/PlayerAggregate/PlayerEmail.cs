﻿// 🃏 The HossGame 🃏
// <copyright file="PlayerEmail.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.PlayerAggregate;

using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using TheHossGame.SharedKernel;

public record PlayerEmail : ValueObject
{
   private PlayerEmail(string address)
   {
      Guard.Against.NullOrEmpty(address);
      Guard.Against.InvalidInput(address, nameof(address), _ => new EmailAddressAttribute().IsValid(address));

      this.Address = address;
   }

   public string Address { get; }

   public static PlayerEmail FromString(string email)
   {
      return new PlayerEmail(email);
   }
}
