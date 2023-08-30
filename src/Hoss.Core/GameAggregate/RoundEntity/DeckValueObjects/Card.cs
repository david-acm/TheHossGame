// 🃏 The HossGame 🃏
// <copyright file="Card.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.DeckValueObjects;

#region

using Hoss.SharedKernel;

#endregion

public record ACard(Rank Rank, Suit Suit) : Card
{
    public override Suit Suit { get; } = Suit;

    public override Rank Rank { get; } = Rank;

    public override string ToString()
    {
        return $"{this.Rank.Value} \t {this.Suit.Value}";
    }

    public void Deconstruct(out Suit Suit, out Rank Rank)
    {
        Suit = this.Suit;
        Rank = this.Rank;
    }
}

public record NoCard() : ACard(Rank.None, Suit.None);

public abstract record Card : ValueObject
{
    public abstract Suit Suit { get; }

    public abstract Rank Rank { get; }

    public static CardComparer CompareWhenTrump(Suit suit)
    {
        return new CardComparer(suit);
    }

    #region Nested type: CardComparer

    public sealed class CardComparer : IComparer<Card>
    {
        private readonly Suit? playedSuit;

        private readonly Suit trump;

        public CardComparer(Suit trump, Suit? playedSuit = null)
        {
            this.trump = trump;
            this.playedSuit = playedSuit;
        }

        #region IComparer<Card> Members

        public int Compare(Card? x, Card? y)
        {
            if (x == y)
                return 0;

            if (x is not null && y is null)
                return 1;

            if (x is null && y is not null)
                return -1;

            if (this.IsTrump(x) && !this.IsTrump(y))
                return 1;

            if (!this.IsTrump(x) && this.IsTrump(y))
                return -1;

            if (this.IsRightBar(x) && !this.IsRightBar(y))
                return 1;

            if (!this.IsRightBar(x) && this.IsRightBar(y))
                return -1;

            if (this.IsLeftBar(x) && !this.IsLeftBar(y))
                return 1;

            if (!this.IsLeftBar(x) && this.IsLeftBar(y))
                return -1;

            if (this.playedSuit is not null
                && CardSuit(x) == this.playedSuit && CardSuit(y) != this.playedSuit)
                return 1;

            if (this.playedSuit is not null
                && CardSuit(x) != this.playedSuit && CardSuit(y) == this.playedSuit)
                return -1;

            return CardRank(x) > CardRank(y) ? 1 : CardRank(x) < CardRank(y) ? -1 : 0;
        }

        #endregion

        private static Rank CardRank(Card? x)
        {
            return x!.Rank;
        }

        private static Suit CardSuit(Card? x)
        {
            return x!.Suit;
        }

        private bool IsRightBar(Card? x)
        {
            return x!.Rank == Rank.Jack && x!.Suit == this.trump;
        }

        private bool IsLeftBar(Card? x)
        {
            return x!.Rank == Rank.Jack &&
                   ((x!.Suit == Suit.Spades && this.trump == Suit.Clubs) ||
                    (x!.Suit == Suit.Clubs && this.trump == Suit.Spades) ||
                    (x!.Suit == Suit.Hearts && this.trump == Suit.Diamonds) ||
                    (x!.Suit == Suit.Diamonds && this.trump == Suit.Hearts));
        }

        private bool IsTrump(Card? x)
        {
            return x!.Suit == this.trump || this.IsLeftBar(x);
        }
    }

    #endregion
}