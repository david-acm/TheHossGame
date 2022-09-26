// 🃏 The HossGame 🃏
// <copyright file="TeamPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using static TheHossGame.Core.GameAggregate.Game;

public abstract record class TeamPlayer(PlayerId PlayerId, TeamId TeamId)
{
   public bool IsReady { get; init; }
}

public record ATeamPlayer(PlayerId PlayerId, TeamId TeamId)
   : TeamPlayer(PlayerId, TeamId)
{
}

public record NoTeamPlayer()
   : TeamPlayer(new NoPlayerId(), TeamId.NoTeamId)
{
}
