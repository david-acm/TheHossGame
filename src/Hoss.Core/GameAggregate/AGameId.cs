// 🃏 The HossGame 🃏
// <copyright file="AGameId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

public record GameId(Guid Id) : ValueId
{
  public static implicit operator GameId(Guid g) => new((Guid)g);

  public static implicit operator Guid?(GameId g) => g.Id;
}

public record AGameId(Guid Id) : GameId(Id)
{
  public static implicit operator Guid?(AGameId g) => g.Id;
  public static implicit operator AGameId(Guid g) => new(g);
}

public record NoGameId() : GameId(Guid.Empty);