// 🃏 The HossGame 🃏
// <copyright file="RoundId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate.RoundEntity;

#region

#endregion

public record RoundId(Guid? Id = null) : ValueId
{
  public static implicit operator RoundId(Guid s) => new((Guid?)s);
  public static implicit operator Guid?(RoundId s) => s.Id;
}