// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationService.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Core.Services;

using System.Threading.Tasks;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.SharedKernel.Interfaces;

public class PlayerRegistrationService
{
   private readonly IRepository<APlayer> repository;
   private readonly IEventStore<APlayer> store;

   public PlayerRegistrationService(
      IRepository<APlayer> repository,
      IEventStore<APlayer> store)
   {
      this.repository = repository;
      this.store = store;
   }

   public async Task RegisterAsync(APlayer player)
   {
      bool playerIsRegistered = await this.PlayerIsRegistered(player);
      if (playerIsRegistered)
      {
         return;
      }

      bool playerNameIsUnique = await this.UserNameIsUniqueAsync(player);
      if (playerNameIsUnique)
      {
#pragma warning disable S3626 // Jump statements should not be redundant
         return;
#pragma warning restore S3626 // Jump statements should not be redundant
      }

      await this.store.PushEventsAsync(player.Events);
   }

   private async Task<bool> UserNameIsUniqueAsync(APlayer player)
   {
      var specification = new PlayerWithNameSpec(player);
      return await this.repository.AnyAsync(specification);
   }

   private async Task<bool> PlayerIsRegistered(APlayer player)
   {
      var specification = new PlayerWithEmailSpec(player);
      return await this.repository.AnyAsync(specification);
   }
}
