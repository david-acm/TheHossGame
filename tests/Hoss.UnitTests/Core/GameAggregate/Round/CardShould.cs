// 🃏 The HossGame 🃏
// <copyright file="CardShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using PlayerAggregate.Generators;
using Xunit;
using static Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects.Rank;
using static Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects.Suit;

public class CardShould
{
    [Fact]
    public void ShouldSerializeWithRankAndSuitInformation()
    {
        var card = new ACard(Ace, Clubs);
        card.ToString().Should().ContainAll(Clubs.Value.ToString(), Ace.Value);
    }

    [Theory]
    [PlayerData]
    public void CompareCorrectly(ADeck deck)
    {
        var spadeIsTrumpComparer = Card.CompareWhenTrump(Spades);

        var orderedDeck = deck.Cards.OrderBy(c => c, spadeIsTrumpComparer)
            .OrderByDescending(c => c, spadeIsTrumpComparer)
            .ToList();

        orderedDeck.Should().ContainInOrder
        (new ACard(Jack, Spades),
            new ACard(Jack, Clubs),
            new ACard(Ace, Spades),
            new ACard(King, Spades),
            new ACard(Queen, Spades),
            new ACard(Ten, Spades),
            new ACard(Nine, Spades));

        orderedDeck[7].Rank.Should().Be(Ace);
        orderedDeck[8].Rank.Should().Be(Ace);
        orderedDeck[9].Rank.Should().Be(Ace);

        spadeIsTrumpComparer.Compare(orderedDeck[7], orderedDeck[8]).Should().Be(0);
        spadeIsTrumpComparer.Compare(orderedDeck[8], orderedDeck[7]).Should().Be(0);

        orderedDeck[10].Rank.Should().Be(King);
        orderedDeck[11].Rank.Should().Be(King);
        orderedDeck[12].Rank.Should().Be(King);

        orderedDeck[13].Rank.Should().Be(Queen);
        orderedDeck[14].Rank.Should().Be(Queen);
        orderedDeck[15].Rank.Should().Be(Queen);

        orderedDeck[16].Rank.Should().Be(Jack);
        orderedDeck[17].Rank.Should().Be(Jack);

        orderedDeck[18].Rank.Should().Be(Ten);
        orderedDeck[19].Rank.Should().Be(Ten);
        orderedDeck[20].Rank.Should().Be(Ten);

        orderedDeck[21].Rank.Should().Be(Nine);
        orderedDeck[22].Rank.Should().Be(Nine);
        orderedDeck[23].Rank.Should().Be(Nine);
    }
}