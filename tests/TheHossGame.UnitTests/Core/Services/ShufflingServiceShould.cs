// 🃏 The HossGame 🃏
// <copyright file="ShufflingServiceShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.Core.Interfaces;
using Xunit;

public class ShufflingServiceShould
{
   [Theory]
   [AutoOrderedDeckData]
   public void OrderTheDeckRandomly(
      [Frozen] Mock<IRandomNumberProvider> randomProvider,
      ShufflingService service,
      ADeck deck,
      RandomNumberProvider random)
   {
      int attemps = 0;
      int maxValue = 0;
      randomProvider.Setup(r => r.NextInt(It.IsAny<int>()))
         .Callback((int i) => maxValue = i)
         .Returns((int i) => random.NextInt(i));
      var cards = deck.Cards.ToList();
      var originalCards = cards.ToList();

      while (attemps < 1000)
      {
         attemps++;

         service.Shuffle(cards);
         cards.Should().NotContainInOrder(originalCards, $"at: {attemps}");

         cards.Should().HaveCount(24);
      }
   }
}
