﻿// 🃏 The HossGame 🃏
// <copyright file="GlobalSuppressions.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Diagnostics.CodeAnalysis;

#endregion

[assembly: SuppressMessage("Minor Code Smell", "S2219:Runtime type checking should be simplified", Justification = "Not null here. Property in initialized in constructor.", Scope = "member", Target = "~P:Hoss.Core.PlayerAggregate.APlayer.IsJoiningGame")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Doesn't apply to record types.", Scope = "module")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "Not needed for records", Scope = "member", Target = "~M:Hoss.Core.GameAggregate.AGame.When(Hoss.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:Statement should not use unnecessary parenthesis", Justification = "Doesn't apply to action methods.", Scope = "member", Target = "~M:Hoss.Core.GameAggregate.AGame.When(Hoss.SharedKernel.DomainEventBase)")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:Statement should not use unnecessary parenthesis", Justification = "Necessary for switch expressions returning actions.", Scope = "module")]