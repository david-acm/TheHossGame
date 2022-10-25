// 🃏 The HossGame 🃏
// <copyright file="Bid.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.BidEntity;

   #region

using Hoss.Core.PlayerAggregate;

#endregion

public record Bid(PlayerId PlayerId, BidValue Value) : Play(PlayerId)
{
   public static bool operator >(Bid bid, Bid other) => bid.Value > other.Value;

   public static bool operator <(Bid bid, Bid other) => bid.Value < other.Value;

   public static implicit operator BidValue(Bid bid) => bid.Value;
}
