// 🃏 The HossGame 🃏
// <copyright file="ValueId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

namespace Hoss.SharedKernel;

/// <summary>
///    A value object with guid identity.
/// </summary>
public abstract record ValueId : ValueObject
{
    /// <summary>
    ///    Initializes a new instance of the <see cref="ValueId" /> class.
    /// </summary>
    protected ValueId(Guid? guid = null)
    {
        this.Id = guid ?? Guid.NewGuid();
    }

    /// <summary>
    /// </summary>
    public Guid Id { get; }
}

/// <summary>
/// </summary>
public record NoValueId : ValueId
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValueId" /> class.
    /// </summary>
    public NoValueId() : base(Guid.Empty)
    {
    }
}