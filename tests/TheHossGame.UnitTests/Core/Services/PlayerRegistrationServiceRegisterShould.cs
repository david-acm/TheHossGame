// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using Ardalis.Specification;
using AutoFixture.Xunit2;
using Moq;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.Services;
using TheHossGame.SharedKernel;
using TheHossGame.SharedKernel.Interfaces;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

public class PlayerRegistrationServiceRegisterShould
{
   [Theory]
   [AutoPlayerData]
   public async Task NotRegisterPlayerWhenPlayerNameIsNotUnique(
      [Frozen] Mock<IRepository<APlayer>> playerRepository,
      [Frozen] Mock<IEventStore<APlayer>> eventStore,
      PlayerRegistrationService service,
      APlayer player)
   {
      playerRepository
         .Setup(r => r.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!))
         .ReturnsAsync(true);

      await service.RegisterAsync(player);

      eventStore.Verify(
         store =>
            store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()),
         Times.Never);
   }

   [Theory]
   [AutoPlayerData]
   public async Task RegisterPlayerWhenPlayerNameIsUnique(
      [Frozen] Mock<IRepository<APlayer>> playerRepository,
      PlayerRegistrationService service,
      APlayer player)
   {
      await service.RegisterAsync(player);

      playerRepository.Verify(
         repo =>
            repo.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!),
         Times.Once);
   }

   [Theory]
   [AutoPlayerData]
   public async Task NotRegisterPlayerWhenPlayerIsAlreadyRegistered(
      [Frozen] Mock<IRepository<APlayer>> playerRepository,
      [Frozen] Mock<IEventStore<APlayer>> eventStore,
      PlayerRegistrationService service,
      APlayer player)
   {
      playerRepository
         .Setup(r => r.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!))
         .ReturnsAsync(true);

      await service.RegisterAsync(player);

      playerRepository.Verify(
         repo =>
            repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!),
         Times.Once);

      eventStore.Verify(
         store =>
            store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()),
         Times.Never);
   }

   [Theory]
   [AutoPlayerData]
   public async Task RegisterPlayerWhenPlayerIsNotRegistered(
      [Frozen] Mock<IRepository<APlayer>> playerRepository,
      [Frozen] Mock<IEventStore<APlayer>> eventStore,
      PlayerRegistrationService service,
      APlayer player)
   {
      await service.RegisterAsync(player);

      playerRepository.Verify(
         repo =>
            repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!),
         Times.Once);

      eventStore.Verify(
         store =>
            store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()),
         Times.Once);
   }
}