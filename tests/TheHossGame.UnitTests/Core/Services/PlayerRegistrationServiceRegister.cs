// 🃏 The HossGame 🃏
// <copyright file="PlayerRegistrationServiceRegister.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.UnitTests.Core.Services;

using Ardalis.Specification;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Moq;
using TheHossGame.Core.PlayerAggregate;
using TheHossGame.Core.Services;
using TheHossGame.SharedKernel.Interfaces;
using Xunit;

public class PlayerRegistrationServiceRegister
{
    [Theory]
    [AutoMoqData]
    public async Task CheckUserNameIsUnique(
       [Frozen] Mock<IRepository<Player>> playerRepository,
       PlayerRegistrationService service,
       Player player)
    {
        await service.RegisterAsync(player);

        playerRepository.Verify(
            repo => repo.AnyAsync(
                It.IsAny<Specification<Player>>(),
                default!),
            Times.Once);
    }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}
