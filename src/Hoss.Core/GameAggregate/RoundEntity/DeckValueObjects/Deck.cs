// 🃏 The HossGame 🃏
// <copyright file="Deck.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Hoss.SharedKernel;

#endregion

public abstract record Deck : ValueObject
{
   public abstract bool HasCards { get; }

   public abstract Card Deal();
}
