// 🃏 The HossGame 🃏
// <copyright file="ShufflingServiceShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

#region

using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.Interfaces;
using Moq;
using Xunit;

#endregion

public class ShufflingServiceShould
{
   [Theory]
   [AutoOrderedDeckData]
   public void OrderTheDeckRandomly([Frozen] Mock<IRandomNumberProvider> randomProvider, ShufflingService service, ADeck deck, RandomNumberProvider random)
   {
      var attempts = 0;
      randomProvider.Setup(r => r.NextInt(It.IsAny<int>())).Callback
         ((int _) =>
         {
         }).Returns((int i) => random.NextInt(i));
      var cards = deck.Cards.ToList();
      var originalCards = cards.ToList();

      while (attempts < 1000)
      {
         attempts++;

         service.Shuffle(cards);
         cards.Should().NotContainInOrder(originalCards, $"at: {attempts}");

         cards.Should().HaveCount(24);
      }
   }
}
