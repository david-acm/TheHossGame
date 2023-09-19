// 🃏 The HossGame 🃏
// <copyright file="Bid.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

namespace Hoss.Core.GameAggregate.RoundEntity.BidValueObject;

#region



#endregion

public record Bid(PlayerId PlayerId, BidValue Value) : Play(PlayerId)
{
    public static bool operator >(Bid bid, Bid other) => bid.Value > other.Value;

    public static bool operator <(Bid bid, Bid other) => bid.Value < other.Value;

    public static implicit operator BidValue(Bid bid) => bid.Value;
}

public record HossRequest(PlayerId PlayerId, Card Card) : Play(PlayerId);

public record HossPartnerCard(PlayerId PlayerId, Card Card) : Play(PlayerId);