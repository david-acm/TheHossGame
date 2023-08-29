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
    public static readonly Suit Hearts = new(nameof(Hearts), '♥');

    public static readonly Suit Diamonds = new(nameof(Diamonds), '♦');

    public static readonly Suit Clubs = new(nameof(Clubs), '♣');

    public static readonly Suit Spades = new(nameof(Spades), '♠');

    public static readonly Suit None = new(nameof(None), ' ');

    private Suit(string name, char id)
        : base(name, id)
    {
    }

    public new static IReadOnlyCollection<Suit> List => SmartEnum<Suit, char>.List.Except
    (new List<Suit>
    {
        None,
    }).ToList().AsReadOnly();

    #region Nested type: SuitComparer

    public sealed class SuitComparer : IEqualityComparer<Card>
    {
        private readonly Suit trump;

        public SuitComparer(Suit trump)
        {
            this.trump = trump;
        }

        #region IEqualityComparer<Card> Members

        public bool Equals(Card? x, Card? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return x.Suit.Equals(y.Suit) ||
                   (this.IsTrump(x) && this.IsLeftBar(y)) ||
                   (this.IsTrump(y) && this.IsLeftBar(x));
        }

        public int GetHashCode(Card obj)
        {
            return obj.Suit.GetHashCode();
        }

        #endregion

        private bool IsLeftBar(Card? x)
        {
            return x!.Rank == Rank.Jack &&
                   ((x!.Suit == Spades && this.trump == Clubs) ||
                    (x!.Suit == Clubs && this.trump == Spades) ||
                    (x!.Suit == Hearts && this.trump == Diamonds) ||
                    (x!.Suit == Diamonds && this.trump == Hearts));
        }

        private bool IsTrump(Card? x)
        {
            return x!.Suit == this.trump || this.IsLeftBar(x);
        }
    }

    #endregion
}