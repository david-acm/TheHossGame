// ---
// 🃏 The HossGame 🃏
// <copyright file="IgnoreMemberAttribute.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// ---

namespace TheHossGame.SharedKernel;

/// <summary>
/// An attribute to mark which properties and fields should not be considered
/// when performing memberwise equality in value objects.
/// </summary>
// source: https://github.com/jhewlett/ValueObject
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class IgnoreMemberAttribute : Attribute
{
}
