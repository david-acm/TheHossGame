// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

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
    [AutoMoqData]
    [AutoPlayerData]
    public async Task CheckUserNameIsUnique(
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
    [AutoMoqData]
    [AutoPlayerData]
    public async Task CheckPlayerIsNotRegistered(
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
