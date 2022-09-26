// 🃏 The HossGame 🃏
// <copyright file="TeamPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;

using TheHossGame.Core.PlayerAggregate;
using static TheHossGame.Core.GameAggregate.Game;

public record TeamPlayer(APlayerId PlayerId, TeamId TeamId);
