// 🃏 The HossGame 🃏
// <copyright file="PlayerEmail.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Hoss.Core.ProfileAggregate;

#region



#endregion

public record ProfileEmailBase : ValueObject;

public record NoProfileEmail : ProfileEmailBase;

public record ProfileEmail : ProfileEmailBase
{
    private ProfileEmail(string address)
    {
        Guard.Against.NullOrEmpty(address, nameof(address));
        Guard.Against.InvalidInput(address, nameof(address), _ => new EmailAddressAttribute().IsValid(address));

        Address = address;
    }

    public string Address { get; }

    public static ProfileEmail FromString(string email)
    {
        return new ProfileEmail(email);
    }
}