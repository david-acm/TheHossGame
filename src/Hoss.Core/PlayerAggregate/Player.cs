// 🃏 The HossGame 🃏
// <copyright file="Player.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.PlayerAggregate;

   #region

using Hoss.Core.GameAggregate;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;

#endregion

public abstract class Player : EntityBase<PlayerId>, IAggregateRoot
{
   protected Player(PlayerId id)
      : base(id)
   {
      this.JoiningGameId = new NoGameId();
   }

   protected bool IsJoiningGame => this.JoiningGameId is AGameId;

   protected GameId JoiningGameId { get; set; }
}

[Serializable]
public class InvalidEntityStateException : Exception
{
   public InvalidEntityStateException()
   {
   }

   public InvalidEntityStateException(string? message)
      : base(message)
   {
   }
}
