// 🃏 The HossGame 🃏
// <copyright file="TrumpSelection.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

using Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;
using Hoss.Core.PlayerAggregate;

public record TrumpSelection(PlayerId PlayerId, Suit Suit) : Play(PlayerId);
