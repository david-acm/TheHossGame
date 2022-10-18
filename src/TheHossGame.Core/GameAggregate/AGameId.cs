// 🃏 The HossGame 🃏
// <copyright file="AGameId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate;
using TheHossGame.SharedKernel;

public abstract record GameId : ValueId;

public record AGameId : GameId;

public record NoGameId : GameId;
