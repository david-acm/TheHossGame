// 🃏 The HossGame 🃏
// <copyright file="PlayerEmailShould.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using Hoss.Core.ProfileAggregate;

namespace TheHossGame.UnitTests.Core.PlayerAggregate;

#region

using System.Reflection;
using System.Runtime.CompilerServices;
using AutoFixture.Xunit2;
using FluentAssertions;
using Hoss.SharedKernel;
using Generators;
using Xunit;

#endregion

public class PlayerEmailShould
{
    [Theory]
    [PlayerData]
    public void HaveValueComparison(ProfileEmail email)
    {
        var otherEmail = ProfileEmail.FromString(email.Address);

        (email == otherEmail).Should().BeTrue();
    }

    [Fact]
    public void BeImmutable()
    {
        var customAttributes = typeof(ProfileEmail).GetTypeInfo().DeclaredProperties
            .SelectMany(p => p.GetCustomAttributes(true));
        var isRecord = customAttributes.FirstOrDefault() is CompilerGeneratedAttribute;

        isRecord.Should().BeTrue();
        typeof(ProfileEmail).Should().BeAssignableTo<ValueObject>();
    }

    [Theory]
    [PlayerData]
    public void NotBeNull(ProfileEmail email)
    {
        email.Should().NotBeNull();
    }

    [Theory]
    [AutoData]
    public void ThrowArgumentExceptionWhenEmailIsNotValid(string emailAddress)
    {
        var playerEmail = () => ProfileEmail.FromString(emailAddress);

        playerEmail.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentNullExceptionWhenEmailIsEmpty()
    {
        var playerEmail = () => ProfileEmail.FromString(string.Empty);

        playerEmail.Should().Throw<ArgumentException>();
    }
}