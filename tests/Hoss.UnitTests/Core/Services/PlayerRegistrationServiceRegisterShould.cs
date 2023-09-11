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
    public async Task NotRegisterPlayerWhenPlayerNameIsNotUnique([Frozen] Mock<IRepository<Profile>> playerRepository,
        [Frozen] Mock<IEventStore<Profile>> eventStore, PlayerRegistrationService service, Profile profile)
    {
        playerRepository.Setup(r => r.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!)).ReturnsAsync(true);

        await service.RegisterAsync(profile);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Never);
    }

    [Theory]
    [PlayerData]
    public async Task RegisterPlayerWhenPlayerNameIsUnique([Frozen] Mock<IRepository<Profile>> playerRepository,
        PlayerRegistrationService service, Profile profile)
    {
        await service.RegisterAsync(profile);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithNameSpec>(), default!), Times.Once);
    }

    [Theory]
    [PlayerData]
    public async Task NotRegisterPlayerWhenPlayerIsAlreadyRegistered(
        [Frozen] Mock<IRepository<Profile>> playerRepository,
        [Frozen] Mock<IEventStore<Profile>> eventStore, PlayerRegistrationService service, Profile profile)
    {
        playerRepository.Setup(r => r.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!)).ReturnsAsync(true);

        await service.RegisterAsync(profile);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!), Times.Once);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Never);
    }

    [Theory]
    [PlayerData]
    public async Task RegisterPlayerWhenPlayerIsNotRegistered([Frozen] Mock<IRepository<Profile>> playerRepository,
        [Frozen] Mock<IEventStore<Profile>> eventStore, PlayerRegistrationService service, Profile profile)
    {
        await service.RegisterAsync(profile);

        playerRepository.Verify(repo => repo.AnyAsync(It.IsAny<PlayerWithEmailSpec>(), default!), Times.Once);

        eventStore.Verify(store => store.PushEventsAsync(It.IsAny<IEnumerable<DomainEventBase>>()), Times.Once);
    }
}