// 🃏 The HossGame 🃏
// <copyright file="CardSuit.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.RoundAggregate;

using Ardalis.SmartEnum;

public sealed class CardSuit : SmartEnum<CardSuit, char>
{
   public static readonly CardSuit Hearts = new (nameof(Hearts), '♥');

   public static readonly CardSuit Diamonds = new (nameof(Diamonds), '♦');

   public static readonly CardSuit Clubs = new (nameof(Clubs), '♣');

   public static readonly CardSuit Spades = new (nameof(Spades), '♠');

   private CardSuit(string name, char id)
      : base(name, id)
   {
      this.Id = id;
   }

   public char Id { get; }
}
