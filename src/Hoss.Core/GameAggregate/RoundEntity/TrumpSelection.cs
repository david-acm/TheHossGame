// 🃏 The HossGame 🃏
// <copyright file="TrumpSelection.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using DeckValueObjects;

public record TrumpSelection(PlayerId PlayerId, Suit Suit) : Play(PlayerId);
