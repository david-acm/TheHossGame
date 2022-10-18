// 🃏 The HossGame 🃏
// <copyright file="Bid.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.GameAggregate.RoundEntity.BidEntity;

using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel;

public record Bid(PlayerId PlayerId, BidValue Value)
   : ValueObject;
