// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Services
{
   using System.Threading.Tasks;
   using TheHossGame.Core.PlayerAggregate;
   using TheHossGame.SharedKernel.Interfaces;

   public class PlayerRegistrationService
   {
      private readonly IRepository<Player> repository;

      public PlayerRegistrationService(IRepository<Player> repository)
      {
         this.repository = repository;
      }

      public async Task RegisterAsync(Player player)
      {
         var specification = new PlayerWithNameSpecification(player);
         if (await this.repository.AnyAsync(specification))
         {
#pragma warning disable S3626 // Jump statements should not be redundant
            return;
#pragma warning restore S3626 // Jump statements should not be redundant
         }
      }
   }
}
