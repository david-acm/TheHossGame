// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Using StyleCop rule instead: use this.<variable> instead of the _prefix", Scope = "namespace", Target = "~N:TheHossGame")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for aggregates.", Scope = "type", Target = "~T:TheHossGame.SharedKernel.Interfaces.IAggregateRoot")]
[assembly: SuppressMessage("Design", "SA1516:Elements should be separated by blank line", Justification = "Not appliable to program", Scope = "type", Target = "~T:TheHossGame.Web.SeedData")]
