// 🃏 The HossGame 🃏
// <copyright file="BidCommand.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record BidCommand
   : ValueObject
{
   public BidCommand(PlayerId PlayerId, BidValue Value)
   {
      this.PlayerId = PlayerId;
      this.Value = Value;
   }

   public PlayerId PlayerId { get; }
   public BidValue Value { get; }
}
