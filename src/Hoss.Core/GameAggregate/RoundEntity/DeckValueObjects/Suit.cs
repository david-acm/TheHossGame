// 🃏 The HossGame 🃏
// <copyright file="CardSuit.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

   #region

using Ardalis.SmartEnum;

#endregion

public sealed class Suit : SmartEnum<Suit, char>
{
   public static readonly Suit Hearts = new (nameof(Hearts), '♥');

   public static readonly Suit Diamonds = new (nameof(Diamonds), '♦');

   public static readonly Suit Clubs = new (nameof(Clubs), '♣');

   public static readonly Suit Spades = new (nameof(Spades), '♠');

   public static readonly Suit None = new (nameof(None), ' ');

   private Suit(string name, char id)
      : base(name, id)
   {
   }

   public new static IReadOnlyCollection<Suit> List => SmartEnum<Suit, char>.List.Except
      (new List<Suit>
      {
         None,
      }).ToList().AsReadOnly();
}
