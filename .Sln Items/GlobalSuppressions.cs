// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Using StyleCop rule instead: use this.<variable> instead of the _prefix", Scope = "namespace", Target = "~N:TheHossGame")]
[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for aggregates.", Scope = "type", Target = "~T:TheHossGame.SharedKernel.Interfaces.IAggregateRoot")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "Not applicable to web projects.", Scope = "namespace", Target = "~N:TheHossGame")]
