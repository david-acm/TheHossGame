// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Diagnostics.CodeAnalysis;

#endregion

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Necessary to comply with the naming standard of event sourcing.", Scope = "member", Target = "~M:Hoss.SharedKernel.EntityBase`1.When(Hoss.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not a concern here.", Scope = "member", Target = "~M:Hoss.SharedKernel.IInternalEventHandler.Handle(Hoss.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not a concern here.", Scope = "member", Target = "~M:Hoss.SharedKernel.EntityBase`1.Apply(Hoss.SharedKernel.DomainEventBase)")]
