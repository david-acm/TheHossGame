// 🃏 The HossGame 🃏
// <copyright file="ReadyGameGenerator.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.PlayerAggregate.Generators;

using AutoFixture.Kernel;
using TheHossGame.Core.GameAggregate;

public class ReadyGameGenerator : ISpecimenBuilder
{
   private ISpecimenContext? context;

   public object Create(object request, ISpecimenContext context)
   {
      this.context = context;
      if (!typeof(AGame).Equals(request))
      {
         return new NoSpecimen();
      }

      return GeneratePlayerEnumerable(request, context);
   }

   private static object GeneratePlayerEnumerable(object request, ISpecimenContext context)
   {
      var generator = new PlayerEnumerableGenerator();

      request = typeof(IEnumerable<TheHossGame.Core.PlayerAggregate.Player>);

      var playerList = ((IEnumerable<TheHossGame.Core.PlayerAggregate.Player>)generator.Create(request, context)).ToList();

      var readyGame = AGame.CreateNewForPlayer(playerList.First().Id);
      readyGame.JoinPlayerToTeam(playerList[0].Id, Game.TeamId.Team1);
      readyGame.JoinPlayerToTeam(playerList[1].Id, Game.TeamId.Team1);
      readyGame.JoinPlayerToTeam(playerList[2].Id, Game.TeamId.Team2);
      readyGame.JoinPlayerToTeam(playerList[3].Id, Game.TeamId.Team2);
      playerList.ForEach(p => readyGame.TeamPlayerReady(p.Id));

      return readyGame;
   }
}