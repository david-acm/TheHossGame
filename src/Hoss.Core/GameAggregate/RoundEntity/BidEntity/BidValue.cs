// 🃏 The HossGame 🃏
// <copyright file="BidValue.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.BidEntity;

using Ardalis.SmartEnum;

// public enum BidValue
// {
//    Pass,
//    One,
//    Two,
//    Three,
//    Four,
//    Five,
//    Six,
//    Hoss,
//    DoubleHoss,
// }

public sealed class BidValue : SmartEnum<BidValue, int>
{
    public static readonly BidValue Pass = new(nameof(Pass), 0);
    public static readonly BidValue One = new(nameof(One), 1);
    public static readonly BidValue Two = new(nameof(Two), 2);
    public static readonly BidValue Three = new(nameof(Three), 3);
    public static readonly BidValue Four = new(nameof(Four), 4);
    public static readonly BidValue Five = new(nameof(Five), 5);
    public static readonly BidValue Six = new(nameof(Six), 6);
    public static readonly BidValue Hoss = new(nameof(Hoss), 12);
    public static readonly BidValue DoubleHoss = new(nameof(DoubleHoss), 24);

    private BidValue(string name, int value)
        : base(name, value)
    {
    }
}