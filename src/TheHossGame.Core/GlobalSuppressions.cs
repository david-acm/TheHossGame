// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S2219:Runtime type checking should be simplified", Justification = "Not null here. Property in initializedn in constructor.", Scope = "member", Target = "~P:TheHossGame.Core.PlayerAggregate.APlayer.IsJoiningGame")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Doesn't apply to record types.", Scope = "module")]