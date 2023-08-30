// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceRegisterShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace TheHossGame.UnitTests.Core.Services;

#region

using AutoFixture.Xunit2;
using Hoss.Core.PlayerAggregate;
using Hoss.Core.Services;
using Hoss.SharedKernel;
using Hoss.SharedKernel.Interfaces;
using Moq;
using TheHossGame.UnitTests.Core.PlayerAggregate.Generators;
using Xunit;

#endregion

public class PlayerRegistrationServiceRegisterShould
{
    [Theory]
    [PlayerData]
    public async Task NotRegisterPlayerWhenPlayerNameIsNotUnique([Frozen] Mock<IRepository<Player>> playerRepository,
        [Frozen] Mock<IEventStore<Player>> eventStore, PlayerRegistrationService service, Player player)
    {
        playerRepository.Setup(r => r.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!)).ReturnsAsync(true);

        await service.RegisterAsync(player);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Never);
    }

    [Theory]
    [PlayerData]
    public async Task RegisterPlayerWhenPlayerNameIsUnique([Frozen] Mock<IRepository<Player>> playerRepository,
        PlayerRegistrationService service, Player player)
    {
        await service.RegisterAsync(player);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!), Times.Once);
    }

    [Theory]
    [PlayerData]
    public async Task NotRegisterPlayerWhenPlayerIsAlreadyRegistered(
        [Frozen] Mock<IRepository<Player>> playerRepository,
        [Frozen] Mock<IEventStore<Player>> eventStore, PlayerRegistrationService service, Player player)
    {
        playerRepository.Setup(r => r.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!)).ReturnsAsync(true);

        await service.RegisterAsync(player);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!), Times.Once);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Never);
    }

    [Theory]
    [PlayerData]
    public async Task RegisterPlayerWhenPlayerIsNotRegistered([Frozen] Mock<IRepository<Player>> playerRepository,
        [Frozen] Mock<IEventStore<Player>> eventStore, PlayerRegistrationService service, Player player)
    {
        await service.RegisterAsync(player);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!), Times.Once);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Once);
    }
}