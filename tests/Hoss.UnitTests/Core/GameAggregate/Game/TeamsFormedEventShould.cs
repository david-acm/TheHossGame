// 🃏 The HossGame 🃏
// <copyright file="TeamsFormedEventShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.GameAggregate.Game;

#region

using FluentAssertions;
using Hoss.Core.GameAggregate.Events;
using Hoss.SharedKernel;
using TheHossGame.UnitTests.Core.Services;
using Xunit;

#endregion

public class TeamsFormedEventShould
{
   [Theory]
   [AutoMoqData]
   public void ShouldBeImmutable(TeamsFormedEvent @event)
   {
      @event.Should().BeAssignableTo<DomainEventBase>();
   }
}
