// 🃏 The HossGame 🃏
// <copyright file="Game.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.Core.GameAggregate;

#region

#endregion

     public enum TeamId
     {
         NoTeamId,
         NorthSouth,
         EastWest,
     }
// public abstract class Game : AggregateRoot<GameId>, IAggregateRoot
// {
//     #region TeamId enum
//
//     public enum TeamId
//     {
//         NoTeamId,
//         NorthSouth,
//         EastWest,
//     }
//
//     #endregion
//
//     protected Game(GameId id)
//         : base(id)
//     {
//     }
//
//     public abstract void JoinPlayerToTeam(PlayerId playerId, TeamId teamId);
// }