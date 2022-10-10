// 🃏 The HossGame 🃏
// <copyright file="NoGame.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;

public class NoGame : Game
{
   public NoGame()
      : base(new NoGameId())
   {
   }

   public override bool IsNull => true;

   public override void JoinPlayerToTeam(PlayerId playerId, TeamId teamId)
   {
   }

   public override void CreateNewGame(PlayerId playerId)
   {
   }

   public override void TeamPlayerReady(PlayerId playerId)
   {
   }
}
