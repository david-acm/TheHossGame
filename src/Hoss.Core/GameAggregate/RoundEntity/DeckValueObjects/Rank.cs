// 🃏 The HossGame 🃏
// <copyright file="CardRank.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Ardalis.SmartEnum;

#endregion

public sealed class Rank : SmartEnum<Rank, string>
{
    public static readonly Rank Ace = new(nameof(Ace), "A");

    public static readonly Rank King = new(nameof(King), "K");

    public static readonly Rank Queen = new(nameof(Queen), "Q");

    public static readonly Rank Jack = new(nameof(Jack), "J");

    public static readonly Rank Ten = new(nameof(Ten), "10");

    public static readonly Rank Nine = new(nameof(Nine), "9");

    public static readonly Rank None = new(nameof(None), string.Empty);

    private Rank(string name, string value)
        : base(name, value)
    {
    }

    public new static IReadOnlyCollection<Rank> List => SmartEnum<Rank, string>.List.Except
    (new List<Rank>
    {
        None,
    }).ToList().AsReadOnly();

    /// <inheritdoc />
    public override int CompareTo(SmartEnum<Rank, string> other)
    {
        var rankOrder = new List<Rank> {Ace, King, Queen, Jack, Ten, Nine};
        return rankOrder.IndexOf(this) < rankOrder.IndexOf((Rank) other) ? 1 :
            rankOrder.IndexOf(this) > rankOrder.IndexOf((Rank) other) ? -1 : 0;
    }
}