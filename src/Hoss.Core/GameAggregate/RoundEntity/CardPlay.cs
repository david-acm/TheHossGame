// 🃏 The HossGame 🃏
// <copyright file="CardPlay.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using DeckValueObjects;

public record CardPlay(PlayerId PlayerId, Card Card) : Play(PlayerId);
