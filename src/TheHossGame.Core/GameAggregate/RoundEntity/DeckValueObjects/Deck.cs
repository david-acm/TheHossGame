// 🃏 The HossGame 🃏
// <copyright file="Deck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.DeckValueObjects;
using TheHossGame.SharedKernel;

public abstract record Deck : ValueObject
{
   public abstract IReadOnlyList<ACard> Cards { get; }

   public abstract bool HasCards { get; }

   public abstract Card Deal();
}
