// 🃏 The HossGame 🃏
// <copyright file="ValueId.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Hoss.SharedKernel;

/// <summary>
///    A value object with guid identity.
/// </summary>
public record ValueId : ValueObject
{
    /// <summary>
    ///    Initializes a new instance of the <see cref="ValueId" /> class.
    /// </summary>
    public ValueId()
    {
    }
}

/// <summary>
/// </summary>
public record NoValueId : ValueId
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValueId" /> class.
    /// </summary>
    public NoValueId()
    {
    }
}