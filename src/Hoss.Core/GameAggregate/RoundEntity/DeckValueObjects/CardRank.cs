// 🃏 The HossGame 🃏
// <copyright file="CardRank.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

   #region

using Ardalis.SmartEnum;

#endregion

public sealed class CardRank : SmartEnum<CardRank, string>
{
   public static readonly CardRank Ace = new (nameof(Ace), "A");

   public static readonly CardRank King = new (nameof(King), "K");

   public static readonly CardRank Queen = new (nameof(Queen), "Q");

   public static readonly CardRank Jack = new (nameof(Jack), "J");

   public static readonly CardRank Ten = new (nameof(Ten), "10");

   public static readonly CardRank Nine = new (nameof(Nine), "9");

   public static readonly CardRank None = new (nameof(None), string.Empty);

   private CardRank(string name, string value)
      : base(name, value)
   {
   }

   public new static IReadOnlyCollection<CardRank> List => SmartEnum<CardRank, string>.List.Except
      (new List<CardRank>
      {
         None,
      }).ToList().AsReadOnly();
}
