// 🃏 The HossGame 🃏
// <copyright file="CardShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Round;

using FluentAssertions;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Xunit;

public class CardShould
{
   [Fact]
   public void ShouldSerializeWithRankAndSuitInformation()
   {
      var rank = Rank.Ace;
      var suit = Suit.Clubs;
      var card = new ACard(suit, rank);
      card.ToString().Should().ContainAll(suit.Value.ToString(), rank.Value);
   }
}
