// 🃏 The HossGame 🃏
// <copyright file="ADeck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Interfaces;

#endregion

public sealed record ADeck : Deck
{
    private readonly Stack<ACard> cards = new();

    private ADeck(IShufflingService shufflingService)
    {
        var cardList = new List<ACard>();
        cardList.AddRange(Suit.List.SelectMany(suit => Rank.List.Select(rank => new ACard(rank, suit))));
        shufflingService.Shuffle(cardList);
        cards = new Stack<ACard>(cardList);
    }

    public IReadOnlyList<ACard> Cards => cards.ToList().AsReadOnly();

    public override bool HasCards => cards.Any();

    public static ADeck ShuffleNew(IShufflingService shufflingService)
    {
        return new ADeck(shufflingService);
    }

    public override Card Deal()
    {
        return cards.Pop();
    }
}