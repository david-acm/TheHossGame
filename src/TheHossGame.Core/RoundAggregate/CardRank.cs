// 🃏 The HossGame 🃏
// <copyright file="CardRank.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using Ardalis.SmartEnum;

public sealed class CardRank : SmartEnum<CardRank, string>
{
   public static readonly CardRank Ace = new (nameof(Ace), "A");
   public static readonly CardRank King = new (nameof(King), "K");
   public static readonly CardRank Queen = new (nameof(Queen), "Q");
   public static readonly CardRank Jack = new (nameof(Jack), "J");
   public static readonly CardRank Ten = new (nameof(Ten), "10");
   public static readonly CardRank Nine = new (nameof(Nine), "9");

   private CardRank(string name, string value)
      : base(name, value)
   {
   }
}
