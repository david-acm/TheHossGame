// 🃏 The HossGame 🃏
// <copyright file="Bid.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity.BidEntity;

#region

using Hoss.Core.PlayerAggregate;
using Hoss.SharedKernel;

#endregion

public record Bid(PlayerId PlayerId, BidValue Value) : ValueObject;
