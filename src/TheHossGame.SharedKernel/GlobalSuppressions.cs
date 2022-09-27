// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Necesary to comply with the naming standar of event sourcing.", Scope = "member", Target = "~M:TheHossGame.SharedKernel.EntityBase`1.When(TheHossGame.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not a concern here.", Scope = "member", Target = "~M:TheHossGame.SharedKernel.IInternalEventHandler.Handle(TheHossGame.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not a concern here.", Scope = "member", Target = "~M:TheHossGame.SharedKernel.EntityBase`1.Apply(TheHossGame.SharedKernel.DomainEventBase)")]
