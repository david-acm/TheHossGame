// 🃏 The HossGame 🃏
// <copyright file="Round.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using TheHossGame.Core.GameAggregate;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public class Round : EntityBase
{
   public Round(GameId gameId, PlayerId firstBidder)
   {
      this.GameId = gameId;
      this.FirstBidder = firstBidder;
   }

   public GameId GameId { get; }

   public PlayerId FirstBidder { get; }

   public static Round StartNew(GameId gameId, PlayerId playerId)
   {
      return new (gameId, playerId);
   }

   protected override void EnsureValidState()
   {
      throw new NotImplementedException();
   }

   protected override void When(DomainEventBase @event)
   {
      throw new NotImplementedException();
   }
}