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

   public static readonly CardSuit None = new (nameof(None), ' ');

   private CardSuit(string name, char id)
      : base(name, id)
   {
      this.Id = id;
   }

   public static new IReadOnlyCollection<CardSuit> List => SmartEnum<CardSuit, char>.List.Except(new List<CardSuit>() { None }).ToList().AsReadOnly();

   public char Id { get; }
}
