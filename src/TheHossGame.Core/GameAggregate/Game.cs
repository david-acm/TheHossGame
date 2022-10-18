// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;

public abstract class Game : AggregateRoot<GameId>, IAggregateRoot
{
   #region TeamId enum

   public enum TeamId
   {
      NoTeamId,
      Team1,
      Team2,
   }

   #endregion

   protected Game(GameId id)
      : base(id)
   {
   }
}
