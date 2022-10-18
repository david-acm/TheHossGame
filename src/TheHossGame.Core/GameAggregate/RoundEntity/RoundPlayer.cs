// 🃏 The HossGame 🃏
// <copyright file="RoundPlayer.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;
using static Game;

public record RoundPlayer(PlayerId PlayerId, TeamId TeamId)
   : ValueObject;
